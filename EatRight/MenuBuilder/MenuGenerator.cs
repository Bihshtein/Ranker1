using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBuilder
{
    class MenuGenerator
    {
        public static double EvaluateMenu(Menu menu, Dictionary<string, double> dailyValues, int dailyCaloriesNum)
        {
            // Initialize graders map
            var graderMap = new Dictionary<Grader,double> ()
            {
                {new NutValuesGrader(dailyValues), 0.5},
                {new CaloriesCountGrader(dailyCaloriesNum), 0.5}
            };

            double graderWeightSum = 0;
            double grade = 0;

            foreach (var entry in graderMap)
            {
                graderWeightSum += entry.Value;

                var grader = entry.Key;
                grade += (grader.Grade(menu) * entry.Value);
            }

            // Return the scaled grade
            return (grade / graderWeightSum) * 100;
        }
    }
}
