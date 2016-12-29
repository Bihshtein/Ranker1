using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using RecommendationBuilder.Graders.DailyMenuGraders;

namespace RecommendationBuilder.Graders.MealGraders
{
    class CaloriesCountMealGrader : MealGrader
    {
        private int dailyCaloriesNum;

        static Dictionary<MealType, double> MealPrecentage = new Dictionary<MealType, double>()
        {
            {MealType.Breakfast, 0.3},
            {MealType.Lunch, 0.4},
            {MealType.Dinner, 0.3}
        };

        public CaloriesCountMealGrader()
        {
            Description = "Calories count compared to the ideal calories count value";
            Type = GraderType.CaloriesCountMealGrader;

            this.dailyCaloriesNum = (int)
                (((double)RecommendationObject.recommendationDB.dailyCaloriesNum) * MealPrecentage[((MealSuggestionRange)RecommendationObject.recommendationDB.range).MealType]);
        }

        protected override double InternalGrade(Meal mealWrapper)
        {
            return CaloriesCountDailyGrader.GradeRatio(mealWrapper.CaloriesNum / dailyCaloriesNum);
        }
    }
}
