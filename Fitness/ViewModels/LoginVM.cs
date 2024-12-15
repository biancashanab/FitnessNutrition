using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Fitness.Views;
using Fitness.Models;

namespace Fitness.ViewModels
{
    public class LoginVM : INotifyPropertyChanged
    {
        private string _username;
        private string _password;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
                ((RelayCommand)LoginCommand).RaiseCanExecuteChanged(); // Notify that CanExecute might have changed
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                ((RelayCommand)LoginCommand).RaiseCanExecuteChanged(); // Notify that CanExecute might have changed
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand SignupCommand { get; }

        public LoginVM()
        {
            LoginCommand = new RelayCommand(Login, CanLogin);
            SignupCommand = new RelayCommand(Signup);
        }

        private bool CanLogin()
        {
            // Enable the button only if Username and Password are not empty or whitespace
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password);
        }

        private void Login()
        {
            // Replace with your actual authentication logic
            try
            {
                User user = new User();
                var dbUser = user.GetUser(Username); // Get user from database

                // Verificăm dacă parola introdusă corespunde cu parola din baza de date
                if (dbUser != null && dbUser.Password == User.HashPasswordSHA256(Password))
                {
                    // Autentificare reușită
                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    mainWindow.MainContent.Content = new MainUC();
                }
                else
                {
                    // Dacă nu există utilizator sau parola nu se potrivește
                    MessageBox.Show("Invalid credentials");
                }
            }
            catch (Exception ex)
            {
                // Manevrarea excepțiilor dacă utilizatorul nu există
                MessageBox.Show(ex.Message);
            }
        }



        public void Signup()
        {
            try
            {
                User user = new User();
                var existingUser = user.GetUser(Username); // Verifică dacă utilizatorul există deja

                if (existingUser != null)
                {
                    MessageBox.Show("User already exists!");
                    return;
                }

                // Crearea unui nou utilizator
                user.AddUser(Username, Password);
                MessageBox.Show("User successfully registered!");

                var mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
                mainWindow.MainContent.Content = new MainUC();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during sign-in: {ex.Message}");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
