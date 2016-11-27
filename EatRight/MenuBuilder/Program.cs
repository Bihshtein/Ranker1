﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestModel;
using MenuBuilder.Graders;

namespace MenuBuilder
{
    class Program
    {
        static RestDBInterface unit;

        static void Main(string[] args)
        {
            unit = new RestDBInterface();

            GenerateTestMenu1();
            var graderDB = new GraderDB();
            graderDB.dailyValues = RestRepository<Product>.DailyValues;
            graderDB.dailyCaloriesNum = 3000;
            graderDB.range = new SuggestionRange() { Type = SuggestionRangeType.Days, Length = 1 };

            MenuGenerator generator = new MenuGenerator(unit, graderDB);
            var menu = generator.GetMenu();
            System.Console.WriteLine("Menu eavluated to " + menu.Grade);
            System.Console.WriteLine("The grader that contributed the highest value to the grade is: " + menu.GradeInfo.BestGraders[0]);
            System.Console.WriteLine("The grader in which the highest number of points was lost is: " + menu.GradeInfo.WorstGraders[0]);
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


            Meal breakfast = new Meal("Sample breakfast 1", new Dictionary<string, double>()
            {
                { "Carrot", 200},
                { "Tomatoes", 123},
                { "Avocados", 50},
                { "bacon", 50},
                { "Tuna", 200},
                { "Almond", 25}
            }, new HashSet<MealType>() { MealType.Breakfast, MealType.Dinner });
            
            var lunch = new Meal("Sample lunch 1", new Dictionary<string, double>()
            {
                {"Salmon", 150 },
                {"Pepper", 100},
                {"Walnut", 20},
                { "bacon", 50},
                { "Almond", 25},
                {"Broccoli", 100}
            }, new HashSet<MealType>() { MealType.Lunch });

            var dinner = new Meal("Sample dinner 1", new Dictionary<string, double>()
            {
                {"loin", 100 },
                { "Cabbage", 100},
                { "Tomatoes", 100},
                { "Lettuce", 100},
                { "Garlic", 50},
                { "Egg", 200},
                { "Almond", 50}
            }, new HashSet<MealType>() { MealType.Dinner, MealType.Breakfast });
            unit.Meals.Empty();
            if (unit.Meals.Get(0) == null)
                unit.Meals.Add(breakfast);
            if (unit.Meals.Get(1) == null)
                unit.Meals.Add(lunch);
            if (unit.Meals.Get(2) == null)
                unit.Meals.Add(dinner);
        }
    }
}
