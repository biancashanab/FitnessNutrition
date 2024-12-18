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
            //ExecuteFitnessWorkflow();
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

        void ExecuteFitnessWorkflow()
        {
            WeeklyWorkout weeklyWorkout = new WeeklyWorkout();
                var dailyWorkout = new DailyWorkout();
                var exercitii = new List<Exercitii>();
                exercitii.Add(new Exercitii { ID = 1 });
                int userID = 1;
                dailyWorkout.AddAntrenamentZilnic(exercitii, userID);
            List<AntrenamentZilnic> antrenamenteZilnice = new List<AntrenamentZilnic>();
            antrenamenteZilnice.Add(new AntrenamentZilnic { ID = 1 });
            weeklyWorkout.AddAntrenamentSaptamanal(antrenamenteZilnice, userID);
            Console.WriteLine("Antrenament Săptămânal added successfully.");
        }

    }

}
