using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KernelFilters.FitersWithoutKernel
{
    class Pixelize : IFilter
    {
        List<int> avgR = new List<int>(100);
        List<int> avgB = new List<int>(100);
        List<int> avgG = new List<int>(100);
        System.Drawing.Color resultPixel = System.Drawing.Color.Empty;

        public ImageSource Filterize(ImageSource sourceImage)
        {
            Bitmap startImageBMP = Converter.ImageSourceToBitmap(sourceImage);
            Bitmap outputImageBMP = new Bitmap(startImageBMP.Width, startImageBMP.Height - 1);

            for (int x = 3; x <= startImageBMP.Width - 3; x+=3)
            {
                for (int y = 3; y <= startImageBMP.Height - 3; y+=3)
                {
                    for (int i = -3; i <= 3; i++)
                    {
                        for (int j = -3; j <= 3; j++)
                        {
                            avgR.Add(startImageBMP.GetPixel(x + j, y + i).R);
                            avgG.Add(startImageBMP.GetPixel(x + j, y + i).G);
                            avgB.Add(startImageBMP.GetPixel(x + j, y + i).B);
                        }
                    }
                    resultPixel = System.Drawing.Color.FromArgb(255, (int)avgR.Average(), (int)avgG.Average(), (int)avgB.Average());
                    for (int i = -3; i <= 3; i++)
                    {
                        for (int j = -3; j <= 3; j++)
                        {
                            outputImageBMP.SetPixel(x + j, y + i, resultPixel);
                        }
                    }
                    avgR.Clear();
                    avgG.Clear();
                    avgB.Clear();
                }
            }
            return Converter.BitmapToImageSource(outputImageBMP);
        }
    }
}
