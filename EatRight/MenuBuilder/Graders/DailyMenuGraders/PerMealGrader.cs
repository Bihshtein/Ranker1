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
            int mealsNumber = 0;
            double gradeSum = 0;

            // Go over all the meals in the day and grade them
            gradeSum += mealGrader.Grade(dailyMenu.Breakfast);
            mealsNumber++;
            gradeSum += mealGrader.Grade(dailyMenu.Lunch);
            mealsNumber++;
            gradeSum += mealGrader.Grade(dailyMenu.Dinner);
            mealsNumber++;

            return gradeSum / mealsNumber;
        }

        protected MealGrader mealGrader;
    }
}
