using log4net;
using Logic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace InitRecipes {
    class ParseRecipes {
        public static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string FolderPath = Assembly.GetExecutingAssembly().Location + @"\..\..\..\..\LocalDB\";
        public static object Locker = new object();
        public static HashSet<int> Indexes;

        public static void CreateDB(bool offline) {
            Indexes = new HashSet<int>();
            var unit = new RestDBInterface();
            unit.Recipes.Empty();
            Sources.MealTypesURNs.ToList().ForEach(s => AddRecipesBySource(s, unit, offline));
        }

        public static void AddRecipesBySource(KeyValuePair<RecipesSource, Dictionary<MealType, Tuple<string, string, int>>> source, RestDBInterface unit, bool offline) {
            Sources.MealTypesURNs[source.Key].ToList().ForEach(m => AddRecipesByMealType(source.Key, m.Key, unit, offline));
        }

        public static void AddRecipesByMealType(RecipesSource source, MealType mealType, RestDBInterface unit, bool offline, int loadBulkSize = 1000) {
            var recipesLimit = Sources.MealTypesURNs[source][mealType].Item3;
            if (offline) {
                var files = Directory.GetFiles(Path.Combine(FolderPath, source.ToString(), mealType.ToString())).Take(recipesLimit).ToList();
                files.ForEach(f =>Indexes.Add(int.Parse(Path.GetFileNameWithoutExtension(f))));
            }
            else
                AddRecipesByURL(source, mealType, unit, recipesLimit);
            ProblematicRecipes.ForEach(x => Indexes.Remove(x));          
                
            while (Indexes.Count > 1) {
                log.Debug("Loading bulk, tasks left : " + Indexes.Count());
                var loadMealsBulkSize = Indexes.Count > loadBulkSize ? loadBulkSize : Indexes.Count;
                var tasks = new List<Task>();
                Indexes.Take(loadMealsBulkSize).ToList().ForEach(a => tasks.Add(new Task(new Action(() => ParseRecipe(a, Sources.RecipesURNs[source], mealType, source)))));
                tasks.ForEach(task => task.Start());
                tasks.ForEach(task => task.Wait());
            }
        }
      
        private static void AddRecipesByURL(RecipesSource source, MealType mealType,  RestDBInterface unit, int recipesLimit) {
            Indexes.Clear();
           
            log.Debug("Locating recipes in ->" + Sources.MealTypesURNs[source][mealType].Item2 + " - started");

            var tasks = new List<Task>();
            for (int i = 0; i < recipesLimit/10; i++) {
                int curr = i;
                tasks.Add(new Task(new Action(()=> ReadPage(source, mealType, unit ,curr, new WebClient()))));
            }
            tasks.ForEach(task => task.Start());
            tasks.ForEach(task => task.Wait());
            Indexes = new HashSet<int>(Indexes.Take(recipesLimit));
            log.Debug("Locating recipes in ->" + Sources.MealTypesURNs[source][mealType].Item2 + " - completed. Indexes count : " + Indexes.Count);
        }

        private static void ReadPage(RecipesSource source, MealType mealType,  RestDBInterface unit, int currPage, WebClient client) {
            string pageStr = null;
            var readWorked = false;
            var urlSuffix = currPage == 0 ? "" : ("?"+ Sources.MealTypesURNs[source][mealType].Item1 + "=" + currPage);
            var uri = Sources.MealTypesURNs[source][mealType].Item2 + "/" + urlSuffix;
            for (int retries = 0; readWorked == false && retries < 10; retries++) {
                try {
                    pageStr = client.DownloadString(uri);
                    readWorked = true;
                    if (source == RecipesSource.Food)
                        AddRecipesFromJson( pageStr, unit);
                    else
                        AddRecipesFromPage(pageStr, unit);
                }
                catch (Exception) {
                    log.Error(string.Format("Failed to load page num {0}, might be the last page", currPage));
                    ++retries;
                }
            }
            log.Debug("Page num :" + currPage);
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

        public static void AddRecipesFromJson(string pageStr, RestDBInterface unit) {

            var parts = pageStr.Split(new string[1] { "var searchResults = " }, StringSplitOptions.None);
            parts = parts[1].Split(new string[1] {";\r\n"}, StringSplitOptions.None);
            var json = parts[0];
            var res = JsonConvert.DeserializeObject<dynamic>(json);
            JArray results = res.response.results;
            results.ToList().ForEach(r => {
                if (r["record_type"].ToString() == "Recipe") {
                    var id = int.Parse(r["recipe_id"].ToString());
                    if (unit.Recipes.GetRecipeById(id).Count == 0)
                        Indexes.Add(id);
                }

            }
            );
        }
        public static void ParseRecipe(int index, string urn, MealType mealType, RecipesSource source) {
            var customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone(); customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = customCulture;
            var unit = new RestDBInterface();
            lock (Locker) {
                if (unit.Recipes.Get(index) != null) {
                    Indexes.Remove(index);
                    return;
                }
            }
            var page = string.Empty;
            try {
                Directory.CreateDirectory(Path.Combine(FolderPath, source.ToString()));
                Directory.CreateDirectory(Path.Combine(FolderPath, source.ToString(), mealType.ToString()));
                var filePath = Path.Combine(FolderPath, source.ToString(), mealType.ToString(),index.ToString() + ".txt");
                if (File.Exists(filePath))
                    page = File.ReadAllText(filePath);
                else {
                    page = new WebClient().DownloadString(urn + index.ToString());
                    File.WriteAllText(filePath, page);
                }
                
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
            name =  name.Split(new string[1] { "Recipe" }, StringSplitOptions.None)[0];
            return name;
        }

        private static List<Tuple<string, double, string>> GetIngredients(string page, RecipesSource source) {
            var ingredients = new List<Tuple<string, double, string>>();

            if (source == RecipesSource.AllRecipes) {
                var ingredientParts = page.Split(new string[1] { "itemprop=\"ingredients\">" }, StringSplitOptions.None);
                for (int i = 1; i < ingredientParts.Length; i++) {
                    if (ingredientParts[i].Contains("<")) {
                        var chars = ingredientParts[i].TakeWhile(a => a != '<');
                        var ingredient = new String(chars.ToArray());

                        ingredients.Add(WeightAndNameParsers.ParseWeightAndNameAllRecipes(ingredient));
                    }
                }
            }
            else if (source == RecipesSource.Cookpad) {
                var ingredientParts = page.Split(new string[1] { "<span class=\"ingredient__quantity\">" }, StringSplitOptions.None);
                for (int i = 1; i < ingredientParts.Length; i++) {

                    var ingredient = ingredientParts[i].Split('\n')[0];
                    var res = WeightAndNameParsers.ParseWeightAndNameCookpad(ingredient);
                    if (res != null)
                        ingredients.Add(res);

                }
            }
            else if (source == RecipesSource.Food) {
                var ingredientParts = page.Split(new string[1] { "<li data-ingredient=" }, StringSplitOptions.None);
                for (int i = 1; i < ingredientParts.Length; i++) {
                    var parts = ingredientParts[i].Split(new string[4] { "<span>", "</span>" ,"</a>", "</li>"}, StringSplitOptions.None);
                    var ingredient = parts[0].Split('"')[1].Replace('+',' ');
                    var weight = 0.0;
                    try {
                        weight = double.Parse(parts[1]);
                    }
                    catch (FormatException ex) {
                        log.Error(ex.Message + parts[1]);
                    }
                    

                    var rest = parts[parts.Length-2];
                    var relativeWeight = Formulas.RelativeSizes.FirstOrDefault(s => Map.WordCheck(s, rest));
                    if (relativeWeight == null)
                        relativeWeight = Formulas.RelativeProductSize.FirstOrDefault(s => Map.WordCheck(s, rest));
                    else
                        relativeWeight = ingredient;
                    ingredients.Add(new Tuple<string, double, string>(ingredient, weight, rest));
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
