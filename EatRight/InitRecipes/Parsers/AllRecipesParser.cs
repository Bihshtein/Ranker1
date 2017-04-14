using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Logic;
using System.Net;
using System.Text.RegularExpressions;

namespace InitRecipes {
    public class AllRecipesParser : IRecipeParser {
        public static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string[] ServingSplitter => new string[1] { "<meta id=\"metaRecipeServings\" itemprop=\"recipeYield\" content=" };
        public string[] IngredientSplitter => new string[1] { "itemprop=\"ingredients\">" };

        public TimeSpan GetPrepTime(string page) {
            var prepTimeParts = page.Split(new string[1] { "<span class=\"ready-in-time\">" }, StringSplitOptions.None);
            if (prepTimeParts.Length < 2) {
                return TimeSpan.MaxValue;
            }
            var prepTimeStr = new String(prepTimeParts[1].TakeWhile(a => a != '<').ToArray());
            return ParsePrepTime(prepTimeStr);
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
            var servingStr = new String(servingParts[1].TakeWhile(a => a != '>').ToArray());
            return int.Parse(servingStr.Replace("\"", ""));
        }

        public int GetStepsNum(string page)
        {
            int myCount = System.Text.RegularExpressions.Regex.Matches(page, "<li class=\"step\" ng-class=\"{").Count;
            return myCount;
        }

        public string GetImageUrl(string page) {
            var part = page.Split(new string[] { "http://images.media-allrecipes.com/userphotos/250x250/" }, StringSplitOptions.None);
            if (part.Length > 1) {
                var num = part[1].TakeWhile(c => c != '.');
                var strNum = new String(num.ToArray());
                return "http://images.media-allrecipes.com/userphotos/250x250/" + strNum + ".jpg";
            }
            else
                throw new Exception("Couldn't load image");
        }

        public string GetRecipeName(string page) {
            var name = page.Split(new string[2] { "<title>", "</title>" }, StringSplitOptions.None)[1];
            name = name.Split(new string[1] { "Recipe" }, StringSplitOptions.None)[0];
            return name;
        }

        private Tuple<string, double, string> ParseWeightAndName(string item) {
            if (item.Contains("<")) {
                var chars = item.TakeWhile(a => a != '<');
                item = new String(chars.ToArray());
            }
            else
                return null;
            item = Map.AdjustNames(item);
            item = Map.AdjustInnerPart(item);
            var weight = 0.0;
            var innerpart = "";
            var weightKey = "";
            if (Map.HasWord(Formulas.MeasuresWeights.Keys.ToList(), item)) {
                var measure = Map.GetWord(Formulas.MeasuresWeights.Keys.ToList(), item);
                var parts = item.Split(new string[1] { measure }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 1) {
                    var res = ParseByAbsoluteMeasures(parts, item, measure);
                    innerpart = res.Item1;
                    weight = res.Item2;
                }
            }
            else {
                if (Map.HasWord(Formulas.RelativeSizes, item)) {
                    var size = Map.GetWord(Formulas.RelativeSizes, item);
                    var parts = item.Split(new string[1] { size }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length > 1) {
                        var res = ParseByRelativeMeasures(parts, item, size);
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
            innerpart = Map.AdjustIngredient(innerpart);
            return new Tuple<string, double, string>(innerpart, weight, weightKey);
        }

        public static Tuple<string, double, string> ParseByRelativeMeasures(string[] parts, string item, string unit) {

            var relativeWeight = 0.0;
            var innerpart = string.Empty;
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
                innerpart = innerpart.Replace("/", "").Trim();
                if (innerpart != string.Empty) {
                    var units = item.Replace(innerpart, "");
                    try {
                        relativeWeight = ParseHelpers.ParseAmount(units.Trim());
                    }
                    catch (Exception ex) {
                        innerpart = string.Empty;
                        log.Error("Failed to parse relative weight for item : " + item, ex);
                    }
                }
            }
            return new Tuple<string, double>(innerpart, relativeWeight);
        }
        public static Tuple<string, double> ParseByAbsoluteMeasures(string[] parts, string item, string unit) {
            var actualWeight = 0.0;
            var innerpart = string.Empty;

            if (Map.HasWord(Formulas.MeasuresWeights.Keys.ToList(), unit)) {
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

        Tuple<string, double, string> IRecipeParser.ParseWeightAndName(string ingredient) {
            throw new NotImplementedException();
        }
    }
}

