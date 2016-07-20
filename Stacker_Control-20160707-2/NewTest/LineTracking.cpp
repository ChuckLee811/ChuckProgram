
#include <stdio.h>
#include "lsd.h"
#include "LineTracking.h"
#include <iostream>
#include "INIReader.h"
using namespace cv; // 不加也可以跑 >> 但會有 "ambiguous" 的顯示??  >> 明明Common.h裡有加

#if LOG_USED_MAP_OF_START_POINT >0
unsigned int g_ulStartPointIndex = 0;
#endif

LineTrackingCFG LineTracking_LoadDefaultCfg()
{
	LineTrackingCFG tCfg;

	tCfg.eAngleStep = 15;
	tCfg.lStepLength = 2;
	tCfg.lLineWidth = 8;
	tCfg.eGapTH = 15.0;
	tCfg.eMaxDeltaTheta = 61;
	tCfg.bDoTracking = false;

	return tCfg;
}

LineTrackingCFG LineTracking_LoadCfgByINI(std::string a_sFilename)
{
	LineTrackingCFG tCfg;
	INIReader tINI_Reader;
	tINI_Reader.Read_INI(a_sFilename);

	tCfg.eAngleStep = tINI_Reader.GetReal("LineTracking", "AngleStep", -1);
	tCfg.lStepLength = tINI_Reader.GetInteger("LineTracking", "StepLength", -1);
	tCfg.lLineWidth = tINI_Reader.GetInteger("LineTracking", "LineWidth", -1);
	tCfg.eGapTH = tINI_Reader.GetReal("LineTracking", "GapTH", -1);
	tCfg.eMaxDeltaTheta = tINI_Reader.GetReal("LineTracking", "MaxDeltaTheta", -1);
	tCfg.bDoTracking = tINI_Reader.GetBoolean("LineTracking", "DoTracking", true);

	return tCfg;
}

Mat LineTracking(
	const Mat a_tSrc, const double a_eAngleStep, const int a_lStepLength, const int a_lBllodWidth,
	const double a_eGapTH, const double a_eMaxDeltaTheta, const bool a_bDoTracking)
{
	double eMin = 0, eMax = 0, eDetectWith = 0;

	Mat tSrcbuffer;
	tSrcbuffer = a_tSrc;

	eDetectWith = a_lBllodWidth*1.5;

	cv::minMaxLoc(tSrcbuffer, &eMin, &eMax);

	// Equalize Histogram
	ImageEqualizeHistByBound(tSrcbuffer, (int)eMax, (int)eMin);

#if DEBUG_LineTrackingLocal_LogImg > 0
	imwrite("./LT_EqualizeHist.png", tSrcbuffer);
#endif

	Mat tCntTable = Mat::zeros(a_tSrc.size(), CV_32F);

	// Detect all Possible Start Point
	printf("Detect all Possible Start Point\n");
	vector<Point2d> vtPossiblePoint;
	vector<double> veMoveDirection;

	Mat tPossibleImg = LineTracking_LogAllPossibleStartPoint(
		a_tSrc, a_eAngleStep, a_lStepLength, eDetectWith, a_eGapTH, vtPossiblePoint, veMoveDirection);

	if (a_bDoTracking)
	{ 
		// tracking all start point until break
		printf("Tracking all start point\n");
#if LOG_USED_MAP_OF_START_POINT >0
		g_ulStartPointIndex = 0;
#endif
		for (size_t i = 0; i < vtPossiblePoint.size(); i++)
		{
			LineTrack_PointTracking(
				a_tSrc, vtPossiblePoint[i], veMoveDirection[i], a_eAngleStep,
				a_lStepLength, eDetectWith, a_eMaxDeltaTheta, a_eGapTH, tCntTable
				);
#if LOG_USED_MAP_OF_START_POINT >0
			g_ulStartPointIndex++;
#endif
		}

		// log tCntTable as final energy(possibility) map
		printf("Log final energy(possibility) map\n");
		Mat tEnergyMap = tCntTable;
		normalize(tEnergyMap, tEnergyMap, 0, 1, CV_MINMAX);
		tEnergyMap = tEnergyMap*MAX_PIXEL_VAL_8UC;

#if DO_LINE_TRACKING_ENHANCE > 0	// 再次增強 圖像
		int alHistogram[MAX_PIXEL_VAL_8UC + 1] = {}, lTH_High = 0, lTH_Low = 0;

		double eMean = 0, eSigma = 0;
		HistogramCal(tEnergyMap, alHistogram, MAX_PIXEL_VAL_8UC + 1);
		StandardDeviationCalImage(alHistogram, MAX_PIXEL_VAL_8UC + 1, eMean, eSigma);
		lTH_High = int(eMean + 0.5 * eSigma);

		Mat tEnergyMapEnhance;
		tEnergyMap.convertTo(tEnergyMapEnhance, CV_8U);
		ImageEqualizeHistByBound(tEnergyMapEnhance, lTH_High, lTH_Low);

		// doing Gaussian Blur
		GaussianBlur(tEnergyMapEnhance, tEnergyMapEnhance, Size(3, 3), 0, 0);

		tEnergyMap = tEnergyMapEnhance.clone();
#endif

#if DEBUG_LineTrackingLocal_LogImg > 0
		imwrite("./EnergyMap.png", tEnergyMap);
#endif
		return tEnergyMap;
	}
	else
	{
		// 直接回傳 可能區域就好
		return tPossibleImg;
	}
}

Mat LineTracking_LogAllPossibleStartPoint(
	const Mat a_tSrc, const double a_eAngleStep, const int a_lStepLength,
	const double a_eDetectWith, const double a_eThreshold, vector<Point2d> &a_vtPossiblePoint, vector<double> &a_veMoveDirection)
{
	Point tStartPoint;
	double eCurrAngleMove = 0;
#if DEBUG_LineTrackingLocal_LogImg > 0
	Mat tDrawImgBuffer = GrayToColor(a_tSrc);
#endif
	Mat tPossibleImg;

	// tPossibleImg.create(a_tSrc.rows, a_tSrc.cols, CV_8UC(1));
	tPossibleImg = Mat::zeros(a_tSrc.rows, a_tSrc.cols, CV_8UC1);

	for (int y = 0; y < a_tSrc.rows; y++)
	{
		for (int x = 0; x < a_tSrc.cols; x++)
		{
			tStartPoint.x = x;
			tStartPoint.y = y;
			eCurrAngleMove = LineTrack_StartAngleCal(
				a_tSrc, tStartPoint, a_eAngleStep, a_lStepLength, a_eDetectWith, a_eThreshold);

			if (eCurrAngleMove >= 0)
			{	// 可以被當做起始點
				// set pixel
#if DEBUG_LineTrackingLocal_LogImg > 0
				tDrawImgBuffer.at<Vec3b>(tStartPoint) = Vec3b(0, 0, 255);
#endif
				tPossibleImg.at<uchar>(tStartPoint) = MAX_PIXEL_VAL_8UC;

				Point2d tPointTmp;
				tPointTmp = tStartPoint;
				a_vtPossiblePoint.push_back(tPointTmp);
				a_veMoveDirection.push_back(eCurrAngleMove);
			}
		}
	}

#if DEBUG_LineTrackingLocal_LogImg > 0
	imwrite("./AllPossibleStartPoint.png", tDrawImgBuffer);
#endif

	return tPossibleImg;
}

void LogIntensity(const Mat &a_tSrc, Point a_tPoint, int a_lStepLength, double a_eDetectWith, double a_eAngle)
{
	Point2d tNext, tCheckP_Left, tCheckP_Right, tVector;
	Point tTmp;

	tNext.x = a_tPoint.x + a_lStepLength*cos(a_eAngle * PI / 180);
	tNext.y = a_tPoint.y + a_lStepLength*sin(a_eAngle * PI / 180);

	tCheckP_Right.x = tNext.x - a_eDetectWith / 2 * sin(a_eAngle * PI / 180);
	tCheckP_Right.y = tNext.y + a_eDetectWith / 2 * cos(a_eAngle * PI / 180);

	tCheckP_Left.x = tNext.x + a_eDetectWith / 2 * sin(a_eAngle * PI / 180);
	tCheckP_Left.y = tNext.y - a_eDetectWith / 2 * cos(a_eAngle * PI / 180);

	// log the Intensity from left to right
	tVector.x = tCheckP_Right.x - tCheckP_Left.x;
	tVector.y = tCheckP_Right.y - tCheckP_Left.y;
	FILE *fptr = NULL;

	fptr = fopen("./LogIntensity_LT.csv", "w");
	unsigned char aucPixelBuff[SAMPLE_POINT_NUM] = {};
	for (int lCnt = 0; lCnt < SAMPLE_POINT_NUM; lCnt++)
	{
		tTmp.x = (int)(tCheckP_Left.x + tVector.x*lCnt / SAMPLE_POINT_NUM);
		tTmp.y = (int)(tCheckP_Left.y + tVector.y*lCnt / SAMPLE_POINT_NUM);
		fprintf(fptr, "%d,\n", a_tSrc.at<uchar>(tTmp.y, tTmp.x));
		aucPixelBuff[lCnt] = a_tSrc.at<uchar>(tTmp.y, tTmp.x);
	}
	fclose(fptr);

	Mat tDrawImgBuffer = GrayToColor(a_tSrc);

	line(tDrawImgBuffer, a_tPoint, Point(int(tNext.x), int(tNext.y)), Scalar(0, 0, 255), 1);
	line(tDrawImgBuffer, Point(int(tCheckP_Left.x), int(tCheckP_Left.y)), Point(int(tCheckP_Right.x), int(tCheckP_Right.y)), Scalar(0, 255, 0), 1);

	imwrite("./LogIntensity_LT.png", tDrawImgBuffer);
}

void LineTrack_PointTracking(
	const Mat a_tSrc, const Point2d a_tCurrPoint, const double a_eCurrAngle, const double eAngleStep,
	const int a_lStepLength, const double a_eDetectWith, const double a_eMaxDeltaTheta,
	const double a_eThreshold, Mat &a_tCntTable)
{
	Point2d tCurrPointBuff, tNextPointTmp, tNextPoint;
#if LOG_USED_MAP_OF_START_POINT > 0
	Mat tUsedTable = Mat::zeros(a_tSrc.size(), CV_8U);
#endif
	bool bBreak = false;
	double *peScoreBuff = NULL, eMaxScore = 0;
	double eCurrAngleBuff = 0, eNextAngle = 0, eTmpAngle = 0, eAngleDiff = 0, eTmpScore = 0;
	int lCnt = 0;

	lCnt = (int)(360 / eAngleStep);
	eCurrAngleBuff = a_eCurrAngle;
	tCurrPointBuff = a_tCurrPoint;

	// 起始點要先ON
#if LOG_USED_MAP_OF_START_POINT > 0
	tUsedTable.at<uchar>((int)tCurrPointBuff.y, (int)tCurrPointBuff.x) = MAX_PIXEL_VAL_8UC;
#endif
	a_tCntTable.at<float>((int)tCurrPointBuff.y, (int)tCurrPointBuff.x) = 1;

	peScoreBuff = new double[lCnt];
	while (!bBreak)
	{
		for (int i = 0; i < lCnt; i++)
		{
			peScoreBuff[i] = -1;
		}

		// find next point
		eMaxScore = -1;
		eNextAngle = 0;
		tNextPoint.x = 0;
		tNextPoint.y = 0;
		for (int i = 0; i < lCnt; i++)
		{
			eTmpAngle = i*eAngleStep;
			eTmpScore = DoLineTrackScoreCal(
				a_tSrc, NULL, a_lStepLength, a_eDetectWith, tCurrPointBuff, eTmpAngle, tNextPointTmp);

			eAngleDiff = abs(eTmpAngle - eCurrAngleBuff);

			if (eAngleDiff < a_eMaxDeltaTheta && eTmpScore >= a_eThreshold)
			{
				peScoreBuff[i] = eTmpScore;

				if (eTmpScore > eMaxScore)
				{
					eMaxScore = eTmpScore;
					tNextPoint = tNextPointTmp;
					eNextAngle = eTmpAngle;
				}
			}
		}

		// 有下一個值
		if (eMaxScore >= 0)
		{
#if LOG_USED_MAP_OF_START_POINT > 0
			tUsedTable.at<uchar>((int)tNextPoint.y, (int)tNextPoint.x) = MAX_PIXEL_VAL_8UC;
#endif
			a_tCntTable.at<float>((int)tNextPoint.y, (int)tNextPoint.x)++;
			tCurrPointBuff = tNextPoint;	// 更新追蹤點
			eCurrAngleBuff = eNextAngle;	// 更新追蹤點方向
		}
		else
		{	// break
			bBreak = true;
		}
	}
	delete[] peScoreBuff;

#if LOG_USED_MAP_OF_START_POINT > 0
	char acFileName[256] = {};

	// log used map
	sprintf(acFileName, "./UsedMap/UsedImg_%d.png", g_ulStartPointIndex);
	imwrite(acFileName, tUsedTable);
#endif
}

double LineTrack_StartAngleCal(
	const Mat a_tSrc, const Point2d a_tStartPoint, const double a_eAngleStep,
	const int a_lStepLength, const double a_eDetectWith, const double a_eThreshold)
{
	int lCnt = 0;
	double eNextAngle = 0, eTmpAngle = 0, eAngleDiff = 0, eTmpScore = 0, eMaxScore = 0;
	Point2d tCurrPointBuff, tNextPointTmp;

	tCurrPointBuff = a_tStartPoint;

	lCnt = (int)(360 / a_eAngleStep);
	eMaxScore = -1;

	for (int i = 0; i < lCnt; i++)
	{
		eTmpAngle = i*a_eAngleStep;
		eTmpScore = DoLineTrackScoreCal(
			a_tSrc, NULL, a_lStepLength, a_eDetectWith, tCurrPointBuff, eTmpAngle, tNextPointTmp);

		if (eTmpScore >= a_eThreshold && eTmpScore > eMaxScore)
		{
			eMaxScore = eTmpScore;
			eNextAngle = eTmpAngle;
		}
	}

	if (eMaxScore >= 0)
	{	// 可設為起始點
		return eNextAngle;
	}
	else
	{	// 沒得追蹤 >> 梯度都太小
		return -1;
	}
}

double DoLineTrackScoreCal(
	const Mat a_tSrc, const double *a_peMask, const int a_lStepLength,
	const double a_eDetectWith, const Point a_tCurrPoint, const double a_eCurrAngle, Point2d &a_tNextPoint)
{
	Point2d tNext, tCheckP_Left, tCheckP_Right, tVector;
	Point tTmp;
	double eScore = 0;

	tNext.x = a_tCurrPoint.x + a_lStepLength*cos(a_eCurrAngle * PI / 180);
	tNext.y = a_tCurrPoint.y + a_lStepLength*sin(a_eCurrAngle * PI / 180);

	tCheckP_Right.x = tNext.x - a_eDetectWith / 2 * sin(a_eCurrAngle * PI / 180);
	tCheckP_Right.y = tNext.y + a_eDetectWith / 2 * cos(a_eCurrAngle * PI / 180);

	tCheckP_Left.x = tNext.x + a_eDetectWith / 2 * sin(a_eCurrAngle * PI / 180);
	tCheckP_Left.y = tNext.y - a_eDetectWith / 2 * cos(a_eCurrAngle * PI / 180);

	// check corrdinate
	Size tSize = a_tSrc.size();
	if (!CheckBoundary(tSize, tNext) || !CheckBoundary(tSize, tCheckP_Right) || !CheckBoundary(tSize, tCheckP_Left))
	{
		eScore = -1;
		return eScore;
	}

	tVector.x = tCheckP_Right.x - tCheckP_Left.x;
	tVector.y = tCheckP_Right.y - tCheckP_Left.y;

	unsigned char aucPixelBuff[SAMPLE_POINT_NUM] = {};
	for (int lCnt = 0; lCnt < SAMPLE_POINT_NUM; lCnt++)
	{
		tTmp.x = (int)(tCheckP_Left.x + tVector.x*lCnt / SAMPLE_POINT_NUM);
		tTmp.y = (int)(tCheckP_Left.y + tVector.y*lCnt / SAMPLE_POINT_NUM);
		aucPixelBuff[lCnt] = a_tSrc.at<uchar>(tTmp.y, tTmp.x);
	}

#if 1	// by paper

#if 0	// only check score
	eScore = aucPixelBuff[0] + aucPixelBuff[SAMPLE_POINT_NUM - 1] - 2 * a_tSrc.at<uchar>(int(tNext.y), int(tNext.x));
#else	// must have valley
	unsigned char ucPixelTmp = 0;
	ucPixelTmp = a_tSrc.at<uchar>(int(tNext.y), int(tNext.x));
	if (aucPixelBuff[0] > ucPixelTmp && aucPixelBuff[SAMPLE_POINT_NUM - 1] > ucPixelTmp)
	{
		eScore = aucPixelBuff[0] + aucPixelBuff[SAMPLE_POINT_NUM - 1] - 2 * ucPixelTmp;
	}
	else
	{
		eScore = -1;
	}
#endif
#else	// own function
	eScore = aucPixelBuff[0] + aucPixelBuff[SAMPLE_POINT_NUM - 1] - 2 * a_tSrc.at<uchar>(int(tNext.y), int(tNext.x));

#endif

	a_tNextPoint = tNext;
	return eScore;
}

void LineTrackingTest(const Mat a_tSrc, Mat &a_tDst)
{
	bool bBreak = false;
	int lBllodWidth = 0, lStepLength = 0;
	int lCnt = 0, lCntMax = 0;
	double eMaxDeltaTheta = 0, eGapTH = 0;
	double eDetectWith = 0;
	double eCurrAngle = 0, eNextAngle = 0, eTmpAngle = 0, eAngleDiff = 0, eTmpScore = 0;
	double eAngleStep = 0;

	lBllodWidth = 8;
	lStepLength = 2;
	eMaxDeltaTheta = 61;
	eGapTH = 15.0; // 10.0
	eCurrAngle = 30;
	eAngleStep = 15;
	eDetectWith = (int)(lBllodWidth*1.5);

	Mat tCntTable = Mat::zeros(a_tSrc.size(), CV_32F);

	// Detect all Possible Start Point
	vector<Point2d> vtPossiblePoint;
	vector<double> veMoveDirection;
	LineTracking_LogAllPossibleStartPoint(a_tSrc, eAngleStep, lStepLength, eDetectWith, eGapTH, vtPossiblePoint, veMoveDirection);

	// tracking all start point until break
#if LOG_USED_MAP_OF_START_POINT >0
	g_ulStartPointIndex = 0;
#endif
	for (size_t i = 0; i < vtPossiblePoint.size(); i++)
	{
		LineTrack_PointTracking(
			a_tSrc, vtPossiblePoint[i], veMoveDirection[i], eAngleStep,
			lStepLength, eDetectWith, eMaxDeltaTheta, eGapTH, tCntTable
			);
#if LOG_USED_MAP_OF_START_POINT >0
		g_ulStartPointIndex++;
#endif
	}

	// log tCntTable as final energy(possibility) map
	Mat tEnergyMap = tCntTable;
	normalize(tEnergyMap, tEnergyMap, 0, 1, CV_MINMAX);
	tEnergyMap = tEnergyMap*MAX_PIXEL_VAL_8UC;
	imwrite("./EnergyMap.png", tEnergyMap);

#if 1	// 測試用
	Point2d tCurrPoint = Point2d(28, 20);

	// tracking from start point until break
	LineTrack_PointTracking(
		a_tSrc, tCurrPoint, eCurrAngle, eAngleStep, lStepLength, eDetectWith, eMaxDeltaTheta, eGapTH, tCntTable);

	LogIntensity(a_tSrc, Point(53, 46), lStepLength, eDetectWith, 330);
#endif
}


void TestLineTrackingAlgorithm(const Mat &a_tSrc, Mat &a_tDst)
{
	double eMin = 0, eMax = 0;

	Mat tSrcbuffer;
	tSrcbuffer = a_tSrc;

	cv::minMaxLoc(tSrcbuffer, &eMin, &eMax);
	printf("Min: %.3f Max: %.3f\n", eMin, eMax);

	ImageEqualizeHistByBound(tSrcbuffer, (int)eMax, (int)eMin);

	LineTrackingTest(tSrcbuffer, a_tDst);

	imwrite("./LT_EqualizeHist.png", tSrcbuffer);
}