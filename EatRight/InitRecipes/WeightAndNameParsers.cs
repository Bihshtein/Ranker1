using log4net;
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InitRecipes {
    class WeightAndNameParsers {
        public static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static Tuple<string, double, string> ParseWeightAndNameCookpad(string ingredient) {

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
            var weightNum = 0.0;
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
                    weightNum = 1;
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

            return new Tuple<string, double, string>(name, weightNum, relativeWeight);

        }
        public static Tuple<string, double, string> ParseWeightAndNameAllRecipes(string item) {

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
    }
}
