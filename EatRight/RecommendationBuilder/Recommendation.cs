using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder
{
    public class Recommendation : RecommendationObject
    {
        public Recommendation(HashSet<MealWrapper> mealsSet)
        {
            ID = IDCounter++;
            this.MealsSet = mealsSet;
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Recommendation m = (Recommendation)obj;
            return ID.Equals(m.ID);
        }

        public HashSet<MealWrapper> MealsSet { get; private set; }

        public int ID { get; private set; }

        private static int IDCounter = 0;
    }
}
