using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestModel;
using RecommendationBuilder;
using RecommendationBuilder.Graders;
using Logic;

namespace Tests
{
    [TestClass]
    public class TestAlgorithm
    {
        [TestMethod]
        public void TestRecommendationGeneration()
        {
            var unit = new RestDBInterface();
            unit.Meals.Empty();
            RecommendationDB recommendationDB = GenerateRandomRecommendationDB();
            GenerateRandomMeals(unit, recommendationDB);
            RecommendationGenerator generator = new RecommendationGenerator(unit, recommendationDB);
            var menu = generator.GetMenu();
            var otherMenu = generator.GetMenu();

            // Assertions
            Assert.IsNotNull(menu);
            Assert.IsTrue(menu.Grade <= 100);
            Assert.IsTrue(menu.Grade >= 0);
            Assert.IsNotNull(otherMenu);
            Assert.IsTrue(otherMenu.Grade <= 100);
            Assert.IsTrue(otherMenu.Grade >= 0);
            Assert.IsFalse(menu.Equals(otherMenu));

            // Replacing a day
            DailyMenu oldDailyMenu = otherMenu.GetDay(0);
            generator.ReplaceDayByInd(otherMenu, 0);
            Assert.IsFalse(oldDailyMenu.Equals(otherMenu.GetDay(0)));

            // Printing some infromation
            System.Console.WriteLine("Menu eavluated to " + menu.Grade);
            System.Console.WriteLine("The grader that contributed the highest value to the grade is: " + menu.GradeInfo.BestGraders[0]);
            System.Console.WriteLine("The grader in which the highest number of points was lost is: " + menu.GradeInfo.WorstGraders[0]);
        }

        private static RecommendationDB GenerateRandomRecommendationDB()
        {
            var rand = new Random(seed);

            // Not random for now
            var recommendationDB = new RecommendationDB();
            recommendationDB.dailyValues = RestRepository<Product>.DailyValues.ToDictionary(k => k.Key, k => new MinMaxDouble(k.Value));
            recommendationDB.dailyCaloriesNum = 3000;
            recommendationDB.range = new MenuSuggestionRange() { Length = rand.Next(1, 7) };
            recommendationDB.productFlavorGrade = new Dictionary<string,double>() {
                {"Mushroom", -1},
                {"Mushrooms", -1},
            };
            recommendationDB.mealCategoryGrade = new Dictionary<MealCategory, double>() {
                {MealCategory.Indian, 1},
                {MealCategory.Chinese, -1},
            };
            return recommendationDB;
        }

        private static double Gaussian(Random rand, double mean, double stdDev)
        {
            double u1 = rand.NextDouble(); //these are uniform(0,1) random doubles
            double u2 = rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            return mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
        }

        private static double RandWeight(Random rand)
        {
            int tempInt = rand.Next(1, 10);
            if (tempInt < 8)
            {
                return Gaussian(rand, 20, 6);
            }
            else
            {
                return Gaussian(rand, 125, 40);
            }
        }

        private void GenerateRandomMeals(RestDBInterface unit, RecommendationDB recommendationDB)
        {
            unit.Meals.Empty();
            var allProducts = unit.Products.GetAll().ToList();
            int prodNum = allProducts.Count;
            var rand = new Random(seed);
            double caloriesInMeal = recommendationDB.dailyCaloriesNum / 3;

            for (int i = 0; i < mealsNum; ++i)
            {
                // Generate random products
                var products = new Dictionary<string, double>();
                double caloriesInCurMeal = 0;
                while (caloriesInCurMeal < caloriesInMeal)
                {
                    string prodName = null;
                    int count = 0;
                    int prodInd = 0;
                    while (prodName == null && count < 1000)
                    {
                        prodInd = rand.Next(0, prodNum - 1);
                        prodName = allProducts[prodInd].GetName();
                        count++;
                    }
                    Assert.IsTrue(prodName != null);
                    double prodWeight = RandWeight(rand);
                    products[prodName] = prodWeight;

                    var product = allProducts[prodInd];

                    caloriesInCurMeal += Formulas.GetTotalCalories(prodWeight, product.Protein, product.Fat, product.Carbs);
                }

                // Generate random type
                Array typeValues = Enum.GetValues(typeof(MealType));
                MealType type = (MealType)typeValues.GetValue(rand.Next(typeValues.Length));

                // Generate random category
                Array categoryValues = Enum.GetValues(typeof(MealCategory));
                MealCategory category = (MealCategory)categoryValues.GetValue(rand.Next(categoryValues.Length));

                Meal randMeal = new Meal() {
                    ID =i,
                    Name = "Random meal " + i,
                    ProductsWeight = products,
                    Types = new HashSet<MealType> { type },
                    Categories = new HashSet<MealCategory> { category }
                };
                unit.Meals.Add(randMeal);
            }
        }

        static int mealsNum = 20;
        static int defaultProductsInMealNum = 6;
        static int seed = 0;
    }
}
