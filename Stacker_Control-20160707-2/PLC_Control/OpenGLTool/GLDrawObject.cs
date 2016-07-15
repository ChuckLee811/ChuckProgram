using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
//for PixelFormat
using System.Drawing.Imaging;
//for 碼錶
using System.Diagnostics;
//for MouseEventArgs
using System.Windows.Forms;
using SharpGL;
//for GraphicPath
using System.Drawing.Drawing2D;

namespace OpenGLTool
{
    class GLDrawObject
    {
        public static int MaxColorH = 260;
        public static int MinColorH = 50;
        public static int MaxErrorColorH = 360;
        public static int MinErrorColorH = 330;

        #region DrawOpenGL

        public static int isNav = 1;
        public static void Trans3D(SharpGL.OpenGL gl_object, int x, int y)
        {
            //Console.Write(_RoX + ", " + _RoY + ", " + " => ");

            //	Create an array that will be the viewport.
            //3D顯示大小
            int[] viewport = new int[4];
            //	Get the viewport, then convert the mouse point to an opengl point.
            gl_object.GetInteger(OpenGL.GL_VIEWPORT, viewport);
            double[] modelview = new double[16];
            gl_object.GetDouble(OpenGL.GL_MODELVIEW_MATRIX, modelview);
            double[] projection = new double[16];
            gl_object.GetDouble(OpenGL.GL_PROJECTION_MATRIX, projection);

            float winX, winY;
            winX = (x - viewport[2] / 2);
            winY = (y - viewport[3] / 2); //與範例不同的是 圖形原點是左上角 
            //Console.Write(winX + ", " + winY + ", " + " => ");

            //(posX, posY, posZ) 轉換之前的座標
            /* double[] posX = new double[4];
             double[] posY = new double[4];
             double[] posZ = new double[4];*/
            double posX = new double();
            double posY = new double();
            double posZ = new double();
            gl_object.UnProject(x, y, 0, modelview, projection, viewport, ref posX, ref posY, ref posZ);

            isNav = 1;
            if (_RoX >= 165 && _RoX <= 200)
            {
                //if (winX * posX[0] < 0 && winY * posY[0] < 0) isNav = -1;
                if (winX * posX < 0 && winY * posY < 0) isNav = -1;
            }
            else if (_RoX >= 45 && _RoX <= 315)
            {
                //if (winX * posX[0] > 0) isNav = -1;
                if (winX * posX > 0) isNav = -1;
            }
            return;
        }

        public static void drawAxis(SharpGL.OpenGL gl_object)
        {
            //畫Z軸
            gl_object.Color(1.0, 1.0, 1.0);
            gl_object.Begin(OpenGL.GL_TRIANGLE_STRIP);
            gl_object.Vertex(-0.02, -0.02, -0.02);
            gl_object.Vertex(-0.02, -0.02, 10);
            gl_object.Vertex(0.02, -0.02, -0.02);
            gl_object.Vertex(0.02, -0.02, 10);
            gl_object.Vertex(0.02, 0.02, -0.02);
            gl_object.Vertex(0.02, 0.02, 10);
            gl_object.Vertex(-0.02, 0.02, -0.02);
            gl_object.Vertex(-0.02, 0.02, 10);
            gl_object.Vertex(-0.02, -0.02, -0.02);
            gl_object.Vertex(-0.02, -0.02, 10);
            gl_object.End();
            //up
            gl_object.Begin(OpenGL.GL_TRIANGLE_STRIP);
            gl_object.Vertex(-0.02, -0.02, 10);
            gl_object.Vertex(0.02, -0.02, 10);
            gl_object.Vertex(-0.02, 0.02, 10);
            gl_object.Vertex(0.02, 0.02, 10);
            gl_object.End();

            //down
            gl_object.Begin(OpenGL.GL_TRIANGLE_STRIP);
            gl_object.Vertex(-0.02, -0.02, -0.02);
            gl_object.Vertex(0.02, -0.02, -0.02);
            gl_object.Vertex(-0.02, 0.02, -0.02);
            gl_object.Vertex(0.02, 0.02, -0.02);
            gl_object.End();

            //畫X軸
            gl_object.Color(0, 1.0, 0);
            gl_object.Begin(OpenGL.GL_TRIANGLE_STRIP);
            gl_object.Vertex(0.02, -0.02, -0.02);
            gl_object.Vertex(10, -0.02, -0.02);
            gl_object.Vertex(0.02, 0.02, -0.02);
            gl_object.Vertex(10, 0.02, -0.02);
            gl_object.Vertex(0.02, 0.02, 0.02);
            gl_object.Vertex(10, 0.02, 0.02);
            gl_object.Vertex(0.02, -0.02, 0.02);
            gl_object.Vertex(10, -0.02, 0.02);
            gl_object.Vertex(0.02, -0.02, -0.02);
            gl_object.Vertex(10, -0.02, -0.02);
            gl_object.End();
            //up
            gl_object.Begin(OpenGL.GL_TRIANGLE_STRIP);
            gl_object.Vertex(10, -0.02, -0.02);
            gl_object.Vertex(10, 0.02, -0.02);
            gl_object.Vertex(10, -0.02, 0.02);
            gl_object.Vertex(10, 0.02, 0.02);
            gl_object.End();

            //down
            gl_object.Begin(OpenGL.GL_TRIANGLE_STRIP);
            gl_object.Vertex(0.02, -0.02, -0.02);
            gl_object.Vertex(0.02, 0.02, -0.02);
            gl_object.Vertex(0.02, -0.02, 0.02);
            gl_object.Vertex(0.02, 0.02, 0.02);
            gl_object.End();


            //畫Y軸
            gl_object.Color(0, 0, 1.0);
            gl_object.Begin(OpenGL.GL_TRIANGLE_STRIP);
            gl_object.Vertex(-0.02, 0.02, -0.02);
            gl_object.Vertex(-0.02, 10, -0.02);
            gl_object.Vertex(0.02, 0.02, -0.02);
            gl_object.Vertex(0.02, 10, -0.02);
            gl_object.Vertex(0.02, 0.02, 0.02);
            gl_object.Vertex(0.02, 10, 0.02);
            gl_object.Vertex(-0.02, 0.02, 0.02);
            gl_object.Vertex(-0.02, 10, 0.02);
            gl_object.Vertex(-0.02, 0.02, -0.02);
            gl_object.Vertex(-0.02, 10, -0.02);
            gl_object.End();
            //up
            gl_object.Begin(OpenGL.GL_TRIANGLE_STRIP);
            gl_object.Vertex(-0.02, 10, -0.02);
            gl_object.Vertex(0.02, 10, -0.02);
            gl_object.Vertex(-0.02, 10, 0.02);
            gl_object.Vertex(0.02, 10, 0.02);
            gl_object.End();

            //down
            gl_object.Begin(OpenGL.GL_TRIANGLE_STRIP);
            gl_object.Vertex(-0.02, 0.02, -0.02);
            gl_object.Vertex(0.02, 0.02, -0.02);
            gl_object.Vertex(-0.02, 0.02, 0.02);
            gl_object.Vertex(0.02, 0.02, 0.02);
            gl_object.End();
        }

        #endregion

        public enum OperationStatus { None, Offline2D, Online2D, Offline3D, Online3D };               //操作模式
        public static OperationStatus _ReviewStatus = OperationStatus.None; //檢視模式開關
        public enum MoveStatus { None, Rotate, Shift };             //移動模式
        public static MoveStatus _3DMoveStatus = MoveStatus.None;   //是否為平移模式(右鍵模式)

        //全部3D座標系同步
        //座標
        public static float _LX = 0;
        public static float _LY = 0;
        public static float _LZ = -10f;
        //旋轉座標
        public static float _RoX = 0;
        public static float _RoY = 0;
        public static float _RoZ = 0;

        public static Point _BakMousePosition = new Point(0, 0);  //記錄原游標位置
        public static void OpenGLCtrl_MouseMove(object sender, MouseEventArgs e)
        {
            OpenGLControl ctrl = (OpenGLControl)sender;
            if (_3DMoveStatus == MoveStatus.Rotate)
            {
                _RoX += (e.X - _BakMousePosition.X) / 5.0f;
                _RoY += isNav * (e.Y - _BakMousePosition.Y) / 5.0f;
                if (_RoX < 0) _RoX += 360;
                else if (_RoX > 360) _RoX -= 360;
                if (_RoY < 0) _RoY += 360;
                else if (_RoY > 360) _RoY -= 360;
            }
            else if (_3DMoveStatus == MoveStatus.Shift)
            {
                _LX += (e.X - _BakMousePosition.X) / (70.0f + _LZ);
                _LY += (_BakMousePosition.Y - e.Y) / (70.0f + _LZ);
            }
            _BakMousePosition.X = e.X;
            _BakMousePosition.Y = e.Y;
        }

        public static void ResetView()
        {
            _LX = 0;
            _LY = 0;
            _LZ = -15f;
            _RoX = _RoY = _RoZ = 0.0f;
            _RoY = 0;
        }

        public static void HsvToRgb(double h, double S, double V, out double r, out double g, out double b)
        {
            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Red is the dominant color
                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color
                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color
                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Red is the dominant color
                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.
                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            r = R;
            g = G;
            b = B;
        }
    }
}
