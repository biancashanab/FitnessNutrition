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
using System.Windows.Controls;
using System.Windows;

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
            var nume = "Plan Alimentar Saptamanal";
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
        
        public void CopyPlanAlimentarSaptamanal(int oldUserID, int newUserID)
        {
            try
            {
                // Step 1: Retrieve the original PlanAlimentarSaptamanal
                var originalPlan = _context.PlanAlimentarSaptamanals
                    .FirstOrDefault(p => p.UserID == oldUserID);

                if (originalPlan == null)
                {
                    throw new Exception($"Plan alimentar săptămânal nu a fost găsit.");
                }

                // Step 2: Create a new PlanAlimentarSaptamanal for the new user
                var newPlanAlimentarSaptamanal = new PlanAlimentarSaptamanal
                {
                    UserID = newUserID,
                    Nume = $"{originalPlan.Nume} - Copie",
                    DataInceput = originalPlan.DataInceput, // Optionally adjust dates
                    DataSfarsit = originalPlan.DataSfarsit
                };

                // Insert the new PlanAlimentarSaptamanal and submit changes to generate ID
                _context.PlanAlimentarSaptamanals.InsertOnSubmit(newPlanAlimentarSaptamanal);
                _context.SubmitChanges();

                // Step 3: Iterate through each linked PlanAlimentarZilnic
                foreach (var originalZilnicLink in originalPlan.PlanAlimentarSaptamanal_Zilnics)
                {
                    var originalZilnic = _context.PlanAlimentarZilnics
                        .FirstOrDefault(z => z.ID == originalZilnicLink.PlanAlimentarZilnicID);

                    if (originalZilnic == null)
                    {
                        throw new Exception($"Plan alimentar zilnic cu ID-ul {originalZilnicLink.PlanAlimentarZilnicID} nu a fost găsit.");
                    }

                    // Step 4: Clone the PlanAlimentarZilnic
                    var newZilnic = new PlanAlimentarZilnic
                    {
                        Data = originalZilnic.Data,
                        Nume = originalZilnic.Nume,
                        UserID = newUserID
                    };

                    // Insert the new PlanAlimentarZilnic and submit changes to generate ID
                    _context.PlanAlimentarZilnics.InsertOnSubmit(newZilnic);
                    _context.SubmitChanges();

                    // Step 5: Clone RetetePlanAlimentarZilnic links
                    var originalReteteLinks = _context.RetetePlanAlimentarZilnics
                        .Where(rpz => rpz.PlanAlimentarZilnicID == originalZilnic.ID)
                        .ToList();

                    foreach (var originalReteteLink in originalReteteLinks)
                    {
                        var newReteteLink = new RetetePlanAlimentarZilnic
                        {
                            ReteteID = originalReteteLink.ReteteID,
                            PlanAlimentarZilnicID = newZilnic.ID
                        };
                        _context.RetetePlanAlimentarZilnics.InsertOnSubmit(newReteteLink);
                    }

                    // Step 6: Link the new PlanAlimentarZilnic to the new PlanAlimentarSaptamanal
                    var newZilnicLink = new PlanAlimentarSaptamanal_Zilnic
                    {
                        PlanAlimentarSaptamanalID = newPlanAlimentarSaptamanal.ID,
                        PlanAlimentarZilnicID = newZilnic.ID
                    };
                    _context.PlanAlimentarSaptamanal_Zilnics.InsertOnSubmit(newZilnicLink);
                }

                // Step 7: Submit all changes to the database
                _context.SubmitChanges();

                // Notify the user of success
                MessageBox.Show("Planul alimentar săptămânal a fost copiat cu succes!");
            }
            catch (Exception ex)
            {
                // Handle and notify any errors that occur during the copy process
                MessageBox.Show($"Eroare la copierea planului alimentar săptămânal: {ex.Message}");
            }
        }

        public void Add(int UserID, DateTime date)
        {
            var nume = "Antrenament Săptămânal";
            var planAlimentarSaptamanal = new PlanAlimentarSaptamanal
            {
                UserID = UserID,
                DataInceput = WeeklyWorkout.ClosestMondayFromPast(DateTime.Now),
                DataSfarsit = WeeklyWorkout.ClosestSundayFromFuture(DateTime.Now),
                Nume = nume,
                PlanAlimentarSaptamanal_Zilnics = new EntitySet<PlanAlimentarSaptamanal_Zilnic>()
            };
            _context.PlanAlimentarSaptamanals.InsertOnSubmit(planAlimentarSaptamanal);
            _context.SubmitChanges();
        }

        public int getSize()
        {
            return _context.PlanAlimentarSaptamanals.Count();
        }

    }
}
