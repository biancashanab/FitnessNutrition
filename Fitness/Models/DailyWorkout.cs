using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Models
{
    public struct WorkoutPlanItem
    {
        public string Day { get; set; }
        public string Name { get; set; }
        public string Repeat { get; set; }
        public string Duration { get; set; }
    };

    public class DailyWorkout : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime Date { get; set; }

        private readonly FitnessDBDataContext _context;
        public event PropertyChangedEventHandler PropertyChanged;

        public DailyWorkout()
        {
            _context = new FitnessDBDataContext();
        }

        public List<Exercitii> CreareAntrenamentZilnic(string grupaMusculara, int timpMaxim)
        {
            var exercitii = _context.Exercitiis
                .Where(e => e.GrupaMusculara == grupaMusculara)
                .OrderBy(x => Guid.NewGuid())
                .ToList();

            var workoutPlan = new List<Exercitii>();
            int totalTimpEstimare = 0;

            foreach (var exercitiu in exercitii)
            {
                if (totalTimpEstimare + exercitiu.TimpEstimareExecutie > timpMaxim)
                {
                    break;
                }

                workoutPlan.Add(exercitiu);
                totalTimpEstimare += exercitiu.TimpEstimareExecutie.GetValueOrDefault();
            }
            return workoutPlan;
        }

        public AntrenamentZilnic AddAntrenamentZilnic(int userID, DateTime day, string grupaMusculara, int timpMaxim)
        {
            var denumireAntrenament = $"Antrenament {day}";
            var descriere = "Antrenament";
            var exercitii = CreareAntrenamentZilnic(grupaMusculara, timpMaxim);
            var antrenamentZilnic = new AntrenamentZilnic
            {
                UserID = userID,
                Data = day,
                DenumireAntrenament = denumireAntrenament,
                Descriere = descriere,
            };
            _context.AntrenamentZilnics.InsertOnSubmit(antrenamentZilnic);
            _context.SubmitChanges();
            var exercitiiAntrenamentZilnic = exercitii.Select(exercitiu => new ExercitiiAntrenamentZilnic
            {
                AntrenamentZilnicID = antrenamentZilnic.ID,
                ExercitiuID = exercitiu.ID
            }).ToList();

            _context.ExercitiiAntrenamentZilnics.InsertAllOnSubmit(exercitiiAntrenamentZilnic);
            _context.SubmitChanges();

            return antrenamentZilnic;
        }

        public List<Exercitii> GetAntrenamentZilnic(int userID, DateTime data)
        {
            return _context.AntrenamentZilnics
                .Where(az => az.UserID == userID && az.Data.Value.Date == data.Date)
                .Join(_context.ExercitiiAntrenamentZilnics,
                    az => az.ID,
                    eaz => eaz.AntrenamentZilnicID,
                    (az, eaz) => eaz.ExercitiuID)
                .Join(_context.Exercitiis,
                    exId => exId,
                    ex => ex.ID,
                    (exId, ex) => ex)
                .ToList();
        }

        public ObservableCollection<WorkoutPlanItem> GetDailyWorkout(int userID, DateTime data)
        {
            var exercitii = GetAntrenamentZilnic(userID, data);
            ObservableCollection<WorkoutPlanItem> workoutPlanItems = new ObservableCollection<WorkoutPlanItem>();

            if (exercitii == null || exercitii.Count == 0)
            {
                var noWorkoutItem = new WorkoutPlanItem
                {
                    Day = data.ToString("dddd, dd MMM yyyy"),
                    Name = "N/A", // Nu sunt antrenamente
                    Repeat = "N/A",
                    Duration = "N/A"
                };
                workoutPlanItems.Add(noWorkoutItem);
            }
            else
            {
                foreach (var exercitiu in exercitii)
                {
                    var workoutPlanItem = new WorkoutPlanItem
                    {
                        Day = data.ToString("dddd, dd MMM yyyy"),
                        Name = exercitiu.DenumireExercitiu,
                        Repeat = exercitiu.Repetari.ToString(),
                        Duration = exercitiu.TimpEstimareExecutie?.ToString() + " min"
                    };
                    workoutPlanItems.Add(workoutPlanItem);
                }
            }
            return workoutPlanItems;
        }

        public AntrenamentZilnic GetAntrenamentZilnicByDate(int userID, DateTime data)
        {
            var ant =  (AntrenamentZilnic)_context.AntrenamentZilnics.FirstOrDefault(az => az.UserID == userID && az.Data.Value.Date == data.Date);
            if (ant != null)
                return ant;
            else
                return null;
        }
    }
}
