using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Fitness.ViewModels
{
    public class SuplementsVM : BaseViewModel
    {
        private string _pageTitle;
        private ObservableCollection<string> _items;

        public ICommand ShowRecipesCommand { get; }
        public ICommand ShowExercisesCommand { get; }
        public ICommand ShowSuplementsCommand { get; }

        public SuplementsVM()
        {
            PageTitle = "Recipes";
            ShowRecipesCommand = new RelayCommand(ShowRecipes);
            ShowExercisesCommand = new RelayCommand(ShowExercises);
            ShowSuplementsCommand = new RelayCommand(ShowSuplements);

            LoadRecipes();
        }
        
        public string PageTitle
        {
            get => _pageTitle;
            set
            {
                _pageTitle = value;
                OnPropertyChanged(PageTitle);
            }
        }

        public ObservableCollection<string> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }


        private void ShowRecipes()
        {
            PageTitle = "Recipes";
            LoadRecipes();
        }

        private void ShowExercises()
        {
            PageTitle = "Exercises";
            LoadExercises();
        }

        private void ShowSuplements()
        {
            PageTitle = "Supplements";
            LoadSuplements();
        }

        // Încărcare date pentru fiecare categorie
        private void LoadRecipes()
        {
            Items = new ObservableCollection<string>
            {
                "Recipe 1: Chicken Salad",
                "Recipe 2: Protein Shake",
                "Recipe 3: Vegan Burger"
            };
        }

        private void LoadExercises()
        {
            Items = new ObservableCollection<string>
            {
                "Exercise 1: Push-ups",
                "Exercise 2: Deadlifts",
                "Exercise 3: Bench Press"
            };
        }

        private void LoadSuplements()
        {
            Items = new ObservableCollection<string>
            {
                "Supplement 1: Whey Protein",
                "Supplement 2: Creatine",
                "Supplement 3: Omega-3"
            };
        }
    }
}
