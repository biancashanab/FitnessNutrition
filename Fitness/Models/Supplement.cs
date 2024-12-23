using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Fitness.Models
{
    public class Supplement : INotifyPropertyChanged
    {
        private int _supplementID;
        private string _name;
        private string _description;
        private string _category;
        private string _dosage;
        private string _benefits;

        private readonly FitnessDBDataContext _context;

        public Supplement()
        {
            _context = new FitnessDBDataContext();
        }

        public int SupplementID
        {
            get => _supplementID;
            set
            {
                if (_supplementID != value)
                {
                    _supplementID = value;
                    OnPropertyChanged(nameof(SupplementID));
                }
            }
        }

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

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public string Category
        {
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged(nameof(Category));
                }
            }
        }

        public string Dosage
        {
            get => _dosage;
            set
            {
                if (_dosage != value)
                {
                    _dosage = value;
                    OnPropertyChanged(nameof(Dosage));
                }
            }
        }

        public string Benefits
        {
            get => _benefits;
            set
            {
                if (_benefits != value)
                {
                    _benefits = value;
                    OnPropertyChanged(nameof(Benefits));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Retrieve a supplement by name
        public Supplement GetSupplement(string name)
        {
            var supplement = _context.Suplimentes.FirstOrDefault(s => s.Name == name);

            if (supplement == null)
            {
                throw new ArgumentNullException("There is no supplement with that name!");
            }

            return new Supplement
            {
                SupplementID = supplement.SupplementID,
                Name = supplement.Name,
                Description = supplement.Description,
                Category = supplement.Category,
                Dosage = supplement.Dosage,
                Benefits = supplement.Benefits
            };
        }

        // Add a new supplement
        public void AddSupplement(string name, string description, string category, string dosage, string benefits)
        {
            var newSupplement = new Suplimente
            {
                Name = name,
                Description = description,
                Category = category,
                Dosage = dosage,
                Benefits = benefits
            };

            _context.Suplimentes.InsertOnSubmit(newSupplement);

            try
            {
                _context.SubmitChanges();
                Console.WriteLine("Supplement added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding supplement: {ex.Message}");
            }
        }

        // Delete a supplement by name and category
        public void DeleteSupplement(string name, string category)
        {
            try
            {
                var supplementToDelete = _context.Suplimentes.FirstOrDefault(s => s.Name == name && s.Category == category);

                if (supplementToDelete == null)
                {
                    Console.WriteLine("Error: Supplement not found.");
                    return;
                }

                _context.Suplimentes.DeleteOnSubmit(supplementToDelete);
                _context.SubmitChanges();

                Console.WriteLine("Supplement deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting supplement: {ex.Message}");
            }
        }

        // Retrieve a list of supplements (e.g., for a specific day)
        public List<Suplimente> GetSupplementsForDay(int day)
        {
            // Example logic: Retrieve a subset or based on certain criteria
            // Here, we'll just return all supplements for simplicity
            return _context.Suplimentes.ToList();
        }

        public ObservableCollection<Supplement> GetAllSupplements()
        {
            var supplements = new ObservableCollection<Supplement>(_context.Suplimentes.Select(s => new Supplement
            {
                SupplementID = s.SupplementID,
                Name = s.Name,
                Description = s.Description,
                Category = s.Category,
                Dosage = s.Dosage,
                Benefits = s.Benefits
            }).ToList()
            );

            return supplements;
        }
    }
}
