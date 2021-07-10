using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KernelFilters.NonLinearFilters
{
    class Median5Filter : IFilter
    {
        public ImageSource Filterize(ImageSource sourceImage)
        {
            Bitmap startImageBMP = null, outputImageBMP = null;

            startImageBMP = Converter.ImageSourceToBitmap(sourceImage);
            outputImageBMP = new Bitmap(startImageBMP.Width, startImageBMP.Height);

            List<int> kernelPixels = new List<int>(25);

            for (int x = 2; x < startImageBMP.Width - 2; x++)
            {
                for (int y = 2; y < startImageBMP.Height - 2; y++)
                {
                    for (int i = -2; i <= 2; i++)
                    {
                        for (int j = -2; j <= 2; j++)
                        {
                            kernelPixels.Add(startImageBMP.GetPixel(x + j, y + i).ToArgb());
                        }
                    }

                    kernelPixels.Sort();

                    System.Drawing.Color resultColor = System.Drawing.Color.FromArgb(kernelPixels[12]);
                    outputImageBMP.SetPixel(x, y, resultColor);

                    kernelPixels.Clear();
                }
            }
            return Converter.BitmapToImageSource(outputImageBMP);
        }
    }
}
