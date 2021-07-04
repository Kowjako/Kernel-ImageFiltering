using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace KernelFilters.FitersWithoutKernel
{
    class SepiaFilter : IFilter
    {
        Bitmap startImageBMP = null, outputImageBMP = null;

        public ImageSource Filterize(ImageSource sourceImage)
        {
            startImageBMP = Converter.ImageSourceToBitmap(sourceImage);
            outputImageBMP = new Bitmap(startImageBMP.Width, startImageBMP.Height);

            for (int j = 0; j < startImageBMP.Height; j++)
            {
                for (int i = 0; i < startImageBMP.Width; i++)
                {
                    UInt32 pixel = (UInt32)(startImageBMP.GetPixel(i, j).ToArgb());

                    float R = (pixel & 0x00FF0000) >> 16;   /* A - 1 byte, R - 1 byte, G - 1 byte, B - 1 byte + logiczne AND oraz trzeba przesunac w koniec (16 bitow w prawo)*/
                    float G = (pixel & 0x0000FF00) >> 8;
                    float B = (pixel & 0x000000FF);

                    float newR, newG, newB;

                    newR = (float)Math.Min((R * .393 + G * .769 + B * .189), 255.0);
                    newG = (float)Math.Min((R * .349 + G * .686 + B * .168), 255.0);
                    newB = (float)Math.Min((R * .272 + G * .534 + B * .131), 255.0);

                    UInt32 newPixel = 0xFF000000 | ((UInt32)newR << 16) | ((UInt32)newG << 8) | (UInt32)newB; /* logiczne dodawanie do utworzenia pixela */
                    outputImageBMP.SetPixel(i, j, System.Drawing.Color.FromArgb((int)newPixel));
                }
            }
            return Converter.BitmapToImageSource(outputImageBMP);

        }
    }
}
