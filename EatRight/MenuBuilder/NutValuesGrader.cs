using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace MenuBuilder
{
    class NutValuesGrader : PerDayGrader
    {
        private Dictionary<string, double> dailyValues;

        public NutValuesGrader(Dictionary<string, double> dailyValues)
        {
            this.dailyValues = dailyValues;
        }

        protected override double GradeDay(DailyMenu day)
        {
            double grade = 0;

            var breakfastValues = GetMealNutritionValues(day.Breakfast);
            var lunchValues = GetMealNutritionValues(day.Lunch);
            var dinnerValues = GetMealNutritionValues(day.Dinner);

            // Compare to daily needed values
            foreach (var entry in dailyValues)
            {
                var nutriate = entry.Key;
                double actualValue = 0;
                if (breakfastValues.ContainsKey(nutriate))
                {
                    actualValue += breakfastValues[nutriate];
                }
                if (lunchValues.ContainsKey(nutriate))
                {
                    actualValue += lunchValues[nutriate];
                }
                if (dinnerValues.ContainsKey(nutriate))
                {
                    actualValue += dinnerValues[nutriate];
                }

                var idealValue = entry.Value;
                grade += GradeRatio(actualValue / idealValue);
            }

            return grade / dailyValues.Count;
        }

        private Dictionary<string, double> GetMealNutritionValues(Meal meal)
        {
            var totalValues = new Dictionary<string, double>();

            // Get nutrition values of all products
            foreach (var prodName in meal.Products) {
                var productWeight = meal.GetProductWeight(prodName);
                var product = productWeight.Key;
                var weight = productWeight.Value;

                var nutValues = product.GetNutritionValues().ToList();
                foreach (var entry in nutValues)
                {
                    if (!totalValues.ContainsKey(entry.Key))
                    {
                        totalValues[entry.Key] = 0;
                    }

                    double curValue = totalValues[entry.Key];
                    totalValues[entry.Key] = curValue + (entry.Value * (weight / Globals.DEFAULT_GRAMֹ_NUM));
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
