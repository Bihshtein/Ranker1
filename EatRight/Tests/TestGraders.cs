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
    public class TestGraders
    {
        [TestMethod]
        public void TestCaloriesCountGrader()
        {
            TestUserProfileDependentGrader(false);
        }

        [TestMethod]
        public void TestCostGrader()
        {

        }

        [TestMethod]
        public void TestFoodCategoryGrader()
        {
            TestTasteGrader(true);
        }

        [TestMethod]
        public void TestNutValuesGrader()
        {
            TestUserProfileDependentGrader(true);
        }

        [TestMethod]
        public void TestProductsTasteGrader()
        {
            TestTasteGrader(false);
        }

        [TestMethod]
        public void TestVarietyGrader()
        {
            var unit = new RestDBInterface();

            var smallBreakfast = new Meal() {
                ID = 0,
                Name = "Small breakfast",
                ProductsWeight = new Dictionary<string, double>() { { "Carrot", 1 } },
                Types =new HashSet<MealType>() { MealType.Breakfast }
            };
            var bigBreakfast = new Meal() {
                ID = 1,
                Name = "Big breakfast",
                ProductsWeight = new Dictionary<string, double>() { { "Carrot", 40} },
                Types = new HashSet<MealType>() { MealType.Breakfast } };
            var smallLunch = new Meal() {
                ID = 2,
                Name = "Small lunch",
                ProductsWeight = new Dictionary<string, double>() { { "Tomatoes", 1 } },
                Types = new HashSet<MealType>() { MealType.Lunch } };
            var bigLunch = new Meal() {
                ID = 3,
                Name = "Big Lunch",
                ProductsWeight = new Dictionary<string, double>() { { "Carrot", 400 } },
                Types = new HashSet<MealType>() { MealType.Lunch } };

            unit.Meals.Empty();
            unit.Meals.Add(smallBreakfast);
            unit.Meals.Add(bigBreakfast);
            unit.Meals.Add(smallLunch);
            unit.Meals.Add(bigLunch);

            var userProfile = new UserProfile() { Age = 30, Gender = GenderType.Male };
            var range = new MenuSuggestionRange()
                { Length = 1, MealsInDailyMenu = new List<MealType>() { MealType.Breakfast, MealType.Lunch } };

            var varietyGraderDB = GraderDBGenerator.FromUserProfile(userProfile, unit);
            varietyGraderDB.range = range;
            varietyGraderDB.GradersWeight = new Dictionary<GraderType, double>()
            {
                {GraderType.VarietyGrader, 1},
                {GraderType.VarietyDailyGrader, 1}
            };

            var normalGraderDB = GraderDBGenerator.FromUserProfile(userProfile, unit);
            normalGraderDB.range = range;

            var varietyMenuGen = new MenuGenerator(unit, varietyGraderDB);
            var normalMenuGen = new MenuGenerator(unit, normalGraderDB);

            var varietyMenu = varietyMenuGen.GetMenu();
            var normalMenu = normalMenuGen.GetMenu();

            // Assertions

            /* Clearly, the big meals are better. But, since it contains a product repetition, we expect it to be given
             * a lower grade when we only use the variety grader.
             */

            // 1. Assert that the variety menu generator didn't generate the big lunch.
            Assert.IsTrue(varietyMenu.GetDay(0).Meals[MealType.Lunch].Meal.Equals(smallLunch));
            // 2. Assert that the normal menu generator generated the big lunch.
            Assert.IsTrue(normalMenu.GetDay(0).Meals[MealType.Lunch].Meal.Equals(bigLunch));
        }

        private void TestUserProfileDependentGrader(Boolean nutValues)
        {
            // Make sure that grader is grading different user profiles in a different manner

            var unit = new RestDBInterface();
            unit.Meals.Empty();
            var breakfast = new Meal() {
                ID = 0,
                Name = "Sample breakfast 1",
                ProductsWeight = new Dictionary<string, double>()
            {
                { "Carrot", 20},
                { "Tomatoes", 23}
            },
                Types = new HashSet<MealType>() { MealType.Breakfast, MealType.Dinner }
            };

            var lunch = new Meal() {
                ID = 1,
                Name = "Sample lunch 1",
                ProductsWeight = new Dictionary<string, double>()
            {
                {"Walnut", 20}
            },
                Types = new HashSet<MealType>() { MealType.Lunch }
            };

            var dinner = new Meal() {
                ID = 2,
                Name = "Sample dinner 1",
                ProductsWeight = new Dictionary<string, double>()
            {
                { "Tomatoes", 10},
                { "Lettuce", 10}
            },
                Types = new HashSet<MealType>() { MealType.Dinner, MealType.Breakfast }
            };
            unit.Meals.Empty();
            unit.Meals.Add(breakfast);
            unit.Meals.Add(lunch);
            unit.Meals.Add(dinner);

            if (nutValues)
            {
                TestNutValuesGraderInternal(unit);
            }
            else
            {
                TestCaloriesCountGraderInternal(unit);
            }
        }

        private void TestNutValuesGraderInternal(RestDBInterface unit)
        {
            var females = unit.DailyValues.Queries.GetByGender(GenderType.Female);
            var males = unit.DailyValues.Queries.GetByGender(GenderType.Male);

            Dictionary<GraderType, double> graderWeights = new Dictionary<GraderType, double>() {
                {GraderType.NutValuesGrader, 1},
                {GraderType.NutValuesDailyGrader, 1}
            };

            var babyMenu = GetMenu(2, GenderType.Any, graderWeights, unit);
            var girlMenu = GetMenu(5, GenderType.Female, graderWeights, unit);
            Assert.IsTrue(babyMenu.Grade > girlMenu.Grade);// babies don't need much

            for (int i = 0; i < males.Count - 1; i++)
            {
                var maleItem1 = males[i];
                var maleItem2 = males[i + 1];
                var maleMenu1 = GetMenu(maleItem1.Age.MinAge, GenderType.Male, graderWeights, unit);
                var maleMenu2 = GetMenu(maleItem2.Age.MinAge, GenderType.Male, graderWeights, unit);
                var femaleItem1 = females[i];
                var femaleItem2 = females[i + 1];
                var femaleMenu1 = GetMenu(femaleItem1.Age.MinAge, GenderType.Female, graderWeights, unit);
                var femaleMenu2 = GetMenu(femaleItem2.Age.MinAge, GenderType.Female, graderWeights, unit);

                Assert.IsTrue(maleMenu1.Grade < 100 && maleMenu2.Grade < 100 &&
                              femaleMenu1.Grade < 100 && femaleMenu1.Grade < 100);

                if (i < 2)
                { // During the growing years the need for vitamins increases with age overall
                    Assert.IsTrue(maleMenu1.Grade > maleMenu2.Grade);
                    Assert.IsTrue(femaleMenu1.Grade > femaleMenu2.Grade);
                }
                else
                {// As we grow order for some nutrients we need more for some less
                    Assert.IsTrue(maleMenu1.Grade != maleMenu2.Grade);
                    Assert.IsTrue(femaleMenu1.Grade != femaleMenu2.Grade);
                }
                // Overall men need more the woman
                Assert.IsTrue(maleMenu1.Grade < femaleMenu1.Grade);
                Assert.IsTrue(maleMenu2.Grade < femaleMenu2.Grade);
            }

        }

        private void TestCaloriesCountGraderInternal(RestDBInterface unit)
        {
            Dictionary<GraderType, double> graderWeights = new Dictionary<GraderType, double>() {
                {GraderType.CaloriesCountGrader, 1},
                {GraderType.CaloriesCountDailyGrader, 1}
            };

            var babyProfile = new UserProfile() { Age = 3, Gender = GenderType.Male };
            var girlProfile = new UserProfile() { Age = 16, Gender = GenderType.Female };

            var babyGraderDB = GraderDBGenerator.FromUserProfile(babyProfile, unit);
            var girlGraderDB = GraderDBGenerator.FromUserProfile(girlProfile, unit);

            babyGraderDB.GradersWeight = graderWeights;
            girlGraderDB.GradersWeight = graderWeights;

            // Since all users currently get the same calories number, force a difference
            babyGraderDB.dailyCaloriesNum = girlGraderDB.dailyCaloriesNum / 2;

            var babyMenuGen = new MenuGenerator(unit, babyGraderDB);
            var girlMenuGen = new MenuGenerator(unit, girlGraderDB);

            var babyMenu = babyMenuGen.GetMenu();
            var girlMenu = girlMenuGen.GetMenu();

            Assert.IsTrue(babyMenu.Grade > girlMenu.Grade);// babies don't need much
        }

        private Menu GetMenu(int age, GenderType gender, Dictionary<GraderType, double> graderWeights, RestDBInterface unit )
        {
            var profile = new UserProfile() { Age = age, Gender = gender };
            var graderDB = GraderDBGenerator.FromUserProfile(profile, unit);
            graderDB.GradersWeight = graderWeights;

            var menuGen = new MenuGenerator(unit, graderDB);

            return menuGen.GetMenu();
        }


        private void TestTasteGrader(Boolean categoryTest)
        {
            // Make sure that grader is grading different user profiles in a different manner

            var unit = new RestDBInterface();
            unit.Meals.Empty();
            var indianBreakfast = new Meal() {
                ID = 0,
                Name = "Indian breakfast",
                ProductsWeight = new Dictionary<string, double>() { { "Carrot", 20 } },
                Types = new HashSet<MealType>() { MealType.Breakfast },
                Categories= new HashSet<MealCategory>() { MealCategory.Indian }
            };
            var chineseBreakfast = new Meal() {
                 ID = 1,
                Name = "Chinese breakfast",
                ProductsWeight = new Dictionary<string, double>()
                {                   
                    { "Carrot", 200},
                    { "Tomatoes", 123},
                    { "Avocados", 50},
                    { "bacon", 50},
                    { "Tuna", 200},
                    { "Almond", 25}
                },
                Types = new HashSet<MealType>() { MealType.Breakfast },
                Categories = new HashSet<MealCategory>() { MealCategory.Chinese }
            };

            unit.Meals.Empty();
            unit.Meals.Add(indianBreakfast);
            unit.Meals.Add(chineseBreakfast);

            var userProfile = new UserProfile() { Age = 30, Gender = GenderType.Male };
            var range = new MenuSuggestionRange() { Length = 1, MealsInDailyMenu = new List<MealType>() { MealType.Breakfast } };

            var indianGraderDB = GraderDBGenerator.FromUserProfile(userProfile, unit);
            if (categoryTest)
            {
                indianGraderDB.mealCategoryGrade = new Dictionary<MealCategory, double>()
                {
                    { MealCategory.Indian, 1 },
                    { MealCategory.Chinese, -1 }
                };
            }
            else
            {
                indianGraderDB.productFlavorGrade = new Dictionary<string, double>()
                {
                    { "Tomatoes", -1},
                    { "Avocados", -1},
                    { "bacon", -1},
                    { "Tuna", -1},
                    { "Almond", -1}
                };
            }
            indianGraderDB.range = range;

            var chineseGraderDB = GraderDBGenerator.FromUserProfile(userProfile, unit);
            if (categoryTest)
            {
                chineseGraderDB.mealCategoryGrade = new Dictionary<MealCategory, double>()
                {
                    { MealCategory.Indian, -1 },
                    { MealCategory.Chinese, 1 }
                };
            }
            chineseGraderDB.range = range;

            var indianMenuGen = new MenuGenerator(unit, indianGraderDB);
            var chineseMenuGen = new MenuGenerator(unit, chineseGraderDB);

            var indianMenu = indianMenuGen.GetMenu();
            var chineseMenu = chineseMenuGen.GetMenu();

            // Assertions

            /* Clearly, the chinese meal is better. Because both users has the same profile, except for their taste
             * preferences, they should get a different grade for the menu (the chinese user should get a higher grade).
             */

            // 1. Assert that this menu's grade isn't 100 (otherwise, there's no point in this test)
            Assert.IsTrue(indianMenu.Grade < 100);
            // 2. Assert that the chinese menu was graded better than the indian menu
            Assert.IsTrue(chineseMenu.Grade > indianMenu.Grade);
        }
    }
}
