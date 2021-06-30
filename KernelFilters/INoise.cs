using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KernelFilters
{
    interface INoise
    {
        ImageSource Noising(ImageSource sourceImage);
    }
}
