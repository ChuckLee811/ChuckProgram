using System;
using System.Collections.Generic;
using System.Text;
//for MessageBox
using System.Windows.Forms;
//for Thread
using System.Threading;



namespace ObjectPLC
{
    public enum TriggerType
    {
        ALL,
        UP,
        DOWN,
        HIEGHT,
        LOW,
    }
    public class Object_Trigger
    {
        public DATABUILDERAXLibLB.DBPlcDevice DeviceType = 0;
        public string StrNo = "";
        public TriggerType TriggerType = TriggerType.ALL;
        public int back_data = -1;
        public int this_data = -1;
        public bool Enable = true;

        //收到Trigger後呼叫
        public delegate void CallbackTriggerData(Object_Trigger trigger);
        public event CallbackTriggerData _triggerback;

        public void checkTrigger(int value)
        {
            this_data = value;
            if (_triggerback != null)
            {
                bool back = false;
                if (TriggerType == TriggerType.ALL) back = true;
                else if (TriggerType == TriggerType.LOW && value == 0) back = true;
                else if (TriggerType == TriggerType.HIEGHT && value == 1) back = true;
                else if (TriggerType == TriggerType.UP && back_data == 0 && value == 1) back = true;
                else if (TriggerType == TriggerType.DOWN && back_data == 1 && value == 0) back = true;

                //if (value == 1) back = true;
                //else back = false;
                if (back == true)
                {
                    //Thread thread = new Thread(ThreadCheckTrigger); //啟動Thread
                    //thread.Start();
                    _triggerback(this);
                }
            }
            back_data = value;
        }

        public void ThreadCheckTrigger()
        {
            _triggerback(this);
        }
    }

    public class ObjectPLC_KV
    {
#region Constant
        public class STATUS
        {
            public const int INI = 0x01;
            public const int CONNECTED = 0x02;
            public const int TRIGGER = 0x04;
            public const int MONITOR = 0x08;
        }

        public static bool _ShowMsg = false;
#endregion

#region Field
        //連線與讀寫
        public AxDATABUILDERAXLibLB.AxDBCommManager axDBCommManager = null;
        //觸發與中斷
        public AxDATABUILDERAXLibLB.AxDBTriggerManager axDBTriggerManager = null;
        //裝置控制
        public AxDATABUILDERAXLibLB.AxDBDeviceManager axDBDeviceManager = null;

        //控制單元
        public string _DeviceNo = "";
        public DATABUILDERAXLibLB.DBPlcDevice _DeviceType;

        public int _Status = STATUS.INI;

        public delegate void CallbackError(Object sender, int status);
        public event CallbackError _errorback;

#endregion

        //觸發相關設定
#region PLC_KV Timer Trigger
        public enum TriggerSource
        {
            ACTIVE,
            ALWAYS,
        }
        public static TriggerSource _TriggerSource = TriggerSource.ALWAYS;
        private List<Object_Trigger> triggers = new List<Object_Trigger>();
        private Thread _ThreadTrigger = null;
        public Object_Trigger addTrigger(DATABUILDERAXLibLB.DBPlcDevice dtype, string no, TriggerType ttype)
        {
            Object_Trigger trigger = new Object_Trigger();
            trigger.DeviceType = dtype;
            trigger.StrNo = no;
            trigger.TriggerType = ttype;
            triggers.Add(trigger);
            return trigger;
        }
        public int ableTrigger(DATABUILDERAXLibLB.DBPlcDevice dtype, string no, bool enable)
        {
            for (int i = 0; i < triggers.Count; i++)
            {
                Object_Trigger trigger = triggers[i];
                if (trigger.DeviceType == dtype && trigger.StrNo == no)
                {
                    trigger.Enable = enable;
                    return i;
                }
            }
            return -1;
        }

        private void checkTrigger()
        {
            int tmpStatus = _Status & (STATUS.TRIGGER);
            do
            {
                for (int i = 0; i < triggers.Count; i++)
                {
                    Object_Trigger trigger = triggers[i];
                    if (trigger.Enable == false) continue;
                    trigger.checkTrigger(doReadDevice(trigger.DeviceType, trigger.StrNo));
                }
                Thread.Sleep(100);
                tmpStatus = _Status & (STATUS.TRIGGER);
            } while (tmpStatus > 0);
        }
#endregion

#region PLC_KV Hander

        public bool checkConnect(bool check, bool show)
        {
            if (axDBCommManager == null) return false;
            //狀態一樣
            if (check == axDBCommManager.Active) return true;

            if (show == true)
            {
                //顯示目前狀態
                if (axDBCommManager.Active == false) MessageBox.Show("尚未連線");
                else MessageBox.Show("已連線");
            }
            if (_errorback != null) _errorback(this, 0x01);
            return false;
        }
        public bool getConnected()
        {
            if (axDBCommManager == null) return false;
            return axDBCommManager.Active;
        }

        public bool doMoniter()
        {
            for (int i = 0; i < triggers.Count; i++)
            {
                Object_Trigger trigger = triggers[i];
                trigger.back_data = trigger.this_data = -1;
            }

            if (doConnect() == false) return false;
            if (ActiveTrigger() == false) return false;
            _Status |= STATUS.MONITOR;
            return true;
        }

        public bool doDeMoniter()
        {
            _Status &= ~STATUS.MONITOR;
            if (doDisConnect() == false) return false;
            if (deActiveTrigger() == false) return false;
            return true;
        }

        public bool doConnect()
        {
            int tmpStatus = _Status & STATUS.MONITOR;
            if (tmpStatus > 0)
            {
                //監控中，不顯示錯誤訊息，已連線直接回傳true
                if (checkConnect(false, false) == false) return true;
            }
            else
            {
                if (checkConnect(false, true) == false) return false;
            }

            try
            {
                axDBCommManager.Connect();
                _Status |= STATUS.CONNECTED;
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Failed to open the port.") == true) MessageBox.Show("PLC連線失敗");
                else MessageBox.Show(ex.Message);
                return false;
            }
        }
        public bool doDisConnect()
        {
            int tmpStatus = _Status & STATUS.MONITOR;
            //監控中，不進行關閉
            if (tmpStatus > 0) return true;

            if (checkConnect(true, true) == false) return false;

            try
            {
                if (axDBTriggerManager != null)
                {
                    if (axDBTriggerManager.Active == true)
                    {
                        axDBTriggerManager.Active = false;
                        _Status |= STATUS.CONNECTED;
                    }
                }

                axDBCommManager.Disconnect();
                _Status &= ~STATUS.CONNECTED;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool getMode()
        {
            if (checkConnect(true, true) == false) return false;
            return axDBCommManager.GetMode();
        }
        public void setMode(bool mode)
        {
            if (checkConnect(true, true) == false) return;
            try
            {
                axDBCommManager.SetMode(mode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool checkTrigger(bool check, bool show)
        {
            if (_TriggerSource == TriggerSource.ALWAYS)
            {
                //狀態一樣
                if (check == true && _ThreadTrigger != null ||
                    check == false && _ThreadTrigger == null) return true;

                //顯示目前狀態
                if (_ThreadTrigger == null) MessageBox.Show("未開啟接收觸發");
                else MessageBox.Show("已開啟接收觸發");
                return false;
            }

            if (axDBTriggerManager == null) return false;
            //狀態一樣
            if (check == axDBTriggerManager.Active) return true;

            if (show == true)
            {
                //顯示目前狀態
                if (axDBTriggerManager.Active == false) MessageBox.Show("未開啟接收觸發");
                else MessageBox.Show("已開啟接收觸發");
            }
            if (_errorback != null) _errorback(this, 0x02);
            return false;
        }
        public bool getTrigger()
        {
            if (checkConnect(true, true) == false) return false;
            if (_TriggerSource == TriggerSource.ALWAYS)
            {
                if (_ThreadTrigger == null) return false;
                return true;
            }

            if (axDBTriggerManager == null) return false;
            return axDBTriggerManager.Active;
        }

        public bool ActiveTrigger()
        {
            for (int i = 0; i < triggers.Count; i++)
            {
                Object_Trigger trigger = triggers[i];
                trigger.back_data = trigger.this_data = -1;
            }

            int tmpStatus = _Status & STATUS.MONITOR;
            if (tmpStatus > 0)
            {
                //監控中，不顯示錯誤訊息，已開啟觸發直接回傳true
                if (checkConnect(true, false) == false) return true;
                if (checkTrigger(false, false) == false) return true;
            }
            else
            {
                if (checkConnect(true, true) == false) return false;
                if (checkTrigger(false, true) == false) return false;
            }

            try
            {
                if (_TriggerSource == TriggerSource.ALWAYS)
                {
                    _Status |= STATUS.TRIGGER;
                    _ThreadTrigger = new Thread(checkTrigger); //啟動Thread
                    _ThreadTrigger.Start();
                }
                else
                {
                    axDBTriggerManager.Active = true;
                    //啟動觸發接收
                    //axDBTriggerManager.Triggers[3].AsCustom.SetState(true);
                }
                _Status |= STATUS.TRIGGER;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }
        public bool deActiveTrigger()
        {
            int tmpStatus = _Status & STATUS.MONITOR;
            //監控中，不進行關閉
            if (tmpStatus > 0) return true;

            //if (checkConnect(true, true) == false) return false;
            if (checkTrigger(true, true) == false) return false;

            try
            {
                if (_TriggerSource == TriggerSource.ALWAYS)
                {
                    _Status &= ~STATUS.TRIGGER;
                    _ThreadTrigger.Join();
                    _ThreadTrigger = null;
                }
                else
                {
                    //停止觸發接收
                    //axDBTriggerManager.Triggers[3].AsCustom.SetState(false);
                    axDBTriggerManager.Active = false;
                }
                _Status &= ~STATUS.TRIGGER;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        public bool doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice type, string strNo, int val)
        {
            _DeviceNo = strNo;
            _DeviceType = type;
            return doWriteDevice(val);
        }

        public bool doWriteDevice(int val)
        {
            if (checkConnect(true, false) == false) return false;
            if (_DeviceNo == "")
            {
                //MessageBox.Show("尚未設定控制單元");
                return false;
            }

            try
            {
                if (_ShowMsg == true) Console.WriteLine(DateTime.Now.TimeOfDay + " WriteDevice: " + _DeviceNo + " = " + val);
                axDBCommManager.WriteDevice(_DeviceType, _DeviceNo, val);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (_errorback != null) _errorback(this, 0x01);
                return false;
            }
        }

        public int doReadDevice(DATABUILDERAXLibLB.DBPlcDevice type, string strNo)
        {
            _DeviceNo = strNo;
            _DeviceType = type;
            return doReadDevice();
        }

        public int doReadDevice()
        {
            if (checkConnect(true, false) == false) return -1;
            if (_DeviceNo == "")
            {
                return -1;
            }
            try
            {
                int r = axDBCommManager.ReadDevice(_DeviceType, _DeviceNo);
                if (_ShowMsg == true) Console.WriteLine(DateTime.Now.TimeOfDay + " ReadDevice: " + _DeviceNo + " = " + r);
                return r;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (_errorback != null) _errorback(this, 0x01);
                return -1;
            }
        }

#endregion
    }
}

