using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace KernelFilters.FitersWithoutKernel
{
    class ColorAccent : IFilter
    {
        float Hue = 0, Radius = 0, h1, h2;
        List<float> hues = new List<float>(3);
        public ImageSource Filterize(ImageSource sourceImage)
        {
            Bitmap startImageBMP = Converter.ImageSourceToBitmap(sourceImage);
            Bitmap outputImageBMP = new Bitmap(startImageBMP.Width, startImageBMP.Height);

            System.Drawing.Color accentedColor = System.Drawing.Color.Empty;

            using (ColorDialog cd = new ColorDialog())
            {
                if (cd.ShowDialog() == DialogResult.OK)
                    accentedColor = cd.Color;
            }

            Hue = RGBtoHSV(accentedColor);
            
            Radius = 40;
            h1 = (Hue - Radius / 2 + 360) % 360;
            h2 = (Hue + Radius / 2 + 360) % 360;

            var HueIntensity = 0.0f;
            System.Drawing.Color newPixel, color;
            for (int j = 0; j < startImageBMP.Height; j++)
            {
                for (int i = 0; i < startImageBMP.Width; i++)
                {
                    color = startImageBMP.GetPixel(i, j);
                    HueIntensity = RGBtoHSV(color);

                    int grayScale = (color.R + color.G + color.B) / 3;
                    newPixel = System.Drawing.Color.FromArgb(255, grayScale, grayScale, grayScale);
                    if (h1 <= h2)
                    {
                        if (HueIntensity >= h1 && HueIntensity <= h2)
                            newPixel = color;
                    }
                    if (h1 > h2)
                    {
                        if (HueIntensity >= h1 || HueIntensity <= h2)
                            newPixel = color;
                    }
                    outputImageBMP.SetPixel(i, j, newPixel);
                }
            }

            return Converter.BitmapToImageSource(outputImageBMP);
        }

        private float RGBtoHSV(System.Drawing.Color accentedColor)
        {
            float HueTmp;
            hues.Add(accentedColor.R / 255);
            hues.Add(accentedColor.G / 255);
            hues.Add(accentedColor.B / 255);

            float minValue = hues.Min(), maxValue = hues.Max();

            /* Obliczenie Hue z koloru RGB */
            if (hues[0] == maxValue)
                HueTmp = (hues[1] - hues[2]) / (maxValue - minValue);
            if (hues[1] == maxValue)
                HueTmp = 2.0f + (hues[2] - hues[0]) / (maxValue - minValue);
            else
                HueTmp = 4.0f + (hues[0] - hues[1]) / (maxValue - minValue);

            hues.Clear();
            return HueTmp;
        }
    }
}
