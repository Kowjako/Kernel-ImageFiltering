using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelFilters
{
    interface IKernelFilter : IFilter
    {
        float[,] kernel { get; }
        int kernelEdge { get; }
    }
}
