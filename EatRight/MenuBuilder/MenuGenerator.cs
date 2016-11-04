using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBuilder
{
    class MenuGenerator
    {
        public static double EvaluateMenu(Menu menu, Dictionary<string, double> dailyValues)
        {
            // Initialize graders map
            List<KeyValuePair<Grader, double>> graderList = new List<KeyValuePair<Grader,double>> ()
            {
                new KeyValuePair<Grader, double>(new NutValuesGrader(dailyValues), 1)
            };

            double graderWeightSum = 0;
            double grade = 0;

            foreach (var entry in graderList)
            {
                graderWeightSum += entry.Value;

                var grader = entry.Key;
                grade += grader.Grade(menu);
            }

            // Return the scaled grade
            return (grade / graderWeightSum) * 100;
        }
    }
}
