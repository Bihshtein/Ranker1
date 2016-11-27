using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace MenuBuilder.Graders.DailyMenuGraders
{
    class NutValuesDailyGrader : DailyMenuGrader
    {
        private Dictionary<string, double> dailyValues;

        public NutValuesDailyGrader()
        {
            Description = "Nutrition values compared to the ideal nutrition values";

            this.dailyValues = Grader.graderDB.dailyValues;
        }

        protected override double InternalGrade(DailyMenu dailyMenu)
        {
            double grade = 0;

            var nutValues = GetNutritionValues(dailyMenu);

            // Compare to daily needed values
            foreach (var entry in dailyValues)
            {
                var nutrient = entry.Key;
                double actualValue = 0;
                if (dailyValues.ContainsKey(nutrient))
                {
                    actualValue = dailyValues[nutrient];
                }

                var idealValue = entry.Value;
                var basicRatio = actualValue / idealValue;
                //if (basicRatio > 1)
                //    Console.WriteLine("Too High Grade  : " + String.Format("{0:0.00}", basicRatio) + " nutrient : " + nutrient);
                var gradeRatio = GradeRatio(basicRatio);
                //if (gradeRatio < 1)
                //    Console.WriteLine("Missing Grade : " + String.Format("{0:0.00}", gradeRatio) + " nutrient : " + nutrient);

                grade += gradeRatio;
            }
            return grade / dailyValues.Count;
        }

        private Dictionary<string, double> GetNutritionValues(DailyMenu dailyMenu)
        {
            var totalValues = new Dictionary<string, double>();

            foreach(var outerEntry in dailyMenu.Meals)
            {
                MenuMeal menuMeal = outerEntry.Value;
                foreach (var entry in menuMeal.NutValues)
                {
                    var nutName = entry.Key;
                    if (!totalValues.ContainsKey(nutName))
                    {
                        totalValues[nutName] = 0;
                    }
                    totalValues[nutName] = totalValues[nutName] + entry.Value;
                }
            }
            return totalValues;
        }

        static private double GradeRatio(double ratio)
        {
            if (ratio <= 1)
            {
                return ratio;
            }

            // UriB change Grader: if ratio is higher than 1 this is considered as perfect (but the final grade will be lower because of the CaloriesCountGrader)
            //if (ratio <= 3)
            //{
            //    return (3 - ratio) * 0.5;
            //}
            if (ratio > 1)
            {
                return 1;
            }

            return 0;
        }
    }
}
