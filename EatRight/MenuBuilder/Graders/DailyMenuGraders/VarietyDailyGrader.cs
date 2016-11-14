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
            var allProducts = day.GetAllProducts();
            int totalProductsNum = allProducts.Count;
            int repeatedProductsNum = totalProductsNum- allProducts.Distinct().Count() ;


            return GradeRatio((double)repeatedProductsNum / totalProductsNum);
        }

        private void RepeatedProductsInMeal(MenuMeal menuMeal, ref HashSet<string> products, ref int totalProductsNum, ref int repeatedProductsNum)
        {
            Meal meal = menuMeal.Meal;
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
