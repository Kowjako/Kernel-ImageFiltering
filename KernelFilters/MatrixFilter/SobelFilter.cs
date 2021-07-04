﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KernelFilters.MatrixFilter
{
    class SobelFilter : IFilter, IKernelFilter
    {
        private float[,] kernel =
        {
            {1, 2, 1},
            {0, 0, 0},
            {-1, -2, -1}
        };

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
