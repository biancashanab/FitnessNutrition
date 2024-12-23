using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Fitness.Models;

namespace Fitness.ViewModels
{
    public class NutritionistNotificationsVM
    {
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<User> Users2 { get; set; }
        public ICommand GenerateMealPlanCommand { get; set; }
        public ICommand GenerateWorkoutPlanCommand { get; set; }

        private FitnessDBDataContext _context;

        public NutritionistNotificationsVM()
        {
            _context = new FitnessDBDataContext();
            Users = new ObservableCollection<User>(GetUsersWithoutDailyMealPlans());
            Users2 = new ObservableCollection<User>(GetUsersWithoutDailyWorkoutPlans());
            GenerateMealPlanCommand = new RelayCommand<User>(GenerateMealPlan);
            GenerateWorkoutPlanCommand = new RelayCommand<User>(GenerateWorkoutPlan);
        }

        private IEnumerable<User> GetUsersWithoutDailyMealPlans()
        {
            var query = from pas in _context.PlanAlimentarSaptamanals
                        where !_context.PlanAlimentarSaptamanal_Zilnics.Any(pz => pz.PlanAlimentarSaptamanalID == pas.ID)
                        join user in _context.Utilizatoris on pas.UserID equals user.ID
                        select new User
                        {
                            Id = user.ID,
                            Name = user.Name,
                            Sex = user.Sex,
                            Height = user.Height.HasValue ? (int)user.Height : 0,
                            Weight = user.Kilograms.HasValue ? (int)user.Kilograms : 0,
                            UserType = user.UserType
                        };
            
            return query.ToList();
        }

        private IEnumerable<User> GetUsersWithoutDailyWorkoutPlans()
        {
            var query = from pas in _context.AntrenamentSaptamanals
                        where !_context.AntrenamentSaptamanal_Zilnics.Any(pz => pz.AntrenamentSaptamanalID == pas.ID)
                        join user in _context.Utilizatoris on pas.UserID equals user.ID
                        select new User
                        {
                            Id = user.ID,
                            Name = user.Name,
                            Sex = user.Sex,
                            Height = user.Height.HasValue ? (int)user.Height : 0,
                            Weight = user.Kilograms.HasValue ? (int)user.Kilograms : 0,
                            UserType = user.UserType
                        };
            
            return query.ToList();
        }

        private void GenerateMealPlan(User user)
        {
            // Delete old entry
            var oldPlans = _context.PlanAlimentarSaptamanals
                .Where(pas => pas.UserID == user.Id && !_context.PlanAlimentarSaptamanal_Zilnics.Any(pz => pz.PlanAlimentarSaptamanalID == pas.ID))
                .ToList();

            foreach (var plan in oldPlans)
            {
                _context.PlanAlimentarSaptamanals.DeleteOnSubmit(plan);
            }

            // Generate new meal plan
            var dailyMealPlan = new DailyMealPlan();
            var dailyPlans = new List<PlanAlimentarZilnic>();
            for (int i = 0; i < 7; i++)
            {
                var date = GetDateForDayInCurrentWeek(i);
                dailyPlans.Add(dailyMealPlan.AddPlanAlimentarZilnic(user.Id, 2000, date));
            }

            var weeklyPlan = new PlanAlimentarSaptamanal
            {
                UserID = user.Id,
                DataInceput = WeeklyWorkout.ClosestMondayFromPast(DateTime.Now),
                DataSfarsit = WeeklyWorkout.ClosestSundayFromFuture(DateTime.Now),
                Nume = "Weekly Meal Plan"
            };

            foreach (var dailyPlan in dailyPlans)
            {
                weeklyPlan.PlanAlimentarSaptamanal_Zilnics.Add(new PlanAlimentarSaptamanal_Zilnic
                {
                    PlanAlimentarZilnicID = dailyPlan.ID
                });
            }

            _context.PlanAlimentarSaptamanals.InsertOnSubmit(weeklyPlan);
            _context.SubmitChanges();
            Users.Clear();


            foreach (var u in GetUsersWithoutDailyMealPlans())
            {
                Users.Add(u);
            }
        }
        private void GenerateWorkoutPlan(User user)
        {
            // Delete old entries
            var oldPlans = _context.AntrenamentSaptamanals
                .Where(pas => pas.UserID == user.Id
                    && !_context.AntrenamentSaptamanal_Zilnics
                        .Any(pz => pz.AntrenamentSaptamanalID == pas.ID))
                .ToList();

            foreach (var plan in oldPlans)
            {
                Console.WriteLine($"Deleting plan ID: {plan.ID}"); // Debugging log
                _context.AntrenamentSaptamanals.DeleteOnSubmit(plan);
            }

            // Save changes to the database
            _context.SubmitChanges(); // Or use SaveChanges() if using Entity Framework

            var WW = new WeeklyWorkout();
            WW.CreareAntrenamentSaptamanal(4, 40, user.Id);

            // Refresh the user list
            Users2.Clear();
            foreach (var u in GetUsersWithoutDailyWorkoutPlans())
            {
                Users2.Add(u);
            }
        }


        private DateTime GetDateForDayInCurrentWeek(int day)
        {
            var monday = WeeklyWorkout.ClosestMondayFromPast(DateTime.Now);
            return monday.AddDays(day);
        }
    }
}
