using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KernelFilters.MatrixFilter
{
    class Emboss : IKernelFilter
    {
        private float[,] kernel =
        {
            {1, 1, 0},
            {1, 1, -1},
            {0, -1, -1}
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
            MatrixConvoluator mc = new MatrixConvoluator(this, sourceImage, 3, 1);
            return mc.Convoluate();
        }
    }
}
