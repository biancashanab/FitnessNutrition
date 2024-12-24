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
    public class User : INotifyPropertyChanged
    {
        // User Properties
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        private string _sex;
        public string Sex
        {
            get => _sex;
            set
            {
                if (_sex != value)
                {
                    _sex = value;
                    OnPropertyChanged(nameof(Sex));
                }
            }
        }

        private int _height;
        public int Height
        {
            get => _height;
            set
            {
                if (_height != value)
                {
                    _height = value;
                    OnPropertyChanged(nameof(Height));
                }
            }
        }

        private int _weight;
        public int Weight
        {
            get => _weight;
            set
            {
                if (_weight != value)
                {
                    _weight = value;
                    OnPropertyChanged(nameof(Weight));
                }
            }
        }

        private string _userType;
        public string UserType
        {
            get => _userType;
            set
            {
                if (_userType != value)
                {
                    _userType = value;
                    OnPropertyChanged(nameof(UserType));
                }
            }
        }

        private string _physicalCondition;
        public string PhysicalCondition
        {
            get => _physicalCondition;
            set
            {
                if (_physicalCondition != value)
                {
                    _physicalCondition = value;
                    OnPropertyChanged(nameof(PhysicalCondition));
                }
            }
        }

        private string _activity;
        public string Activity
        {
            get => _activity;
            set
            {
                if (_activity != value)
                {
                    _activity = value;
                    OnPropertyChanged(nameof(Activity));
                }
            }
        }

        private readonly FitnessDBDataContext _context;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public User()
        {
            _context = new FitnessDBDataContext();
        }

        public User GetUser(string name)
        {
            var user = _context.Utilizatoris.FirstOrDefault(us => us.Name == name);

            if (user == null)
            {
                return null;
            }

            return new User
            {
                Id = user.ID,
                Name = user.Name,
                Password = user.HashedPassword,
                Sex = user.Sex,
                Height = user.Height.HasValue ? (int)user.Height : 0,
                Weight = user.Kilograms.HasValue ? (int)user.Kilograms : 0,
                UserType = user.UserType,
                PhysicalCondition = user.PhysicalCondition,
                Activity = user.Activity
            };
        }

        public void AddUser(string name, string password,
            string userType = "Utilizator", string sex = "Masculin", decimal height = 0,
            decimal kilograms = 0, string physicalCondition = "Unspecified", string activity = "Sedentary")
        {
            var existingUser = _context.Utilizatoris.FirstOrDefault(u => u.Name == name);

            if (existingUser != null)
            {
                Console.WriteLine("Utilizatorul exista deja.");
                return;
            }

            var newUser = new Utilizatori
            {
                Name = name,
                HashedPassword = HashPasswordSHA256(password),
                UserType = userType,
                Sex = sex,
                Height = height,
                Kilograms = kilograms,
                PhysicalCondition = physicalCondition,
                Activity = activity
            };

            _context.Utilizatoris.InsertOnSubmit(newUser);

            try
            {
                _context.SubmitChanges();
                Console.WriteLine("Utilizator adaugat cu succes.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la adaugarea utilizatorului: {ex.Message}");
            }
        }

        public static string HashPasswordSHA256(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);

                return string.Join("", hash.Select(b => b.ToString("x2")));
            }
        }

        // Update Methods for Each Property

        // Update Name
        public void UpdateName(string newName)
        {
            try
            {
                var user = _context.Utilizatoris.FirstOrDefault(u => u.ID == this.Id);
                if (user != null)
                {
                    user.Name = newName;
                    _context.SubmitChanges();

                    // Update local property
                    this.Name = newName;
                }
                else
                {
                    throw new Exception("Utilizatorul nu a fost găsit.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Eroare la actualizarea numelui: {ex.Message}");
            }
        }

        // Update Sex
        public void UpdateSex(string newSex)
        {
            try
            {
                var user = _context.Utilizatoris.FirstOrDefault(u => u.ID == this.Id);
                if (user != null)
                {
                    user.Sex = newSex;
                    _context.SubmitChanges();

                    // Update local property
                    this.Sex = newSex;
                }
                else
                {
                    throw new Exception("Utilizatorul nu a fost găsit.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Eroare la actualizarea sexului: {ex.Message}");
            }
        }

        // Update Height
        public void UpdateHeight(int newHeight)
        {
            try
            {
                var user = _context.Utilizatoris.FirstOrDefault(u => u.ID == this.Id);
                if (user != null)
                {
                    user.Height = newHeight;
                    _context.SubmitChanges();

                    // Update local property
                    this.Height = newHeight;
                }
                else
                {
                    throw new Exception("Utilizatorul nu a fost găsit.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Eroare la actualizarea înălțimii: {ex.Message}");
            }
        }

        // Update Weight
        public void UpdateWeight(int newWeight)
        {
            try
            {
                var user = _context.Utilizatoris.FirstOrDefault(u => u.ID == this.Id);
                if (user != null)
                {
                    user.Kilograms = newWeight;
                    _context.SubmitChanges();

                    // Update local property
                    this.Weight = newWeight;
                }
                else
                {
                    throw new Exception("Utilizatorul nu a fost găsit.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Eroare la actualizarea greutății: {ex.Message}");
            }
        }

        // Update UserType
        public void UpdateUserType(string newUserType)
        {
            try
            {
                var user = _context.Utilizatoris.FirstOrDefault(u => u.ID == this.Id);
                if (user != null)
                {
                    user.UserType = newUserType;
                    _context.SubmitChanges();

                    // Update local property
                    this.UserType = newUserType;
                }
                else
                {
                    throw new Exception("Utilizatorul nu a fost găsit.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Eroare la actualizarea tipului utilizatorului: {ex.Message}");
            }
        }

        // Update PhysicalCondition
        public void UpdatePhysicalCondition(string newPhysicalCondition)
        {
            try
            {
                var user = _context.Utilizatoris.FirstOrDefault(u => u.ID == this.Id);
                if (user != null)
                {
                    user.PhysicalCondition = newPhysicalCondition;
                    _context.SubmitChanges();

                    this.PhysicalCondition = newPhysicalCondition;
                }
                else
                {
                    throw new Exception("Utilizatorul nu a fost găsit.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Eroare la actualizarea condiției fizice: {ex.Message}");
            }
        }

        // Update Activity
        public void UpdateActivity(string newActivity)
        {
            try
            {
                var user = _context.Utilizatoris.FirstOrDefault(u => u.ID == this.Id);
                if (user != null)
                {
                    user.Activity = newActivity;
                    _context.SubmitChanges();

                    this.Activity = newActivity;
                }
                else
                {
                    throw new Exception("Utilizatorul nu a fost găsit.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Eroare la actualizarea nivelului de activitate: {ex.Message}");
            }
        }

        public void UpdatePassword(string newPassword)
        {
            try
            {
                var user = _context.Utilizatoris.FirstOrDefault(u => u.ID == this.Id);
                if (user != null)
                {
                    user.HashedPassword = HashPasswordSHA256(newPassword);
                    _context.SubmitChanges();

                    // Update local property
                    this.Password = HashPasswordSHA256(newPassword);
                }
                else
                {
                    throw new Exception("Utilizatorul nu a fost găsit.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Eroare la actualizarea parolei: {ex.Message}");
            }
        }

        public void DeleteUser(int userId)
        {
            try
            {
                // Find the user in the database
                var user = _context.Utilizatoris.FirstOrDefault(u => u.ID == userId);

                if (user == null)
                {
                    throw new Exception("Utilizatorul nu a fost găsit.");
                }

                // Delete related records in linking tables
                // ExercitiiAntrenamentZilnic
                var exerciseDailyTrainings = _context.ExercitiiAntrenamentZilnics
                    .Where(link => _context.AntrenamentZilnics
                                    .Any(a => a.UserID == userId && a.ID == link.AntrenamentZilnicID));
                _context.ExercitiiAntrenamentZilnics.DeleteAllOnSubmit(exerciseDailyTrainings);

                // RetetePlanAlimentarZilnic
                var recipesDailyPlans = _context.RetetePlanAlimentarZilnics
                    .Where(link => _context.PlanAlimentarZilnics
                                    .Any(p => p.UserID == userId && p.ID == link.PlanAlimentarZilnicID));
                _context.RetetePlanAlimentarZilnics.DeleteAllOnSubmit(recipesDailyPlans);

                // PlanAlimentarSaptamanal_Zilnic
                var weeklyDailyPlansLinks = _context.PlanAlimentarSaptamanal_Zilnics
                    .Where(link => _context.PlanAlimentarSaptamanals
                                    .Any(p => p.UserID == userId && p.ID == link.PlanAlimentarSaptamanalID));
                _context.PlanAlimentarSaptamanal_Zilnics.DeleteAllOnSubmit(weeklyDailyPlansLinks);

                // AntrenamentSaptamanal_Zilnic
                var weeklyDailyTrainingsLinks = _context.AntrenamentSaptamanal_Zilnics
                    .Where(link => _context.AntrenamentSaptamanals
                                    .Any(a => a.UserID == userId && a.ID == link.AntrenamentSaptamanalID));
                _context.AntrenamentSaptamanal_Zilnics.DeleteAllOnSubmit(weeklyDailyTrainingsLinks);

                // Delete related records in dependent tables
                // PlanAlimentarSaptamanal
                var weeklyPlans = _context.PlanAlimentarSaptamanals.Where(p => p.UserID == userId);
                _context.PlanAlimentarSaptamanals.DeleteAllOnSubmit(weeklyPlans);

                // AntrenamentSaptamanal
                var weeklyTrainings = _context.AntrenamentSaptamanals.Where(a => a.UserID == userId);
                _context.AntrenamentSaptamanals.DeleteAllOnSubmit(weeklyTrainings);

                // PlanAlimentarZilnic
                var dailyPlans = _context.PlanAlimentarZilnics.Where(p => p.UserID == userId);
                _context.PlanAlimentarZilnics.DeleteAllOnSubmit(dailyPlans);

                // AntrenamentZilnic
                var dailyTrainings = _context.AntrenamentZilnics.Where(a => a.UserID == userId);
                _context.AntrenamentZilnics.DeleteAllOnSubmit(dailyTrainings);

                // IstoricGreutate
                var weightHistory = _context.IstoricGreutates.Where(w => w.UserID == userId);
                _context.IstoricGreutates.DeleteAllOnSubmit(weightHistory);

                // Finally, delete the user
                _context.Utilizatoris.DeleteOnSubmit(user);

                // Commit changes
                _context.SubmitChanges();

                Console.WriteLine("Utilizator și toate referințele sale au fost șterse cu succes.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Eroare la ștergerea utilizatorului: {ex.Message}");
            }
        }

        public ObservableCollection<User> GetAllUsers()
        {
            // Assuming `Users` is a DbSet<User> in your DbContext
            var users = _context.Utilizatoris
                                .Select(u => new User
                                {
                                    Id = u.ID,
                                    Name = u.Name,
                                    UserType = u.UserType
                                })
                                .ToList();

            return new ObservableCollection<User>(users);
        }
    }
}
