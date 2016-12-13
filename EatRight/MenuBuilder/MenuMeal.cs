using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using Logic;

namespace MenuBuilder
{
    public class MenuMeal : GradableObject
    {
        public MenuMeal(Meal meal)
        {
            Meal = meal;

            // Calculate nutrition values
            NutValues = new Dictionary<string, double>();

            // Get nutrition values of all products
            foreach (var prodName in Meal.ProductsWeight.Keys)
            {
                var productWeight = Meal.GetProductWeight(prodName);
                var product = productWeight.Key;
                var weight = productWeight.Value;

                var prodNutValues = product.Nutrients().ToList();
                foreach (var entry in prodNutValues)
                {
                    if (!NutValues.ContainsKey(entry.Key))
                    {
                        NutValues[entry.Key] = 0;
                    }

                    double curValue = NutValues[entry.Key];
                    NutValues[entry.Key] = curValue + (entry.Value * (weight / Formulas.DefaultGrams));
                }
            }

            // Calculate calories number
            CaloriesNum = 0;
            // Get nutrition values of all products
            foreach (var prodName in Meal.ProductsWeight.Keys)
            {
                var productWeight = Meal.GetProductWeight(prodName);
                var product = productWeight.Key;
                var weight = productWeight.Value;

                CaloriesNum += Formulas.GetTotalCalories(weight, product.Protein, product.Fat, product.Carbs);
            }
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            MenuMeal mm = (MenuMeal)obj;
            return Meal.Equals(mm.Meal);
        }

        public Meal Meal { get; private set; }
        public Dictionary<string, double> NutValues { get; private set; }
        public double CaloriesNum { get; private set; }
    }
}
