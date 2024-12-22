using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fitness.Models;
using Fitness.Views;

namespace Fitness.ViewModels 
{
    public class HomeVM : BaseViewModel
    {
        private UserControl _currentUC;
        private Visibility _mainContentVisibility = Visibility.Visible;

        public UserControl CurrentUC
        {
            get => _currentUC;
            set
            {
                _currentUC = value;
                OnPropertyChanged(nameof(CurrentUC));
            }
        }

        public Visibility MainContentVisibility
        {
            get => _mainContentVisibility;
            set
            {
                _mainContentVisibility = value;
                OnPropertyChanged(nameof(MainContentVisibility));
            }
        }


        public ICommand NavigateToMealCommand { get; }
        public ICommand NavigateToWorkoutCommand { get; }
        public ICommand NavigateToSupplementsCommand { get; }

        public HomeVM()
        {
            NavigateToMealCommand = new RelayCommand(NavigateToMeal);
            NavigateToWorkoutCommand = new RelayCommand(NavigateToWorkout);
            NavigateToSupplementsCommand = new RelayCommand(NavigateToSupplements);

            // Inițializarea cu un UserControl
            CurrentUC = null;  // Poți schimba cu un UserControl default
        }

        private void HideMainContent()
        {
            MainContentVisibility = Visibility.Collapsed;
        }

        private void NavigateToMeal()
        {
            HideMainContent();
            CurrentUC = new SuplementsUC(); // Schimbă cu UserControl-ul Meal
            
        }

        private void NavigateToWorkout()
        {
            HideMainContent();
            CurrentUC = new SuplementsUC(); // Schimbă cu UserControl-ul Workout
        }

        private void NavigateToSupplements()
        {
            HideMainContent();
            CurrentUC = new SuplementsUC(); // Schimbă cu UserControl-ul Supplements
        }
    }
}
