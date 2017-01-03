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
    public class TestFilters
    {
        [TestMethod]
        public void TestForbiddenProductsFilter()
        {
            var unit = new RestDBInterface();

            Recipe goodBreakfast = new Recipe()
            {
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

            Recipe badBreakfast = new Recipe()
            {
                ID = 1,
                Name = "Bad breakfast",
                ProductsWeight = new Dictionary<string, double>() { { "Carrot", 20 } },
                Types = new HashSet<MealType>() { MealType.Breakfast }
            };

            goodBreakfast.CalculateNutValuesAndCalories();
            badBreakfast.CalculateNutValuesAndCalories();

            unit.TestsRecipes.Empty();
            unit.TestsRecipes.Add(goodBreakfast);
            unit.TestsRecipes.Add(badBreakfast);

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

            var recommendationGeneratorWOT = new RecommendationGenerator(unit, recommendationDBWithoutTomatoes, false, true);
            var recommendationGeneratorWT = new RecommendationGenerator(unit, recommendationDBWithTomatoes, false, true);
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
            Assert.IsTrue(menuWOT.GetDay(0).Meals[MealType.Breakfast].Recipe.Equals(badBreakfast));
            // And we get the good meal with not restrictions
            Assert.IsTrue(menuWT.GetDay(0).Meals[MealType.Breakfast].Recipe.Equals(goodBreakfast));
        }

        [TestMethod]
        public void TestMealTypeFilter()
        {
            var unit = new RestDBInterface();

            Recipe breakfast = new Recipe()
            {
                ID = 0,
                Name = "Breakfast",
                ProductsWeight = new Dictionary<string, double>(){{ "Carrot", 1}},
                Types = new HashSet<MealType>() { MealType.Breakfast }
            };

            breakfast.CalculateNutValuesAndCalories();

            unit.TestsRecipes.Empty();
            unit.TestsRecipes.Add(breakfast);

            var userProfile = new UserProfile() { Age = 30, Gender = GenderType.Male };
            var range = new MealSuggestionRange()
            { Length = 1, MealType = MealType.Lunch };

            var recommendationDB = RecommendationDBGenerator.FromUserProfile(userProfile, unit);
            recommendationDB.range = range;

            var recommendationGenerator = new RecommendationGenerator(unit, recommendationDB, false, true);
            var meal = recommendationGenerator.GetMeal();

            // Assertions
            // There's no lunch in the meals list, so we expect no meals to be generated.
            Assert.IsNull(meal);
        }

        private void TestHasProperyFilter(string productName, UserRestrictions pref)
        {
            var unit = new RestDBInterface();

            Recipe breakfast = new Recipe()
            {
                ID = 0,
                Name = "Breakfast",
                ProductsWeight = new Dictionary<string, double>() { { productName, 1 } },
                Types = new HashSet<MealType>() { MealType.Breakfast }
            };

            breakfast.CalculateNutValuesAndCalories();

            unit.TestsRecipes.Empty();
            unit.TestsRecipes.Add(breakfast);

            var userProfile = new UserProfile()
            {
                Age = 30,
                Gender = GenderType.Male,
                Restrictions = new HashSet<UserRestrictions>() { pref }
            };

            var range = new MealSuggestionRange()
            { Length = 1, MealType = MealType.Breakfast };

            var recommendationDB = RecommendationDBGenerator.FromUserProfile(userProfile, unit);
            recommendationDB.range = range;

            var recommendationGenerator = new RecommendationGenerator(unit, recommendationDB, false, true);
            var reco = recommendationGenerator.GetRecommendation();

            // Assertions
            // There's no meal with the property in the meals list, so we expect no meals to be generated.
            Assert.IsNull(reco);
        }

        [TestMethod]
        public void TestHasMeatFilter()
        {
            TestHasProperyFilter("Carrot", UserRestrictions.Meat);
        }

        [TestMethod]
        public void TestHasNoMeatFilter()
        {
            TestHasProperyFilter("Ham", UserRestrictions.NoMeat);
        }

        [TestMethod]
        public void TestHasDairyFilter()
        {
            TestHasProperyFilter("Egg", UserRestrictions.Dairy);
        }

        [TestMethod]
        public void TestHasNoDairyFilter()
        {
            TestHasProperyFilter("Butter", UserRestrictions.NoDairy);
        }

        [TestMethod]
        public void TestHasFishFilter()
        {
            TestHasProperyFilter("Carrot", UserRestrictions.Fish);
        }

        [TestMethod]
        public void TestHasNoFishFilter()
        {
            TestHasProperyFilter("Salmon", UserRestrictions.NoFish);
        }
    }
}
