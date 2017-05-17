﻿using System;
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
    public class BBCParser : IRecipeParser {
        public static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string[] ServingSplitter => new string[1] { "<span class=\"recipe - details__text\" itemprop=\"recipeYield\">" };
        public string[] IngredientSplitter => new string[1] { "itemprop=\"ingredients\">" };
        public string[] ImageUrlSplitter => new string[1] { "https://www.bbcgoodfood.com/sites/default/files/recipe_images" };
        public string[] StepsSplitter => new string[1] { "itemprop=\"recipeInstructions\"" };

        private string GetTimeStrFromPagePart(string pagePart)
        {
            var prepParts = pagePart.Split('>');
            if (prepParts.Length < 2)
            {
                return "";
            }

            var innerPrepParts = prepParts[1].Split('<');
            if (innerPrepParts.Length < 2)
            {
                return "";
            }

            return innerPrepParts[0];
        }

        private static TimeSpan ParsePrepTime(string prepTimeStr, string cooktTimeStr)
        {
            return GeneralRecipeParser.ParsePrepTime(prepTimeStr) + GeneralRecipeParser.ParsePrepTime(cooktTimeStr);
        }

        public TimeSpan GetPrepTime(string page)
        {
            var prepTimeStr = "";
            var cookTimeStr = "";
            while (true)
            {
                var pageParts = page.Split(new string[1] { "cooking-time-prep\"> <strong>Prep:</strong> <span class=\"" }, StringSplitOptions.None);
                if (pageParts.Length < 2)
                {
                    break;
                }

                var pagePart = pageParts[1];

                // Preperation time
                prepTimeStr = GetTimeStrFromPagePart(pagePart);

                // Cook time
                var innerPageParts = pagePart.Split(new string[1] { "cooking-time-cook\"> <strong>Cook:</strong> <span class=\"" }, StringSplitOptions.None);
                if (innerPageParts.Length < 2)
                {
                    break;
                }

                cookTimeStr = GetTimeStrFromPagePart(innerPageParts[1]);

                break;
            }
            
            if (prepTimeStr == string.Empty && cookTimeStr == string.Empty)
            {
                return new TimeSpan(0);
            }
            if (prepTimeStr == string.Empty)
            {
                return GeneralRecipeParser.ParsePrepTime(cookTimeStr);
            }
            if (cookTimeStr == string.Empty)
            {
                return GeneralRecipeParser.ParsePrepTime(prepTimeStr);
            }

            // If we got here, both prep time and cook time are defined
            return ParsePrepTime(prepTimeStr, cookTimeStr);
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
            name = name.Split(new string[1] { "Recipe" }, StringSplitOptions.None)[0];
            return name;
        }

        public IngredientInfo ParseWeightAndName(string origItem) {
            var item = origItem;
            var itemsParts = item.Split('<');
            item = itemsParts[0] + itemsParts[1].Split('>')[1];
            
            var weight = 1.0;
            var innerpart = "";
            var weightKey = "";
            var words = item.Split(' ');
            words = words[0].Split('/');
            if (Regex.Replace(words[0], @"\d", "") == "ml") {
                weight = Double.Parse(words[0].Replace("ml", ""));
                innerpart = item.Replace(words[0], "").Trim();
            }
            else if (Regex.Replace(words[0], @"\d", "") == "g") {
                weight = Double.Parse(words[0].Replace("g", ""));
                innerpart = item.Replace(words[0], "").Trim();
            }
            else if (Map.HasWord(Formulas.MeasuresWeights.Keys.ToList(), item)) {
                var measure = Map.GetWord(Formulas.MeasuresWeights.Keys.ToList(), item);
                var parts = item.Split(new string[1] { measure }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 1)
                    parts = new string[2] { measure, parts[0] };
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
                    if (parts.Length == 1)
                        parts = new string[2] { size, parts[0] };
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
            if (item.Contains("garlic clove")) {
                weightKey = "clove";
                innerpart = "garlic";
            }
            if (innerpart == "")
                innerpart = item;
            innerpart = Map.AdjustNames(innerpart);
            innerpart = Map.AdjustInnerPart(innerpart);
            innerpart = Map.AdjustIngredient(innerpart);
          
      

            return new IngredientInfo { Name = innerpart, Quantity = weight, ReltiveSizeMeasure = weightKey };
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

