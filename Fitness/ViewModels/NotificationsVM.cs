using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Fitness.Models;

namespace Fitness.ViewModels
{
    public class NotificationsVM : BaseViewModel
    {
        private User _user;

        public NotificationsVM(User user)
        {
            _user = user;

            // Initialize commands
            RequestMealPlanCommand = new RelayCommand(ExecuteCreateMealPlan);
            RequestWorkoutPlanCommand = new RelayCommand(ExecuteCreateWorkoutPlan);
        }

        // Commands to bind to the view
        public ICommand RequestMealPlanCommand { get; }
        public ICommand RequestWorkoutPlanCommand { get; }

        private void ExecuteCreateMealPlan()
        {
            try
            {
                var wm = new WeeklyMealPlan();
                wm.Add(_user.Id, DateTime.Now);
                MessageBox.Show("Meal plan request sent.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating meal plan: {ex.Message}");
            }
        }

        private void ExecuteCreateWorkoutPlan()
        {
            try
            {
                var ww = new WeeklyWorkout();
                ww.Add(_user.Id, DateTime.Now);
                MessageBox.Show("Workout plan request sent.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating workout plan: {ex.Message}");
            }
        }
    }

}
