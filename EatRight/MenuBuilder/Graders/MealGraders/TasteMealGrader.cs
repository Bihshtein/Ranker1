using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBuilder.Graders.MealGraders
{
    class TasteMealGrader : MealGrader
    {
        protected override double InternalGrade(MenuMeal meal)
        {
            return 1;
        }
    }
}
