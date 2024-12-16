using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Fitness.Views;

namespace Fitness.ViewModels
{
    public class WelcomeVM : BaseViewModel
    {
        public ICommand NavigateToLoginCommand { get; }

        public WelcomeVM()
        {
            NavigateToLoginCommand = new RelayCommand(NavigateToLogin);
        }

        private void NavigateToLogin()
        {
            var mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
            mainWindow.MainContent.Content = new LoginUC();
        }
    }
}
