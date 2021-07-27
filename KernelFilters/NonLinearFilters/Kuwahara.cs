﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KernelFilters.NonLinearFilters
{
    class Kuwahara : IFilter
    {
        List<byte> regionR = new List<byte>(25);
        List<byte> regionG = new List<byte>(25);
        List<byte> regionB = new List<byte>(25);

        public ImageSource Filterize(ImageSource sourceImage)
        {
            Bitmap startImageBMP = Converter.ImageSourceToBitmap(sourceImage);
            System.Drawing.Color pixel, resultPixel;

            for (int j = 2; j < startImageBMP.Height - 2; j++)
            {
                for (int i = 2; i < startImageBMP.Width - 2; i++)
                {
                    pixel = startImageBMP.GetPixel(i, j);
                    regionR.Add(pixel.R);
                    regionB.Add(pixel.B);
                    regionG.Add(pixel.G);
                    resultPixel = ProcessRegion(regionR);
                }
            }

            return null;
        }

        private System.Drawing.Color ProcessRegion(List<int> regionR)
        {
            Dictionary<double, double> meanAndVariance = new Dictionary<double, double>();
            /* Obliczanie dla R */
            var region1 = new List<int>();
            var region2 = new List<int>();
            var region3 = new List<int>();
            var region4 = new List<int>();

            var counter = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    region1.Add(regionR[i * counter + j]);
                }
                counter += 5;
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
                counter += 5;
            }

            counter = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 2; j < 5; j++)
                {
                    region4.Add(regionR[i * counter + j + 10]);
                }
                counter += 5;
            }

            meanAndVariance.Add(Variance(region1, region1.Average()), region1.Average());
            meanAndVariance.Add(Variance(region2, region2.Average()), region2.Average());
            meanAndVariance.Add(Variance(region3, region3.Average()), region3.Average());
            meanAndVariance.Add(Variance(region4, region4.Average()), region4.Average());

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
