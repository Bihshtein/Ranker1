﻿using RestModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace InitRecipes {
    class Program {
        public static int ID = 213000;
        public static object Locker = new object();
        static void Main(string[] args) {
            var unit = new RestDBInterface();
            unit.Meals.Empty();
            List<Task> list = new List<Task>();
            for (int i = 213000; i < 300000; ++i) {
                ThreadPool.QueueUserWorkItem(delegate { ParseRecipe(); },null);
            }
            for (int i = 0; i < 60*60; i++) {
                Thread.Sleep(1000);

            }
        }

    
        public static void ParseRecipe() {
            var unit = new RestDBInterface();
            unit.Meals.GetAll();
            string idStr;
            string page = string.Empty;
            try {
                lock (Locker){
                    ++ID;
                    idStr = ID.ToString();
                }
                page = new WebClient().DownloadString("http://allrecipes.com/recipe/" + idStr);
            }
            catch { return; }

            var nameParts = page.Split(new string[2] { "<title>", "</title>" }, StringSplitOptions.None);
            var mealParts = page.Split(new string[1] { "<span class=\"toggle-similar__title\" itemprop=\"title\">" }, StringSplitOptions.None);
            if (mealParts.Length != 4)
                return;
            var name = nameParts[1];
            var mealType = new String(mealParts[3].TakeWhile(a => a != '<').ToArray()).Trim().ToLower();

            


            var ingredientParts = page.Split(new string[1] { "itemprop=\"ingredients\">" }, StringSplitOptions.None);
            var ingredients = new Dictionary<string,double>();
            for (int i = 1; i < ingredientParts.Length; i++) { 
                if (ingredientParts[i].Contains("<")) {
                    var chars = ingredientParts[i].TakeWhile(a => a != '<');

                    var igredient = new String(chars.ToArray());
                    var words = igredient.Split(' ').ToList();
                    if (!ingredients.ContainsKey(igredient))
                        ingredients.Add(igredient,0);
                }
            }
            var mealsType = new HashSet<MealType>();
            
            if (mealType.Contains(MealType.Breakfast.ToString().ToLower()))
                mealsType.Add(MealType.Breakfast);
            if (mealType.Contains(MealType.Lunch.ToString().ToLower()))
                mealsType.Add(MealType.Lunch);
            if (mealType.Contains(MealType.Dinner.ToString().ToLower()))
                mealsType.Add(MealType.Dinner);
            lock (Locker) {
                try {
                    unit.Meals.Add(new Meal(name, ingredients, mealsType));
                }
                catch { }
            }
            Console.WriteLine("Num : " + idStr);

            Console.WriteLine();
        }



    }

}
