﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestModel;


namespace MenuBuilder
{
    class Program
    {
        static RestDBInterface unit;

        static void Main(string[] args)
        {
            unit = new RestDBInterface();

            var testMenu1 = GenerateTestMenu1();
            var eval = MenuGenerator.EvaluateMenu(testMenu1, RestRepository<Product>.DailyValues, 3000);
            System.Console.WriteLine("Menu eavluated to " + eval);
        }

        private static Menu GenerateTestMenu1()
        {
            var list = unit.Products.GetTopFoods(150,1);
            //list.RemoveAll((p) => p.Animal != string.Empty);
            foreach (Product prod in list)
            {
                System.Console.WriteLine(prod.Name);
            }
            InitSampleMeal(unit);// currently resets the db and add the meal every time
            var day1 = new DailyMenu(unit.Meals.Get(1), unit.Meals.Get(2), unit.Meals.Get(3));

            var dayList = new List<DailyMenu>() { day1 };
            return new Menu(dayList);
        }

        private static void InitSampleMeal(RestDBInterface unit) {


            Meal breakfast = new Meal(1, new Dictionary<string, double>()
            {
               // { "Bread", 25},
             //   { "Cottage", 150},
                { "Carrot", 200},
                { "Tomato", 123},
                { "Avocado", 50},
                { "bacon", 50},
                { "Tuna", 200},
                { "Almond", 25}
            });
            
            var lunch = new Meal(2, new Dictionary<string, double>()
            {
                {"Salmon", 150 },
                {"Pepper", 100},
                {"Walnut", 20},
                { "bacon", 50},
                { "Almond", 25},
                {"Broccoli", 100}
            });

            var dinner = new Meal(3, new Dictionary<string, double>()
            {
                {"tenderloin", 100 },
                { "Cabbage", 100},
                { "Tomato", 100},
                { "Lettuce", 100},
                { "Garlic", 50},
                { "Egg", 200},
                { "Almond", 50}
            });
            unit.Meals.Empty();
            if (unit.Meals.Get(1) == null)
                unit.Meals.Add(breakfast);
            if (unit.Meals.Get(2) == null)
                unit.Meals.Add(lunch);
            if (unit.Meals.Get(3) == null)
                unit.Meals.Add(dinner);
        }
    }
}