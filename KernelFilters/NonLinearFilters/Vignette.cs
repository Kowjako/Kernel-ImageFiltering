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


            return Converter.BitmapToImageSource(startImageBMP);
        }

        private double EuclideanDistance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow((double)(a.X - b.X), 2) + Math.Pow((double)(a.Y - b.Y), 2));
        }

        private double GetMaxDistanceFromConrners(draw.Bitmap image, Point center)
        {
            /* Tworzenie pointów na rogach obrazku */
            Point[] points = new Point[4];
            points[0] = new Point(0, 0);
            points[1] = new Point(image.Width, 0);
            points[2] = new Point(0, image.Height);
            points[3] = new Point(image.Width, image.Height);

            /* Obliczenie najbardziej oddalonego pointu od centrum */
            double maxDistance = 0;
            for (int i = 0; i < 4; i++)
            {
                double distance = EuclideanDistance(points[i], center);
                if (distance > maxDistance)
                    maxDistance = distance;
            }
            return maxDistance;
        }

        void GenerateGradient(int[][] matrix)
        {
            Point fisrtPoint = new Point(matrix.GetLength(1) / 2, matrix.GetLength(0) / 2);   /* GetLength(1) - ilość kolumn, GetLength(0) - ilość wierszy */
        }
    }
}
