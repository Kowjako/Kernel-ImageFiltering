using KernelFilters.FitersWithoutKernel;
using KernelFilters.MatrixFilter;
using KernelFilters.Noises;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        private RelayCommand changeFilterCommand;
        private RelayCommand setNoiseCommand;
        private RelayCommand acceptMatrixCommand;

        /* Aktualny filtr/szum */
        private IFilter actualFilter;
        private INoise actualNoise;

        /* Properties for Binding*/
        public ImageSource LoadedImage => loadedImage;
        public ImageSource FilteredImage => filteredImage;
        public int NoiseScale { get; set; }
        public int KernelScale { get; set; }
       
        public ApplicationViewModel(IDialogService service)
        {
            this.dialogService = service;
        }

        public RelayCommand AcceptMatrixCommand
        {
            get
            {
                return acceptMatrixCommand ??
                    (acceptMatrixCommand = new RelayCommand(obj =>
                    {
                        MessageBox.Show(KernelScale.ToString());
                    }));
            }
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

        public RelayCommand ChangeFilterCommand
        {
            get
            {
                return changeFilterCommand ??
                    (changeFilterCommand = new RelayCommand(obj =>
                    {
                        string filterName = obj as string;
                        switch(filterName)
                        {
                            case "blackwhite":
                                actualFilter = new BlackWhiteFilter();
                                break;
                            case "negative":
                                actualFilter = new NegativeFilter();
                                break;
                            case "sepia":
                                actualFilter = new SepiaFilter();
                                break;
                            case "grayscale":
                                actualFilter = new GrayScaleFilter();
                                break;
                            case "boxblur":
                                actualFilter = new NormalizedBoxBlur();
                                break;
                            case "edgedetection":
                                actualFilter = new EdgeDetection();
                                break;
                            case "gaussian3x3":
                                actualFilter = new GaussianBlur3x3();
                                break;
                            case "sharpen":
                                actualFilter = new Sharpen();
                                break;
                            case "sobel":
                                actualFilter = new SobelFilter();
                                break;
                            case "embossing":
                                actualFilter = new Emboss();
                                break;
                            case "extension":
                                actualFilter = new ExtensionFilter();
                                break;
                            case "prewett":
                                actualFilter = new Prewitt();
                                break;
                            case "gaussian5x5":
                                actualFilter = new GaussianBlur5x5();
                                break;
                        }
                        filteredImage = actualFilter.Filterize(loadedImage);
                        OnPropertyChanged("FilteredImage");
                    }));
            }
        }

        public RelayCommand SetNoiseCommand
        {
            get
            {
                return setNoiseCommand ??
                    (setNoiseCommand = new RelayCommand(obj =>
                    {
                        string noiseName = null;
                        UIElementCollection noiseFilters = (obj as StackPanel).Children; /* dostajemy co wlozone do kontenera StackPanel */
                        foreach(var tmpNoise in noiseFilters)
                        {
                            RadioButton x = tmpNoise as RadioButton;    /* castujemy do RadioButton */
                            if (x.IsChecked == true)
                            {
                                noiseName = x.Name;
                                break;
                            }
                        }

                        switch(noiseName)
                        {
                            case "saltpepper":
                                actualNoise = new SaltPepperNoise(NoiseScale);
                                break;

                            case "gaussian":
                                actualNoise = new GaussianNoise(NoiseScale);
                                break;

                            case "spackle":
                                actualNoise = new SpackleNoise(NoiseScale);
                                break;
                            
                        }
                        filteredImage = actualNoise?.Noising(loadedImage);
                        OnPropertyChanged("FilteredImage");
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
