using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestModel;
using MenuBuilder;
using MenuBuilder.Graders;

namespace Tests
{
    [TestClass]
    public class TestAlgorithm
    {
        [TestMethod]
        public void TestMenuGeneration()
        {
            var unit = new RestDBInterface();
            GraderDB graderDB = GenerateRandomGraderDB();
            GenerateRandomMeals(unit, graderDB);
            MenuGenerator generator = new MenuGenerator(unit, graderDB);
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

        private static GraderDB GenerateRandomGraderDB()
        {
            var rand = new Random(seed);

            // Not random for now
            var graderDB = new GraderDB();
            graderDB.dailyValues = RestRepository<Product>.DailyValues;
            graderDB.dailyCaloriesNum = 3000;
            graderDB.range = new SuggestionRange() { Type = SuggestionRangeType.Days, Length = rand.Next(1, 7) };
            graderDB.productFlavorGrade = new Dictionary<string,double>() {
                {"Mushroom", -1},
                {"Mushrooms", -1},
            };
            graderDB.mealCategoryGrade = new Dictionary<MealCategory, double>() {
                {MealCategory.Indian, 1},
                {MealCategory.Chinese, -1},
            };
            return graderDB;
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

        private void GenerateRandomMeals(RestDBInterface unit, GraderDB graderDB)
        {
            unit.Meals.Empty();
            var allProducts = unit.Products.GetAll().ToList();
            int prodNum = allProducts.Count;
            var rand = new Random(seed);
            double caloriesInMeal = graderDB.dailyCaloriesNum / 3;

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
                    double protein = (prodWeight / Globals.DEFAULT_GRAM_NUM) * product.Protein;
                    double fat = (prodWeight / Globals.DEFAULT_GRAM_NUM) * product.Fat;
                    double carbs = (prodWeight / Globals.DEFAULT_GRAM_NUM) * product.Carbs;

                    caloriesInCurMeal += (Globals.CALORIES_PER_CARB_GRAM * carbs
                        + Globals.CALORIES_PER_FAT_GRAM * fat
                        + Globals.CALORIES_PER_PROTEIN_GRAM * protein);
                }

                // Generate random type
                Array typeValues = Enum.GetValues(typeof(MealType));
                MealType type = (MealType)typeValues.GetValue(rand.Next(typeValues.Length));

                // Generate random category
                Array categoryValues = Enum.GetValues(typeof(MealCategory));
                MealCategory category = (MealCategory)categoryValues.GetValue(rand.Next(categoryValues.Length));

                Meal randMeal = new Meal("Random meal " + i, products, new HashSet<MealType>{type}, new HashSet<MealCategory>{category});
                unit.Meals.Add(randMeal);
            }
        }

        static int mealsNum = 20;
        static int defaultProductsInMealNum = 6;
        static int seed = 0;
    }
}
