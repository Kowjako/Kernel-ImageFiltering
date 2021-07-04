using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KernelFilters.MatrixFilter
{
    class NormalizedBoxBlur : IFilter
    {
        private int[,] kernel =
        {
            {-1, -1, -1 },
            {-1, 8, -1 },
            {-1, -1, -1}
        };

        public ImageSource Filterize(ImageSource sourceImage)
        {
            Bitmap startImageBMP = null, outputImageBMP = null;

            startImageBMP = Converter.ImageSourceToBitmap(sourceImage);
            outputImageBMP = new Bitmap(startImageBMP.Width, startImageBMP.Height);

            int[,] kernelPixels = new int[3, 3];

            for (int x = 1; x < startImageBMP.Width - 1; x++)
            {
                for (int y = 1; y < startImageBMP.Height - 1; y++)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            kernelPixels[i + 1, j + 1] = startImageBMP.GetPixel(x + j, y + i).ToArgb();
                        }
                    }

                    int resultPixel = (int)Convoluter.Convolute(kernelPixels, kernel);
                    if (resultPixel > 255) resultPixel = 255;
                    if (resultPixel < 0) resultPixel = 0;

                    System.Drawing.Color resultColor = System.Drawing.Color.FromArgb(resultPixel);
                    outputImageBMP.SetPixel(x, y, resultColor);
                }
            }
            return Converter.BitmapToImageSource(outputImageBMP);
        }
    }
}
