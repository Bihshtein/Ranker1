using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace RecommendationBuilder.Graders.DailyMenuGraders
{
    class VarietyDailyGrader : DailyMenuGrader
    {
        public VarietyDailyGrader()
        {
            Description = "Variety in products";
            Type = GraderType.VarietyDailyGrader;
        }

        protected override double InternalGrade(DailyMenu day)
        {
            var allProducts = day.GetAllProducts();
            int totalProductsNum = allProducts.Count;
            int repeatedProductsNum = totalProductsNum- allProducts.Distinct().Count() ;


            return GradeRatio((double)repeatedProductsNum / totalProductsNum);
        }

        private void RepeatedProductsInMeal(Meal mealWrapper, ref HashSet<string> products, ref int totalProductsNum, ref int repeatedProductsNum)
        {
            Recipe meal = mealWrapper.Recipe;
            foreach (var prodName in meal.ProductsWeight.Keys)
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
