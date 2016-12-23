﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace MenuBuilder.Graders.DailyMenuGraders
{
    abstract class NutValuesDailyGrader : DailyMenuGrader
    {
        private Dictionary<string, double> dailyValues;
        protected bool IsMinGrader;

        public NutValuesDailyGrader()
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

            this.dailyValues = Grader.graderDB.dailyValues;
        }

        protected override double InternalGrade(DailyMenu dailyMenu)
        {
            var nutValues = GetNutritionValues(dailyMenu);
            return GradeByNutValues(dailyValues, nutValues, IsMinGrader);
        }

        static public double GradeByNutValues(Dictionary<string, double> idealValues, Dictionary<string, double> actualValues, bool min)
        {
            double grade = 0;

            // Compare to daily needed values
            foreach (var entry in idealValues)
            {
                var nutrient = entry.Key;
                double actualValue = 0;
                if (idealValues.ContainsKey(nutrient))
                {
                    actualValue = actualValues[nutrient];
                }

                var idealValue = entry.Value;
                double basicRatio = 0;
                if (min)
                {
                    basicRatio = actualValue / idealValue;
                }
                else
                {
                    basicRatio = idealValue / actualValue;
                }
                var gradeRatio = GradeRatio(basicRatio);

                grade += gradeRatio;
            }

            return grade / idealValues.Count;
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
