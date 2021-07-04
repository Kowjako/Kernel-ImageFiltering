using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelFilters
{
    static class Convoluter
    {
        public static float Convolute(int[,] image, int[,] kernel, float scale = 1)
        {
            int result = 0;
            for(int i=0;i<image.Length/3 - 1;i++)
            {
                for(int j=0;j<image.Length/3 - 1;j++)
                {
                    result += image[i, j] * kernel[i, j];
                }
            }
            return scale * result;
        }
    }
}
