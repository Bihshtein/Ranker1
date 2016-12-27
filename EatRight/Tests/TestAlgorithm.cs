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
        public void TestForbiddenProducts()
        {
            var unit = new RestDBInterface();

            Meal goodBreakfast = new Meal() {
                ID = 0,
                Name = "Good breakfast",
                ProductsWeight = new Dictionary<string, double>(){
                    { "Carrot", 200},
                    { "Tomato", 123},
                    { "Avocados", 50},
                    { "bacon", 50},
                    { "Tuna", 200},
                    { "Almonds", 25}
                },
                Types = new HashSet<MealType>() { MealType.Breakfast }
            };

            Meal badBreakfast = new Meal() {
                ID = 1,
                Name = "Bad breakfast",
                ProductsWeight =new Dictionary<string, double>(){{ "Carrot", 20}},
                Types =new HashSet<MealType>() { MealType.Breakfast }
            };

            unit.Meals.Empty();
            unit.Meals.Add(goodBreakfast);
            unit.Meals.Add(badBreakfast);

            var userProfile = new UserProfile() { Age = 30, Gender = GenderType.Male };
            var range = new MenuSuggestionRange()
                { Length = 1, MealsInDailyMenu = new List<MealType>() { MealType.Breakfast } };

            /* We create 2 recommendationDB's: one that allows tomatoes, and the other one forbids it.
             * It's obvious that without any restriction, the good breakfast is better than the bad breakfast.
             * We'll make sure that the good breakfast is winning, but when not allowing tomatoes-
             * the bad one wins.
             */
            var recommendationDBWithoutTomatoes = RecommendationDBGenerator.FromUserProfile(userProfile, unit);
            recommendationDBWithoutTomatoes.range = range;
            var recommendationDBWithTomatoes = RecommendationDBGenerator.FromUserProfile(userProfile, unit);
            recommendationDBWithTomatoes.range = range;

            recommendationDBWithoutTomatoes.forbiddenProducts = new HashSet<string>() { "Tomato" };

            recommendationDBWithoutTomatoes.GradersWeight = null;
            recommendationDBWithTomatoes.GradersWeight = null;

            var recommendationGeneratorWOT = new RecommendationGenerator(unit, recommendationDBWithoutTomatoes);
            var recommendationGeneratorWT = new RecommendationGenerator(unit, recommendationDBWithTomatoes);
            var menuWOT = recommendationGeneratorWOT.GetMenu();
            var menuWT = recommendationGeneratorWT.GetMenu();
            
            // Assertions
            // 1. Assert the menu isn't null
            Assert.IsNotNull(menuWOT);
            Assert.IsNotNull(menuWT);
            // 2. Assert that the menu contains 1 day
            Assert.IsTrue(menuWOT.GetDaysNumber() == 1);
            Assert.IsTrue(menuWT.GetDaysNumber() == 1);
            // 3. Assert that this day contains breakfast
            Assert.IsTrue(menuWOT.GetDay(0).Meals.ContainsKey(MealType.Breakfast));
            Assert.IsTrue(menuWT.GetDay(0).Meals.ContainsKey(MealType.Breakfast));
            // 4. Main assertion: assert that we got the bad meal (because the good one contains forbidden products)
            Assert.IsTrue(menuWOT.GetDay(0).Meals[MealType.Breakfast].Meal.Equals(badBreakfast));
            // And we get the good meal with not restrictions
            Assert.IsTrue(menuWT.GetDay(0).Meals[MealType.Breakfast].Meal.Equals(goodBreakfast));
        }

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
