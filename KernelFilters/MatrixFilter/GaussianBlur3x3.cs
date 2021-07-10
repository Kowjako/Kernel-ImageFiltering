using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KernelFilters.MatrixFilter
{
    class GaussianBlur3x3 : IKernelFilter
    {
        private float[,] kernel =
        {
            {1, 2, 1},
            {2, 4, 2},
            {1, 2, 1}
        };

        public int kernelEdge => 3;

        float[,] IKernelFilter.kernel
        {
            get
            {
                return kernel;
            }
        }

        public ImageSource Filterize(ImageSource sourceImage)
        {
            MatrixConvoluator mc = new MatrixConvoluator(this, sourceImage, 3, 16);
            return mc.Convoluate();
        }
    }
}
