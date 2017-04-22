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
            Sources.RecipesURNs.ToList().ForEach(s => AddRecipesBySource(s.Key, unit, offline));
        }

        public static void AddRecipesBySource(RecipesSource source, RestDBInterface unit, bool offline) {
            Sources.RecipesURNs[source].Meals.ToList().ForEach(m => AddRecipesByMealType(source, m, unit, offline));
        }

        public static void AddRecipesByMealType(RecipesSource source, MealData mealData, RestDBInterface unit, bool offline, int loadBulkSize = 1000) {
            if (source == RecipesSource.AllRecipes && !offline)
                loadBulkSize = 10;
            var recipesLimit = mealData.MealsLimit;
            if (offline) {
                var files = Directory.GetFiles(Path.Combine(FolderPath, source.ToString(), mealData.Meal.ToString())).Take(recipesLimit).ToList();
                files.ForEach(f =>Indexes.Add(int.Parse(Path.GetFileNameWithoutExtension(f))));
            }
            else
                AddRecipesByURL(source, mealData, unit, recipesLimit);
                
            while (Indexes.Count > 10) {
                log.Debug("Loading bulk, tasks left : " + Indexes.Count());
                var loadMealsBulkSize = Indexes.Count > loadBulkSize ? loadBulkSize : Indexes.Count;
                var tasks = new List<Task>();
                Indexes.Take(loadMealsBulkSize).ToList().ForEach(a => tasks.Add(new Task(new Action(() => ParseRecipe(a, Sources.RecipesURNs[source].Url, mealData.Meal, source)))));
                tasks.ForEach(task => task.Start());
                tasks.ForEach(task => task.Wait());
            }
        }
      
        private static void AddRecipesByURL(RecipesSource source, MealData mealType,  RestDBInterface unit, int recipesLimit) {
            Indexes.Clear();
            log.Debug("Locating recipes in ->" +mealType.Url + " - started");
            var tasksCount = 8;
            int page = 0;
            shouldStop = false;
            while (Indexes.Count < recipesLimit && !shouldStop) {
                var tasks = new List<Task>();
                var beforeCount = Indexes.Count;
                for (int i = 0; i < tasksCount; i++) {
                    tasks.Add(new Task(new Action(() => ReadPage(source, mealType, unit, page++, new WebClient()))));
                
                }

                tasks.ForEach(task => task.Start());
                tasks.ForEach(task => task.Wait());
                if (beforeCount == Indexes.Count)
                    shouldStop = true;

            }
            
            log.Debug("Locating recipes in ->" + mealType.Url + " - completed. Indexes count : " + Indexes.Count);
        }
        private static bool shouldStop;
        private static void ReadPage(RecipesSource source, MealData mealType,  RestDBInterface unit, int currPage, WebClient client) {
            string pageStr = null;
            var readWorked = false;
            var urlSuffix = currPage == 0 ? "" : ("?"+ Sources.RecipesURNs[source].PageKeyword + "=" + currPage);
            var uri = mealType.Url +  urlSuffix;
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
                    if (retries >9)
                        shouldStop = true;
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
        static int serial = 0;
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
            try {
                unit.Recipes.Add(new Recipe() {
                    ID = serial++,
                    OriginalID = index,
                    Urn = Sources.RecipesURNs[source].Url.Split(new string[1] { "//" }, StringSplitOptions.None)[1],
                    Name = Sources.RecipesURNs[source].Parser.Parser.GetRecipeName(page),
                    Ingredients = Sources.RecipesURNs[source].Parser.GetIngredients(page),
                    Types = new HashSet<MealType>() { mealType },
                    Servings = Sources.RecipesURNs[source].Parser.Parser.GetServings(page),
                    StepsNum = Sources.RecipesURNs[source].Parser.Parser.GetStepsNum(page),
                    PrepTime = Sources.RecipesURNs[source].Parser.Parser.GetPrepTime(page),
                    ImageUrl = Sources.RecipesURNs[source].Parser.Parser.GetImageUrl(page)
                });
            }
            catch (Exception ex) {
             //   log.Error("Couldn'r properly parse recipe : " + ex.Message);
            }

            lock (Locker) {
                Indexes.Remove(index);
            }
        }
    }
}
