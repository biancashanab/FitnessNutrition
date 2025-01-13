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
using System.Windows.Controls;
using System.Security.Policy;

namespace Fitness.ViewModels
{
    public class SettingsVM : BaseViewModel
    {
        private User _user;
        private ObservableCollection<MealPlanItem> _weeklyMealPlan;
        private ObservableCollection<WorkoutPlanItem> _weeklyWorkoutPlan;
        private UserControl _currentUC;
        private Visibility _mainContentVisibility = Visibility.Visible;
        private WeeklyWorkout _selectedWorkoutPlan1;
        private WeeklyWorkout _selectedWorkoutPlan2;
        private WeeklyMealPlan _selectedMealPlan1;
        private WeeklyMealPlan _selectedMealPlan2;
        private ObservableCollection<MealPlanItem> _availableMealPlans;
        public ObservableCollection<MealPlanItem> AvailableMealPlans
        {
            get => _availableMealPlans;
            set
            {
                _availableMealPlans = value;
                OnPropertyChanged(nameof(AvailableMealPlans));
            }
        }

        private ObservableCollection<WorkoutPlanItem> _availableWorkoutPlans;
        public ObservableCollection<WorkoutPlanItem> AvailableWorkoutPlans
        {
            get => _availableWorkoutPlans;
            set
            {
                _availableWorkoutPlans = value;
                OnPropertyChanged(nameof(AvailableWorkoutPlans));
            }
        }


        private void LoadAvailableMealPlans()
        {
            AvailableMealPlans = MealPlanService.GetWeeklyMealPlanForDisplay(_user.GetUser("admin").Id, DateTime.Now);
        }

        private void LoadAvailableWorkoutPlans()
        {
            AvailableWorkoutPlans = WorkoutService.GetWeeklyWorkoutPlanForDisplay(_user.GetUser("admin").Id, DateTime.Now);
        }

        private ICommand _modifyNameCommand;
        public ICommand ModifyNameCommand
        {
            get
            {
                if (_modifyNameCommand == null)
                {
                    _modifyNameCommand = new RelayCommand(ModifyName);
                }
                return _modifyNameCommand;
            }
        }

        private ICommand _modifySexCommand;
        public ICommand ModifySexCommand
        {
            get
            {
                if (_modifySexCommand == null)
                {
                    _modifySexCommand = new RelayCommand(ModifySex);
                }
                return _modifySexCommand;
            }
        }

        private ICommand _modifyHeightCommand;
        public ICommand ModifyHeightCommand
        {
            get
            {
                if (_modifyHeightCommand == null)
                {
                    _modifyHeightCommand = new RelayCommand(ModifyHeight);
                }
                return _modifyHeightCommand;
            }
        }

        private ICommand _modifyWeightCommand;
        public ICommand ModifyWeightCommand
        {
            get
            {
                if (_modifyWeightCommand == null)
                {
                    _modifyWeightCommand = new RelayCommand(ModifyWeight);
                }
                return _modifyWeightCommand;
            }
        }

        private ICommand _modifyUserTypeCommand;
        public ICommand ModifyUserTypeCommand
        {
            get
            {
                if (_modifyUserTypeCommand == null)
                {
                    _modifyUserTypeCommand = new RelayCommand(ModifyUserType);
                }
                return _modifyUserTypeCommand;
            }
        }

        private ICommand _modifyUserPhysicalCondCommand;
        public ICommand ModifyUserPhysicalCondCommand
        {
            get
            {
                if (_modifyUserPhysicalCondCommand == null)
                {
                    _modifyUserPhysicalCondCommand = new RelayCommand(ModifyUserPhysicalCondition);
                }
                return _modifyUserPhysicalCondCommand;
            }
        }

        private ICommand _modifyUserActivityCommand;
        public ICommand ModifyUserActivityCommand
        {
            get
            {
                if (_modifyUserActivityCommand == null)
                {
                    _modifyUserActivityCommand = new RelayCommand(ModifyUserActivity);
                }
                return _modifyUserActivityCommand;
            }
        }

        private ICommand _modifyUserMealPlanCommand;
        public ICommand ModifyUserMealPlanCommand
        {
            get
            {
                if (_modifyUserMealPlanCommand == null)
                {
                    _modifyUserMealPlanCommand = new RelayCommand(ModifyUserMealPlan);
                }
                return _modifyUserMealPlanCommand;
            }
        }

        private ICommand _modifyUserWorkoutPlanCommand;
        public ICommand ModifyUserWorkoutPlanCommand
        {
            get
            {
                if (_modifyUserWorkoutPlanCommand == null)
                {
                    _modifyUserWorkoutPlanCommand = new RelayCommand(ModifyUserWorkoutPlan);
                }
                return _modifyUserWorkoutPlanCommand;
            }
        }

        private void ModifyName()
        {
            try
            {
                // Replace with your actual update logic
                _user.UpdateName(Name);
                RefreshUserData();
                MessageBox.Show("Nume actualizat cu succes!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la actualizarea numelui: {ex.Message}");
            }
        }

        private void ModifySex()
        {
            try
            {
                _user.UpdateSex(Sex);
                RefreshUserData();
                MessageBox.Show("Sex actualizat cu succes!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la actualizarea sexului: {ex.Message}");
            }
        }

        private void ModifyHeight()
        {
            try
            {
                _user.UpdateHeight(Height);
                RefreshUserData();
                MessageBox.Show("Înălțime actualizată cu succes!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la actualizarea înălțimii: {ex.Message}");
            }
        }

        private void ModifyWeight()
        {
            try
            {
                _user.UpdateWeight(Weight);
                RefreshUserData();
                MessageBox.Show("Greutate actualizată cu succes!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la actualizarea greutății: {ex.Message}");
            }
        }

        private void ModifyUserType()
        {
            try
            {
                _user.UpdateUserType(UserType);
                RefreshUserData();
                MessageBox.Show("Tip Utilizator actualizat cu succes!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la actualizarea tipului utilizatorului: {ex.Message}");
            }
        }

        private void ModifyUserPhysicalCondition()
        {
            try
            {
                _user.UpdatePhysicalCondition(PhysicalCondition);
                RefreshUserData();
                MessageBox.Show("Condiție Fizică actualizată cu succes!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la actualizarea condiției fizice: {ex.Message}");
            }
        }

        private void ModifyUserActivity()
        {
            try
            {
                _user.UpdateActivity(Activity);
                RefreshUserData();
                MessageBox.Show("Nivel Activitate actualizat cu succes!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la actualizarea nivelului de activitate: {ex.Message}");
            }
        }

        private void ModifyUserMealPlan()
        {
            try
            {
                var mp = new WeeklyMealPlan();
                mp.CopyPlanAlimentarSaptamanal(_user.GetUser("admin").Id, _user.Id);
                MessageBox.Show("Planul alimentar saptamanal a fost copiat cu succes");
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la copierea planului alimentar săptămânal: {ex.Message}");

            }
        }

        private void ModifyUserWorkoutPlan()
        {
            try
            {
                var wp = new WeeklyWorkout();
                wp.CopyAntrenamentSaptamanal(_user.GetUser("admin").Id, _user.Id);
                MessageBox.Show("Planul de antrenament săptămânal a fost copiat cu succes!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la copierea planului de antrenament săptămânal: {ex.Message}");
            }
        }
        public ObservableCollection<string> SexOptions { get; } = new ObservableCollection<string>
            {
                "Masculin",
                "Feminin",
                "Unspecified"
            };

        public ObservableCollection<string> UserTypeOptions { get; } = new ObservableCollection<string>
            {
                "Utilizator",
                "Nutritionist",
                "Administrator"
            };

        public ObservableCollection<string> ActivityOptions { get; } = new ObservableCollection<string>
            {
                "Sedentary",
                "Lightly active",
                "Moderately active",
                "Very Active",
                "Extremely Active"
            };

        public ObservableCollection<string> MealOptions { get; } = new ObservableCollection<string>
            {
                "Basic Meal Plan, 2000 kcal",
            };

        public ObservableCollection<string> WorkoutOptions { get; } = new ObservableCollection<string>
            {
                "Basic Workout Plan, 3 Days/Week",
            };

        public WeeklyMealPlan MealPlanService { get; set; }

        public WeeklyWorkout WorkoutService { get; set; }

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

        public string Activity
        {
            get => _user.Activity;
            set
            {
                if (_user.Activity != value)
                {
                    _user.Activity = value;
                    OnPropertyChanged(nameof(Activity));
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

        private void HideMainContent()
        {
            MainContentVisibility = Visibility.Collapsed;
        }

        public void LoadWeeklyMealPlan(DateTime startOfWeek)
        {
            WeeklyMealPlan = MealPlanService.GetWeeklyMealPlanForDisplay(_user.Id, startOfWeek);
        }

        public void LoadWeeklyWorkoutPlan(DateTime startOfWeek)
        {
            WeeklyWorkoutPlan = WorkoutService.GetWeeklyWorkoutPlanForDisplay(_user.Id, startOfWeek);
        }

        public SettingsVM(User user)
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
                OnPropertyChanged(nameof(Activity));
                OnPropertyChanged(nameof(WeeklyMealPlan));
                OnPropertyChanged(nameof(WeeklyWorkoutPlan));

            }
        }
    }

}
