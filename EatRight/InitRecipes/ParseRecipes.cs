using log4net;
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
        public static string FolderPath = Assembly.GetExecutingAssembly().Location + @"\..\..\..\..\LocalDB\";
        public static List<int> Indexes;
        private static Dictionary<MealType, string> typeURLs = new Dictionary<MealType, string>() {
            { MealType.Breakfast,  "78/breakfast-and-brunch" },
            { MealType.Dinner,  "17562/dinner" }
        };

        private static HashSet<int> GetRecipeIdsByURL(string categoryURL)
        {
            var client = new WebClient();
            var first = true;
            var pageCount = 2;
            var ids = new HashSet<int>();
            while (true)
            {
                string pageStr = null;
                var urlSuffix = first ? "" : ("?page=" + pageCount);
                var url = "http://allrecipes.com/recipes/" + categoryURL + "/" + urlSuffix;
                try
                {
                    pageStr = client.DownloadString(url);
                }
                catch (Exception) // We've passed the last page
                {
                    break;
                }

                if (first)
                {
                    first = false;
                }
                else
                {
                    if (pageCount % 10 == 0)
                    {
                        System.Console.WriteLine("Locating recipes in " + categoryURL + ": Parsing page " + pageCount);
                    }
                    pageCount++;
                }

                // Split the whole page str by recipe URLs. We assume that each recipe URL on this page is relevant
                string[] parts = pageStr.Split(new string[] { "/recipe/" }, StringSplitOptions.None);
                foreach (var part in parts)
                {
                    string[] splittedPart = part.Split('/');
                    if (splittedPart.Length == 0)
                    {
                        continue;
                    }
                    var idStr = splittedPart[0];
                    int id;
                    if (int.TryParse(idStr, out id))
                    {
                        ids.Add(id);
                    }
                }
            }

            return ids;
        }

        public static void CreateDB() {
            Indexes = File.ReadAllLines(FolderPath + "recipes_num.txt").ToList().ConvertAll<int>((a => int.Parse(a)));
            
            var loadMealsBulkSize = Indexes.Count > 1000 ? 1000 : Indexes.Count;
            var unit = new RestDBInterface();
            unit.Recipes.Empty();
            var recipes = unit.Recipes.GetAll().ToList();
            
            var indexesToRemove = recipes.FindAll(recipe => !Indexes.Contains(recipe.ID ));
            indexesToRemove.ForEach(index => unit.Recipes.Delete(s => s.ID  , index.ID));
            while (Indexes.Count > 0) {
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
            lock (Locker) {
                if (unit.Recipes.Get(index) != null) {
                    Indexes.Remove(index);
                    return;
                }
            }
            string page = string.Empty;
            try {
                page = new WebClient().DownloadString("http://allrecipes.com/recipe/" + index.ToString());
            }
            catch {
                log.Error("Failed to load recipe number : " + index);
                return;
            }
           
            unit.Recipes.Add(new Recipe() {
                ID = index,
                Name = GetRecipeName(page).Replace(" - Allrecipes.com",""),
                Ingredients = GetIngredients(page),
                Types = new HashSet<MealType>() { MealType.Breakfast, MealType.Lunch, MealType.Dinner},
                Servings = GetServings(page),
                PrepTime = GetPrepTime(page)

            });

            lock (Locker) {
                log.Debug("Num " + index);
                Indexes.Remove(index);
            }
        }

        private static string GetRecipeName(string page)
        {
            return  page.Split(new string[2] { "<title>", "</title>" }, StringSplitOptions.None)[1];
        }

        private static List<string> GetIngredients(string page)
        {
            var ingredientParts = page.Split(new string[1] { "itemprop=\"ingredients\">" }, StringSplitOptions.None);
            var ingredients = new List<string>();
            for (int i = 1; i < ingredientParts.Length; i++)
            {
                if (ingredientParts[i].Contains("<"))
                {
                    var chars = ingredientParts[i].TakeWhile(a => a != '<');
                    var igredient = new String(chars.ToArray());
                    var words = igredient.Split(' ').ToList();
                    if (!ingredients.Contains(igredient))
                        ingredients.Add(igredient);
                }
            }
            return ingredients;
        }

        private static TimeSpan GetPrepTime(string page)
        {
            var prepTimeParts = page.Split(new string[1] { "<span class=\"ready-in-time\">" }, StringSplitOptions.None);
            var prepTimeStr = new String(prepTimeParts[1].TakeWhile(a => a != '<').ToArray());
            return ParsePrepTime(prepTimeStr);
        }

        private static int GetServings(string page)
        {
            var servingParts = page.Split(new string[1] { "<meta id=\"metaRecipeServings\" itemprop=\"recipeYield\" content=" }, StringSplitOptions.None);
            var servingStr = new String(servingParts[1].TakeWhile(a => a != '>').ToArray());
            return int.Parse(servingStr.Replace("\"", ""));
        }

        private static TimeSpan ParsePrepTime(string time) {
            var hours = GetTimeUnit(ref time, 'h');
            return new TimeSpan(hours, GetTimeUnit(ref time, 'm'), 0);
        }

        public static int GetTimeUnit(ref string time, char timeUnit) {
            var parts = time.Split(timeUnit);
            if (parts.Length > 1) {
                time = parts[1];
                return int.Parse(parts[0]);
            }
            else
                return 0;
        }
    }
}
