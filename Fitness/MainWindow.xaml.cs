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
            var ww = new WeeklyWorkout();
            var wm = new WeeklyMealPlan();
            var u = new User();
            u.AddUser("admin", "admin_password", "Administrator");
            if (ww.getSize() < 1)
            {
                ww.CreareAntrenamentSaptamanal(3, 50, u.GetUser("admin").Id);
                ww.CreareAntrenamentSaptamanal(5, 100, u.GetUser("admin").Id);
            }
            if (wm.getSize() < 1)
            {
                wm.CrearePlanAlimentarSaptamanal(1500, u.GetUser("admin").Id);
                wm.CrearePlanAlimentarSaptamanal(3000, u.GetUser("admin").Id);
            }
            LoadWelcomeScreen();
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
