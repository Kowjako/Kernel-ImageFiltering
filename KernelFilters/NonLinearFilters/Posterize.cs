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

            for (int j = 0; j < startImageBMP.Height; j++)
            {
                for (int i = 0; i < startImageBMP.Width; i++)
                {
                    uint oldPixel = (uint)startImageBMP.GetPixel(i, j).ToArgb();

                    uint R = ((0x00FF0000) & (uint)oldPixel) >> 16;
                    uint G = ((0x0000FF00) & (uint)oldPixel) >> 8;
                    uint B = ((0x000000FF) & (uint)oldPixel);

                    /* Dla koloru R */
                    long redAreaFloat = R / numOfAreas;
                    int redArea = (int)redAreaFloat;
                    if (redArea > redAreaFloat)
                        redArea--;
                    int newRedFloat = numOfValues * redArea;
                    int newRed = (int)newRedFloat;
                    if (newRed > newRedFloat)
                        newRed--;

                    /* Dla koloru G */
                    long greenAreaFloat = G / numOfAreas;
                    int greenArea = (int)greenAreaFloat;
                    if (greenArea > greenAreaFloat)
                        greenArea--;
                    int newGreenFloat = numOfValues * greenArea;
                    int newGreen = (int)newGreenFloat;
                    if (newGreen > newGreenFloat)
                        newGreen--;

                    /* Dla koloru B */
                    long blueAreaFloat = B / numOfAreas;
                    int blueArea = (int)blueAreaFloat;
                    if (blueArea > blueAreaFloat)
                        blueArea--;
                    int newBlueFloat = numOfValues * blueArea;
                    int newBlue = (int)newBlueFloat;
                    if (newBlue > newBlueFloat)
                        newBlue--;

                    uint newPixel = (0xFF000000) | (uint)newRed << 16 | (uint)newGreen << 8 | (uint)newBlue;
                    outputImageBMP.SetPixel(i, j, System.Drawing.Color.FromArgb((int)newPixel));              
                }
            }
            return Converter.BitmapToImageSource(outputImageBMP);
        }
    }
}
