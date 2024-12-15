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
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Sex { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string UserType { get; set; }
        public string PhysicalCondition { get; set; }

        public User()
        {
            _context = new FitnessDBDataContext();
        }

        private readonly FitnessDBDataContext _context;
        public event PropertyChangedEventHandler PropertyChanged;

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
                Height = (int)(user.Height ?? 0),
                Weight = (int)user.Kilograms,
                UserType = user.UserType,
                PhysicalCondition = user.PhysicalCondition
            };
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

        public void AddUser(string name, string password, string userType = "Utilizator", string sex = "Masculin", decimal height = 0, decimal kilograms = 0,string physicalCondition = "Unspecified")
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
                PhysicalCondition = physicalCondition
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
    }
}
