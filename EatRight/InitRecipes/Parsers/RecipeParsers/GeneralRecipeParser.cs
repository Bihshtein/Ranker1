using Logic;
using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitRecipes {
    public class GeneralRecipeParser {
        public IRecipeParser Parser;
        public GeneralRecipeParser(IRecipeParser parser) {
            Parser = parser;
        }

        public string GetImageUrl(string page)
        {
            try
            {
                if (Parser.ImageUrlSplitter.Length > 0)
                {
                    var parts = page.Split(Parser.ImageUrlSplitter, StringSplitOptions.None);
                    if (parts.Length > 1)
                    {
                        var innerParts = parts[1].Split('\"');
                        var imageUrl = Parser.ImageUrlSplitter[0] + innerParts[0];
                        return imageUrl;
                    }

                    throw new Exception("Couldn't load image");
                }
                else
                {
                    return Parser.GetImageUrl(page);
                }
            }
            catch
            {
                return null;
            }
        }

        public int GetStepsNum(string page)
        {
            if (Parser.StepsSplitter.Length > 0)
            {
                int myCount = System.Text.RegularExpressions.Regex.Matches(page, Parser.StepsSplitter[0]).Count;
                return myCount;
            }
            else
            {
                return Parser.GetStepsNum(page);
            }
        }

        public int GetServings(string page) {
            try {
                return Parser.GetServings(page);
            }
            catch {
                return 1;
            }
        }
        public List<IngredientInfo> GetIngredients(string page) {
            var ingredients = new List<IngredientInfo>();
            var ingredientParts = page.Split(Parser.IngredientSplitter, StringSplitOptions.None);
            for (int i = 1; i < ingredientParts.Length; i++) {
                var res = Parser.ParseWeightAndName(ingredientParts[i].ToLower());
                if (res != null)
                    ingredients.Add(res);
            }
            return ingredients;
        }

        public static TimeSpan ParsePrepTime(string time) {
            var days = GetTimeUnit(ref time, "days");
            if (days == 0)
                days = GetTimeUnit(ref time, "day");
            if (days == 0)
                days = GetTimeUnit(ref time, "d");
            var hours = GetTimeUnit(ref time, "hrs");
            if (hours==0)
                hours = GetTimeUnit(ref time, "hr");
            if (hours == 0)
                hours = GetTimeUnit(ref time, "h");
            hours = hours + days * 24;
            var minutes = GetTimeUnit(ref time, "mins");
            if (minutes == 0)
                minutes= GetTimeUnit(ref time, "min");
            if (minutes == 0)
                minutes= GetTimeUnit(ref time, "m");
            return new TimeSpan(hours,minutes, 0);
        }

        public static int GetTimeUnit(ref string time, string timeUnit) {
            var parts = time.Split(new string[] { timeUnit }, StringSplitOptions.None);
            if (parts.Length > 1) {
                time = parts[1];
                return int.Parse(parts[0]);
            }
            else {
                return 0;
            }
        }

        public static string GetAdditionalInfo(string ingredient)
        {
            var additionalInfo = "";
            foreach (var word in Map.ActionInfo)
            {
                var wordIdx = ingredient.IndexOf(word);
                if (wordIdx > 0)
                {
                    // Make sure that the word is a complete match
                    if (wordIdx > 1 && Char.IsLetter(ingredient[wordIdx - 1]))
                    {
                        continue;
                    }
                    if ((wordIdx + word.Length) < ingredient.Length && Char.IsLetter(ingredient[wordIdx + word.Length]))
                    {
                        continue;
                    }
                    additionalInfo = word;
                    break;
                }
            }

            return additionalInfo;
        }
    }
}
