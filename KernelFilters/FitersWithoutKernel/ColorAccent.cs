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

            float Hue = 0;
            List<float> hues = new List<float>(3);
            hues.Add(accentedColor.R / 255);
            hues.Add(accentedColor.G / 255);
            hues.Add(accentedColor.B / 255);
            float minValue = hues.Min(), maxValue = hues.Max();

            if (hues[0] == maxValue) Hue = (hues[1] - hues[2]) / (maxValue - minValue);
            if (hues[1] == maxValue) Hue = 2.0f + (hues[2] - hues[0]) / (maxValue - minValue);
            else Hue = 4.0f + (hues[0] - hues[1]) / (maxValue - minValue);

            

            return null;
        }
    }
}
