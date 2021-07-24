using System;
using System.Collections.Generic;
using draw = System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Drawing.Drawing2D;
using System.Windows;

namespace KernelFilters.NonLinearFilters
{
    class Vignette : IFilter
    {
        public ImageSource Filterize(ImageSource sourceImage)
        {
            draw.Bitmap startImageBMP = Converter.ImageSourceToBitmap(sourceImage);
            draw.Bitmap outputImageBMP = new draw.Bitmap(startImageBMP.Width, startImageBMP.Height);
            double radius = 1;
            double cx = (double)startImageBMP.Width / 2, cy = (double)startImageBMP.Height / 2;
            double maxDistance = radius * EuclideanDistance(new Point(0, 0), new Point(cx, cy));
            double temp;

            for (int j = 0; j < startImageBMP.Height; j++)
            {
                for (int i = 0; i < startImageBMP.Width; i++)
                {
                    temp = MyCosinus(EuclideanDistance(new Point(cx, cy), new Point(i, j)) / maxDistance);
                    temp = Math.Pow(temp, 3.5);
                    outputImageBMP.SetPixel(i, j, draw.Color.FromArgb(255, (byte)(startImageBMP.GetPixel(i, j).R * temp), (byte)(startImageBMP.GetPixel(i, j).G * temp), (byte)(startImageBMP.GetPixel(i, j).B * temp)));
                }
            }
            return Converter.BitmapToImageSource(outputImageBMP);
        }

        private double MyCosinus(double x)
        {
            x += 1.5707;
            if (x > Math.PI)
                x -= 2 * Math.PI;
            if(x < 0)
                return 1.27323954 * x + 0.405284735 * x * x;
            else
                return 1.27323954 * x - 0.405284735 * x * x;
        }

        private double EuclideanDistance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow((double)(a.X - b.X), 2) + Math.Pow((double)(a.Y - b.Y), 2));
        }
    }
}
