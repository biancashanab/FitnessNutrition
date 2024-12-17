using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Models
{
    public class MealPlanHistory : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int MealPlanID { get; set; }
        public DateTime Date { get; set; }
        public string MealType { get; set; }  

        public MealPlanHistory()
        {
            _context = new FitnessDBDataContext();
        }

        private readonly FitnessDBDataContext _context;
        public event PropertyChangedEventHandler PropertyChanged;

        void addMealPlanHistory(string username, int mealPlanId, DateTime date, string type)
        {
            var userId = _context.Utilizatoris.Where(u => u.Name == username)
                .Select(u => u.ID).FirstOrDefault();

            if (userId == 0)
            {
                Console.WriteLine("User not found.");
                return;
            }

            var newIstoric = new PlanAlimentarIstoric
            {
                UserID = userId,
         //       RetetaID = MealPlanID,
                Data = date,
                TipMasa = type
            };

            _context.PlanAlimentarIstorics.InsertOnSubmit(newIstoric);

            try
            {
                _context.SubmitChanges();
                Console.WriteLine("Istoric planuri mese adaugat cu succes.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la adaugarea istoricului planuri mese: {ex.Message}");
            }
        }

        public ObservableCollection<MealPlanHistory> getUserHistory(int userID)
        {
            ObservableCollection<MealPlanHistory> history = new ObservableCollection<MealPlanHistory>();
            var istoric = _context.PlanAlimentarIstorics.Where(i => i.UserID == userID).ToList();

            foreach (var item in istoric)
            {
                history.Add(new MealPlanHistory
                {
                    ID = item.ID,
                    UserID = item.UserID,
               //     MealPlanID = item.RetetaID,
                    Date = item.Data,
                    MealType = item.TipMasa
                });
            }
            return history;
        }
    }

}
