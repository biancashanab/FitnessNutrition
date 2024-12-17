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
    internal class WorkoutHistory
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int ExerciseID { get; set; }
        public DateTime ExecutionDate { get; set; }
        public TimeSpan ExecutionTime { get; set; }

        public WorkoutHistory()
        {
            _context = new FitnessDBDataContext();
        }

        private readonly FitnessDBDataContext _context;
        public event PropertyChangedEventHandler PropertyChanged;


        void addWorkoutHistory(string username, int exercitiuId, DateTime date, TimeSpan time)
        {
            var userId = _context.Utilizatoris
                .Where(u => u.Name == username)
                .Select(u => u.ID)
                .FirstOrDefault();

            if (userId == 0)
            {
                Console.WriteLine("User not found.");
                return;
            }

            var newIstoric = new IstoricAntrenamente
            {
                UserID = userId,
           //     ExercitiuID = exercitiuId,
                DataExecutie = date,
                OraExecutie = time
            };

            _context.IstoricAntrenamentes.InsertOnSubmit(newIstoric);

            try
            {
                _context.SubmitChanges();
                Console.WriteLine("Istoric antrenamente adaugat cu succes.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la adaugarea istoricului: {ex.Message}");
            }
        }

        public ObservableCollection<WorkoutHistory> GetWorkoutHistory(int userID)
        {
            ObservableCollection<WorkoutHistory> history = new ObservableCollection<WorkoutHistory>();
            var istoric = _context.IstoricAntrenamentes.Where(i => i.UserID == userID).ToList();

            foreach (var a in istoric)
            {
                history.Add(new WorkoutHistory
                {
                    UserID = a.UserID,
                //  ExerciseID = a.ExercitiuID,
                    ExecutionDate = a.DataExecutie,
                    ExecutionTime = a.OraExecutie
                });
            }
            return history;
        }






    }
}
