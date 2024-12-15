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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Fitness.ViewModels;

namespace Fitness.Views
{
    public partial class LoginUC : UserControl
    {
        public LoginUC()
        {
            InitializeComponent();
        }

        private void SwitchToSignup(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AnimateGridTransition(LoginGrid, SignupGrid);
        }

        private void SwitchToLogin(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AnimateGridTransition(SignupGrid, LoginGrid);
        }

        private void AnimateGridTransition(Grid fromGrid, Grid toGrid)
        {
            // Fade out current grid
            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(1)
            };
            fromGrid.BeginAnimation(OpacityProperty, fadeOutAnimation);

            // Hide current grid and show new grid
            fromGrid.Visibility = Visibility.Collapsed;
            toGrid.Visibility = Visibility.Visible;

            // Fade in new grid
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromSeconds(1)
            };
            toGrid.BeginAnimation(OpacityProperty, fadeInAnimation);
        }

        private void ClearPlaceholderText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && (textBox.Text == "Username" || textBox.Text == ""))
            {
                textBox.Text = "";
            }
        }

        private void RestorePlaceholderText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Username";
            }
        }

        private void ClearPlaceholderPassword(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            if (passwordBox != null && (passwordBox.Password == "Password" || passwordBox.Password == ""))
            {
                passwordBox.Password = "";
            }
        }

        private void RestorePlaceholderPassword(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            if (passwordBox != null && string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                passwordBox.Password = "Password";
            }
        }

        private void LoginPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginVM viewModel)
            {
                viewModel.Password = LoginPasswordBox.Password;
            }
        }

        private void SignupPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginVM viewModel)
            {
                viewModel.Password = SignupPasswordBox.Password;
            }
        }
    }
}
