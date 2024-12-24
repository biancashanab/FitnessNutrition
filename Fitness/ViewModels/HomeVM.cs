using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Fitness.Models;
using Fitness.Views;
using LiveCharts;

namespace Fitness.ViewModels 
{
    public class HomeVM : BaseViewModel
    {
        private UserControl _currentUC;
        private Visibility _mainContentVisibility = Visibility.Visible;
        User _user;
        private string _calorie;

        public string Calorie
        {
            get => _calorie;
            set
            {
                _calorie = value;
                OnPropertyChanged(nameof(Calorie));
            }
        }

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
        public ICommand ShowCaloriesCommand { get; }

        public ChartValues<decimal> WeightChartValues { get; set; }
        public List<string> WeightDates { get; set; }

        private void LoadUserHistory()
        {
            int userId = _user.Id;
            var _weightHistoryModel = new WeightHistory();
            var history = _weightHistoryModel.getUserHistory(userId);

            WeightChartValues = new ChartValues<decimal>(history.Select(h => h.Weight));
            WeightDates = history.Select(h => h.Date.ToShortDateString()).ToList();

            OnPropertyChanged(nameof(WeightChartValues));
            OnPropertyChanged(nameof(WeightDates));
        }


        public ObservableCollection<WeightHistory> WeightHistoryValues { get; set; }

        private decimal _newWeight;

        public decimal NewWeight
        {
            get => _newWeight;
            set
            {
                _newWeight = value;
            }
        }

        public ICommand SubmitWeightCommand { get; }

        private void OnSubmitWeight()
        {
            var _weightHistoryModel = new WeightHistory();
            _weightHistoryModel.addWeight(_user.Name, DateTime.Now, NewWeight);
            _user.UpdateWeight((int)NewWeight);
            LoadUserHistory();
            Initialize();
        }
        public HomeVM(User user)
        {
            _user = user;
            NavigateToMealCommand = new RelayCommand(NavigateToMeal);
            NavigateToWorkoutCommand = new RelayCommand(NavigateToWorkout);
            NavigateToSupplementsCommand = new RelayCommand(NavigateToSupplements);
            SubmitWeightCommand = new RelayCommand(OnSubmitWeight);
            CurrentUC = null;
            DelayedInitialize();


        }

        private async void DelayedInitialize()
        {
            await Task.Delay(1000); 
            Initialize();
            LoadUserHistory();

        }

        public void Initialize()
        {
            if (_user == null)
            {
                return;
            }
            if (CalculateDailyCalories(
                CalculateBMR(_user.Height, _user.Weight, 20, _user.Sex),
                _user.Activity) > 0)
                Calorie = CalculateDailyCalories(
                    CalculateBMR(_user.Height, _user.Weight, 20, _user.Sex),
                    _user.Activity
                ).ToString();
            else
                Calorie = "0";
        }

        public static double CalculateBMR(int HeightCm, int WeightKg, int Age, string Sex)
        {
            double bmr;
            if (Sex == "Masculin")
            {
                bmr = (10 * WeightKg) + (6.25 * HeightCm) - (5 * Age) + 5;
            }
            else
            {
                bmr = (10 * WeightKg) + (6.25 * HeightCm) - (5 * Age) - 161;
            }
            return Math.Round(bmr, 0);
        }

        public static double CalculateDailyCalories(double bmr, string activity)
        {
            double activityMultiplier;
            switch (activity)
            {
                case "Sedentary":
                    activityMultiplier = 1.2;
                    break;
                case "Lightly active":
                    activityMultiplier = 1.375;
                    break;
                case "Moderately active":
                    activityMultiplier = 1.55;
                    break;
                case "Very active":
                    activityMultiplier = 1.725;
                    break;
                case "Extremely active":
                    activityMultiplier = 1.9;
                    break;
                default:
                    activityMultiplier = 1.2;
                    break;
            }
            return Math.Round(bmr * activityMultiplier, 0);
        }

        private void HideMainContent()
        {
            MainContentVisibility = Visibility.Collapsed;
        }

        private void NavigateToMeal()
        {
            HideMainContent();
            CurrentUC = new SuplementsUC();
            
        }

        private void NavigateToWorkout()
        {
            HideMainContent();
            CurrentUC = new SuplementsUC();
        }

        private void NavigateToSupplements()
        {
            HideMainContent();
            CurrentUC = new SuplementsUC();
        }
    }
}
