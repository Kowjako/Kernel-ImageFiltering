using System;
using System.Collections.Generic;
using draw = System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KernelFilters.NonLinearFilters
{
    class Solarize : IFilter
    {
        int dimension, threshold;
        public ImageSource Filterize(ImageSource sourceImage)
        {
            draw.Bitmap startImageBMP = null, outputImageBMP = null;

            startImageBMP = Converter.ImageSourceToBitmap(sourceImage);
            outputImageBMP = new draw.Bitmap(startImageBMP.Width, startImageBMP.Height);

            threshold = 128;

            /* Obliczanie średnich */
            for (int j = 0; j < startImageBMP.Height; j++)
            {
                for (int i = 0; i < startImageBMP.Width; i++)
                {
                    uint oldPixel = (uint)startImageBMP.GetPixel(i, j).ToArgb();

                    uint R = ((0x00FF0000) & (uint)oldPixel) >> 16;
                    uint G = ((0x0000FF00) & (uint)oldPixel) >> 8;
                    uint B = ((0x000000FF) & (uint)oldPixel);

                    if (R > threshold)
                        R = 255 - R;
                    if (G > threshold)
                        G = 255 - G;
                    if (B > threshold)
                        B = 255 - B;

                    uint newPixel = (0xFF000000) | R << 16 | G << 8 | B;
                    outputImageBMP.SetPixel(i, j, draw.Color.FromArgb((int)newPixel));
                }
            }

            return Converter.BitmapToImageSource(outputImageBMP);
        }
    }
}
