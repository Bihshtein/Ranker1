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
       
        public Dictionary<string, double> NutValues { get { return Recipe.TotalNutValues.ToDictionary(x => x.Key, x => x.Value / Recipe.Servings); } }
        public double CaloriesNum { get {return Recipe.TotalCaloriesNum / Recipe.Servings;}}
        public Recipe Recipe { get; set; }
      

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

      


    }



}
