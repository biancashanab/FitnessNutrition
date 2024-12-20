using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness.Models;

namespace Fitness.Models
{
    public class WeeklyWorkout : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }


        private readonly FitnessDBDataContext _context;
        public event PropertyChangedEventHandler PropertyChanged;

        public WeeklyWorkout()
        {
            _context = new FitnessDBDataContext();
        }

        public static DateTime ClosestMondayFromPast(DateTime currentDate)
        {
            int daysSinceMonday = (7 + (currentDate.DayOfWeek - DayOfWeek.Monday)) % 7;
            return currentDate.AddDays(-daysSinceMonday);
        }

        public static DateTime ClosestSundayFromFuture(DateTime currentDate)
        {
            int daysUntilSunday = (7 - (currentDate.DayOfWeek - DayOfWeek.Sunday)) % 7;
            return currentDate.AddDays(daysUntilSunday);
        }


        public List<AntrenamentZilnic> GetAntrenamentSaptamanal(int userID, DateTime data)
        {
            return _context.AntrenamentSaptamanals
                .Where(asap => asap.UserID == userID &&
                              asap.DataInceput <= ClosestMondayFromPast(data) &&
                              asap.DataSfarsit >= ClosestSundayFromFuture(data))
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


        public List<string> GenereazaGrupeMusculare(int numarZile)
        {
            List<string> grupeMusculare = new List<string>();

            switch (numarZile)
            {
                case 3:
                    grupeMusculare = new List<string> { "Push", "Pull", "Legs" };
                    break;
                case 4:
                    grupeMusculare = new List<string> { "biceps", "spate", "legs", "umeri" };
                    break;
                case 5:
                    grupeMusculare = new List<string> { "Push", "Pull", "Legs", "Push", "Pull" };
                    break;
                case 6:
                    grupeMusculare = new List<string> { "Push", "Pull", "Legs", "Push", "Pull", "Legs" };
                    break;
                default:
                    throw new ArgumentException("Numar invalid de zile. Alege intre 3, 4, 5 sau 6 zile.");
            }

            return grupeMusculare;
        }

        public List<DateTime> GenereazaZileAntrenament(int numarZile)
        {
            // Mapping of training days as weekday names
            var zileMap = new Dictionary<int, List<DayOfWeek>>()
    {
        { 3, new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Thursday, DayOfWeek.Saturday } },
        { 4, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday, DayOfWeek.Saturday } },
        { 5, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday } },
        { 6, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday } }
    };
            if (!zileMap.ContainsKey(numarZile))
            {
                throw new ArgumentException("Numar invalid de zile. Alege intre 3, 4, 5 sau 6 zile.");
            }

            DateTime today = DateTime.Now;
            int daysSinceMonday = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime closestMonday = today.AddDays(-daysSinceMonday);
            var trainingDays = zileMap[numarZile];
            List<DateTime> trainingDates = trainingDays
                .Select(day => closestMonday.AddDays((int)day - (int)DayOfWeek.Monday))
                .ToList();

            return trainingDates;
        }

        public void CreareAntrenamentSaptamanal(int numarZile, int timpMaximZilnic, int userID)
        {
            var grupeMusculare = GenereazaGrupeMusculare(numarZile);
            var zileAntrenament = GenereazaZileAntrenament(numarZile);
            var w = new DailyWorkout();
            var antrenamenteZilnice = new List<AntrenamentZilnic>();
            for (int i = 0; i < numarZile; i++)
            {
                var dailyWorkout = w.AddAntrenamentZilnic(userID, zileAntrenament[i], grupeMusculare[i], timpMaximZilnic);
                antrenamenteZilnice.Add(dailyWorkout);
            }
            AddAntrenamentSaptamanal(antrenamenteZilnice, userID);
        }

        public void AddAntrenamentSaptamanal(List<AntrenamentZilnic> antrenamenteZilnice, int userID)
        {
            var nume = "Antrenament Săptămânal";
            var antrenamentSaptamanal = new AntrenamentSaptamanal
            {
                UserID = userID,
                DataInceput = ClosestMondayFromPast(DateTime.Now),
                DataSfarsit = ClosestSundayFromFuture(DateTime.Now),
                DenumireAntrenamentSaptamanal = nume,
                AntrenamentSaptamanal_Zilnics = new EntitySet<AntrenamentSaptamanal_Zilnic>()
            };
            foreach (var antrenamentZilnic in antrenamenteZilnice)
            {
                antrenamentSaptamanal.AntrenamentSaptamanal_Zilnics.Add(new AntrenamentSaptamanal_Zilnic
                {
                    AntrenamentZilnicID = antrenamentZilnic.ID,
                    AntrenamentSaptamanalID = antrenamentSaptamanal.ID
                });
            }
            _context.AntrenamentSaptamanals.InsertOnSubmit(antrenamentSaptamanal);
            _context.SubmitChanges();
        }

        public int getSize()
        {
            return _context.AntrenamentSaptamanals.Count();
        }
    }
}