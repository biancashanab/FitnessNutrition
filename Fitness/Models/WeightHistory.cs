using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Models
{
    public class WeightHistory: INotifyPropertyChanged
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime Date { get; set; }
        public decimal Weight { get; set; }

        public WeightHistory()
        {
            _context = new FitnessDBDataContext();
        }

        private readonly FitnessDBDataContext _context;
        public event PropertyChangedEventHandler PropertyChanged;

        void addWeight(string username, DateTime date, decimal weight)
        {
            var userId = _context.Utilizatoris.Where(u => u.Name == username)
                .Select(u => u.ID).FirstOrDefault();

            if (userId == 0)
            {
                Console.WriteLine("User not found.");
                return;
            }

            var newIstoricGreutate = new IstoricGreutate
            {
                UserID = userId,
                Data = date,
                Greutate = weight
            };

            _context.IstoricGreutates.InsertOnSubmit(newIstoricGreutate);

            try
            {
                _context.SubmitChanges();
                Console.WriteLine("Istoric greutate adaugat cu succes.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la adaugarea istoricului greutate: {ex.Message}");
            }
        }

        public ObservableCollection<WeightHistory> getUserHistory(int userID)
        {
            ObservableCollection<WeightHistory> history = new ObservableCollection<WeightHistory>();
            var istoric = _context.IstoricGreutates.Where(i => i.UserID == userID).ToList();

            foreach (var a in istoric)
            {
                history.Add(new WeightHistory
                {
                    UserID = a.UserID,
                    Date = a.Data,
                    Weight = a.Greutate
                });
            }
            return history;
        }

    }
}
