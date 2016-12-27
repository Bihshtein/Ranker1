using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using RecommendationBuilder.Graders.DailyMenuGraders;

namespace RecommendationBuilder.Graders.MealGraders
{
    abstract class NutValuesMealGrader : MealGrader
    {
        private Dictionary<string, MinMaxDouble> mealValues;
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

            double precentage = MealPrecentage[((MealSuggestionRange)RecommendationObject.recommendationDB.range).MealType];
            this.mealValues = RecommendationObject.recommendationDB.dailyValues.Select(x => new KeyValuePair<string, MinMaxDouble>
                (x.Key, new MinMaxDouble(x.Value.MinValue * precentage, x.Value.MaxValue * precentage)))
                .ToDictionary(x => x.Key, x => x.Value);
        }

        protected override double InternalGrade(MealWrapper mealWrapper)
        {
            return NutValuesDailyGrader.GradeByNutValues(mealValues, mealWrapper.NutValues, IsMinGrader);
        }
    }
}
