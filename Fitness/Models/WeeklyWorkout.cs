using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public ObservableCollection<WorkoutPlanItem> GetWeeklyWorkoutPlanForDisplay(int userID, DateTime startDate)
        {
            ObservableCollection<WorkoutPlanItem> workoutPlanItems = new ObservableCollection<WorkoutPlanItem>();
            for (int i = 0; i < 7; ++i)
            {
                var WorkoutPlan = new DailyWorkout().GetDailyWorkout(userID, startDate.AddDays(i));
                foreach (var plan in WorkoutPlan)
                {
                    workoutPlanItems.Add(new WorkoutPlanItem
                    {
                        Day = plan.Day,
                        Name = plan.Name,
                        Repeat = plan.Repeat,
                        Duration = plan.Duration
                    });
                }
            }
            return workoutPlanItems;
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

        public void CopyAntrenamentSaptamanal(int oldUserID, int newUserID)
        {
            try
            {
                // Step 1: Retrieve the original AntrenamentSaptamanal for the old user
                var originalAntrenament = _context.AntrenamentSaptamanals
                    .FirstOrDefault(a => a.UserID == oldUserID);

                if (originalAntrenament == null)
                {
                    throw new Exception("Antrenamentul săptămânal nu a fost găsit.");
                }

                // Step 2: Create a new AntrenamentSaptamanal for the new user
                var newAntrenamentSaptamanal = new AntrenamentSaptamanal
                {
                    UserID = newUserID,
                    DataInceput = originalAntrenament.DataInceput, // Adjust dates if necessary
                    DataSfarsit = originalAntrenament.DataSfarsit, // Adjust dates if necessary
                    DenumireAntrenamentSaptamanal = $"{originalAntrenament.DenumireAntrenamentSaptamanal} - Copie",
                    AntrenamentSaptamanal_Zilnics = new EntitySet<AntrenamentSaptamanal_Zilnic>()
                };

                // Insert the new AntrenamentSaptamanal into the context to generate its ID
                _context.AntrenamentSaptamanals.InsertOnSubmit(newAntrenamentSaptamanal);
                _context.SubmitChanges(); // Submit to generate ID for newAntrenamentSaptamanal

                // Step 3: Iterate through each linked AntrenamentZilnic in the original weekly plan
                foreach (var originalZilnicLink in originalAntrenament.AntrenamentSaptamanal_Zilnics)
                {
                    // Retrieve the original AntrenamentZilnic
                    var originalZilnic = _context.AntrenamentZilnics
                        .FirstOrDefault(z => z.ID == originalZilnicLink.AntrenamentZilnicID);

                    if (originalZilnic == null)
                    {
                        throw new Exception($"Antrenamentul zilnic cu ID-ul {originalZilnicLink.AntrenamentZilnicID} nu a fost găsit.");
                    }

                    // Step 4: Clone the AntrenamentZilnic for the new user
                    var newZilnic = new AntrenamentZilnic
                    {
                        UserID = newUserID,
                        DenumireAntrenament = originalZilnic.DenumireAntrenament,
                        Descriere = originalZilnic.Descriere,
                        Data = originalZilnic.Data,
                        ExercitiiAntrenamentZilnics = new EntitySet<ExercitiiAntrenamentZilnic>()
                    };

                    // Insert the new AntrenamentZilnic into the context to generate its ID
                    _context.AntrenamentZilnics.InsertOnSubmit(newZilnic);
                    _context.SubmitChanges(); // Submit to generate ID for newZilnic

                    // Step 5: Clone each ExercitiiAntrenamentZilnic linked to the original AntrenamentZilnic
                    foreach (var originalExercise in originalZilnic.ExercitiiAntrenamentZilnics)
                    {
                        var newExercise = new ExercitiiAntrenamentZilnic
                        {
                            ExercitiuID = originalExercise.ExercitiuID,
                            AntrenamentZilnicID = newZilnic.ID // Correctly associate with the new AntrenamentZilnic
                        };

                        // Add the new exercise association to the new AntrenamentZilnic
                        newZilnic.ExercitiiAntrenamentZilnics.Add(newExercise);
                        _context.ExercitiiAntrenamentZilnics.InsertOnSubmit(newExercise);
                    }

                    // Step 6: Link the new AntrenamentZilnic to the new AntrenamentSaptamanal
                    var newZilnicLink = new AntrenamentSaptamanal_Zilnic
                    {
                        AntrenamentSaptamanalID = newAntrenamentSaptamanal.ID, // Newly generated ID
                        AntrenamentZilnicID = newZilnic.ID // Newly generated ID
                    };
                    newAntrenamentSaptamanal.AntrenamentSaptamanal_Zilnics.Add(newZilnicLink);
                    _context.AntrenamentSaptamanal_Zilnics.InsertOnSubmit(newZilnicLink);
                }

                // Step 7: Submit all changes to save the new associations
                _context.SubmitChanges(); // Save all changes

                // Notify the user of successful copy operation
                MessageBox.Show("Antrenamentul săptămânal a fost copiat cu succes!");
            }
            catch (Exception ex)
            {
                // Handle and notify any errors that occur during the copy process
                MessageBox.Show($"Eroare la copierea antrenamentului săptămânal: {ex.Message}");
            }
        }
            
        public void Add(int UserID, DateTime date)
        {
            var nume = "Antrenament Săptămânal";
            var antrenamentSaptamanal = new AntrenamentSaptamanal
            {
                UserID = UserID,
                DataInceput = ClosestMondayFromPast(DateTime.Now),
                DataSfarsit = ClosestSundayFromFuture(DateTime.Now),
                DenumireAntrenamentSaptamanal = nume,
            };
            _context.AntrenamentSaptamanals.InsertOnSubmit(antrenamentSaptamanal);
            _context.SubmitChanges();
        }

        public int getSize()
        {
            return _context.AntrenamentSaptamanals.Count();
        }
    }
}