using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBuilder.Graders.MealGraders
{
    class TypeFitnessMealGrader : MealGrader
    {
        public TypeFitnessMealGrader()
        {
            Description = "Fitness of products to meal type";
        }

        protected override double InternalGrade(MenuMeal meal)
        {
            // TODO: implement
            return 1;
        }
    }
}
