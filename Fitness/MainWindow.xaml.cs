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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Fitness.Models;
using Fitness.ViewModels;
using Fitness.Views;

namespace Fitness
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
            LoadWelcomeScreen();


            var wv = new WeeklyWorkout();

            if (wv.getSize() < 2)
            {
                wv.CreareAntrenamentSaptamanal(3, 30, 1);
                wv.CreareAntrenamentSaptamanal(6, 100, 1);
            }

            var wp = new WeeklyMealPlan();

            if (wp.getSize() < 2)
            {
                wp.CrearePlanAlimentarSaptamanal(1500, 1);
                wp.CrearePlanAlimentarSaptamanal(3000, 1);

            }

        }

        private void LoadWelcomeScreen()
        {
            MainContent.Content = new WelcomeUC();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }

      

    }

}
