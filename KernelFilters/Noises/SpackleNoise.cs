using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KernelFilters.Noises
{
    class SpackleNoise : INoise
    {
        public int NoiseScale { get; private set; }
        private Random randomizer;

        public SpackleNoise(int noiseScale)
        {
            randomizer = new Random();
            NoiseScale = noiseScale;
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

                    if (randomizer.Next(0, 101) > 100 - NoiseScale)
                    {
                        var noise = (randomizer.NextDouble() - 0.5f) * 2 * Math.Sqrt(0.04f);
                        R += R * (float)noise;
                        G += G * (float)noise;
                        B += B * (float)noise;
                        if (R > 255) R = 255;
                        if (G > 255) G = 255;
                        if (B > 255) B = 255;
                    }

                    UInt32 newPixel = 0xFF000000 | ((UInt32)R << 16) | ((UInt32)G << 8) | (UInt32)B;
                    outputImageBMP.SetPixel(i, j, System.Drawing.Color.FromArgb((int)newPixel));
                }
            }
            return Converter.BitmapToImageSource(outputImageBMP);
        }
    }
}
