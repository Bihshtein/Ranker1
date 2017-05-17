using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;
using System.Net;
using RestModel;
using log4net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace InitRecipes {
    public class FoodParser : IRecipeParser {
        public static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public string[] ServingSplitter =>new string[1] { "Servings Per Recipe:" }; 
        public string[] StepsSplitter => new string[0];
        public string[] IngredientSplitter=> new string[1] { "<li data-ingredient=" };
        public string[] ImageUrlSplitter => new string[0];

        public string[] FoodStepsSplitter => new string[1] { "\"recipeInstructions\"" }; // Not to be confused with StepsSplitter which is for the general implementation

        public IngredientInfo ParseWeightAndName(string ingredient) {
            var parts = ingredient.Split(new string[3] { "<span>", "</span>", "<a" }, StringSplitOptions.None);
            var name = parts[0].Split('"')[1].Replace('+', ' ');
            var weightStr = parts[1].Split('-')[0];
            var rest = parts[2].Trim();
          
                var weight = 1.0;
                try {
                    weight = double.Parse(weightStr);
                }
                catch (FormatException) {
                    var weightParts = weightStr.Split(new string[4] { "<sup>", "</sup>", "<sub>", "</sub", }, StringSplitOptions.None);
                    var amount = "";
                    weightParts.ToList().ForEach(p => amount += WebUtility.HtmlDecode(p));
                    weight = ParseHelpers.ParseAmount(amount);

                }
                var relativeWeight = Formulas.MeasuresWeights.Keys.ToList().FirstOrDefault(s => Map.WordCheck(s, rest));
                
                if (relativeWeight == null) {
                    relativeWeight = Formulas.RelativeSizes.FirstOrDefault(s => Map.WordCheck(s, rest));
                    if (relativeWeight == null) {
                        relativeWeight = "";
                    }
                }
            if (name.Contains("garlic clove")) {
                relativeWeight = "clove";
                name = "garlic";
            }
            try { 
            name = Map.AdjustNames(name);
            name = Map.AdjustInnerPart(name);
            name = Map.AdjustIngredient(name);
            }
            catch (Exception ex) {
                log.Error(ex);
            }
            return new IngredientInfo { Name = name, Quantity = weight, ReltiveSizeMeasure = relativeWeight }; 
        }

        public TimeSpan GetPrepTime(string page)
        {
            var res = new TimeSpan(0);

            var pageParts = page.Split(new string[1] { "<span class=\"ready-in-text\"><strong>READY IN:</strong></span>" }, StringSplitOptions.None);
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
            res += GeneralRecipeParser.ParsePrepTime(timeStr);

            return res;
        }
       

        public int GetServings(string page) {
            var servingParts = page.Split(ServingSplitter, StringSplitOptions.None);
            var servingStr = new String(servingParts[1].TakeWhile(a => a != '<').ToArray());
            return int.Parse(servingStr);
        }

        public int GetStepsNum(string page)
        {
            var tempParts = page.Split(FoodStepsSplitter, StringSplitOptions.None);
            if (tempParts == null || tempParts.Length < 2)
            {
                return 0;
            }

            var partStr = tempParts[1];
            tempParts = partStr.Split(new string[1] { "\",\"" }, StringSplitOptions.None);
            var stepsStr = partStr;
            if (tempParts != null && tempParts.Length > 1)
            {
                stepsStr = tempParts[0];
            }

            int myInt = System.Text.RegularExpressions.Regex.Matches(stepsStr, "\\. [A-Z]").Count;
            /* We have the first step, and every new step is represented by a dot, a space and then
            a capital letter. */
            return 1 + myInt;
        }

        public  string GetImageUrl(string page) {
            var part = page.Split(new string[1] { "<link rel=\"image_src\" href=\"" }, StringSplitOptions.None);
            if (part.Length > 1) {
                var uri = part[1].TakeWhile(c => c != '>');
                var strUri = new String(uri.ToArray());

                return strUri.Remove(strUri.Length - 3, 3);
            }
            else
                throw new Exception("Couldn't load image");
        }

        public string GetRecipeName(string page) {
            var name = page.Split(new string[2] { "<title>", "</title>" }, StringSplitOptions.None)[1];
            name = name.Split(new string[1] { "Recipe" }, StringSplitOptions.None)[0];
            return name;
        }

        public string GetIngredient(string phrase) {
            throw new NotImplementedException();
        }
    }
}
