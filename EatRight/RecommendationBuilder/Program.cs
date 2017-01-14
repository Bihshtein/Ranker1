using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestModel;
using RecommendationBuilder.Graders;
using Logic;

namespace RecommendationBuilder
{
    class Program
    {
        static RestDBInterface unit;

        static void Main(string[] args)
        {
            unit = new RestDBInterface();

            GenerateTestMenu1();

            var uProfile = new UserProfile()
            {
                Id = 777,
                Age = 27,
                Gender = GenderType.Male,
                Pregnant = 0,
                Name = "Hen"
            };

            var recommendationDB = RecommendationDBGenerator.FromUserProfile(uProfile, unit);

            RecommendationGenerator generator = new RecommendationGenerator(unit, recommendationDB, false, true);
            var menu = generator.GetMenu();
            System.Console.WriteLine("Menu eavluated to " + menu.Grade);
            System.Console.WriteLine("The grader that contributed the highest value to the grade is: " + menu.GradeInfo.BestGraders[0]);
            System.Console.WriteLine("The grader in which the highest number of points was lost is: " + menu.GradeInfo.WorstGraders[0]);
            Console.ReadKey();
        }

        public static void GenerateTestMenu1()
        {
            var list = unit.Products.GetTopFoods(150,1);
            //list.RemoveAll((p) => p.Animal != string.Empty);
            foreach (Product prod in list)
            {
                System.Console.WriteLine(prod.Name1);
            }
            InitSampleMeal(unit);// currently resets the db and add the meal every time
        }

        private static void InitSampleMeal(RestDBInterface unit) {


            Recipe breakfast = new Recipe() {
                ID = 0,
                Name = "Sample breakfast 1",
                ProductsWeight = new Dictionary<string, double>()
            {
                { "Carrot", 200},
                { "Tomato", 123},
                { "Avocados", 50},
                { "bacon", 50},
                { "Tuna", 200},
                { "Almonds", 25}
            },
                Types = new HashSet<MealType>() { MealType.Breakfast, MealType.Dinner }
            };
            
            var lunch = new Recipe() {
                ID = 1,
                Name = "Sample lunch 1",
                ProductsWeight = new Dictionary<string, double>()
            {
                {"Salmon", 150 },
                {"Pepper", 100},
                {"walnuts", 20},
                { "bacon", 50},
                { "almonds", 25},
                {"Broccoli", 100}
            },
                Types = new HashSet<MealType>() { MealType.Lunch }
            };

            var dinner = new Recipe() {
                ID = 2,
                Name = "Sample dinner 1",
                ProductsWeight = new Dictionary<string, double>()
            {
                {"loin", 100 },
                { "Cabbage", 100},
                { "Tomato", 100},
                { "Lettuce", 100},
                { "Garlic", 50},
                { "Egg", 200},
                { "Almonds", 50}
            },
                Types = new HashSet<MealType>() { MealType.Dinner, MealType.Breakfast }
            };
            unit.Recipes.Empty();
            if (unit.Recipes.Get(0) == null)
                unit.Recipes.Add(breakfast);
            if (unit.Recipes.Get(1) == null)
                unit.Recipes.Add(lunch);
            if (unit.Recipes.Get(2) == null)
                unit.Recipes.Add(dinner);
        }
    }
}
