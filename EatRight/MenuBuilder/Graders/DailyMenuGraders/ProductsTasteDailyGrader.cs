using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuBuilder.Graders.MealGraders;

namespace MenuBuilder.Graders.DailyMenuGraders
{
    class ProductsTasteDailyGrader : PerMealGrader
    {
        public ProductsTasteDailyGrader()
        {
            Description = "Compatibility of products to the user flavor";

            mealGrader = new ProductsTasteMealGrader();
        }
    }
}
