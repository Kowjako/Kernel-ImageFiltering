using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KernelFilters.NonLinearFilters
{
    class GlassEffect : IFilter
    {
        Random rnd = new Random();
        public ImageSource Filterize(ImageSource sourceImage)
        {
            Bitmap startImageBMP = null, outputImageBMP = null;

            startImageBMP = Converter.ImageSourceToBitmap(sourceImage);
            outputImageBMP = new Bitmap(startImageBMP.Width, startImageBMP.Height);

            /* Obliczanie średnich */
            for (int j = 0; j < startImageBMP.Width; j++)
            {
                for (int i = 0; i < startImageBMP.Height; i++)
                {
                    int xx = (startImageBMP.Width + j + UniformRandom(-3, 3)) % startImageBMP.Width;
                    int yy = (startImageBMP.Height + i + UniformRandom(-3, 3)) % startImageBMP.Height;

                    System.Drawing.Color resultColor = startImageBMP.GetPixel(xx, yy);
                    outputImageBMP.SetPixel(j, i, resultColor);
                }
            }

            return Converter.BitmapToImageSource(outputImageBMP);
        }

        private int UniformRandom(int a, int b)
        { 
            return (int)(a + rnd.NextDouble() * (b - a));
        }
    }
}
