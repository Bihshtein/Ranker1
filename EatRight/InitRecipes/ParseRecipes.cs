using log4net;
using Logic;
using RestModel;
using System;
using System.Collections.Generic;
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
        public enum RecipesSource { AllRecipes, Cookpad }
        public static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string FolderPath = Assembly.GetExecutingAssembly().Location + @"\..\..\..\..\LocalDB\";

        public static Dictionary<RecipesSource, string> RecipesURLs = new Dictionary<RecipesSource, string>() {
           // {RecipesSource.Cookpad,  "https://cookpad.com/us/" },
            {RecipesSource.AllRecipes,  "http://allrecipes.com/recipes/" },
         
        };

        public static Dictionary<RecipesSource, string> RecipesURNs = new Dictionary<RecipesSource, string>() {
            {RecipesSource.Cookpad,  "https://cookpad.com/us/recipes/" },
            {RecipesSource.AllRecipes,  "http://allrecipes.com/recipe/" },
        };

        private static Dictionary<RecipesSource, Dictionary<MealType, string>> MealTypesURNs = new Dictionary<RecipesSource, Dictionary<MealType, string>>() {
            {RecipesSource.AllRecipes, new Dictionary<MealType, string>() {
                {MealType.Dinner,  "17562/dinner" },
                {MealType.Breakfast,  "78/breakfast-and-brunch" } }
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
            if (source == RecipesSource.AllRecipes) {
                loadMealsBulkSize = 30;
            }
                while (Indexes.Count > 0) {               
                List<Task> tasks = new List<Task>();
                if (Indexes.Count > loadMealsBulkSize)
                    Indexes.Take(loadMealsBulkSize).ToList().ForEach(a => tasks.Add(new Task(new Action(() => ParseRecipe(a, RecipesURNs[source], mealType, source)))));
                else
                    Indexes.Take(Indexes.Count).ToList().ForEach(a => tasks.Add(new Task(new Action(() => ParseRecipe(a, RecipesURNs[source], mealType, source)))));
                tasks.ForEach(task => task.Start());
                tasks.ForEach(task => task.Wait());
            }
        }

        private static void AddRecipesByURL(string categoryURN, RestDBInterface unit, int pagesLimit = 5) {
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
                    break;
                }
                log.Debug("Page num :" + currPage);
            }
            log.Debug("Locating recipes in ->" + categoryURN + " - completed. Indexes count : " + Indexes.Count);
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
                //log.Debug("Num " + index);
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

        private static List<Tuple<string, double, string>> GetIngredients(string page, RecipesSource source) {
            var ingredients = new List<Tuple<string, double, string>>();

            if (source == RecipesSource.AllRecipes) {
                var ingredientParts = page.Split(new string[1] { "itemprop=\"ingredients\">" }, StringSplitOptions.None);
                for (int i = 1; i < ingredientParts.Length; i++) {
                    if (ingredientParts[i].Contains("<")) {
                        var chars = ingredientParts[i].TakeWhile(a => a != '<');
                        var ingredient = new String(chars.ToArray());
                        var words = ingredient.Split(' ').ToList();
                        ingredient = Map.AdjustNames(ingredient);
                        ingredient = Map.AdjustInnerPart(ingredient);
                        ingredients.Add(ParseWeightAndName(ingredient));
                    }
                }
            }
            else {
                var ingredientParts = page.Split(new string[1] { "<span class=\"ingredient__quantity\">" }, StringSplitOptions.None);
                for (int i = 1; i < ingredientParts.Length; i++) {
                    var ingredient = ingredientParts[i].Split('\n')[0];
                    var nameAndWeight = ingredient.Split(new string[1] { "</span>" }, StringSplitOptions.None);
                    var name = nameAndWeight[1];
                    var weight = nameAndWeight[0];
                    var weightSplit = weight.Split('-');
                    if (weightSplit.Length==2)
                    weight = weightSplit[1]; // take the bigger number from range
                    var weightNum = 0.0;
                    var relativeWeight = "";
                    var splitBySpace = weight.Split(' ');
                    if (splitBySpace.Length > 1 && splitBySpace[1] == "c") {
                        weight = weight.Replace("c", "cup");
                    }
                    if (splitBySpace.Length > 1 && splitBySpace[1] == "c.") {
                        weight = weight.Replace("c.", "cup");
                    }
                    if (splitBySpace.Length > 1 && splitBySpace[1] == "C") {
                        weight = weight.Replace("C", "cup");
                    }
                    if (weight.Contains("oz.")) {
                        weight = weight.Replace("oz.", "oz");
                    }
                   
                    if (splitBySpace.Length > 1 && splitBySpace[1] == "t") {
                        weight = weight.Replace("t", "teaspoon");
                    }
                    if (splitBySpace.Length > 1 && splitBySpace[1] == "T") {
                        weight = weight.Replace("T", "tablespoon");
                    }
                    if (Formulas.MeasuresWeights.Keys.Any(w => weight.Contains(w))) {
                        var keyword = Formulas.MeasuresWeights.Keys.First(w => weight.Contains(w));
                        try {
                            weightNum = ParseHelpers.ParseAmount(weight.Replace(keyword, "")) * Formulas.MeasuresWeights[keyword];
                        }
                        catch {
                            log.Error("Can't parse weight : " + weight + ", ingredient name:" + name);
                        }
                    }
                    else if (Formulas.RelativeSizes.Any(s => weight.Contains(s))) {
                        relativeWeight = Formulas.RelativeSizes.First(s => weight.Contains(s));
                        try {
                            var amount = weight.Replace(relativeWeight, "");
                            if (amount == "")
                                weightNum = 1;
                            else
                                weightNum = ParseHelpers.ParseAmount(weight.Replace(relativeWeight, ""));
                        }
                        catch {
                            log.Error("Can't parse weight : " + weight + ", ingredient name:" + name);
                        }
                    }
                   

                    else {
                        if (splitBySpace.Length > 1 && (splitBySpace[1] == "g" || splitBySpace[1] == "ml")) {
                            weightNum = double.Parse(splitBySpace[0]);
                        }
                        else if (weight == "") {
                            weightNum = 1;
                        }
                        else {
                           
                                relativeWeight = name;
                            try {
                                weightNum = ParseHelpers.ParseAmount(weight);
                            }
                            catch {
                                log.Error("Can't parse weight : " + weight + ", ingredient name:" + name);
                            }
                        }
                        
                    }
                    ingredients.Add(new Tuple<string, double, string>(name, weightNum, relativeWeight));
                    
                }
            }

            return ingredients;
        }

        public static Tuple<string, double, string> ParseWeightAndName(string item) {
            var weight = 0.0;
            var innerpart = "";
            var weightKey = "";
            var measure = Formulas.MeasuresWeights.Keys.FirstOrDefault(k => item.Contains(k));
            if (measure != null) {
                var parts = item.Split(new string[1] { measure }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 1) {
                    var res = ParseByAbsoluteMeasures(parts, item);
                    innerpart = res.Item1;
                    weight = res.Item2;
                }
            }
            else {
                var size = Formulas.RelativeSizes.FirstOrDefault(k => item.Contains(k));
                if (size != null) {
                    var parts = item.Split(new string[1] { size }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length > 1) {
                        var res = ParseByRelativeMeasures(parts, item);
                        innerpart = res.Item1;
                        weight = res.Item2;
                        weightKey = res.Item3;
                    }
                }
                else {
                    var res = ParseByRelativeNumber(item);
                    innerpart = res.Item1;
                    weight = res.Item2;
                    weightKey = res.Item1; // the product is the actual key
                }
            }
            return new Tuple<string, double, string>(innerpart, weight, weightKey);
        }

        public static Tuple<string, double, string> ParseByRelativeMeasures(string[] parts, string item) {

            var relativeWeight = 0.0;
            var innerpart = string.Empty;
            var unit = item.Replace(parts[0], "").Replace(parts[1], "");
            if (Formulas.RelativeSizes.Contains(unit)) {
                try {
                    relativeWeight = ParseHelpers.ParseAmount(parts[0]);
                }
                catch (Exception ex) {
                    log.Error("Failed to parse relative weight for item : " + item, ex);
                }
                innerpart = parts[1];
            }
            else
                log.Error("Failed to parse relative weight for item : " + item);
            return new Tuple<string, double, string>(innerpart, relativeWeight, unit);
        }
        public static Tuple<string, double> ParseByRelativeNumber(string item) {
            var relativeWeight = 0.0;
            var innerpart = string.Empty;
            if (item != Regex.Replace(item, @"\d", "")) {
                innerpart = Regex.Replace(item, @"\d", "").Trim();
                if (innerpart != string.Empty) {
                    var units = item.Replace(innerpart, "");
                    try {
                        relativeWeight = int.Parse(units.Trim());
                    }
                    catch (Exception ex) {
                        innerpart = string.Empty;
                        log.Error("Failed to parse relative weight for item : " + item, ex);
                    }
                }
            }
            return new Tuple<string, double>(innerpart, relativeWeight);
        }
        public static Tuple<string, double> ParseByAbsoluteMeasures(string[] parts, string item) {
            var actualWeight = 0.0;
            var innerpart = string.Empty;
            var unit = item.Replace(parts[0], "").Replace(parts[1], "");

            if (Formulas.MeasuresWeights.ContainsKey(unit)) {
                try {
                    actualWeight = ParseHelpers.ParseAmount(parts[0]) * Formulas.MeasuresWeights[unit];
                }
                catch (Exception ex) {
                    log.Error("Failed to parse actual weight for item : " + item, ex);
                }
                innerpart = parts[1];

            }
            else
                log.Error("Failed to parse actual weight for item : " + item);
            return new Tuple<string, double>(innerpart, actualWeight);
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
