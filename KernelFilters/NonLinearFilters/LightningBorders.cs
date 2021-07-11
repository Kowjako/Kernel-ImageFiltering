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

            SobelFilter sobel = new SobelFilter();
            var afterSobel = sobel.Filterize(afterMedian);

            Bitmap startImageBMP = Converter.ImageSourceToBitmap(afterSobel);
            Bitmap outputImageBMP = new Bitmap(startImageBMP.Width, startImageBMP.Height);

            List<int> kernelPixels = new List<int>();
            return Converter.BitmapToImageSource(startImageBMP);
        }
    }
}   
