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
        public string MealType { get; set; }

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

        public void AddRecipe(string name, string ingrediente,
            int calorii, decimal carbohidrati,
            decimal proteine, decimal grasimi)
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

        public void CrearePlanAlimentar(int numarCalorii)
        {
            List<string> mealPlan = new List<string>();
            int calorii_totale = 0;
            const int eroare_acceptable = 100;

            try
            {
                do
                {
                    calorii_totale = 0;
                    mealPlan.Clear();
                    var breakfastOptions = _context.Retetes
                        .Where(r => r.TipMasa == "Mic Dejun" && r.Calorii <= numarCalorii)
                        .ToList();
                    var breakfast = breakfastOptions.OrderBy(_ => Guid.NewGuid()).FirstOrDefault();
                    if (breakfast != null)
                    {
                        mealPlan.Add($"Mic Dejun: {breakfast.Ingrediente} ({breakfast.Calorii} Cal)");
                        calorii_totale += breakfast.Calorii;
                    }

                    var lunchOptions = _context.Retetes
                            .Where(r => r.TipMasa == "Pranz" && r.Calorii <= numarCalorii)
                            .ToList();
                    var lunch = lunchOptions.OrderBy(_ => Guid.NewGuid()).FirstOrDefault();
                    if (lunch != null)
                    {
                        mealPlan.Add($"Pranz: {lunch.Ingrediente} ({lunch.Calorii} Cal)");
                        calorii_totale += lunch.Calorii;
                    }

                    var dinnerOptions = _context.Retetes
                        .Where(r => r.TipMasa == "Cina" && r.Calorii <= numarCalorii)
                        .ToList();
                    var dinner = dinnerOptions.OrderBy(_ => Guid.NewGuid()).FirstOrDefault();
                    if (dinner != null)
                    {
                        mealPlan.Add($"Cina: {dinner.Ingrediente} ({dinner.Calorii} Cal)");
                        calorii_totale += dinner.Calorii;
                    }

                } while (Math.Abs(calorii_totale - numarCalorii) > eroare_acceptable);

                Console.WriteLine("\nPlan alimentar zilnic:");
                Console.WriteLine($"Calorii totale: {calorii_totale}");
                foreach (var meal in mealPlan)
                {
                    Console.WriteLine(meal);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Exception occurred: {ex.Message}");
                Console.WriteLine("[ERROR] Stack Trace:");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
