using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;
using log4net;
using System.Reflection;
using System.Net;
using RestModel;

namespace InitRecipes {
    public class CookpadParser : IRecipeParser {
        public string[] ServingSplitter => new string[1] { "<div class=\"subtle\" data-field data-field-name=\"serving\" data-placeholder=\"How many servings?\" data-maxlength=\"15\">" }; 
        public string[] IngredientSplitter => new string[1] { "<span class=\"ingredient__quantity\">" }; 

        public static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public TimeSpan GetPrepTime(string page) {
            return new TimeSpan(0, 10, 0);
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

        public int GetServings(string page) {
            var servingParts = page.Split(ServingSplitter, StringSplitOptions.None);
            var servingStr = new String(servingParts[1].TakeWhile(a => a != '<').ToArray());
            servingStr = servingStr.Replace("\n", "");
            servingStr = servingStr.Replace("servings", "").Trim();
            servingStr = servingStr.Replace("serving", "").Trim();
            return int.Parse(servingStr);

        }

        public int GetStepsNum(string page)
        {
            return System.Text.RegularExpressions.Regex.Matches(page, "p class=\"step__text\"").Count;
        }

        public string GetImageUrl(string page) {
            var part = page.Split(new string[2] { "https://img-global.cpcdn.com/001_recipes/", "/400x400cq70/photo.jpg" }, StringSplitOptions.None);
            if (part.Length > 1) {
                var num = part[2].TakeWhile(c => c != '/');
                var strNum = new String(num.ToArray());
                return "https://img-global.cpcdn.com/001_recipes/" + strNum + "/400x400cq70/photo.jpg";
            }
            else
                throw new Exception("Couldn't load image");
        }

        public string GetRecipeName(string page) {
            var name = page.Split(new string[2] { "<title>", "</title>" }, StringSplitOptions.None)[1];
            name = name.Split(new string[1] { "Recipe" }, StringSplitOptions.None)[0];
            return name;
        }

        public IngredientInfo ParseWeightAndName(string ingredient) {
            ingredient = ingredient.Split('\n')[0];
            var nameAndWeight = ingredient.Split(new string[1] { "</span>" }, StringSplitOptions.None);
            var name = nameAndWeight[1].Trim().ToLower();
            if (name == string.Empty || Map.ShouldSkip(name))
                return null;
            name = Map.AdjustNames(name);
            name = Map.AdjustInnerPart(name).Trim();
            name = Map.AdjustIngredient(name);
            name = ParseHelpers.FixIllegalCharacters(name);

            var weight = nameAndWeight[0].Replace(".", "");

            var weightSplit = weight.Split('-');
            if (weightSplit.Length == 2)
                weight = weightSplit[1]; // take the bigger number from range
            var weightNum = 1.0;
            var relativeWeight = "";
            weight = ParseHelpers.GetWeightFullName(weight);
            if (Map.HasWord(Formulas.MeasuresWeights.Keys.ToList(), weight)) {
                var keyword = Map.GetWord(Formulas.MeasuresWeights.Keys.ToList(), weight);
                try {
                    weightNum = ParseHelpers.ParseAmount(weight.Replace(keyword, "")) * Formulas.MeasuresWeights[keyword];
                }
                catch {
                    log.Error("Can't parse weight : " + weight + ", ingredient name:" + name);
                }
            }
            else if (Map.HasWord(Formulas.RelativeSizes, weight)) {
                relativeWeight = Map.GetWord(Formulas.RelativeSizes, weight);
                try {
                    var amount = weight.Replace(relativeWeight, "");
                    if (amount != "")
                        weightNum = ParseHelpers.ParseAmount(weight.Replace(relativeWeight, ""));
                }
                catch {
                    log.Error("Can't parse weight : " + weight + ", ingredient name:" + name);
                }
            }


            else {
                var splitBySpace = weight.Split(' ');
                if (splitBySpace.Length > 1 && (splitBySpace[1] == "g" || splitBySpace[1] == "ml")) {
                    weightNum = ParseHelpers.ParseAmount(splitBySpace[0]);
                }

                else if (Formulas.RelativeProductSize.Any(s => name.Contains(s))) {
                    relativeWeight = Formulas.RelativeProductSize.First(s => name.Contains(s));
                    name = name.Replace(relativeWeight, string.Empty).Trim();
                    if (name != string.Empty) {
                        name = Map.AdjustNames(name);
                        name = Map.AdjustInnerPart(name).Trim();
                        name = Map.AdjustIngredient(name);
                    }
                }
                else if (weight == "") {
                    relativeWeight = name;
                }

                else {
                    try {
                        weightNum = ParseHelpers.ParseAmount(weight);
                    }
                    catch {
                        log.Error("Can't parse weight : " + weight + ", ingredient name:" + name);
                    }
                }

            }
            if (name.Contains("garlic clove")) {
                relativeWeight = "clove";
                name = "garlic";
            }

            return new IngredientInfo { Name = name, Quantity = weightNum, ReltiveSizeMeasure = relativeWeight }; 

        }

    }
}
