using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KernelFilters
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void closeBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void headerPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    TextBox tmp = null;
                    if (i != 4)
                    {
                        tmp = new TextBox() { Text = "0", Margin = new Thickness(0,0,0,6), Width = 25, Height = 25, Name = $"box{i}{j}" };
                    }
                    else
                    {
                        tmp = new TextBox() { Text = "0", Margin = new Thickness(0,0,0,6), Width = 25, Height = 25, Name = $"box{i}{j}" };
                    }
                    Grid.SetRow(tmp, i);
                    Grid.SetColumn(tmp, j);
                    matrixGrid.Children.Add(tmp);
                }
            }
        }
    }
}
