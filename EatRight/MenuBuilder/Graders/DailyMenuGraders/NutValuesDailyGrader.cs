﻿using System;
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
            this.dailyValues = Grader.graderDB.dailyValues;
        }

        protected override double InternalGrade(DailyMenu day)
        {
            double grade = 0;

            var breakfastValues = GetMealNutritionValues(day.Breakfast);
            var lunchValues = GetMealNutritionValues(day.Lunch);
            var dinnerValues = GetMealNutritionValues(day.Dinner);

            // Compare to daily needed values
            foreach (var entry in dailyValues)
            {
                var nutrient = entry.Key;
                double actualValue = 0;
                if (breakfastValues.ContainsKey(nutrient))
                {
                    actualValue += breakfastValues[nutrient];
                }
                if (lunchValues.ContainsKey(nutrient))
                {
                    actualValue += lunchValues[nutrient];
                }
                if (dinnerValues.ContainsKey(nutrient))
                {
                    actualValue += dinnerValues[nutrient];
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

        private Dictionary<string, double> GetMealNutritionValues(MenuMeal menuMeal)
        {
            Meal meal = menuMeal.Meal;
            var totalValues = new Dictionary<string, double>();

            // Get nutrition values of all products
            foreach (var prodName in meal.Products)
            {
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
