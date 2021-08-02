using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KernelFilters
{
    class DefaultDialogService : IDialogService
    {
        public string FilePath { get; set; }

        public bool OpenFileDialog()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                FilePath = ofd.FileName;
                return true;
            }
            return false;
        }

        public bool SaveFileDialog()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Image files (*.png, *.bmp, *.jpg)|*.png;*.bmp;*.jpg";
            if (sfd.ShowDialog() == true)
            {
                FilePath = sfd.FileName;
                return true;
            }
            return false;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show($"Message: {message}", "Dialog Service Message", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
