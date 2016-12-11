using RestModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using InitDB;

namespace InitRecipes {
    class Program {
        public static string FolderPath = Assembly.GetExecutingAssembly().Location + @"\..\..\..\..\FruitsDB\";
        public static int CurrIndex = 0;
        public static List<int> Indexes;
        public static object Locker = new object();
        static void Main(string[] args) {
            var initDB = !args.Contains("skip_init");
            var enrichDB= !args.Contains("skip_enrich");
            if (initDB)
                PopulateMealsDB();
            if (enrichDB)
            AddActualProductsToMealsDB();
        }
        public static void AddActualProductsToMealsDB() {
            int total = 0;
            var list = new string [] {"slice ","bunch", "cloves", "whole","pinch","slices","large","small","medium","teaspoon ", "teaspoons", "tablespoons", "tablespoon ","pound ","pound)", "pounds ", "ounce ","ounce)", "cup ", "cups " };
            var unit = new RestDBInterface();
            Console.WriteLine("FoodGroup \t\t Original ");
            var meals = unit.Meals.GetAll().ToList();
            var foundProducts = new List<Product>();
            foreach (var meal in meals) {
                foreach (var item in meal.Ingredients) {
                    var parts = item.Split(list, StringSplitOptions.None);
                    if (parts.Length >1) {
                        var innerparts = parts[1].Split(',');
                        var innerSplit = innerparts[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        if (innerSplit.Length == 2) {
                            var res = unit.Products.Queries.TryMatchWholeProduct(innerSplit[0], innerSplit[1]);
                            if (res.Count > 0)
                                Console.WriteLine("Group: {0}, Name1: {1}, Name2: {2}, Name3 {3}, Count: {4}, \t Original: {5}", res[0].FoodGroup, res[0].Name1, res[0].Name2, res[0].Name3, res.Count,item);
                        ++total;
                    }
                    }
                   
                       
                }
            }
            Console.WriteLine("total meals : "  + meals.Count);
            Console.WriteLine("total ingredients parsed : "  + total);
        }

        public static void PopulateMealsDB() {

            Indexes = File.ReadAllLines(FolderPath + "recipes_num.txt").ToList().ConvertAll<int>((a =>  int.Parse(a)));
            var unit = new RestDBInterface();
            unit.Meals.Empty();
            Indexes.ForEach(a=>ThreadPool.QueueUserWorkItem(delegate { ParseRecipe();},null));
            Thread.Sleep(1000*60*5);
        }


        public static void ParseRecipe() {
            var unit = new RestDBInterface();
            string idStr;
            string page = string.Empty;
            try {
                lock (Locker){
                    ++CurrIndex;
                    idStr = Indexes[CurrIndex].ToString();
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
            var mealsType = new HashSet<MealType>();
            
            if (mealType.Contains(MealType.Breakfast.ToString().ToLower()))
                mealsType.Add(MealType.Breakfast);
            if (mealType.Contains(MealType.Lunch.ToString().ToLower()))
                mealsType.Add(MealType.Lunch);
            if (mealType.Contains(MealType.Dinner.ToString().ToLower()))
                mealsType.Add(MealType.Dinner);
            lock (Locker) {
                try {
                    unit.Meals.Add(new Meal() {
                        ID = CurrIndex, Name =name,Ingredients = ingredients, MealType =mealType
                    });
                    Console.WriteLine(
                        string.Format("Local num : {0}, Source num {1} ", CurrIndex, Indexes[CurrIndex]));
                }
                catch { }
            }

        }



    }

}
