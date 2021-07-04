﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Media;

namespace KernelFilters.MatrixFilter
{
    class NormalizedBoxBlur : IKernelFilter, IFilter
    {
        private float[,] kernel =
        {
            {1, 1, 1},
            {1, 1, 1},
            {1, 1, 1}
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
            MatrixConvoluator mc = new MatrixConvoluator(this, sourceImage, 3, 9);
            return mc.Convoluate();
        }
    }
}
