using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KernelFilters
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        /* Otwieranie/zapisywanie plikow */
        protected IDialogService dialogService;
        /* Ladowanie obrazku */
        private ImageSource loadedImage, filteredImage;
        /* Commandy */
        private RelayCommand loadImageCommand;
        private RelayCommand saveImageCommand;
        /* Properties for Binding*/
        public ImageSource LoadedImage => loadedImage;
        public ImageSource FilteredImage => filteredImage;
        

        public ApplicationViewModel(IDialogService service)
        {
            this.dialogService = service;
        }

        public RelayCommand LoadImageCommand
        {
            get
            {
                return loadImageCommand ??
                    (loadImageCommand = new RelayCommand(obj =>
                    {
                        if(dialogService.OpenFileDialog() == true)
                        {
                            string imagePath = dialogService.FilePath;
                            loadedImage = BitmapFromUri(new Uri(imagePath));
                            OnPropertyChanged("LoadedImage");

                            /* GrayScaleFilter gsf = new GrayScaleFilter();
                            filteredImage = gsf.Filterize(loadedImage);
                            OnPropertyChanged("FilteredImage"); */
                        }
                    }));
            }
        }

        public RelayCommand SaveImageCommand
        {
            get
            {
                return saveImageCommand ??
                    (saveImageCommand = new RelayCommand(obj =>
                    {
                        BitmapSource tmpImage = obj as BitmapSource;
                        if(dialogService.SaveFileDialog() == true)
                        {
                            string imagePath = dialogService.FilePath;
                            using (var fileStream = new FileStream(imagePath, FileMode.Create))
                            {
                                BitmapEncoder encoder = new PngBitmapEncoder();
                                encoder.Frames.Add(BitmapFrame.Create(tmpImage as BitmapSource));
                                encoder.Save(fileStream);
                            }
                        }
                        
                    }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private ImageSource BitmapFromUri(Uri source)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = source;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }
    }
}
