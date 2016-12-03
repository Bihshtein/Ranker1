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
    }
}
