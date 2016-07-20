#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <iostream>

#include "LineTracking.h"
#include "VeinAnalyzing.h"

using namespace std;

void TestArray() {
	Mat A(3, 3, CV_32F, Scalar(8));

	Mat B(3, 3, CV_32F, Scalar(2));
	
	printf("Ayyay A:\n");
	for (int i = 0; i < 3; i++)
	{
		for (int j = 0; j < 3; j++)
		{
			printf("%.3f\t", A.at<float>(j, i));
		}
		printf("\n");
	}

	printf("Ayyay B:\n");
	for (int i = 0; i < 3; i++)
	{
		for (int j = 0; j < 3; j++)
		{
			printf("%.3f\t", B.at<float>(j, i));
		}
		printf("\n");
	}

	// A.mul(B);
	multiply(A, B, A);

	printf("Ayyay A X B:\n");
	for (int i = 0; i < 3; i++)
	{
		for (int j = 0; j < 3; j++)
		{
			printf("%.3f\t", A.at<float>(j, i));
		}
		printf("\n");
	}
	printf("END\n");
}

void TestSubTract()
{
	Mat tOut;
	Mat tImage = imread("Sample_2.jpg", CV_LOAD_IMAGE_GRAYSCALE);
	Mat tImageSmooth = imread("Sample_3.jpg", CV_LOAD_IMAGE_GRAYSCALE);

	absdiff(tImage, tImageSmooth, tOut);

	imwrite("./Sub.png", tOut);
}


int main()
{	
	double eTimeCost = 0;
	Mat tImg;
	char acInFileName[100] = {};
	char acOutFileName[100] = {};
	VeinAnalyzingCFG tCfg;
	VeinAnalyzingData tData;

	printf("Load Default configure...\n");
	tCfg = VeinAnalyzing_LoadDefaultCfg();

	printf("Load configure from INI file [CfgFile.ini]...\n");
	tCfg = VeinAnalyzing_LoadCfgByINI("./CfgFile.ini");

#if 1 // 跑單張
	sprintf(acInFileName, "./Src/Src_%d.png", 43);
	tImg = imread(acInFileName, CV_LOAD_IMAGE_GRAYSCALE);

	if (tImg.empty())
	{
		printf("The tImg is empty!\n");
		return -1;
	}

	eTimeCost = (double)getTickCount();

	printf("Analyzing...\n");
	tData = VeinAnalyzing(tImg, tCfg);
	printf("Analyzing done !\n");
	eTimeCost = ((double)getTickCount() - eTimeCost) / getTickFrequency();
	printf("The average Vein Width is %.3f (pixel)\n", tData.eVeinWidth);
	printf("Time Cost is %.3f sec\n", eTimeCost);

	sprintf(acOutFileName, "./ROI.png");
	imwrite(acOutFileName, tData.tROI_Pic);

	sprintf(acOutFileName, "./Vein.png");
	imwrite(acOutFileName, tData.tVeinPic);

#else // 用for 跑圖庫
	int lCnt = 0;
	
	while (1)
	{
		printf("Process Pic %d\n", lCnt + 1);

		sprintf(acInFileName, "./Src/Src_%d.png", lCnt+1);
		tImg = imread(acInFileName, CV_LOAD_IMAGE_GRAYSCALE);

		if (tImg.empty())
		{
			printf("The tImg is empty >> All Pic is done\n");
			break;
		}

		tData = VeinAnalyzing(tImg, tCfg);

		sprintf(acOutFileName, "./ROI_Result/ROI_%d.png", lCnt + 1);
		imwrite(acOutFileName, tData.tROI_Pic);

		sprintf(acOutFileName, "./VeinResult/Vein_%d.png", lCnt + 1);
		imwrite(acOutFileName, tData.tVeinPic);

		lCnt++;
	}
#endif
	system("pause");
	return 0;
}
