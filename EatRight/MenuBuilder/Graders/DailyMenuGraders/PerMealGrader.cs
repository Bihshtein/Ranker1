using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuBuilder.Graders.MealGraders;

namespace MenuBuilder.Graders.DailyMenuGraders
{
    abstract class PerMealGrader : DailyMenuGrader
    {
        protected override double InternalGrade(DailyMenu dailyMenu)
        {
            double gradeSum = 0;

            // Go over all the meals in the day and grade them
            try
            {
                dailyMenu.Meals.ForEach(x => gradeSum += mealGrader.Grade(x));
            }
            catch
            {
                throw;
            }

            return gradeSum / dailyMenu.Meals.Count;
        }

        protected MealGrader mealGrader;
    }
}
