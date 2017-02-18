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
        public enum RecipesSource { AllRecipes, Cookpad }
        public static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string FolderPath = Assembly.GetExecutingAssembly().Location + @"\..\..\..\..\LocalDB\";

        public static Dictionary<RecipesSource, string> RecipesURLs = new Dictionary<RecipesSource, string>() {
            {RecipesSource.Cookpad,  "https://cookpad.com/us/" },
            {RecipesSource.AllRecipes,  "http://allrecipes.com/recipes/" },
        };

        public static Dictionary<RecipesSource, string> RecipesURNs = new Dictionary<RecipesSource, string>() {
            {RecipesSource.Cookpad,  "https://cookpad.com/us/recipes/" },
            {RecipesSource.AllRecipes,  "http://allrecipes.com/recipe/" },
        };

        private static Dictionary<RecipesSource, Dictionary<MealType, string>> MealTypesURNs = new Dictionary<RecipesSource, Dictionary<MealType, string>>() {
            {RecipesSource.AllRecipes, new Dictionary<MealType, string>() {
                {MealType.Breakfast,  "78/breakfast-and-brunch" },
                {MealType.Dinner,  "17562/dinner" }}
            },
            {RecipesSource.Cookpad, new Dictionary<MealType, string>() {
                {MealType.Breakfast,  "search/breakfast" },
                {MealType.Lunch,  "search/lunch" },
                {MealType.Dinner,  "search/dinner" }}
            }
        };

        public static object Locker = new object();
        public static HashSet<int> Indexes;

        public static void CreateDB() {
            Indexes = new HashSet<int>();
            var unit = new RestDBInterface();
            unit.Recipes.Empty();
            RecipesURLs.ToList().ForEach(s => AddRecipesBySource(s, unit));
        }

        public static void AddRecipesBySource(KeyValuePair<RecipesSource, string> source, RestDBInterface unit) {
            MealTypesURNs[source.Key].ToList().ForEach(m => AddRecipesByMealType(source.Key, m.Key, source.Value + m.Value, unit));
        }

        public static void AddRecipesByMealType(RecipesSource source, MealType mealType, string mealTypeURN, RestDBInterface unit, int loadBulkSize = 1000) {
            AddRecipesByURL(mealTypeURN, unit);
            ProblematicRecipes.ForEach(x => Indexes.Remove(x));
            var loadMealsBulkSize = Indexes.Count > loadBulkSize ? loadBulkSize : Indexes.Count;
            while (Indexes.Count > 0) {
                log.Debug("Indexes count : " + Indexes.Count);
                List<Task> tasks = new List<Task>();
                if (Indexes.Count > loadMealsBulkSize)
                    Indexes.Take(loadMealsBulkSize).ToList().ForEach(a => tasks.Add(new Task(new Action(() => ParseRecipe(a, RecipesURNs[source], mealType, source)))));
                else
                    Indexes.Take(Indexes.Count).ToList().ForEach(a => tasks.Add(new Task(new Action(() => ParseRecipe(a, RecipesURNs[source], mealType, source)))));
                tasks.ForEach(task => task.Start());
                tasks.ForEach(task => task.Wait());
            }
        }

        private static void AddRecipesByURL(string categoryURN, RestDBInterface unit, int pagesLimit = 1) {
            Indexes.Clear();
            var client = new WebClient();
            log.Debug("Locating recipes in ->" + categoryURN + " - started");
            for (int currPage = 0; currPage < pagesLimit; currPage++) {

                string pageStr = null;
                var urlSuffix = currPage == 0 ? "" : ("?page=" + currPage);
                var uri = categoryURN + "/" + urlSuffix;
                try {
                    pageStr = client.DownloadString(uri);
                    AddRecipesFromPage(pageStr, unit);
                }
                catch (Exception) {
                    log.Error(string.Format("Failed to load page num {0}, might be the last page", currPage));
                }
                log.Debug("Page num :" + currPage);
            }
            log.Debug("Locating recipes in ->" + categoryURN + " - completed");
        }

        public static void AddRecipesFromPage(string pageStr, RestDBInterface unit) {
            // Split the whole page str by recipe URLs. We assume that each recipe URL on this page is relevant
            string[] parts = pageStr.Split(new string[] { "data-id=\"" }, StringSplitOptions.None);
            for (int i = 1; i < parts.Length; i++) {

                string[] splittedPart = parts[i].Split(new string[] { "\"" }, StringSplitOptions.None);
                if (splittedPart.Length == 0) {
                    continue;
                }
                var idStr = splittedPart[0];
                int id;
                if (int.TryParse(idStr, out id)) {
                    if (unit.Recipes.GetRecipeById(id).Count == 0)
                        Indexes.Add(id);
                }
            }
        }

        public static void ParseRecipe(int index, string urn, MealType mealType, RecipesSource source) {

            var unit = new RestDBInterface();
            lock (Locker) {
                if (unit.Recipes.Get(index) != null) {
                    Indexes.Remove(index);
                    return;
                }
            }
            string page = string.Empty;
            try {
                page = new WebClient().DownloadString(urn + index.ToString());
            }
            catch {
                log.Error("Failed to load recipe number : " + index);
                return;
            }

            unit.Recipes.Add(new Recipe() {
                ID = index,
                Name = GetRecipeName(page),
                Ingredients = GetIngredients(page, source),
                Types = new HashSet<MealType>() { mealType },
                Servings = GetServings(page, source),
                PrepTime = GetPrepTime(page, source),
                ImageUrl = GetImageUrl(page, source)
            });

            lock (Locker) {
                log.Debug("Num " + index);
                Indexes.Remove(index);
            }
        }

        private static string GetImageUrl(string page, RecipesSource source) {
            if (source == RecipesSource.AllRecipes) {
                var part = page.Split(new string[] { "http://images.media-allrecipes.com/userphotos/250x250/" }, StringSplitOptions.None);
                var num = part[1].TakeWhile(c => c != '.');
                var strNum = new String(num.ToArray());
                return "http://images.media-allrecipes.com/userphotos/250x250/" + strNum + ".jpg";
            }
            else
                return "";
        }


        private static string GetRecipeName(string page) {
            var name  = page.Split(new string[2] { "<title>", "</title>" }, StringSplitOptions.None)[1];
            return name.Split(new string[1] { "Recipe" }, StringSplitOptions.None)[0];
        }

        private static List<string> GetIngredients(string page, RecipesSource source) {
            var ingredients = new List<string>();

            if (source == RecipesSource.AllRecipes) {
                var ingredientParts = page.Split(new string[1] { "itemprop=\"ingredients\">" }, StringSplitOptions.None);
                for (int i = 1; i < ingredientParts.Length; i++) {
                    if (ingredientParts[i].Contains("<")) {
                        var chars = ingredientParts[i].TakeWhile(a => a != '<');
                        var ingredient = new String(chars.ToArray());
                        var words = ingredient.Split(' ').ToList();
                        if (!ingredients.Contains(ingredient))
                            ingredients.Add(ingredient);
                    }
                }
            }
            else {
                var ingredientParts = page.Split(new string[1] { "<span class=\"ingredient__quantity\">" }, StringSplitOptions.None);
                for (int i = 1; i < ingredientParts.Length; i++) {
                    var ingredient = ingredientParts[i].Split( '\n' )[0];
                    ingredient = ingredient.Replace("</span>", "");
                    if (!ingredients.Contains(ingredient))
                        ingredients.Add(ingredient);
                }
            }

            return ingredients;
        }

        private static TimeSpan GetPrepTime(string page,RecipesSource source) {
            if (source == RecipesSource.AllRecipes) {
                var prepTimeParts = page.Split(new string[1] { "<span class=\"ready-in-time\">" }, StringSplitOptions.None);
                if (prepTimeParts.Length < 2) {
                    return TimeSpan.MaxValue;
                }
                var prepTimeStr = new String(prepTimeParts[1].TakeWhile(a => a != '<').ToArray());
                return ParsePrepTime(prepTimeStr);
            }
            else
                return new TimeSpan(0, 10, 0);
        }
        private static Dictionary<RecipesSource, string> ServingsSplitters = new Dictionary<RecipesSource, string> {
            {RecipesSource.Cookpad, "<div class=\"subtle\" data-field data-field-name=\"serving\" data-placeholder=\"How many servings?\" data-maxlength=\"15\">"},
            {RecipesSource.AllRecipes, "<meta id=\"metaRecipeServings\" itemprop=\"recipeYield\" content=" },
        };
        private static int GetServings(string page, RecipesSource source)
        {
            if (source == RecipesSource.AllRecipes) {
                var servingParts = page.Split(new string[1] { ServingsSplitters[source] }, StringSplitOptions.None);
                var servingStr = new String(servingParts[1].TakeWhile(a => a != '>').ToArray());
                return int.Parse(servingStr.Replace("\"", ""));
            }
            else
                return 1;
        }

        private static TimeSpan ParsePrepTime(string time) {
            var days = GetTimeUnit(ref time, 'd');
            var hours = GetTimeUnit(ref time, 'h');
            hours = hours + days * 24;
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

        private static List<int> ProblematicRecipes = new List<int>
        {
            235853,
            20096
        };
    }
}
