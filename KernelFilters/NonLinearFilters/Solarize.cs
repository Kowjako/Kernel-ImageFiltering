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
        byte r, g, b;
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
                    draw.Color color = startImageBMP.GetPixel(i, j);
                    draw.Color newColor = draw.Color.Empty;

                    if (color.R > threshold)
                        r = (byte)(255 - color.R);
                    else r = color.R;
                    if (color.G > threshold)
                        g = (byte)(255 - color.G);
                    else g = color.G;
                    if (color.B > threshold)
                        b = (byte)(255 - color.B);
                    else b = color.B;

                    outputImageBMP.SetPixel(i, j, draw.Color.FromArgb(0, r, g, b));
                }
            }

            return Converter.BitmapToImageSource(outputImageBMP);
        }
    }
}
