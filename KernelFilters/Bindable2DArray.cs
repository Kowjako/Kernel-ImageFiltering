using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace KernelFilters
{
    /* Klasa żeby zrobić Bind na 2D tablicę skoro XAML pozwala zrobić Bind tylko na jednowymiarową więc podmianiamy indeksy */
    public class Bindable2DArray<T> : INotifyPropertyChanged
    {
        T[,] data;

        public Bindable2DArray(int size1, int size2)
        {
            data = new T[size1, size2];
        }

        public T this[int c1, int c2]
        {
            get { return data[c1, c2]; }
            set
            {
                data[c1, c2] = value;
                OnPropertyChanged(Binding.IndexerName);
            }
        }

        public string GetStringIndex(int c1, int c2)
        {
            return c1.ToString() + "-" + c2.ToString();
        }

        private void SplitIndex(string index, out int c1, out int c2)
        {
            var parts = index.Split('-');
            c1 = Convert.ToInt32(parts[0]);
            c2 = Convert.ToInt32(parts[1]);
        }

        public T this[string index]
        {
            get
            {
                int c1, c2;
                SplitIndex(index, out c1, out c2);
                return data[c1, c2];
            }
            set
            {
                int c1, c2;
                SplitIndex(index, out c1, out c2);
                data[c1, c2] = value;
                OnPropertyChanged(Binding.IndexerName);
            }
        }

        /* Żeby można było cast-ować BindableArray na tablice prymitywnych typów */
        public static implicit operator T[,] (Bindable2DArray<T> a)
        {
            return a.data;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}

