using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Models
{
    public class WeeklyWorkout : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        private readonly FitnessDBDataContext _context;
        public event PropertyChangedEventHandler PropertyChanged;

        public WeeklyWorkout()
        {
            _context = new FitnessDBDataContext();
        }

        public void AddAntrenamentSaptamanal
            (List<AntrenamentZilnic> antrenamenteZilnice, int userID)
        {
            var antrenamentSaptamanal = new AntrenamentSaptamanal
            {
                DenumireAntrenamentSaptamanal = $"Antrenament Săptămânal {DateTime.Now.ToShortDateString()}",
                DataInceput = DateTime.Now,
                DataSfarsit = DateTime.Now.AddDays(7),
                UserID = userID,
                Descriere = "Antrenament săptămânal nou"
            };
            _context.AntrenamentSaptamanals.InsertOnSubmit(antrenamentSaptamanal);
            _context.SubmitChanges();
            var antrenamenteLegatura = antrenamenteZilnice.Select(antrenament => new AntrenamentSaptamanal_Zilnic
            {
                AntrenamentSaptamanalID = antrenamentSaptamanal.ID,
                AntrenamentZilnicID = antrenament.ID
            });
            _context.AntrenamentSaptamanal_Zilnics.InsertAllOnSubmit(antrenamenteLegatura);
            _context.SubmitChanges();
        }

        public List<AntrenamentZilnic> GetAntrenamentSaptamanal(int userID, DateTime data)
        {
            return _context.AntrenamentSaptamanals
                .Where(asap => asap.UserID == userID &&
                              asap.DataInceput <= data &&
                              asap.DataSfarsit >= data)
                .Join(_context.AntrenamentSaptamanal_Zilnics,
                    asap => asap.ID,
                    asz => asz.AntrenamentSaptamanalID,
                    (asap, asz) => asz.AntrenamentZilnicID)
                .Join(_context.AntrenamentZilnics,
                    azId => azId,
                    az => az.ID,
                    (azId, az) => az)
                .ToList();
        }
    
    }
}