using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBuilder.Graders.MealGraders
{
    class TasteMealGrader : MealGrader
    {
        public TasteMealGrader()
        {
            Description = "Compatibility to the user flavour";
        }

        protected override double InternalGrade(MenuMeal meal)
        {
            // TODO: implement
            return 1;
        }
    }
}
