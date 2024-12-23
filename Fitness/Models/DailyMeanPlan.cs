using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Models
{
    public class MealPlanItem
    {
        public string Day { get; set; }
        public string Breakfast { get; set; }
        public string Lunch { get; set; }
        public string Dinner { get; set; }
        public string Snack { get; set; }
    };

    public class DailyMealPlan : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime Date { get; set; }

        private readonly FitnessDBDataContext _context;
        public event PropertyChangedEventHandler PropertyChanged;

        public DailyMealPlan()
        {
            _context = new FitnessDBDataContext();
        }

        public List<Retete>CrearePlanAlimentarZilnic(int numarCalorii, int userID, DateTime data)
        {
            List<Retete> mealPlan = new List<Retete>();
            int calorii_totale = 0;
            const int eroare_acceptata = 0;

            try
            {
                do
                {
                    calorii_totale = 0;
                    mealPlan.Clear();
                    var breakfastOptions = _context.Retetes
                        .Where(r => r.TipMasa == "Mic Dejun" && r.Calorii <= numarCalorii)
                        .ToList();
                    var breakfast = breakfastOptions.OrderBy(_ => Guid.NewGuid()).FirstOrDefault();
                    if (breakfast != null)
                    {
                        mealPlan.Add(breakfast);
                        calorii_totale += breakfast.Calorii;
                    }

                    var lunchOptions = _context.Retetes
                        .Where(r => r.TipMasa == "Pranz" && r.Calorii <= numarCalorii)
                        .ToList();
                    var lunch = lunchOptions.OrderBy(_ => Guid.NewGuid()).FirstOrDefault();
                    if (lunch != null)
                    {
                        mealPlan.Add(lunch);
                        calorii_totale += lunch.Calorii;
                    }

                    var dinnerOptions = _context.Retetes
                        .Where(r => r.TipMasa == "Cina" && r.Calorii <= numarCalorii)
                        .ToList();
                    var dinner = dinnerOptions.OrderBy(_ => Guid.NewGuid()).FirstOrDefault();
                    if (dinner != null)
                    {
                        mealPlan.Add(dinner);
                        calorii_totale += dinner.Calorii;
                    }

                    var gustareOptions = _context.Retetes
                        .Where(r => r.TipMasa == "Gustare" && r.Calorii <= numarCalorii)
                        .ToList();
                    var gustrae = gustareOptions.OrderBy(_ => Guid.NewGuid()).FirstOrDefault();

                } while (Math.Abs(calorii_totale - numarCalorii) > eroare_acceptata);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Exception occurred: {ex.Message}");
                Console.WriteLine("[ERROR] Stack Trace:");
                Console.WriteLine(ex.StackTrace);
            }

            return mealPlan;
        }
            
        public PlanAlimentarZilnic AddPlanAlimentarZilnic(int userID, int numarCalorii, DateTime data)
        {
            var reteteList = CrearePlanAlimentarZilnic(numarCalorii, 1, data);
            var numePlanZilnic = $"Plan Zilnic {data}";
            var planAlimentar = new PlanAlimentarZilnic
            {
                UserID = userID,
                Data = data,
                Nume = numePlanZilnic
            };
            _context.PlanAlimentarZilnics.InsertOnSubmit(planAlimentar);
            _context.SubmitChanges();

            var retetePlanAlimentarZilnic = reteteList.Select(reteta => new RetetePlanAlimentarZilnic
            {
                PlanAlimentarZilnicID = planAlimentar.ID,
                ReteteID = reteta.ID
            }).ToList();

            _context.RetetePlanAlimentarZilnics.InsertAllOnSubmit(retetePlanAlimentarZilnic);
            _context.SubmitChanges();
            return planAlimentar;
        }

        public List<Retete> GetPlanAlimentarZilnic(int userID, DateTime data)
        {
            return _context.PlanAlimentarZilnics
                .Where(paz => paz.UserID == userID && paz.Data.Date == data.Date)
                .Join(_context.RetetePlanAlimentarZilnics,
                    paz => paz.ID,
                    rpaz => rpaz.PlanAlimentarZilnicID,
                    (paz, rpaz) => rpaz.ReteteID)
                .Join(_context.Retetes,
                    retId => retId,
                    ret => ret.ID,
                    (retId, ret) => ret)
                .ToList();
        }

        public PlanAlimentarZilnic GetPlanAlimentarByDate(int userID, DateTime data)
        {
            var pln = (PlanAlimentarZilnic)_context.PlanAlimentarZilnics.FirstOrDefault(az => az.UserID == userID && az.Data == data.Date);
            if (pln != null)
                return pln;
            else
                return null;
        }

        public ObservableCollection<MealPlanItem> GetMealPlansForRange(int userID, DateTime startDate, DateTime endDate)
        {
            ObservableCollection<MealPlanItem> mealPlanItems = new ObservableCollection<MealPlanItem>();

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var meals = GetPlanAlimentarZilnic(userID, date);

                var dailyMeal = new MealPlanItem
                {
                    Day = date.ToString("dddd, dd MMM yyyy"),
                    Breakfast = meals.FirstOrDefault(m => m.TipMasa == "Mic Dejun")?.Nume ?? "N/A",
                    Lunch = meals.FirstOrDefault(m => m.TipMasa == "Pranz")?.Nume ?? "N/A",
                    Dinner = meals.FirstOrDefault(m => m.TipMasa == "Cina")?.Nume ?? "N/A",
                    Snack = meals.FirstOrDefault(m => m.TipMasa == "Gustare")?.Nume ?? "N/A"
                };
                mealPlanItems.Add(dailyMeal);
            }
            return mealPlanItems;
        }

        public List<DateTime> GetAllPlanDatesForUser(int userID)
        {
            try
            {
                var dates = _context.PlanAlimentarZilnics
                    .Where(paz => paz.UserID == userID)
                    .Select(paz => paz.Data.Date)
                    .Distinct()
                    .OrderBy(date => date) // Sortează datele în ordine crescătoare
                    .ToList();

                return dates;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Exception occurred: {ex.Message}");
                Console.WriteLine("[ERROR] Stack Trace:");
                Console.WriteLine(ex.StackTrace);
                return new List<DateTime>();
            }
        }

    }
}