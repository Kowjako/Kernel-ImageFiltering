using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KernelFilters.Noises
{
    class SaltPepperNoise : INoise
    {
        private Random randomizer = new Random();
        public int Scale { get; private set; }

        public SaltPepperNoise(int scale)
        {
            this.Scale = scale;
        }

        public ImageSource Noising(ImageSource sourceImage)
        {
            Bitmap startImageBMP = null, outputImageBMP = null;
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

                    if (randomizer.Next(0, 101) > 100 - Scale)
                    {
                        int resultNoisedColor = randomizer.Next(0, 256) > 127 ? 255 : 0;
                        R = G = B = resultNoisedColor;
                    }

                    UInt32 newPixel = 0xFF000000 | ((UInt32)R << 16) | ((UInt32)G << 8) | (UInt32)B; 
                    outputImageBMP.SetPixel(i, j, System.Drawing.Color.FromArgb((int)newPixel));
                }
            }
            return Converter.BitmapToImageSource(outputImageBMP);
        }
    }
}
