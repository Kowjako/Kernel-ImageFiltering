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
        private float[,] kernel;
        private ImageSource image;
        private int kernelEdge;
        private float kernelScale;

        public MatrixConvoluator(IKernelFilter filter, ImageSource image, int kernelEdge, int kernelScale)
        {
            this.kernel = filter.kernel;
            this.image = image;
            this.kernelEdge = kernelEdge;
            this.kernelScale = kernelScale;
        }

        public ImageSource Convoluate()
        {
            Bitmap startImageBMP = null, outputImageBMP = null;

            startImageBMP = Converter.ImageSourceToBitmap(image);
            outputImageBMP = new Bitmap(startImageBMP.Width, startImageBMP.Height);

            int[,] kernelPixelsR = new int[kernelEdge, kernelEdge];
            int[,] kernelPixelsG = new int[kernelEdge, kernelEdge];
            int[,] kernelPixelsB = new int[kernelEdge, kernelEdge];

            //int x = 0, y = 0;
            //if (kernelEdge == 3) x = y = 1;
            //if (kernelEdge == 5) x = y = 2;

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

                    int resultPixelR = (int)Convoluter.Convolute(kernelPixelsR, kernel, kernelEdge, 1.0f / kernelScale);
                    int resultPixelG = (int)Convoluter.Convolute(kernelPixelsG, kernel, kernelEdge, 1.0f / kernelScale);
                    int resultPixelB = (int)Convoluter.Convolute(kernelPixelsB, kernel, kernelEdge, 1.0f / kernelScale);

                    if (resultPixelR < 0) resultPixelR = 0;
                    else if (resultPixelR > 255) resultPixelR = 255;
                    if (resultPixelG < 0) resultPixelG = 0;
                    else if (resultPixelG > 255) resultPixelG = 255;
                    if (resultPixelB < 0) resultPixelB = 0;
                    else if (resultPixelB > 255) resultPixelB = 255;

                    System.Drawing.Color resultColor = System.Drawing.Color.FromArgb(resultPixelR, resultPixelG, resultPixelB);
                    outputImageBMP.SetPixel(x, y, resultColor);
                }
            }
            return Converter.BitmapToImageSource(outputImageBMP);
        }
    }
}
