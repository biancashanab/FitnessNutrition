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
using System.Globalization;
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace Fitness.ViewModels
{
    public class UserVM : BaseViewModel
    {
        private User _user;
        private ObservableCollection<MealPlanItem> _weeklyMealPlan;
        private ObservableCollection<WorkoutPlanItem> _weeklyWorkoutPlan;

        public WeeklyMealPlan MealPlanService { get; set; } 
        public WeeklyWorkout WorkoutService { get; set; }


        // Proprietăți pentru legare în UI
        public string Name
        {
            get => _user.Name; 
            set
            {
                if (_user.Name != value)
                {
                    _user.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Sex
        {
            get => _user.Sex;
            set
            {
                if (_user.Sex != value)
                {
                    _user.Sex = value;
                    OnPropertyChanged(nameof(Sex));
                }
            }
        }

        public int Height
        {
            get => _user.Height;
            set
            {
                if (_user.Height != value)
                {
                    _user.Height = value;
                    OnPropertyChanged(nameof(Height));
                }
            }
        }

        public int Weight
        {
            get => _user.Weight;
            set
            {
                if (_user.Weight != value)
                {
                    _user.Weight = value;
                    OnPropertyChanged(nameof(Weight));
                }
            }
        }

        public string UserType
        {
            get => _user.UserType;
            set
            {
                if (_user.UserType != value)
                {
                    _user.UserType = value;
                    OnPropertyChanged(nameof(UserType));
                }
            }
        }

        public string PhysicalCondition
        {
            get => _user.PhysicalCondition;
            set
            {
                if (_user.PhysicalCondition != value)
                {
                    _user.PhysicalCondition = value;
                    OnPropertyChanged(nameof(PhysicalCondition));
                }
            }
        }

        public ObservableCollection<MealPlanItem> WeeklyMealPlan
        {
            get { return _weeklyMealPlan; }
            set
            {
                _weeklyMealPlan = value;
                OnPropertyChanged(nameof(WeeklyMealPlan));
            }
        }

        public ObservableCollection<WorkoutPlanItem> WeeklyWorkoutPlan
        {
            get { return _weeklyWorkoutPlan; }
            set
            {
                _weeklyWorkoutPlan = value;
                OnPropertyChanged(nameof(_weeklyWorkoutPlan));
            }
        }


        public void LoadWeeklyMealPlan(DateTime startOfWeek)
        {
            WeeklyMealPlan = MealPlanService.GetWeeklyMealPlanForDisplay(_user.Id, startOfWeek);
        }

        public void LoadWeeklyWorkoutPlan(DateTime startOfWeek)
        {
           WeeklyWorkoutPlan = WorkoutService.GetWeeklyWorkoutPlanForDisplay(1, startOfWeek);
        }

        public UserVM(User user)
        {
            _user = user ?? throw new ArgumentNullException(nameof(user));
            MealPlanService = new WeeklyMealPlan();
            WorkoutService = new WeeklyWorkout();

            DateTime startOfWeek = WeeklyWorkout.ClosestMondayFromPast(DateTime.Now);

            LoadWeeklyMealPlan(startOfWeek);
            LoadWeeklyWorkoutPlan(startOfWeek);
        }

        public void RefreshUserData()
        {
            var updatedUser = new User().GetUser(_user.Name);
            if (updatedUser != null)
            {
                _user = updatedUser;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Sex));
                OnPropertyChanged(nameof(Height));
                OnPropertyChanged(nameof(Weight));
                OnPropertyChanged(nameof(UserType));
                OnPropertyChanged(nameof(PhysicalCondition));
            }
        }
    }

}
