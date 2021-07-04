using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KernelFilters
{
    static class Convoluter
    {
        public static float Convolute(int[,] image, float[,] kernel, int kernelEdge, float scale = 1)
        {
            float result = 0;
            for (int i = 0; i < image.Length / kernelEdge; i++) 
            {
                for (int j = 0; j < image.Length / kernelEdge; j++) 
                {
                    result += (image[i, j] * kernel[i, j]);
                }
            }
            return result *scale;
        }
    }
}
