using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Models
{
    public class WeeklyMealPlan : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime DataInceput { get; set; }
        public DateTime DataSfarsit { get; set; }

        private readonly FitnessDBDataContext _context;
        public event PropertyChangedEventHandler PropertyChanged;

        public WeeklyMealPlan()
        {
            _context = new FitnessDBDataContext();
        }

        public static DateTime GetDateForDayInCurrentWeek(int dayOfWeek)
        {
            if (dayOfWeek < 0 || dayOfWeek > 6)
            {
                throw new ArgumentOutOfRangeException(nameof(dayOfWeek), "dayOfWeek must be between 0 (Monday) and 6 (Sunday).");
            }
            DateTime today = DateTime.Today;
            int currentDayOfWeek = ((int)today.DayOfWeek + 6) % 7;
            int daysDifference = dayOfWeek - currentDayOfWeek;
            return today.AddDays(daysDifference);
        }

        public void CrearePlanAlimentarSaptamanal(int nrCalorii, int userID)
        {
            var p = new DailyMealPlan();
            var planuriZilnice = new List<PlanAlimentarZilnic>();
            for (int i = 0; i < 7; ++i)
            {
                var dailyMealPlan = p.AddPlanAlimentarZilnic(userID, nrCalorii, GetDateForDayInCurrentWeek(i));
                planuriZilnice.Add(dailyMealPlan);
            }
            AddPlanAlimentarSaptamanal(planuriZilnice, userID);

        }

        public void AddPlanAlimentarSaptamanal(List<PlanAlimentarZilnic> planuriZilnice, int userID)
        {
            var nume = "Antrenament Săptămânal";
            var planAlimentarSaptamanal = new PlanAlimentarSaptamanal
            {
                UserID = userID,
                DataInceput = WeeklyWorkout.ClosestMondayFromPast(DateTime.Now),
                DataSfarsit = WeeklyWorkout.ClosestSundayFromFuture(DateTime.Now),
                Nume = nume,
                PlanAlimentarSaptamanal_Zilnics = new EntitySet<PlanAlimentarSaptamanal_Zilnic>()
            };
            foreach (var planAlimentarZilnic in planuriZilnice)
            {
                planAlimentarSaptamanal.PlanAlimentarSaptamanal_Zilnics.Add(new PlanAlimentarSaptamanal_Zilnic
                {
                    PlanAlimentarZilnicID = planAlimentarZilnic.ID,
                    PlanAlimentarSaptamanalID = planAlimentarSaptamanal.ID
                });
            }
            _context.PlanAlimentarSaptamanals.InsertOnSubmit(planAlimentarSaptamanal);
            _context.SubmitChanges();
        }

        public List<PlanAlimentarZilnic> GetPlanAlimentarSaptamanal(int userID, DateTime data)
        {
                DateTime endDate = data.AddDays(7);
                return (from pas in _context.PlanAlimentarSaptamanals
                        join pasz in _context.PlanAlimentarSaptamanal_Zilnics
                            on pas.ID equals pasz.PlanAlimentarSaptamanalID
                        join paz in _context.PlanAlimentarZilnics
                            on pasz.PlanAlimentarZilnicID equals paz.ID
                        where pas.UserID == userID
                            && paz.Data >= data
                            && paz.Data <= endDate
                        select paz).ToList();
        }

        public ObservableCollection<MealPlanItem> GetWeeklyMealPlanForDisplay(int userID, DateTime startDate)
        {
            var weeklyMeals = GetPlanAlimentarSaptamanal(userID, startDate); 
            ObservableCollection<MealPlanItem> mealPlanItems = new ObservableCollection<MealPlanItem>();

            var mealPlans = new DailyMealPlan().GetMealPlansForRange(userID, startDate, startDate.AddDays(6));

            foreach (var plan in mealPlans)
            {
                mealPlanItems.Add(plan);
            }

            return mealPlanItems;
        }

        public int getSize()
        {
            return _context.PlanAlimentarSaptamanals.Count();
        }

    }
}
