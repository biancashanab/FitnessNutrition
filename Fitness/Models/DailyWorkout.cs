using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Models
{
    public class DailyWorkout : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime Date { get; set; }

        public DailyWorkout()
        {
            _context = new FitnessDBDataContext();
        }

        private readonly FitnessDBDataContext _context;
        public event PropertyChangedEventHandler PropertyChanged;

        public static void AddAntrenamentZilnic(List<Exercitii> exercitii, int userID)
        {
            using (var context = new FitnessDBDataContext())
            {
                var antrenamentZilnic = new AntrenamentZilnic
                {
                    DenumireAntrenament = $"Antrenament {DateTime.Now.ToShortDateString()}",
                    Data = DateTime.Now,
                    UserID = userID,
                    Descriere = "Antrenament nou"
                };
                context.AntrenamentZilnics.InsertOnSubmit(antrenamentZilnic);
                context.SubmitChanges();
                var exercitiiAntrenament = exercitii.Select(exercitiu => new ExercitiiAntrenamentZilnic
                {
                    ExercitiuID = exercitiu.ID,
                    AntrenamentZilnicID = antrenamentZilnic.ID
                });
                context.ExercitiiAntrenamentZilnics.InsertAllOnSubmit(exercitiiAntrenament);
                context.SubmitChanges();
            }
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
    }
}
