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
    public class Exercise : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string ExerciseName { get; set; }
        public int Repetitions { get; set; }
        public string MuscleGroup { get; set; }
        public int Sets { get; set; }
        public string Description { get; set; }
        public int EstimatedExecutionTime { get; set; }

        public Exercise()
        {
            _context = new FitnessDBDataContext();
        }

        private readonly FitnessDBDataContext _context;
        public event PropertyChangedEventHandler PropertyChanged;

        public Exercise GetExercise(string name)
        {
            var exercise = _context.Exercitiis.First(e => e.DenumireExercitiu == name);

            if (exercise == null)
            {
                throw new ArgumentNullException("There is no exercise with that name!");
            }

            return new Exercise
            {
                ExerciseName = exercise.DenumireExercitiu,
                Repetitions = (int)exercise.Repetari,
                MuscleGroup = exercise.GrupaMusculara,
                Sets = (int)exercise.Seturi,
                Description = exercise.Descriere,
                EstimatedExecutionTime = (int)exercise.TimpEstimareExecutie,
            };
        }

        public void AddExercise(string denumireExercitiu, string grupaMusculara, string descriere, int repetari, int seturi, int timpEstimareExecutie)
        {
            var newExercitiu = new Exercitii
            {
                DenumireExercitiu = denumireExercitiu,
                GrupaMusculara = grupaMusculara,
                Descriere = descriere,
                Repetari = repetari,
                Seturi = seturi,
                TimpEstimareExecutie = timpEstimareExecutie
            };

            _context.Exercitiis.InsertOnSubmit(newExercitiu);

            try
            {
                _context.SubmitChanges();
                Console.WriteLine("Exercitiu adaugat cu succes.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la adaugarea exercitiului: {ex.Message}");
            }
        }

        public void DeleteExercise(string denumireExercitiu, string grupaMusculara)
        {
            try
            {
                var exercitiuToDelete = _context.Exercitiis.FirstOrDefault(e => e.DenumireExercitiu == denumireExercitiu && e.GrupaMusculara == grupaMusculara);

                if (exercitiuToDelete == null)
                {
                    Console.WriteLine("Eroare: Exercitiul nu a fost gasit.");
                    return;
                }

                _context.Exercitiis.DeleteOnSubmit(exercitiuToDelete);
                _context.SubmitChanges();

                Console.WriteLine("Exercitiu sters cu succes.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la stergerea exercitiului: {ex.Message}");
            }
        }
    }
}
