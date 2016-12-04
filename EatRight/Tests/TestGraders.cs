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
            // Currently all the users has the same calories ideal value, so there's nothing to check

            //TestUserProfileDependentGrader(new Dictionary<GraderType, double>() {
            //{GraderType.CaloriesCountGrader, 1},
            //{GraderType.CaloriesCountDailyGrader, 1}
            //});
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
            TestUserProfileDependentGrader(new Dictionary<GraderType, double>() {
            {GraderType.NutValuesGrader, 1},
            {GraderType.NutValuesDailyGrader, 1}
            });
        }

        [TestMethod]
        public void TestProductsTasteGrader()
        {
            TestTasteGrader(false);
        }

        [TestMethod]
        public void TestVarietyGrader()
        {

        }

        private void TestUserProfileDependentGrader(Dictionary<GraderType, double> graderWeights)
        {
            // Make sure that grader is grading different user profiles in a different manner

            var unit = new RestDBInterface();

            var breakfast = new Meal("Sample breakfast 1", new Dictionary<string, double>()
            {
                { "Carrot", 20},
                { "Tomatoes", 23}
            }, new HashSet<MealType>() { MealType.Breakfast, MealType.Dinner });

            var lunch = new Meal("Sample lunch 1", new Dictionary<string, double>()
            {
                {"Walnut", 20}
            }, new HashSet<MealType>() { MealType.Lunch });

            var dinner = new Meal("Sample dinner 1", new Dictionary<string, double>()
            {
                { "Tomatoes", 10},
                { "Lettuce", 10}
            }, new HashSet<MealType>() { MealType.Dinner, MealType.Breakfast });
            unit.Meals.Empty();
            unit.Meals.Add(breakfast);
            unit.Meals.Add(lunch);
            unit.Meals.Add(dinner);

            var babyProfile = new UserProfile() { Age = 3, Gender = GenderType.Male };
            var oldWomanProfile = new UserProfile() { Age = 75, Gender = GenderType.Female };

            var babyGraderDB = GraderDBGenerator.FromUserProfile(babyProfile, unit);
            babyGraderDB.gradersWeight = graderWeights;
            var oldWomanGraderDB = GraderDBGenerator.FromUserProfile(oldWomanProfile, unit);
            oldWomanGraderDB.gradersWeight = graderWeights;

            var babyMenuGen = new MenuGenerator(unit, babyGraderDB);
            var oldWomanMenuGen = new MenuGenerator(unit, oldWomanGraderDB);

            var babyMenu = babyMenuGen.GetMenu();
            var oldWomanMenu = oldWomanMenuGen.GetMenu();

            // Assertions
            // 1. Assert that this menu's grade isn't 100 (otherwise, there's no point in this test)
            Assert.IsTrue(babyMenu.Grade < 100 || oldWomanMenu.Grade < 100);
            // 2. Assert that the 2 menus got different grades
            Assert.IsTrue(babyMenu.Grade != oldWomanMenu.Grade);
        }

        private void TestTasteGrader(Boolean categoryTest)
        {
            // Make sure that grader is grading different user profiles in a different manner

            var unit = new RestDBInterface();

            var indianBreakfast = new Meal("Indian breakfast", new Dictionary<string, double>() { { "Carrot", 20 } },
                new HashSet<MealType>() { MealType.Breakfast }, new HashSet<MealCategory>() { MealCategory.Indian });
            var chineseBreakfast = new Meal("Chinese breakfast", new Dictionary<string, double>()
                {
                    { "Carrot", 200},
                    { "Tomatoes", 123},
                    { "Avocados", 50},
                    { "bacon", 50},
                    { "Tuna", 200},
                    { "Almond", 25}
                }, new HashSet<MealType>() { MealType.Breakfast }, new HashSet<MealCategory>() { MealCategory.Chinese });

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
