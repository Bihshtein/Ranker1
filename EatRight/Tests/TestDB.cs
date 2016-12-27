using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestModel;
using RecommendationBuilder;
using RecommendationBuilder.Graders;

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
            var recommendationDB = RecommendationDBGenerator.FromUserProfile(profile, unit);
            recommendationDB.range = new MealSuggestionRange();
            var recomGen = new RecommendationGenerator(unit, recommendationDB);
            MealWrapper meal = recomGen.GetMeal();

            Assert.IsNotNull(meal);
            Assert.IsNotNull(meal.Meal);
            Assert.IsTrue(meal.Grade > 0);
        }
       
    }
}
