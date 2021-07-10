using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KernelFilters.NonLinearFilters
{
    class MirrorFilter : IFilter
    {
        public ImageSource Filterize(ImageSource sourceImage)
        {
            Bitmap startImageBMP = null;
            startImageBMP = Converter.ImageSourceToBitmap(sourceImage);
            startImageBMP.RotateFlip(RotateFlipType.RotateNoneFlipX);
            return Converter.BitmapToImageSource(startImageBMP);
        }
    }
}
