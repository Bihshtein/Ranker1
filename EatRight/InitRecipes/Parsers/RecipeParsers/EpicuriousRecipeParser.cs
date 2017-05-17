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
using RestModel;

namespace InitRecipes {
    public class EpicuriousParser : IRecipeParser {
        public static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string[] ServingSplitter => new string[1] { "<span class=\"recipe - details__text\" itemprop=\"recipeYield\">" };
        public string[] IngredientSplitter => new string[1] { "itemprop=\"ingredients\">" };
        public string[] ImageUrlSplitter => new string[1] { "http://assets.epicurious.com/photos/" };
        public string[] StepsSplitter => new string[1] { "li class=\"preparation-step\"" };

        public TimeSpan GetPrepTime(string page)
        {
            var res = new TimeSpan(0);

            var pageParts = page.Split(new string[1] { "<dt class=\"total-time\">Total Time</dt><dd class=\"total-time\">" }, StringSplitOptions.None);
            if (pageParts.Length < 2)
            {
                return res;
            }

            var innerParts = pageParts[1].Split('<');
            if (innerParts.Length < 2)
            {
                return res;
            }

            var timeStr = innerParts[0];
            var timeStrParts = timeStr.Split();
            var remainingParts = timeStrParts.Length;
            var start = 0;

            while (remainingParts > 1)
            {
                var curTimeStr = timeStrParts[start] + " " + timeStrParts[start + 1];
                res += GeneralRecipeParser.ParsePrepTime(curTimeStr);
                start += 2;
                remainingParts -= 2;
            }

            return res;
        }

        public int GetServings(string page) {
            var servingParts = page.Split(ServingSplitter, StringSplitOptions.None);
            var servingStr = new String(servingParts[1].TakeWhile(a => a != '<').ToArray());
            var parts = servingStr.Split(' ').ToList();
            var num = "";
            if (parts.Contains("Serves"))
                num = parts[parts.IndexOf("Serves") + 1];
            else num = "1";
            return int.Parse(num);
        }

        public int GetStepsNum(string page)
        {
            // Implemented in GeneralRecipeParser
            return 1;
        }

        public string GetImageUrl(string page)
        {
            // Implemented in GeneralRecipeParser
            return "";
        }

        public string GetRecipeName(string page) {
            var name = page.Split(new string[2] { "<title>", "</title>" }, StringSplitOptions.None)[1];
            name = name.Split(new string[1] { " | Epicurious.com" }, StringSplitOptions.None)[0];
            return name;
        }

        public IngredientInfo ParseWeightAndName(string phrase) {
            var itemsParts = phrase.Split('<');
            var origitem = itemsParts[0];
            var item = origitem;
            if (item == string.Empty) {
                return null;
            }
            var branchParts = item.Split('(', ')');
            if (branchParts.Length == 3)
                item = branchParts[0] + branchParts[2];
            if (branchParts.Length == 5)
                item = branchParts[0] + branchParts[2]+ branchParts[4];
            var iParts = item.Split('/');
            if (Map.HasWord(Formulas.MeasuresWeights.Keys.ToList(), iParts[0]) ||
                Map.HasWord(Formulas.RelativeSizes, iParts[0]))
                item = iParts[0];
            else
                item = iParts[iParts.Length-1];
            var weight = 1.0;
            var name = item;
            var weightKey = "";
            if (Map.HasWord(Formulas.MeasuresWeights.Keys.ToList(), item)) {
                var measure = Map.GetWord(Formulas.MeasuresWeights.Keys.ToList(), item);
                var parts = item.Split(new string[1] { measure }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 1) {
                    var res = ParseByAbsoluteMeasures(parts, item, measure);
                    name = res.Item1;
                    weight = res.Item2;
                }
            }
            else {
                if (Map.HasWord(Formulas.RelativeSizes, item)) {
                    var size = Map.GetWord(Formulas.RelativeSizes, item);
                    var parts = item.Split(new string[1] { size }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length > 1) {
                        var res = ParseByRelativeMeasures(parts, item, size);
                        name = res.Item1;
                        weight = res.Item2;
                        weightKey = res.Item3;
                    }
                }
                else {
                    
                    var res = ParseByRelativeNumber(item);
                    if (res.Item1 != "") {
                        name = res.Item1;                     
                        weightKey = res.Item1; // the product is the actual key
                    }
                    if (res.Item2 != 0) 
                        weight = res.Item2;
                }
            }
            if (name.Contains("garlic") && name.Contains("clove")) {
                weightKey = "clove";
                name = "garlic";
            }

            try {
                name = Map.AdjustNames(name);
                name = Map.AdjustInnerPart(name);
                name = Map.AdjustIngredient(name);
            }
            catch(Exception ex) {
                log.Error(ex);
            }
            return new IngredientInfo { Name = name, Quantity = weight, ReltiveSizeMeasure = weightKey };
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

      


    }
}

