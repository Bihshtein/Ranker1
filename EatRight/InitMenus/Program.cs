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
using System.Text.RegularExpressions;

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
        public static List<string> CutDetails = new List<string> {"sprig", "sprigs","ground","shredded","cubed","head","heads","sliced","stalk", "stalks", "diced", "minced", "chopped" };
        public static List<string> CookDetails = new List<string> { "cooked","fresh"};
        public static List<string> PackDetails = new List<string> { "can" };
        public static RestDBInterface unit = new RestDBInterface();
        public static void AddActualProductsToMealsDB() {
            int totalMatched = 0;
            int total = 0;
            int totalParsed = 0;
            var list = new string [] {"slice ","bunch", "cloves", "whole","pinch","slices","large","small","medium","teaspoon ", "teaspoons", "tablespoons", "tablespoon ","pound ","pound)", "pounds ", "ounces", "ounce ","ounce)", "cup ", "cups " };
            
            
            var meals = unit.Meals.GetAll().ToList();
            var foundProducts = new List<Product>();
            Console.WriteLine("Group:\t\tName1:\t\tName2:\t\tName3:\t\tCount:\t\t Original:");
            Console.WriteLine();
            foreach (var meal in meals) {
                foreach (var item in meal.Ingredients) {
                    var parts = item.ToLower().Split(list, StringSplitOptions.None);
                    ++total;
                    if (parts.Length == 1) 
                        parts =  Regex.Split(item, @"\d");
                    if (parts.Length > 1) {
                        ++totalParsed;
                        var innerparts = parts[1].Split(',');
                        var innerSplit = innerparts[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        List<Product> res = null;
                        if (innerSplit.Length == 1) {
                            if (innerSplit[0] == "water")
                                ++totalMatched;
                            else
                                res = HandleSingleWordName(innerSplit[0]);
                        }
                        else if (innerSplit.Length == 2)
                            res = HandleDoubleWordName(innerSplit[0], innerSplit[1]);
                        else if (innerSplit.Length == 3)
                            res = HandleTripleWordName(innerSplit[0], innerSplit[1], innerSplit[2]);

                        if (res != null && res.Count > 0) {
                            //  Console.WriteLine(res[0].FoodGroup+"\t"+res[0].Name1+"\t" + res[0].Name2 + "\t" + res[0].Name3 + "\t" + res.Count + "\t" + item);
                            ++totalMatched;
                        }
                        if (innerSplit.Length == 3 && res.Count == 0) {
                            Console.WriteLine(innerparts[0]);
                        }
                    }
                   
                }
            }
            Console.WriteLine("total meals : "  + meals.Count);
            Console.WriteLine("total ingredients : " + total);
            Console.WriteLine("total ingredients parsed : " + totalParsed);
            Console.WriteLine("total ingredients matched : " + totalMatched);
        }

        public static List<Product> HandleSingleWordName(string name) {
            return unit.Products.Queries.TryMatchWholeProduct(name); 
        }

        public static List<Product> HandleDoubleWordName(string part1, string part2) {
            if (CutDetails.Any(cut => cut.Equals(part1)) || CookDetails.Any(cook => cook.Equals(part1)))
                return  HandleSingleWordName(part2);
            else
                return unit.Products.Queries.TryMatchWholeProduct(part1, part2);
        }

        public static List<Product> HandleTripleWordName(string part1, string part2, string part3) {
            if (CutDetails.Any(cut => cut.Equals(part1)) || PackDetails.Any(pack => pack.Equals(part1)))
                return HandleDoubleWordName(part2,part3);
            else
                return unit.Products.Queries.TryMatchWholeProduct(part1, part2, part3);
        }

        public static void PopulateMealsDB() {

            Indexes = File.ReadAllLines(FolderPath + "recipes_num.txt").ToList().ConvertAll<int>((a =>  int.Parse(a)));
            var unit = new RestDBInterface();
            unit.Meals.Empty();
            Indexes.ForEach(a=>ThreadPool.QueueUserWorkItem(delegate { ParseRecipe();},null));
            Thread.Sleep(1000*60*2);
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
