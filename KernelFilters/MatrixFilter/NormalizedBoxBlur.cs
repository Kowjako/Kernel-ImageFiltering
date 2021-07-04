using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Media;

namespace KernelFilters.MatrixFilter
{
    class NormalizedBoxBlur : IFilter
    {
        private float[,] kernel =
        {
            {1, 1, 1},
            {1, 1, 1},
            {1, 1, 1}
        };

        public ImageSource Filterize(ImageSource sourceImage)
        {
            Bitmap startImageBMP = null, outputImageBMP = null;

            startImageBMP = Converter.ImageSourceToBitmap(sourceImage);
            outputImageBMP = new Bitmap(startImageBMP.Width, startImageBMP.Height);

            int[,] kernelPixelsR = new int[3, 3];
            int[,] kernelPixelsG = new int[3, 3];
            int[,] kernelPixelsB = new int[3, 3];

            //float[] a = new float[5];
            //a[0] = (float)-0.5;
            //a[1] = (float)-0.5;
            //a[2] = (float)0;
            //a[3] = (float)0.5;
            //a[4] = (float)0.5;


            for (int x = 1; x < startImageBMP.Width - 1; x++)
            {
                for (int y = 1; y < startImageBMP.Height - 1; y++)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            kernelPixelsR[i + 1, j + 1] = startImageBMP.GetPixel(x + j, y + i).R;
                            kernelPixelsG[i + 1, j + 1] = startImageBMP.GetPixel(x + j, y + i).G;
                            kernelPixelsB[i + 1, j + 1] = startImageBMP.GetPixel(x + j, y + i).B;
                        }
                    }

                    //for (int i = -1; i <= 1; i++)
                    //{
                    //    for (int j = -1; j <= 1; j++)
                    //    {
                    //        MessageBox.Show(kernelPixels[i + 1, j + 1].ToString());
                    //    }
                    //}

                    //MessageBox.Show(Convoluter.Convolute(kernelPixels, kernel).ToString());
                    int resultPixelR = (int)Convoluter.Convolute(kernelPixelsR, kernel, 3, 1.0f/9);
                    int resultPixelG = (int)Convoluter.Convolute(kernelPixelsG, kernel, 3, 1.0f / 9);
                    int resultPixelB = (int)Convoluter.Convolute(kernelPixelsB, kernel, 3, 1.0f / 9);

                    //MessageBox.Show(resultPixel.ToString());

                    if (resultPixelR < 0) resultPixelR = 0;
                    else if (resultPixelR > 255) resultPixelR = 255;
                    if (resultPixelG < 0) resultPixelG = 0;
                    else if (resultPixelG > 255) resultPixelG = 255;
                    if (resultPixelB < 0) resultPixelB = 0;
                    else if (resultPixelB > 255) resultPixelB = 255;

                    System.Drawing.Color resultColor = System.Drawing.Color.FromArgb(resultPixelR,resultPixelG, resultPixelB);
                    outputImageBMP.SetPixel(x, y, resultColor);

                    //System.Drawing.Color w1 = startImageBMP.GetPixel(x - 1, y);
                    //System.Drawing.Color w2 = startImageBMP.GetPixel(x + 1, y);
                    //System.Drawing.Color w3 = startImageBMP.GetPixel(x, y - 1);
                    //System.Drawing.Color w4 = startImageBMP.GetPixel(x, y + 1);
                    //System.Drawing.Color w = startImageBMP.GetPixel(x, y);

                    //int x1 = w1.R;
                    //int x2 = w2.R;
                    //int x3 = w3.R;
                    //int x4 = w4.R;
                    //int xg = w.R;

                    //int xb = (int)(a[0] * xg);
                    //xb = (int)(xb + a[1] * x1 + a[2] * x2 + a[3] * x3 + a[4] * x4);
                    //if (xb < 0) xb = 0;
                    //else if (xb > 255) xb = 255;
                    //System.Drawing.Color wb = System.Drawing.Color.FromArgb(xb, xb, xb);
                    //outputImageBMP.SetPixel(x, y, wb);
                }
            }
            return Converter.BitmapToImageSource(outputImageBMP);
        }
    }
}
