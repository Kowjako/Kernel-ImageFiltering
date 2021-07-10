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
        private int scale, start;

        public Median5Filter(int scale = 5)
        {
            this.scale = scale;
        }

        public ImageSource Filterize(ImageSource sourceImage)
        {
            start = scale == 5 ? 2 : 1;
            Bitmap startImageBMP = null, outputImageBMP = null;

            startImageBMP = Converter.ImageSourceToBitmap(sourceImage);
            outputImageBMP = new Bitmap(startImageBMP.Width, startImageBMP.Height);

            List<int> kernelPixels = new List<int>();

            for (int x = start; x < startImageBMP.Width - start; x++)
            {
                for (int y = start; y < startImageBMP.Height - start; y++)
                {
                    for (int i = -start; i <= start; i++)
                    {
                        for (int j = -start; j <= start; j++)
                        {
                            kernelPixels.Add(startImageBMP.GetPixel(x + j, y + i).ToArgb());
                        }
                    }

                    kernelPixels.Sort();

                    System.Drawing.Color resultColor = System.Drawing.Color.FromArgb(kernelPixels[kernelPixels.Count/2]);
                    outputImageBMP.SetPixel(x, y, resultColor);

                    kernelPixels.Clear();
                }
            }
            return Converter.BitmapToImageSource(outputImageBMP);
        }
    }
}
