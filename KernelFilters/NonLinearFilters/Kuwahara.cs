using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KernelFilters.NonLinearFilters
{
    class Kuwahara : IFilter        /* KUWAHARA 5x5 FILTER */
    {
        List<int> regionR = new List<int>(25);
        List<int> regionG = new List<int>(25);
        List<int> regionB = new List<int>(25);

        List<int> region1 = new List<int>();    /* mając kernel 5x5 mamy 4 regiony 3x3 */
        List<int> region2 = new List<int>();
        List<int> region3 = new List<int>();
        List<int> region4 = new List<int>();
        List<KeyValuePair<double, double>> meanAndVariance = new List<KeyValuePair<double, double>>();

        public ImageSource Filterize(ImageSource sourceImage)
        {
            Bitmap startImageBMP = Converter.ImageSourceToBitmap(sourceImage);
            Bitmap outputImageBMP = new Bitmap(startImageBMP.Width, startImageBMP.Height);
            System.Drawing.Color resultPixel;

            for (int x = 2; x < startImageBMP.Width - 2; x++)
            {
                for (int y = 2; y < startImageBMP.Height - 2; y++)
                {
                    for (int i = -2; i <= 2; i++)
                    {
                        for (int j = -2; j <= 2; j++)
                        {
                            regionR.Add(startImageBMP.GetPixel(x + j, y + i).R);
                            regionG.Add(startImageBMP.GetPixel(x + j, y + i).G);
                            regionB.Add(startImageBMP.GetPixel(x + j, y + i).B);
                        }
                    }
                    resultPixel = System.Drawing.Color.FromArgb(255, ProcessRegion(regionR), ProcessRegion(regionG), ProcessRegion(regionB));
                    regionR.Clear();
                    regionB.Clear();
                    regionG.Clear();
                    outputImageBMP.SetPixel(x,y, resultPixel);
                }
            }

            return Converter.BitmapToImageSource(outputImageBMP);
        }

        private int ProcessRegion(List<int> regionR)
        {
            var counter = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    region1.Add(regionR[i * counter + j]);
                }
                counter = 5;
            }

            counter = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 2; j < 5; j++)
                {
                    region2.Add(regionR[i * counter + j]);
                }
                counter += 5;
            }

            counter = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    region3.Add(regionR[i * counter + j + 10]);
                }
                counter = 5;
            }

            counter = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 2; j < 5; j++)
                {
                    region4.Add(regionR[i * counter + j + 10]);
                }
                counter = 5;
            }

            meanAndVariance.Clear();
            meanAndVariance.Add(new KeyValuePair<double, double>(Variance(region1, region1.Average()), region1.Average()));
            meanAndVariance.Add(new KeyValuePair<double, double>(Variance(region2, region2.Average()), region2.Average()));
            meanAndVariance.Add(new KeyValuePair<double, double>(Variance(region3, region3.Average()), region3.Average()));
            meanAndVariance.Add(new KeyValuePair<double, double>(Variance(region4, region4.Average()), region4.Average()));

            double minVariance = meanAndVariance.OrderBy(e => e.Key).First().Key;

            region1.Clear();
            region2.Clear();
            region3.Clear();
            region4.Clear();

            return (int)meanAndVariance.First(e => e.Key == minVariance).Value;   /* bierzemy wartość średnią regionu z minimalną wariancją */
        }
        private double Variance(List<int> list, double avg)
        {
            double sum = 0;
            for (int i = 0; i < 9; i++)
            {
                sum += Math.Pow(avg - list[i], 2);
            }
            return sum / 9.0f;
        }
    }
}
