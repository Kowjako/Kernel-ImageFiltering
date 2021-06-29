using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private ImageSource loadedImage;
        /* Commandy */
        private RelayCommand loadImageCommand;
        /* Properties for Binding*/
        public ImageSource LoadedImage => loadedImage;

        

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
