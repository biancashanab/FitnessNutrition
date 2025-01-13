using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Fitness.Models;

namespace Fitness.ViewModels
{
    public class SuplementsVM : INotifyPropertyChanged
    {
        // Event for property changes
        public event PropertyChangedEventHandler PropertyChanged;

        // Private fields
        private readonly FitnessDBDataContext _context;
        private string _pageTitle;
        private string _searchText;
        private string _filterText;

        // Public Properties
        private ObservableCollection<Supplement> _supplements;
        public ObservableCollection<Supplement> Supplements
        {
            get => _supplements;
            set
            {
                if (_supplements != value)
                {
                    _supplements = value;
                    OnPropertyChanged(nameof(Supplements));
                }
            }
        }

        public string PageTitle
        {
            get => _pageTitle;
            set
            {
                if (_pageTitle != value)
                {
                    _pageTitle = value;
                    OnPropertyChanged(nameof(PageTitle));
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    ApplySearch();
                }
            }
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                if (_filterText != value)
                {
                    _filterText = value;
                    OnPropertyChanged(nameof(FilterText));
                    ApplyFilter();
                }
            }
        }

        // Commands
        public ICommand EditItemCommand { get; }
        public ICommand DeleteItemCommand { get; }
        public ICommand SelectCategoryCommand { get; }

        public SuplementsVM()
        {
            _context = new FitnessDBDataContext();
            Supplements = new ObservableCollection<Supplement>();
            EditItemCommand = new RelayCommand(EditSupplement, CanModifySupplement);
            DeleteItemCommand = new RelayCommand(DeleteSupplement, CanModifySupplement);
            SelectCategoryCommand = new RelayCommand<object>(SelectCategory);
            PageTitle = "Content Manager";
            LoadSupplements();
        }

        public SuplementsVM(int NUMBER)
        {
            _context = new FitnessDBDataContext();
            Supplements = new ObservableCollection<Supplement>();
            EditItemCommand = new RelayCommand(EditSupplement, CanModifySupplement);
            DeleteItemCommand = new RelayCommand(DeleteSupplement, CanModifySupplement);
            SelectCategoryCommand = new RelayCommand<object>(SelectCategory);
            PageTitle = "Content Manager";
            SelectCategoryByNumber(NUMBER);
           // LoadSupplements();
        }

        private void LoadSupplements()
        {
            try
            {
                var supplements = _context.Suplimentes.Select(s => new Supplement
                {
                    SupplementID = s.SupplementID,
                    Name = s.Name,
                    Description = s.Description,
                    Category = s.Category,
                    Dosage = s.Dosage,
                    Benefits = s.Benefits
                }).ToList();

                Supplements = new ObservableCollection<Supplement>(supplements);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading supplements: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Command Methods

        // Edit Supplement
        private void EditSupplement(object parameter)
        {
            if (parameter is Supplement supplement)
            {
                supplement.Description += " (Edited)";
                OnPropertyChanged(nameof(Supplements));

                var dbSupplement = _context.Suplimentes.FirstOrDefault(s => s.SupplementID == supplement.SupplementID);
                if (dbSupplement != null)
                {
                    dbSupplement.Description = supplement.Description;

                    try
                    {
                        _context.SubmitChanges();
                        MessageBox.Show("Supplement edited successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error editing supplement: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private bool CanModifySupplement(object parameter)
        {
            return parameter is Supplement;
        }

        // Delete Supplement
        private void DeleteSupplement(object parameter)
        {
            if (parameter is Supplement supplement)
            {
                var result = MessageBox.Show($"Are you sure you want to delete '{supplement.Name}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    var dbSupplement = _context.Suplimentes.FirstOrDefault(s => s.SupplementID == supplement.SupplementID);
                    if (dbSupplement != null)
                    {
                        _context.Suplimentes.DeleteOnSubmit(dbSupplement);

                        try
                        {
                            _context.SubmitChanges();
                            Supplements.Remove(supplement);
                            MessageBox.Show("Supplement deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error deleting supplement: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        // Select Category Command
        private void SelectCategory(object parameter)
        {
            if (parameter is string category)
            {
                try
                {
                    switch (category)
                    {
                        case "Recipes":
                            var recipes = _context.Retetes.Select(r => new Supplement
                            {
                                SupplementID = r.ID,
                                Name = r.Nume,
                                Description = r.Ingrediente,
                                Category = "Recipe",
                                Dosage = r.Calorii.ToString(),
                                Benefits = r.TipMasa
                            }).ToList();

                            Supplements = new ObservableCollection<Supplement>(recipes);
                            break;

                        case "Exercises":
                            var exercises = _context.Exercitiis.Select(e => new Supplement
                            {
                                SupplementID = e.ID,
                                Name = e.DenumireExercitiu,
                                Description = e.Descriere,
                                Category = "Exercise",
                                Dosage = e.Repetari.ToString(),
                                Benefits = e.Seturi.ToString()
                            }).ToList();

                            Supplements = new ObservableCollection<Supplement>(exercises);
                            break;

                        case "Supplements":
                            LoadSupplements();
                            break;

                        default:
                            MessageBox.Show("Unknown category selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data for category '{category}': {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Search Method
        private void ApplySearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadSupplements();
                return;
            }

            var filteredSupplements = _context.Suplimentes
                .Where(s => s.Name.Contains(SearchText) || s.Description.Contains(SearchText))
                .Select(s => new Supplement
                {
                    SupplementID = s.SupplementID,
                    Name = s.Name,
                    Description = s.Description,
                    Category = s.Category,
                    Dosage = s.Dosage,
                    Benefits = s.Benefits
                }).ToList();

            Supplements = new ObservableCollection<Supplement>(filteredSupplements);
        }

        // Filter Method
        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(FilterText))
            {
                LoadSupplements();
                return;
            }

            var filteredSupplements = _context.Suplimentes
                .Where(s => s.Category.Equals(FilterText, StringComparison.OrdinalIgnoreCase))
                .Select(s => new Supplement
                {
                    SupplementID = s.SupplementID,
                    Name = s.Name,
                    Description = s.Description,
                    Category = s.Category,
                    Dosage = s.Dosage,
                    Benefits = s.Benefits
                }).ToList();

            Supplements = new ObservableCollection<Supplement>(filteredSupplements);
        }

        // Helper Method to Raise PropertyChanged Event
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // New function that selects a category based on a number (0: Recipes, 1: Exercises, 2: Supplements)
        private void SelectCategoryByNumber(int categoryNumber)
        {
            try
            {
                switch (categoryNumber)
                {
                    case 0: // Recipes
                        var recipes = _context.Retetes.Select(r => new Supplement
                        {
                            SupplementID = r.ID,
                            Name = r.Nume,
                            Description = r.Ingrediente,
                            Category = "Recipe",
                            Dosage = r.Calorii.ToString(),
                            Benefits = r.TipMasa
                        }).ToList();

                        Supplements = new ObservableCollection<Supplement>(recipes);
                        break;

                    case 1: // Exercises
                        var exercises = _context.Exercitiis.Select(e => new Supplement
                        {
                            SupplementID = e.ID,
                            Name = e.DenumireExercitiu,
                            Description = e.Descriere,
                            Category = "Exercise",
                            Dosage = e.Repetari.ToString(),
                            Benefits = e.Seturi.ToString()
                        }).ToList();

                        Supplements = new ObservableCollection<Supplement>(exercises);
                        break;

                    case 2: // Supplements
                        LoadSupplements();
                        break;

                    default:
                        MessageBox.Show("Invalid category number. Please use 0 for Recipes, 1 for Exercises, or 2 for Supplements.",
                                        "Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data for category number '{categoryNumber}': {ex.Message}",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }


    }
}