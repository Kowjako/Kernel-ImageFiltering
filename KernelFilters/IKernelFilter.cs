using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelFilters
{
    interface IKernelFilter
    {
        float[,] kernel { get; }
    }
}
