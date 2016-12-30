using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace RecommendationBuilder.Graders.MenuGraders
{
    class CostGrader : MenuGrader
    {
        public CostGrader()
        {
            Description = "Compatibility of cost to user's budget";
            Type = GraderType.CostGrader;
        }

        protected override double InternalGrade(Menu menu)
        {
            double cost = 0;

            menu.Days.ForEach(dm => dm.Meals.ToList().ForEach(m => m.Value.Recipe.ProductsWeight.Keys.ToList().ForEach(p => /* TODO: add cost per product */cost += 0)));

            return GradeRatio(cost / RecommendationObject.recommendationDB.budget);
        }

        private double GradeRatio(double ratio)
        {
            /* We want to give high grade to each ratio which is not bigger than 1- 0.9 or more.
             * Ratios bigger than 1 means budget exception, and will be graded 0.
             */
            
            if (ratio <= 1)
            {
                return 0.9 + 0.1 * (1 - ratio);
            }
            else
            {
                return 0;
            }
        }
    }
}
