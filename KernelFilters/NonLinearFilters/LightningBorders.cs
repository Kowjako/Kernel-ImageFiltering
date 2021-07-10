using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using KernelFilters.MatrixFilter;
namespace KernelFilters.NonLinearFilters
{
    class LightningBorders : IFilter
    {
        public ImageSource Filterize(ImageSource sourceImage)
        {
            Median5Filter median = new Median5Filter(3);
            var afterMedian = median.Filterize(sourceImage);

            ExtensionFilter sobel = new ExtensionFilter();
            var afterSobel = sobel.Filterize(afterMedian);

            Bitmap startImageBMP = Converter.ImageSourceToBitmap(afterSobel);
            Bitmap outputImageBMP = new Bitmap(startImageBMP.Width, startImageBMP.Height);

            List<int> kernelPixels = new List<int>();

            //Maximum filter realiztion//
            for (int x = 1; x < startImageBMP.Width - 1; x++)
            {
                for (int y = 1; y < startImageBMP.Height - 1; y++)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            kernelPixels.Add(startImageBMP.GetPixel(x + j, y + i).ToArgb());
                        }
                    }
                    System.Drawing.Color resultColor = System.Drawing.Color.FromArgb(kernelPixels.Max()); //MaximumFilter
                    outputImageBMP.SetPixel(x, y, resultColor);
                    kernelPixels.Clear();
                }
            }           
            return Converter.BitmapToImageSource(outputImageBMP);
        }
    }
}   
