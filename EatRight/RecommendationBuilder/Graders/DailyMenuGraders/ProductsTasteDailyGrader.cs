using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecommendationBuilder.Graders.MealGraders;
using RestModel;

namespace RecommendationBuilder.Graders.DailyMenuGraders
{
    class ProductsTasteDailyGrader : PerMealGrader
    {
        public ProductsTasteDailyGrader()
        {
            Description = "Compatibility of products to the user flavor";
            Type = GraderType.ProductsTasteDailyGrader;

            mealGrader = new ProductsTasteMealGrader();
        }
    }
}
