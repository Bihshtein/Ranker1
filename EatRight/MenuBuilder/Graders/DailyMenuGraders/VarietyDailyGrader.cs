using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace MenuBuilder.Graders.DailyMenuGraders
{
    class VarietyDailyGrader : DailyMenuGrader
    {
        protected override double InternalGrade(DailyMenu day)
        {
            var products = new HashSet<string>();
            int totalProductsNum = 0;
            int repeatedProductsNum = 0;

            RepeatedProductsInMeal(day.Breakfast, ref products, ref totalProductsNum, ref repeatedProductsNum);
            RepeatedProductsInMeal(day.Lunch, ref products, ref totalProductsNum, ref repeatedProductsNum);
            RepeatedProductsInMeal(day.Dinner, ref products, ref totalProductsNum, ref repeatedProductsNum);

            return GradeRatio((double)repeatedProductsNum / totalProductsNum);
        }

        private void RepeatedProductsInMeal(Meal meal, ref HashSet<string> products, ref int totalProductsNum, ref int repeatedProductsNum)
        {
            foreach (var prodName in meal.Products)
            {
                totalProductsNum++;

                if (products.Contains(prodName))
                {
                    repeatedProductsNum++;
                }
                else
                {
                    products.Add(prodName);
                }
            }
        }

        private double GradeRatio(double ratio)
        {
            return 1 - ratio;
        }
    }
}
