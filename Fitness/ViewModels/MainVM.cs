using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Fitness.Views;
using Fitness.Models;
using System.Windows.Navigation;

namespace Fitness.ViewModels
{
    public class MainVM : BaseViewModel
    {
        private UserControl _currentUC;
        public UserControl CurrentUC
        {
            get => _currentUC;
            set
            {
                _currentUC = value;
                OnPropertyChanged(nameof(CurrentUC));
            }
        }

        public ICommand LogoutCommand { get; }
        public ICommand Click_AcasaCommand { get; }
        public ICommand Click_SetariCommand { get; }
        public ICommand Click_NotificariCommand { get; }
        public ICommand Click_UserCommand { get; }
        public NavigationService ContentFrame { get; }

        public MainVM()
        {
            LogoutCommand = new RelayCommand(Logout);
            Click_AcasaCommand = new RelayCommand(Click_Acasa);
            Click_SetariCommand = new RelayCommand(Click_Setari);
            Click_NotificariCommand = new RelayCommand(Click_Notificari);
            Click_UserCommand = new RelayCommand(Click_User);

            CurrentUC = new HomeUC(); 
        }

        private void Logout()
        {
            var mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
            mainWindow.MainContent.Content = new LoginUC();
        }

        private void Click_Setari()
        {
           // CurrentUC = new SetariUC();
        }

        private void Click_Notificari()
        {
           // CurrentUC = new NotificariUC();
        }

        private void Click_User()
        {
            CurrentUC = new UserUC();
        }

        private void Click_Acasa()
        {
            CurrentUC = new HomeUC();
        }
    }
}
