using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KernelFilters.NonLinearFilters
{
    class Kuwahara : IFilter
    {
        List<byte> regionR = new List<byte>(25);
        List<byte> regionG = new List<byte>(25);
        List<byte> regionB = new List<byte>(25);

        public ImageSource Filterize(ImageSource sourceImage)
        {
            Bitmap startImageBMP = Converter.ImageSourceToBitmap(sourceImage);
            System.Drawing.Color pixel, resultPixel;

            for (int j = 2; j < startImageBMP.Height - 2; j++)
            {
                for (int i = 2; i < startImageBMP.Width - 2; i++)
                {
                    pixel = startImageBMP.GetPixel(i, j);
                    regionR.Add(pixel.R);
                    regionB.Add(pixel.B);
                    regionG.Add(pixel.G);
                    ProcessRegion();
                }
            }
        }

        private byte Process(List<System.Drawing.Color> kernelKuwahara)
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    region1R.Add(kernelKuwahara[i * 3 + j].R);
                    region1G.Add(kernelKuwahara[i * 3 + i].G);
                    region1B.Add(kernelKuwahara[i * 3 + i].B);
                }
        }
    }
}
