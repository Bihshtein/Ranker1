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
    public class TestGraders
    {
        [TestMethod]
        public void TestMaxNutValuesGrader()
        {
            var unit = new RestDBInterface();

            var smallBreakfast = new Recipe()
            {
                ID = 0,
                Name = "Small breakfast",
                ProductsWeight = new Dictionary<string, double>() { { "Carrot", 1 } },
                Types = new HashSet<MealType>() { MealType.Breakfast }
            };

            var bigBreakfast = new Recipe()
            {
                ID = 1,
                Name = "Big breakfast",
                ProductsWeight = new Dictionary<string, double>() { { "Carrot", 1000 } },
                Types = new HashSet<MealType>() { MealType.Breakfast }
            };

            smallBreakfast.CalculateNutValuesAndCalories();
            bigBreakfast.CalculateNutValuesAndCalories();

            unit.TestsRecipes.Empty();
            unit.TestsRecipes.Add(smallBreakfast);
            unit.TestsRecipes.Add(bigBreakfast);

            var userProfile = new UserProfile() { Age = 30, Gender = GenderType.Male };
            var range = new MealSuggestionRange() { Length = 1, MealType = MealType.Breakfast };

            var recommendationDB = RecommendationDBGenerator.FromUserProfile(userProfile, unit);
            recommendationDB.range = range;
            recommendationDB.GradersWeight = new Dictionary<GraderType, double>()
                {
                    {GraderType.MaxNutValuesMealGrader, 1}
                };

            var recommendationGen = new RecommendationGenerator(unit, recommendationDB, false, true);

            var meal = recommendationGen.GetMeal();

            // Assertions

            /*
             * The big meal is clearly better. But since it passes the maximum value, the recommendationGen should prefer the small one.
             */
            Assert.IsTrue(meal.Recipe.ID == 0);
        }

        [TestMethod]
        public void TestPrepTimeMealGrader()
        {
            var unit = new RestDBInterface();

            var fastBreakfast = new Recipe() {
                ID = 0,
                Name = "Fast breakfast",
                ProductsWeight = new Dictionary<string, double>() { { "Carrot", 1 } },
                Types = new HashSet<MealType>() { MealType.Breakfast },
                PrepTime = new TimeSpan(0, 2, 0)
            };

            var slowBreakfast = new Recipe() {
                ID = 1,
                Name = "Slow breakfast",
                ProductsWeight = new Dictionary<string, double>() { { "Carrot", 1 } },
                Types = new HashSet<MealType>() { MealType.Breakfast },
                PrepTime = new TimeSpan(0, 20, 0)
            };

            fastBreakfast.CalculateNutValuesAndCalories();
            slowBreakfast.CalculateNutValuesAndCalories();

            unit.TestsRecipes.Empty();
            unit.TestsRecipes.Add(fastBreakfast);
            unit.TestsRecipes.Add(slowBreakfast);

            var userProfile = new UserProfile() { Age = 30, Gender = GenderType.Male };
            var range = new MealSuggestionRange() { Length = 1, MealType = MealType.Breakfast };

            var recommendationDB = RecommendationDBGenerator.FromUserProfile(userProfile, unit);
            recommendationDB.range = range;
            recommendationDB.GradersWeight = null;

            var recommendationGen = new RecommendationGenerator(unit, recommendationDB, false, true);

            var firstMeal = recommendationGen.GetMeal();
            var secondMeal = recommendationGen.GetMeal();

            // Assertions

            /* Since the 2 meals are equal except the prep time, we want the faster to be chosen.
             */

            // 1. Assert that the best graded meal is the fast one
            Assert.IsTrue(firstMeal.Recipe.ID == 0);
            // 2. Assert that the other meal is the slow one
            Assert.IsTrue(secondMeal.Recipe.ID == 1);
            // 3. Assert that the best graded meal got a better grade than the other one
            Assert.IsTrue(firstMeal.Grade > secondMeal.Grade);
        }

        [TestMethod]
        public void ServingsNumMealGrader()
        {
            var unit = new RestDBInterface();

            var singleBreakfast = new Recipe()
            {
                ID = 0,
                Name = "Single breakfast",
                ProductsWeight = new Dictionary<string, double>() { { "Carrot", 1 } },
                Types = new HashSet<MealType>() { MealType.Breakfast },
                Servings = 1
            };

            singleBreakfast.CalculateNutValuesAndCalories();

            var multipleBreakfast = new Recipe()
            {
                ID = 1,
                Name = "Multiple breakfast",
                ProductsWeight = new Dictionary<string, double>() { { "Carrot", 1 } },
                Types = new HashSet<MealType>() { MealType.Breakfast },
                Servings = 4
            };

            multipleBreakfast.CalculateNutValuesAndCalories();

            unit.TestsRecipes.Empty();
            unit.TestsRecipes.Add(singleBreakfast);
            unit.TestsRecipes.Add(multipleBreakfast);

            var userProfile = new UserProfile() { Age = 30, Gender = GenderType.Male };
            var range = new MealSuggestionRange() { Length = 1, MealType = MealType.Breakfast, ServingsNum = 1 };

            var recommendationDB = RecommendationDBGenerator.FromUserProfile(userProfile, unit);
            recommendationDB.range = range;
            recommendationDB.GradersWeight = null;

            var recommendationGen = new RecommendationGenerator(unit, recommendationDB, false, true);

            var firstMeal = recommendationGen.GetMeal();
            var secondMeal = recommendationGen.GetMeal();

            // Assertions

            /* Since the 2 meals are equal except the servings, we want the single to be chosen.
             */

            // 1. Assert that the best graded meal is the fast one
            Assert.IsTrue(firstMeal.Recipe.ID == 0);
            // 2. Assert that the other meal is the slow one
            Assert.IsTrue(secondMeal.Recipe.ID == 1);
            // 3. Assert that the best graded meal got a better grade than the other one
            Assert.IsTrue(firstMeal.Grade > secondMeal.Grade);
        }

        [TestMethod]
        public void TestNutValuesMealGrader()
        {

        }

        [TestMethod]
        public void TestCaloriesCountMealGrader()
        {
            TestUserProfileDependentMealGrader(false);
        }

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

            var smallBreakfast = new Recipe() {
                ID = 0,
                Name = "Small breakfast",
                ProductsWeight = new Dictionary<string, double>() { { "Carrot", 1 } },
                Types =new HashSet<MealType>() { MealType.Breakfast }
            };
            var bigBreakfast = new Recipe() {
                ID = 1,
                Name = "Big breakfast",
                ProductsWeight = new Dictionary<string, double>() { { "Carrot", 40} },
                Types = new HashSet<MealType>() { MealType.Breakfast } };
            var smallLunch = new Recipe() {
                ID = 2,
                Name = "Small lunch",
                ProductsWeight = new Dictionary<string, double>() { { "Tomato", 1 } },
                Types = new HashSet<MealType>() { MealType.Lunch } };
            var bigLunch = new Recipe() {
                ID = 3,
                Name = "Big Lunch",
                ProductsWeight = new Dictionary<string, double>() { { "Carrot", 400 } },
                Types = new HashSet<MealType>() { MealType.Lunch } };

            smallBreakfast.CalculateNutValuesAndCalories();
            bigBreakfast.CalculateNutValuesAndCalories();
            smallLunch.CalculateNutValuesAndCalories();
            bigLunch.CalculateNutValuesAndCalories();

            unit.TestsRecipes.Empty();
            unit.TestsRecipes.Add(smallBreakfast);
            unit.TestsRecipes.Add(bigBreakfast);
            unit.TestsRecipes.Add(smallLunch);
            unit.TestsRecipes.Add(bigLunch);

            var userProfile = new UserProfile() { Age = 30, Gender = GenderType.Male };
            var range = new MenuSuggestionRange()
                { Length = 1, MealsInDailyMenu = new List<MealType>() { MealType.Breakfast, MealType.Lunch } };

            var varietyRecommendationDB = RecommendationDBGenerator.FromUserProfile(userProfile, unit);
            varietyRecommendationDB.range = range;
            varietyRecommendationDB.GradersWeight = new Dictionary<GraderType, double>()
            {
                {GraderType.VarietyGrader, 1},
                {GraderType.VarietyDailyGrader, 1}
            };

            var normalRecommendationDB = RecommendationDBGenerator.FromUserProfile(userProfile, unit);
            normalRecommendationDB.range = range;

            normalRecommendationDB.GradersWeight = null;

            var varietyRecommendationGen = new RecommendationGenerator(unit, varietyRecommendationDB, false, true);
            var normalRecommendationGen = new RecommendationGenerator(unit, normalRecommendationDB, false, true);

            var varietyMenu = varietyRecommendationGen.GetMenu();
            var normalMenu = normalRecommendationGen.GetMenu();

            // Assertions

            /* Clearly, the big meals are better. But, since it contains a product repetition, we expect it to be given
             * a lower grade when we only use the variety grader.
             */

            // 1. Assert that the variety menu generator didn't generate the big lunch.
            Assert.IsTrue(varietyMenu.GetDay(0).Meals[MealType.Lunch].Recipe.Equals(smallLunch));
            // 2. Assert that the normal menu generator generated the big lunch.
            Assert.IsTrue(normalMenu.GetDay(0).Meals[MealType.Lunch].Recipe.Equals(bigLunch));
        }

        private void TestUserProfileDependentMealGrader(Boolean nutValues)
        {
            // Make sure that grader is grading different user profiles and different meals in a different manner

            var unit = new RestDBInterface();
            unit.TestsRecipes.Empty();

            // Create 2 very small breakfasts. We expect the "big" breakfast (big compared to the small one) to be better
            var smallBreakfast = new Recipe()
            {
                ID = 0,
                Name = "Small breakfast",
                ProductsWeight = new Dictionary<string, double>()
                {
                    { "Carrot", 1 }
                },
                Types = new HashSet<MealType>() { MealType.Breakfast }
            };

            var bigBreakfast = new Recipe()
            {
                ID = 1,
                Name = "Big breakfast",
                ProductsWeight = new Dictionary<string, double>()
                {
                    { "Carrot", 20 },
                    { "Tomato", 20 }
                },
                Types = new HashSet<MealType>() { MealType.Breakfast }
            };

            smallBreakfast.CalculateNutValuesAndCalories();
            bigBreakfast.CalculateNutValuesAndCalories();

            unit.TestsRecipes.Empty();
            unit.TestsRecipes.Add(smallBreakfast);
            unit.TestsRecipes.Add(bigBreakfast);

            if (nutValues)
            {
                TestNutValuesMealGraderInternal(unit);
            }
            else
            {
                TestCaloriesCountMealGraderInternal(unit);
            }
        }

        private void TestUserProfileDependentGrader(Boolean nutValues)
        {
            // Make sure that grader is grading different user profiles in a different manner

            var unit = new RestDBInterface();
            unit.TestsRecipes.Empty();
            var breakfast = new Recipe() {
                ID = 0,
                Name = "Sample breakfast 1",
                ProductsWeight = new Dictionary<string, double>()
            {
                { "Carrot", 20},
                { "Tomato", 23}
            },
                Types = new HashSet<MealType>() { MealType.Breakfast, MealType.Dinner }
            };

            var lunch = new Recipe() {
                ID = 1,
                Name = "Sample lunch 1",
                ProductsWeight = new Dictionary<string, double>()
            {
                {"Walnuts", 20}
            },
                Types = new HashSet<MealType>() { MealType.Lunch }
            };

            var dinner = new Recipe() {
                ID = 2,
                Name = "Sample dinner 1",
                ProductsWeight = new Dictionary<string, double>()
            {
                { "Tomato", 10},
                { "Lettuce", 10}
            },
                Types = new HashSet<MealType>() { MealType.Dinner, MealType.Breakfast }
            };

            breakfast.CalculateNutValuesAndCalories();
            lunch.CalculateNutValuesAndCalories();
            dinner.CalculateNutValuesAndCalories();

            unit.TestsRecipes.Empty();
            unit.TestsRecipes.Add(breakfast);
            unit.TestsRecipes.Add(lunch);
            unit.TestsRecipes.Add(dinner);

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
                {GraderType.MinNutValuesGrader, 1},
                {GraderType.MinNutValuesDailyGrader, 1}
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

        private void TestNutValuesMealGraderInternal(RestDBInterface unit)
        {
            var females = unit.DailyValues.Queries.GetByGender(GenderType.Female);
            var males = unit.DailyValues.Queries.GetByGender(GenderType.Male);

            Dictionary<GraderType, double> graderWeights = new Dictionary<GraderType, double>() {
                {GraderType.MinNutValuesGrader, 1}
            };

            var babyMeal = GetMeal(2, GenderType.Any, MealType.Breakfast, graderWeights, unit);
            var girlMeal = GetMeal(5, GenderType.Female, MealType.Breakfast, graderWeights, unit);
            Assert.IsTrue(babyMeal.Grade > girlMeal.Grade);// babies don't need much

            for (int i = 0; i < males.Count - 1; i++)
            {
                var maleItem1 = males[i];
                var maleItem2 = males[i + 1];
                var maleMeal1 = GetMeal(maleItem1.Age.MinAge, GenderType.Male, MealType.Breakfast, graderWeights, unit);
                var maleMeal2 = GetMeal(maleItem2.Age.MinAge, GenderType.Male, MealType.Breakfast, graderWeights, unit);
                var femaleItem1 = females[i];
                var femaleItem2 = females[i + 1];
                var femaleMeal1 = GetMeal(femaleItem1.Age.MinAge, GenderType.Female, MealType.Breakfast, graderWeights, unit);
                var femaleMeal2 = GetMeal(femaleItem2.Age.MinAge, GenderType.Female, MealType.Breakfast, graderWeights, unit);

                Assert.IsTrue(maleMeal1.Grade < 100 && maleMeal2.Grade < 100 &&
                              femaleMeal1.Grade < 100 && femaleMeal1.Grade < 100);

                if (i < 2)
                { // During the growing years the need for vitamins increases with age overall
                    Assert.IsTrue(maleMeal1.Grade > maleMeal2.Grade);
                    Assert.IsTrue(femaleMeal1.Grade > femaleMeal2.Grade);
                }
                else
                {// As we grow order for some nutrients we need more for some less
                    Assert.IsTrue(maleMeal1.Grade != maleMeal2.Grade);
                    Assert.IsTrue(femaleMeal1.Grade != femaleMeal2.Grade);
                }
                // Overall men need more the woman
                Assert.IsTrue(maleMeal1.Grade < femaleMeal1.Grade);
                Assert.IsTrue(maleMeal2.Grade < femaleMeal2.Grade);
            }

        }

        private void TestCaloriesCountMealGraderInternal(RestDBInterface unit)
        {
            Dictionary<GraderType, double> graderWeights = new Dictionary<GraderType, double>() {
                {GraderType.CaloriesCountMealGrader, 1}
            };

            var babyProfile = new UserProfile() { Age = 3, Gender = GenderType.Male };
            var manProfile = new UserProfile() { Age = 30, Gender = GenderType.Male, Height = 175, Weight = 75 };
            var sportitianProfile = new UserProfile()
            { Age = 30, Gender = GenderType.Male, Height = 175, Weight = 75, ActivityLevel = PhysicalActivityLevel.VeriActive };
            var womanProfile = new UserProfile() { Age = 30, Gender = GenderType.Female, Height = 175, Weight = 75 };

            var babyRecoDB = RecommendationDBGenerator.FromUserProfile(babyProfile, unit);
            var manRecoDB = RecommendationDBGenerator.FromUserProfile(manProfile, unit);
            var sportitianRecoDB = RecommendationDBGenerator.FromUserProfile(sportitianProfile, unit);
            var womanRecoDB = RecommendationDBGenerator.FromUserProfile(womanProfile, unit);

            babyRecoDB.GradersWeight = graderWeights;
            manRecoDB.GradersWeight = graderWeights;
            sportitianRecoDB.GradersWeight = graderWeights;
            womanRecoDB.GradersWeight = graderWeights;

            babyRecoDB.range = new MealSuggestionRange() { MealType = MealType.Breakfast, Length = 1 };
            manRecoDB.range = new MealSuggestionRange() { MealType = MealType.Breakfast, Length = 1 };
            sportitianRecoDB.range = new MealSuggestionRange() { MealType = MealType.Breakfast, Length = 1 };
            womanRecoDB.range = new MealSuggestionRange() { MealType = MealType.Breakfast, Length = 1 };

            var babyRecoGen = new RecommendationGenerator(unit, babyRecoDB, false, true);
            var manRecoGen = new RecommendationGenerator(unit, manRecoDB, false, true);
            var sportitianRecoGen = new RecommendationGenerator(unit, sportitianRecoDB, false, true);
            var womanRecoGen = new RecommendationGenerator(unit, womanRecoDB, false, true);

            var babyMeal = babyRecoGen.GetMeal();
            var manMeal = manRecoGen.GetMeal();
            var sportitianMeal = sportitianRecoGen.GetMeal();
            var womanMeal = womanRecoGen.GetMeal();

            // First, assert that the bigger meal was preferred
            Assert.IsTrue(babyMeal.Recipe.ID == 1 && manMeal.Recipe.ID == 1 &&
                sportitianMeal.Recipe.ID == 1 && womanMeal.Recipe.ID == 1);
            // Second, assert that the man's grade was lower- the baby needs to eat less
            Assert.IsTrue(babyMeal.Grade > manMeal.Grade);
            // Third, assert that the sportitian's grade was lower- he needs to eat more
            Assert.IsTrue(manMeal.Grade > sportitianMeal.Grade);
            // Last, assert that the man's grade was lower- woman needs to eat less
            Assert.IsTrue(womanMeal.Grade > manMeal.Grade);
        }

        private void TestCaloriesCountGraderInternal(RestDBInterface unit)
        {
            Dictionary<GraderType, double> graderWeights = new Dictionary<GraderType, double>() {
                {GraderType.CaloriesCountGrader, 1},
                {GraderType.CaloriesCountDailyGrader, 1}
            };

            var babyProfile = new UserProfile() { Age = 3, Gender = GenderType.Male };
            var girlProfile = new UserProfile() { Age = 16, Gender = GenderType.Female };

            var babyRecommendationDB = RecommendationDBGenerator.FromUserProfile(babyProfile, unit);
            var girlRecommendationDB = RecommendationDBGenerator.FromUserProfile(girlProfile, unit);

            babyRecommendationDB.GradersWeight = graderWeights;
            girlRecommendationDB.GradersWeight = graderWeights;

            // Since all users currently get the same calories number, force a difference
            babyRecommendationDB.dailyCaloriesNum = girlRecommendationDB.dailyCaloriesNum / 2;

            var babyRecommendationGen = new RecommendationGenerator(unit, babyRecommendationDB, false, true);
            var girlRecommendationGen = new RecommendationGenerator(unit, girlRecommendationDB, false, true);

            var babyMenu = babyRecommendationGen.GetMenu();
            var girlMenu = girlRecommendationGen.GetMenu();

            Assert.IsTrue(babyMenu.Grade > girlMenu.Grade);// babies don't need much
        }

        private Menu GetMenu(int age, GenderType gender, Dictionary<GraderType, double> graderWeights, RestDBInterface unit )
        {
            var profile = new UserProfile() { Age = age, Gender = gender };
            var recommendationDB = RecommendationDBGenerator.FromUserProfile(profile, unit);
            recommendationDB.GradersWeight = graderWeights;

            var recommendationGen = new RecommendationGenerator(unit, recommendationDB, false, true);

            return recommendationGen.GetMenu();
        }

        private Meal GetMeal(int age, GenderType gender, MealType mealType,
            Dictionary<GraderType, double> graderWeights, RestDBInterface unit)
        {
            var profile = new UserProfile() { Age = age, Gender = gender };
            var recommendationDB = RecommendationDBGenerator.FromUserProfile(profile, unit);
            recommendationDB.GradersWeight = graderWeights;
            recommendationDB.range = new MealSuggestionRange() { Length = 1, MealType = mealType };

            var recommendationGen = new RecommendationGenerator(unit, recommendationDB, false, true);

            return recommendationGen.GetMeal();
        }

        private void TestTasteGrader(Boolean categoryTest)
        {
            // Make sure that grader is grading different user profiles in a different manner

            var unit = new RestDBInterface();
            unit.TestsRecipes.Empty();
            var indianBreakfast = new Recipe() {
                ID = 0,
                Name = "Indian breakfast",
                ProductsWeight = new Dictionary<string, double>() { { "Carrot", 20 } },
                Types = new HashSet<MealType>() { MealType.Breakfast },
                Categories= new HashSet<RecipeCategory>() { RecipeCategory.Indian }
            };
            var chineseBreakfast = new Recipe() {
                 ID = 1,
                Name = "Chinese breakfast",
                ProductsWeight = new Dictionary<string, double>()
                {                   
                    { "Carrot", 200},
                    { "Tomato", 123},
                    { "Avocados", 50},
                    { "bacon", 50},
                    { "Tuna", 200},
                    { "Almonds", 25}
                },
                Types = new HashSet<MealType>() { MealType.Breakfast },
                Categories = new HashSet<RecipeCategory>() { RecipeCategory.Chinese }
            };

            indianBreakfast.CalculateNutValuesAndCalories();
            chineseBreakfast.CalculateNutValuesAndCalories();

            unit.TestsRecipes.Empty();
            unit.TestsRecipes.Add(indianBreakfast);
            unit.TestsRecipes.Add(chineseBreakfast);

            var userProfile = new UserProfile() { Age = 30, Gender = GenderType.Male };
            var range = new MenuSuggestionRange() { Length = 1, MealsInDailyMenu = new List<MealType>() { MealType.Breakfast } };

            var indianRecommendationDB = RecommendationDBGenerator.FromUserProfile(userProfile, unit);
            if (categoryTest)
            {
                indianRecommendationDB.recipeCategoryGrade = new Dictionary<RecipeCategory, double>()
                {
                    { RecipeCategory.Indian, 1 },
                    { RecipeCategory.Chinese, -1 }
                };
            }
            else
            {
                indianRecommendationDB.productFlavorGrade = new Dictionary<string, double>()
                {
                    { "Tomato", -1},
                    { "Avocados", -1},
                    { "bacon", -1},
                    { "Tuna", -1},
                    { "Almonds", -1}
                };
            }
            indianRecommendationDB.range = range;

            var chineseRecommendationDB = RecommendationDBGenerator.FromUserProfile(userProfile, unit);
            if (categoryTest)
            {
                chineseRecommendationDB.recipeCategoryGrade = new Dictionary<RecipeCategory, double>()
                {
                    { RecipeCategory.Indian, -1 },
                    { RecipeCategory.Chinese, 1 }
                };
            }
            chineseRecommendationDB.range = range;

            indianRecommendationDB.GradersWeight = null;
            chineseRecommendationDB.GradersWeight = null;

            var indianRecommendationGen = new RecommendationGenerator(unit, indianRecommendationDB, false, true);
            var chineseRecommendationGen = new RecommendationGenerator(unit, chineseRecommendationDB, false, true);

            var indianMenu = indianRecommendationGen.GetMenu();
            var chineseMenu = chineseRecommendationGen.GetMenu();

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
