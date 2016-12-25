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
    public class TestDB
    {
        [TestMethod]
        public void TestFixedMeals()
        {
            var unit = new RestDBInterface();
            MealsCSVReader.CreateFixedMealsList(unit);
            var profile = new UserProfile() { Age = 30, Gender = GenderType.Male };
            var graderDB = GraderDBGenerator.FromUserProfile(profile, unit);
            graderDB.range = new MealSuggestionRange();
            var menuGen = new MenuGenerator(unit, graderDB);
            MenuMeal meal = menuGen.GetMeal();

            Assert.IsNotNull(meal);
            Assert.IsNotNull(meal.Meal);
            Assert.IsTrue(meal.Grade > 0);
        }
       
    }
}
