using System;
using System.Collections.Generic;
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

        public MatrixConvoluator(float[,] kernel, ImageSource image, int kernelEdge, float kernelScale)
        {
            this.kernel = kernel;
            this.image = image;
            this.kernelEdge = kernelEdge;
            this.kernelScale = kernelScale;

        }
    }
}
