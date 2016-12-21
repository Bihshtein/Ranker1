using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using MenuBuilder.Graders.DailyMenuGraders;

namespace MenuBuilder.Graders.MealGraders
{
    class NutValuesMealGrader : MealGrader
    {
        private Dictionary<string, double> dailyValues;

        static Dictionary<MealType, double> MealPrecentage = new Dictionary<MealType, double>()
        {
            {MealType.Breakfast, 0.3},
            {MealType.Lunch, 0.4},
            {MealType.Dinner, 0.3}
        };

        public NutValuesMealGrader()
        {
            Description = "Nutrition values compared to the ideal nutrition values";
            Type = GraderType.NutValuesMealGrader;

            double precentage = MealPrecentage[((MealSuggestionRange)Grader.graderDB.range).MealType];

            // TODO: (Hen) Uri, convert your dictionary's double to MinMaxDouble and use the MinValue & MaxValue.
            this.dailyValues =
                Grader.graderDB.dailyValues.Select(x => new KeyValuePair<string, double>(x.Key, x.Value.MinValue * precentage)).
                ToDictionary(x => x.Key, x => x.Value);
        }

        protected override double InternalGrade(MenuMeal menuMeal)
        {
            return NutValuesDailyGrader.GradeByNutValues(dailyValues, menuMeal.NutValues);
        }
    }
}
