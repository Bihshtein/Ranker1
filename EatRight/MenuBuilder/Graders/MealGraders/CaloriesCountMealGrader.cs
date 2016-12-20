using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using MenuBuilder.Graders.DailyMenuGraders;

namespace MenuBuilder.Graders.MealGraders
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
                (((double)Grader.graderDB.dailyCaloriesNum) * MealPrecentage[((MealSuggestionRange)Grader.graderDB.range).MealType]);
        }

        protected override double InternalGrade(MenuMeal menuMeal)
        {
            return CaloriesCountDailyGrader.GradeRatio(menuMeal.CaloriesNum / dailyCaloriesNum);
        }
    }
}
