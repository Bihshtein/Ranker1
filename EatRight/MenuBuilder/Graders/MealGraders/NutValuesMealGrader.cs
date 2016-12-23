using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using MenuBuilder.Graders.DailyMenuGraders;

namespace MenuBuilder.Graders.MealGraders
{
    abstract class NutValuesMealGrader : MealGrader
    {
        private Dictionary<string, double> mealValues;
        protected bool IsMinGrader;

        static Dictionary<MealType, double> MealPrecentage = new Dictionary<MealType, double>()
        {
            {MealType.Breakfast, 0.3},
            {MealType.Lunch, 0.4},
            {MealType.Dinner, 0.3}
        };

        public NutValuesMealGrader()
        {
            string minMaxStr = "";
            if (IsMinGrader)
            {
                minMaxStr = "minimum";
            }
            else
            {
                minMaxStr = "maximum";
            }
            Description = "Nutrition values compared to the " + minMaxStr + " nutrition values";

            double precentage = MealPrecentage[((MealSuggestionRange)Grader.graderDB.range).MealType];
            if (IsMinGrader)
            {
                this.mealValues =
                Grader.graderDB.dailyValues.Select(x => new KeyValuePair<string, double>(x.Key, x.Value * precentage)).
                ToDictionary(x => x.Key, x => x.Value);
            }
            else
            {
                this.mealValues =
                Grader.graderDB.dailyMaxValues.Select(x => new KeyValuePair<string, double>(x.Key, x.Value * precentage)).
                ToDictionary(x => x.Key, x => x.Value);
            }
        }

        protected override double InternalGrade(MenuMeal menuMeal)
        {
            return NutValuesDailyGrader.GradeByNutValues(mealValues, menuMeal.NutValues, IsMinGrader);
        }
    }
}
