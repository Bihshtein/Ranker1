using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecommendationBuilder.Graders.DailyMenuGraders;
using RestModel;

namespace RecommendationBuilder.Graders.MenuGraders
{
    class FoodCategoryGrader : PerDayGrader
    {
        public FoodCategoryGrader()
        {
            Description = "Compatibility of meals category to the user flavor";
            Type = GraderType.FoodCategoryGrader;

            dailyGrader = new FoodCategoryDailyGrader();
        }
    }
}
