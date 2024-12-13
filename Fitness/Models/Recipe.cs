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
    public class Recipe : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int Calories { get; set; }
        public decimal Carbohydrates { get; set; }
        public decimal Proteins { get; set; }
        public decimal Fats { get; set; }
        public string Name { get; set; }
        public string Ingredients { get; set; }
        public string MealType { get; set; } // "Breakfast", "Lunch", "Dinner", "Snack"


        public Recipe()
        {
            _context = new FitnessDBDataContext();
        }

        private readonly FitnessDBDataContext _context;
        public event PropertyChangedEventHandler PropertyChanged;

        public Recipe GetRecipe(string name)
        {
            var recipe = _context.Retetes.First(r => r.Nume == name);

            if (recipe == null)
            {
                throw new ArgumentNullException("There is no recipe with that name!");
            }

            return new Recipe
            {
                Id = recipe.ID,
                Name = recipe.Nume,
                Calories = (int)recipe.Calorii,
                Carbohydrates = (decimal)recipe.Carbohidrati,
                Proteins = (decimal)recipe.Proteine,
                Fats = (decimal)recipe.Grasimi,
                Ingredients = recipe.Ingrediente,
                MealType = recipe.TipMasa

            };
        }

        public void AddRecipe(string name, string ingrediente, int calorii, decimal carbohidrati, decimal proteine, decimal grasimi)
        {
            var recipe = new Retete
            {
                Nume = name,
                Ingrediente = ingrediente,
                Calorii = calorii,
                Carbohidrati = carbohidrati,
                Proteine = proteine,
                Grasimi = grasimi


            };

            _context.Retetes.InsertOnSubmit(recipe);
            try
            {
                _context.SubmitChanges();
                Console.WriteLine("Reteta adaugata cu succes.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la adaugarea retetei: {ex.Message}");
            }
        }

        public void DeleteRecipe(string name, string tip)
        {
            var recipe = _context.Retetes.FirstOrDefault(r => r.Nume == name && r.TipMasa == tip);

            if (recipe == null)
            {
                Console.WriteLine("Reteta nu exista.");
                return;
            }

            _context.Retetes.DeleteOnSubmit(recipe);

            try
            {
                _context.SubmitChanges();
                Console.WriteLine("Reteta stearsa cu succes.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la stergerea retetei: {ex.Message}");
            }
        }


    }
}
