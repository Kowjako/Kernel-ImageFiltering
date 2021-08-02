using KernelFilters.MatrixFilter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KernelFilters
{
    class MatrixConvoluator
    {
        bool isSobel = false;
        public float[,] kernel { get; set; }
        private float[,] kernel2;
        private ImageSource image;
        private int kernelEdge;
        private float kernelScale;

        public MatrixConvoluator(IKernelFilter filter, ImageSource image, int kernelEdge, int kernelScale)
        {
            this.kernel = filter.kernel;
            if(filter is SobelFilter)
            {
                kernel2 = ((SobelFilter)filter).kernel2;
                isSobel = true;
            }
            this.image = image;
            this.kernelEdge = kernelEdge;
            this.kernelScale = kernelScale;
        }

        public ImageSource Convoluate()
        {
            Bitmap startImageBMP = null, outputImageBMP = null;

            startImageBMP = Converter.ImageSourceToBitmap(image);
            outputImageBMP = new Bitmap(startImageBMP.Width, startImageBMP.Height);

            int[,] kernelPixelsR = new int[kernelEdge, kernelEdge];  //tworzenie macierz ZxZ
            int[,] kernelPixelsG = new int[kernelEdge, kernelEdge];
            int[,] kernelPixelsB = new int[kernelEdge, kernelEdge];

            int start = 0;
            int kernelstart = 0;
            if (kernelEdge == 3)
            {
                start = 1;
                kernelstart = 1;
            }
            if (kernelEdge == 5)
            {
                start = 2;
                kernelstart = 2;
            }

            for (int x = start; x < startImageBMP.Width - kernelstart; x++)
            {
                for (int y = start; y < startImageBMP.Height - kernelstart; y++)
                {
                    for (int i = -kernelstart; i <= kernelstart; i++)
                    {
                        for (int j = -kernelstart; j <= kernelstart; j++)
                        {
                            kernelPixelsR[i + kernelstart, j + kernelstart] = startImageBMP.GetPixel(x + j, y + i).R; //uzupelnienie macierzy 3x3 lub 5x5 
                            kernelPixelsG[i + kernelstart, j + kernelstart] = startImageBMP.GetPixel(x + j, y + i).G;
                            kernelPixelsB[i + kernelstart, j + kernelstart] = startImageBMP.GetPixel(x + j, y + i).B;
                        }
                    }

                    int resultPixelR = (int)Convoluter.Convolute(kernelPixelsR, kernel, kernelEdge, 1.0f / kernelScale); //mnozenie macierzy * kernel
                    int resultPixelG = (int)Convoluter.Convolute(kernelPixelsG, kernel, kernelEdge, 1.0f / kernelScale);
                    int resultPixelB = (int)Convoluter.Convolute(kernelPixelsB, kernel, kernelEdge, 1.0f / kernelScale);

                    /*If filter is Sobel*/
                    if (isSobel)
                    {
                        int resultPixelR1 = (int)Convoluter.Convolute(kernelPixelsR, kernel2, kernelEdge, 1.0f / kernelScale);
                        int resultPixelG1 = (int)Convoluter.Convolute(kernelPixelsG, kernel2, kernelEdge, 1.0f / kernelScale);
                        int resultPixelB1 = (int)Convoluter.Convolute(kernelPixelsB, kernel2, kernelEdge, 1.0f / kernelScale);

                        int resR = (int)Math.Sqrt(resultPixelR * resultPixelR + resultPixelR1 * resultPixelR1);
                        int resB = (int)Math.Sqrt(resultPixelB * resultPixelB + resultPixelB1 * resultPixelB1);
                        int resG = (int)Math.Sqrt(resultPixelG * resultPixelG + resultPixelG1 * resultPixelG1);

                        if (resB < 0) resB = 0; //kontrola przepelnienia
                        else if (resB > 255) resB = 255;
                        if (resR < 0) resR = 0;
                        else if (resR > 255) resR = 255;
                        if (resG < 0) resG = 0;
                        else if (resG > 255) resG = 255;

                        System.Drawing.Color resultColor = System.Drawing.Color.FromArgb(resR, resG, resB); //ustawienie kolora po konwolucji
                        outputImageBMP.SetPixel(x, y, resultColor);
                    }
                    /* Another filters */
                    else
                    {
                        if (resultPixelR < 0) resultPixelR = 0; //kontrola przepelnienia
                        else if (resultPixelR > 255) resultPixelR = 255;
                        if (resultPixelG < 0) resultPixelG = 0;
                        else if (resultPixelG > 255) resultPixelG = 255;
                        if (resultPixelB < 0) resultPixelB = 0;
                        else if (resultPixelB > 255) resultPixelB = 255;
                        System.Drawing.Color resultColor = System.Drawing.Color.FromArgb(resultPixelR, resultPixelG, resultPixelB); //ustawienie kolora po konwolucji
                        outputImageBMP.SetPixel(x, y, resultColor);
                    }
                }
            }
            return Converter.BitmapToImageSource(outputImageBMP);
        }
    }
}
