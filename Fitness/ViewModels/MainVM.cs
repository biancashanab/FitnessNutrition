using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Fitness.Views;

namespace Fitness.ViewModels
{
    public class MainVM
    {
        public ICommand LogoutCommand { get; }

        public MainVM()
        {
            LogoutCommand = new RelayCommand(Logout);
        }

        private void Logout()
        {
            var mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
            mainWindow.MainContent.Content = new LoginUC();
        }
    }
}
