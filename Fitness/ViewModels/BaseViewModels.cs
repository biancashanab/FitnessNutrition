using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;   //proprietatea care va fi folosita pentru a notifica View-ul ca s-a schimbat ceva in ViewModel

        protected void OnPropertyChanged(string propertyName)     //metoda care va fi folosita pentru a notifica View-ul ca s-a schimbat ceva in ViewModel
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
