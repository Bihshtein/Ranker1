﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;
using log4net;
using System.Reflection;
using System.Net;
using RestModel;
using System.Text.RegularExpressions;

namespace InitRecipes {
    public class CookpadParser : IRecipeParser {
        public string[] ServingSplitter => new string[1] { "<div class=\"subtle\" data-field data-field-name=\"serving\" data-placeholder=\"How many servings?\" data-maxlength=\"15\">" }; 
        public string[] IngredientSplitter => new string[1] { "<span class=\"ingredient__quantity\">" };
        public string[] ImageUrlSplitter => new string[0];
        public string[] StepsSplitter => new string[1] { "p class=\"step__text\"" };

        public static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public TimeSpan GetPrepTime(string page)
        {
            var res = new TimeSpan(0);

            var pageParts = page.Split(new string[1] { "data-field-name=\"cooking_time\"" }, StringSplitOptions.None);
            if (pageParts.Length < 2)
            {
                return res;
            }

            var innerParts = pageParts[1].Split('>');
            if (innerParts.Length < 2)
            {
                return res;
            }

            var timeParts = innerParts[1].Split('<');
            if (innerParts.Length < 2)
            {
                return res;
            }

            var timeStr = timeParts[0];

            // Adjust string
            // 1. Remove new line chars
            timeStr = Regex.Replace(timeStr, @"\t|\n|\r", "");
            // 2. Convert to lowercase
            timeStr = timeStr.ToLower();
            // 4. Check if we have the '-' char (as in 20-40 minutes)
            if (timeStr.Contains('-'))
            {
                // Take the long option
                timeStr = timeStr.Split('-')[1];
            }

            var timeStrParts = timeStr.Split();

            var start = 0;

            while (start < (timeStrParts.Length - 1))
            {
                // Validate that the first token is numeric
                int tempNum;
                bool isNumeric = int.TryParse(timeStrParts[start], out tempNum);
                if (!isNumeric)
                {
                    start++;
                    continue;
                }

                var curTimeStr = timeStrParts[start] + " " + timeStrParts[start + 1];
                res += GeneralRecipeParser.ParsePrepTime(curTimeStr);
                break;
            }

            return res;
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
            // Implemented in GeneralRecipeParser
            return 1;
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
            if (name.Contains("garlic") && name.Contains("clove")) {
                relativeWeight = "clove";
                name = "garlic";
            }
            try { 
            name = Map.AdjustNames(name);
            name = Map.AdjustInnerPart(name).Trim();
            name = Map.AdjustIngredient(name);
            name = ParseHelpers.FixIllegalCharacters(name);
        }
            catch(Exception ex) {
                log.Error(ex);
            }
              
            return new IngredientInfo { Name = name, Quantity = weightNum, ReltiveSizeMeasure = relativeWeight }; 

        }

    }
}
