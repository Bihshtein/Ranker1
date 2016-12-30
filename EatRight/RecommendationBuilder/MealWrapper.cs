using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using Logic;

namespace RecommendationBuilder
{
    public class Meal : RecommendationObject
    {
        public Dictionary<string, double> NutValues { get; set; }
        public double CaloriesNum { get; set; }

        public Meal(Recipe recipe)
        {
            var timer = GlobalProfilingManger.Instance.Manager;

            Recipe = recipe;

            NutValues = Recipe.TotalNutValues.ToDictionary(x => x.Key, x => x.Value / Recipe.Servings);
            CaloriesNum = Recipe.TotalCaloriesNum / Recipe.Servings;

            timer.TakeTime("calculating calories num ");
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Meal mm = (Meal)obj;
            return this.Recipe.Equals(mm.Recipe);
        }

        public Recipe Recipe { get; set; }
    }
}
