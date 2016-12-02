using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

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
            foreach (var prodName in Meal.Products)
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
                    NutValues[entry.Key] = curValue + (entry.Value * (weight / Globals.DEFAULT_GRAM_NUM));
                }
            }

            // Calculate calories number
            CaloriesNum = 0;
            // Get nutrition values of all products
            foreach (var prodName in Meal.Products)
            {
                var productWeight = Meal.GetProductWeight(prodName);
                var product = productWeight.Key;
                var weight = productWeight.Value;

                double protein = (weight / Globals.DEFAULT_GRAM_NUM) * product.Protein;
                double fat = (weight / Globals.DEFAULT_GRAM_NUM) * product.Fat;
                double carbs = (weight / Globals.DEFAULT_GRAM_NUM) * product.Carbs;

                CaloriesNum += (Globals.CALORIES_PER_CARB_GRAM * carbs
                    + Globals.CALORIES_PER_FAT_GRAM * fat
                    + Globals.CALORIES_PER_PROTEIN_GRAM * protein);
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
