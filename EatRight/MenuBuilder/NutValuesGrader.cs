using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace MenuBuilder
{
    class NutValuesGrader : Grader
    {
        private Dictionary<string, double> dailyValues;

        public NutValuesGrader(Dictionary<string, double> dailyValues)
        {
            this.dailyValues = dailyValues;
        }

        public override double Grade(Menu menu)
        {
            int daysNumber = menu.GetDaysNumber();
            if (daysNumber == 0)
            {
                return 0;
            }

            int dayInd = 0;
            double gradeSum = 0;

            for (; dayInd < daysNumber; dayInd++)
            {
                gradeSum += GradeDay(menu.GetDay(dayInd));
            }

            return gradeSum / daysNumber;
        }

        private double GradeDay(DailyMenu day)
        {
            double grade = 0;

            Dictionary<string, double> breakfastValues = GetMealNutritionValues(day.Breakfast);
            Dictionary<string, double> lunchValues = GetMealNutritionValues(day.Lunch);
            Dictionary<string, double> dinnerValues = GetMealNutritionValues(day.Dinner);

            // Compare to daily needed values
            foreach (KeyValuePair<string, double> entry in dailyValues)
            {
                string nutriate = entry.Key;
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

                double idealValue = entry.Value;
                grade += GradeRatio(actualValue / idealValue);
            }

            return grade / dailyValues.Count;
        }

        private Dictionary<string, double> GetMealNutritionValues(Meal meal)
        {
            Dictionary<string, double> totalValues = new Dictionary<string, double>();

            int prodNumber = meal.GetProductsNumber();
            if (prodNumber == 0)
            {
                return totalValues;
            }

            // Get nutrition values of all products
            int prodInd = 0;
            for (; prodInd < prodNumber; prodInd++)
            {
                KeyValuePair<Product, double> productWeight = meal.GetProductWeight(prodInd);
                Product product = productWeight.Key;
                double weight = productWeight.Value;

                List<KeyValuePair<string, double>> nutValues = product.GetNutritionValues().ToList();
                foreach (KeyValuePair<string, double> entry in nutValues)
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

            if (ratio <= 3)
            {
                return (3 - ratio) * 0.5;
            }

            return 0;
        }
    }
}
