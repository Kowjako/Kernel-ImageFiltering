using KernelFilters.FitersWithoutKernel;
using System;
using System.Drawing;
using System.Windows.Media;

namespace KernelFilters.NonLinearFilters
{
    class Grassfire : IFilter
    {
        private int width, heigth;
        public ImageSource Filterize(ImageSource sourceImage)
        {
            Random r = new Random();
            Bitmap startImageBMP = Converter.ImageSourceToBitmap(sourceImage);
            width = startImageBMP.Width;
            heigth = startImageBMP.Height;
            byte[] colors = new byte[width * heigth];
            byte newColor, R, G, B;

            for (int j = 0; j < startImageBMP.Height; j++)
            {
                for (int i = 0; i < startImageBMP.Width; i++)
                {
                    colors[j * width + i] = (byte)((startImageBMP.GetPixel(i, j).R + startImageBMP.GetPixel(i, j).B + startImageBMP.GetPixel(i, j).G) / 3);
                    if (colors[j * width + i] > 200) colors[j * width + i] = 255;
                    else colors[j * width + i] = 0;
                }
            }

            for (int y = 0; y < startImageBMP.Height; y++)
            {
                for (int x = 0; x < startImageBMP.Width; x++)
                {
                    if (colors[y * width + x] == 255)
                    {
                        newColor = (byte)(r.Next(0, 221) + 30);
                        GrassFire(ref colors, x, y, newColor);
                    }
                }
            }

            for (int i = 0; i < startImageBMP.Height; i++)
            {
                for (int j = 0; j < startImageBMP.Width; j++)
                {
                    R = G = B = colors[i * width + j];
                    uint newPixel = 0xFF000000 | (uint)R << 16 | (uint)G << 8 | B;
                    startImageBMP.SetPixel(j, i, System.Drawing.Color.FromArgb((int)newPixel));
                }
            } 

            return Converter.BitmapToImageSource(startImageBMP);
        }

        private void GrassFire(ref byte[] img, int x, int y, byte newColor)
        {
            img[y * width + x] = newColor;
            if (x + 1 < width && img[y * width + x + 1] == 255) GrassFire(ref img, x + 1, y, newColor);
            if (x - 1 > 0 && img[y * width + x - 1] == 255) GrassFire(ref img, x - 1, y, newColor);
            if (y + 1 < heigth && img[(y + 1) * width + x] == 255) GrassFire(ref img, x, y + 1, newColor);
            if (y - 1 >= 0 && img[(y - 1) * width + x] == 255) GrassFire(ref img, x, y - 1, newColor);
            return;
        }
    }
}
