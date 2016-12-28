﻿using log4net;
using RestModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InitRecipes {
    class ParseRecipes {

        public static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static object Locker = new object();
        public static string FolderPath = Assembly.GetExecutingAssembly().Location + @"\..\..\..\..\FruitsDB\";
        public static int CurrIndex = 0;
        public static List<int> Indexes;

      

        public static void PopulateMealsDB() {
            Indexes = File.ReadAllLines(FolderPath + "recipes_num.txt").ToList().ConvertAll<int>((a => int.Parse(a)));

            var loadMealsBulkSize = Indexes.Count > 1000 ? 1000 : Indexes.Count;
            var unit = new RestDBInterface();
         
                unit.Meals.Empty();

            while (Indexes.Count > 0) {
                CurrIndex = 0;
                log.Debug("Indexes count : " + Indexes.Count);
                List<Task> tasks = new List<Task>();
                if (Indexes.Count > loadMealsBulkSize)
                    Indexes.Take(loadMealsBulkSize).ToList().ForEach(a => tasks.Add(new Task(new Action(() => ParseRecipe(a)))));
                else
                    Indexes.Take(Indexes.Count).ToList().ForEach(a => tasks.Add(new Task(new Action(() => ParseRecipe(a)))));
                tasks.ForEach(task => task.Start());
                tasks.ForEach(task => task.Wait());
            }

        }


        public static void ParseRecipe(int index) {
            var unit = new RestDBInterface();
            string page = string.Empty;
            try {
                page = new WebClient().DownloadString("http://allrecipes.com/recipe/" + index.ToString());
            }
            catch {
                log.Error("Failed to load recipe number : " + index);
                return;
            }
            var nameParts = page.Split(new string[2] { "<title>", "</title>" }, StringSplitOptions.None);
            var mealParts = page.Split(new string[1] { "<span class=\"toggle-similar__title\" itemprop=\"title\">" }, StringSplitOptions.None);
            var servingParts = page.Split(new string[1] { "<meta id=\"metaRecipeServings\" itemprop=\"recipeYield\" content=" }, StringSplitOptions.None); 
            if (mealParts.Length < 4) {
                log.Error("Wrong meal parts count : " + mealParts.Length);
                lock (Locker) {
                    Indexes.Remove(index);
                }
                return;
            }
            var name = nameParts[1];
            var servingStr = new String(servingParts[1].TakeWhile(a => a != '>').ToArray());
            int servings = int.Parse(servingStr.Replace("\"", ""));
            //var mealType =new String(mealParts[3].TakeWhile(a => a != '<').ToArray()).Trim().ToLower();

            var ingredientParts = page.Split(new string[1] { "itemprop=\"ingredients\">" }, StringSplitOptions.None);
            var ingredients = new List<string>();
            for (int i = 1; i < ingredientParts.Length; i++) {
                if (ingredientParts[i].Contains("<")) {
                    var chars = ingredientParts[i].TakeWhile(a => a != '<');
                    var igredient = new String(chars.ToArray());
                    var words = igredient.Split(' ').ToList();
                    if (!ingredients.Contains(igredient))
                        ingredients.Add(igredient);
                }
            }
            unit.Meals.Add(new Meal() {
                ID = index,
                Name = name,
                Ingredients = ingredients,
                Types = new HashSet<MealType>() { MealType.Breakfast },
                Servings = servings
            });

            lock (Locker) {
                log.Debug("Num " + index);
                Indexes.Remove(index);
            }
        }
    }
}
