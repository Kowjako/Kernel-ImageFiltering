using System;
using System.Collections.Generic;
using draw = System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Drawing.Drawing2D;

namespace KernelFilters.NonLinearFilters
{
    class Vignette : IFilter
    {
        public ImageSource Filterize(ImageSource sourceImage)
        {
            draw.Bitmap startImageBMP = Converter.ImageSourceToBitmap(sourceImage);
            draw.Bitmap outputImageBMP = new draw.Bitmap(startImageBMP.Width, startImageBMP.Height);
            


            return Converter.BitmapToImageSource(startImageBMP);
        }
    }
}
