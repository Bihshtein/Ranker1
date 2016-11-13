using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBuilder.Graders.DailyMenuGraders
{
    class CaloriesCountDailyGrader : DailyMenuGrader
    {
        private int dailyCaloriesNum;

        public CaloriesCountDailyGrader()
        {
            this.dailyCaloriesNum = Grader.graderDB.dailyCaloriesNum;
        }

        protected override double InternalGrade(DailyMenu day)
        {
            var caloriesNum = MealCaloriesNum(day.Breakfast) + MealCaloriesNum(day.Lunch) + MealCaloriesNum(day.Dinner);

            return GradeRatio(caloriesNum / dailyCaloriesNum);
        }

        private double MealCaloriesNum(MenuMeal menuMeal)
        {
            Meal meal = menuMeal.Meal;
            double caloriesNum = 0;
            // Get nutrition values of all products
            foreach (var prodName in meal.Products)
            {

                var productWeight = meal.GetProductWeight(prodName);
                var product = productWeight.Key;
                var weight = productWeight.Value;

                double protein = (weight / Globals.DEFAULT_GRAMֹ_NUM) * product.Protein;
                double fat = (weight / Globals.DEFAULT_GRAMֹ_NUM) * product.Fat;
                double carbs = (weight / Globals.DEFAULT_GRAMֹ_NUM) * product.Carbs;

                caloriesNum += (Globals.CALORIES_PER_CARB_GRAM * carbs
                    + Globals.CALORIES_PER_FAT_GRAM * fat
                    + Globals.CALORIES_PER_PROTEIN_GRAM * protein);
            }
            //Console.WriteLine("Total Calories : " + caloriesNum);
            return caloriesNum;
        }

        static private double GradeRatio(double ratio)
        {
            if (ratio <= 1)
            {
                return ratio;
            }

            if (ratio <= 2)
            {
                return 2 - ratio;
            }

            return 0;
        }
    }
}
