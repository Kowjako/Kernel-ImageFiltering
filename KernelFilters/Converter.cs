using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KernelFilters
{
    static class Converter
    {
        public static Bitmap ImageSourceToBitmap(ImageSource source)
        {
            if (source == null) return null;
            BitmapImage startImage = source as BitmapImage;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(startImage));
                enc.Save(outStream);
                Bitmap bitmap = new Bitmap(outStream);
                return new Bitmap(bitmap);
            }
        }

        public static ImageSource BitmapToImageSource(Bitmap source)
        {
            if (source == null) return null;
            using (MemoryStream memory = new MemoryStream())
            {
                source.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }
    }
}
