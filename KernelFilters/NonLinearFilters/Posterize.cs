using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KernelFilters.NonLinearFilters
{
    class Posterize : IFilter
    {
        public ImageSource Filterize(ImageSource sourceImage)
        {
            Bitmap startImageBMP = Converter.ImageSourceToBitmap(sourceImage);
            Bitmap outputImageBMP = new Bitmap(startImageBMP.Width, startImageBMP.Height);

            int posterizeLevel = 3;
            int numOfAreas = 256 / posterizeLevel;
            int numOfValues = 255 / (posterizeLevel - 1);
            List<uint> colors = new List<uint>(3);
            List<uint> newColors = new List<uint>(3);

            for (int j = 0; j < startImageBMP.Height; j++)
            {
                for (int i = 0; i < startImageBMP.Width; i++)
                {
                    uint oldPixel = (uint)startImageBMP.GetPixel(i, j).ToArgb();

                    uint R = ((0x00FF0000) & (uint)oldPixel) >> 16;
                    uint G = ((0x0000FF00) & (uint)oldPixel) >> 8;
                    uint B = ((0x000000FF) & (uint)oldPixel);

                    colors.Add(R);  colors.Add(G);  colors.Add(B);

                    foreach(var item in colors)
                    {
                        long colorAreaFloat = item / numOfAreas;
                        int colorArea = (int)colorAreaFloat;
                        if (colorArea > colorAreaFloat)
                            colorArea--;
                        int newColorFloat = numOfValues * colorArea;
                        int newColor = (int)newColorFloat;
                        if (newColor > newColorFloat)
                            newColor--;
                        newColors.Add((uint)newColor);
                    }

                    uint newPixel = (0xFF000000) | newColors[0] << 16 | newColors[1] << 8 | newColors[2];
                    outputImageBMP.SetPixel(i, j, System.Drawing.Color.FromArgb((int)newPixel));

                    newColors.Clear();
                    colors.Clear();           
                }
            }
            return Converter.BitmapToImageSource(outputImageBMP);
        }
    }
}
