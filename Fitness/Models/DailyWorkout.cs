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

        public void AddAntrenamentZilnic(List<Exercitii> exercitii, int userID)
        {
            var exercitiiList = string.Join(",", exercitii.Select(e => e.ID));
            var denumireAntrenament = $"Antrenament {DateTime.Now.ToShortDateString()}";
            var descriere = "Antrenament nou";
            _context.ExecuteCommand(
                "EXEC addAntrenamentZilnic @UserID = {0}, @Data = {1}, @DenumireAntrenament = {2}, @Descriere = {3}, @ExercitiiList = {4}",
                userID,
                DateTime.Now,
                denumireAntrenament,
                descriere,
                exercitiiList
            );
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
