using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;
using System.Net;

namespace InitRecipes {
    public class FoodParser : IRecipeParser {
        public string[] ServingSplitter { get; set; }

        public List<Tuple<string, double, string>> GetIngredients(string page) {
            var ingredients = new List<Tuple<string, double, string>>();
            var ingredientParts = page.Split(new string[1] { "<li data-ingredient=" }, StringSplitOptions.None);
            for (int i = 1; i < ingredientParts.Length; i++) {
                var parts = ingredientParts[i].Split(new string[3] { "<span>", "</span>", "<a" }, StringSplitOptions.None);
                var ingredient = parts[0].Split('"')[1].Replace('+', ' ');
                ingredient = Map.AdjustNames(ingredient);
                var weight = 0.0;
                var weightStr = "";
                try {
                    weightStr = parts[1].Split('-')[0];
                    weight = double.Parse(weightStr);
                }
                catch (FormatException) {
                    var weightParts = weightStr.Split(new string[4] { "<sup>", "</sup>", "<sub>", "</sub", }, StringSplitOptions.None);
                    var amount = "";
                    weightParts.ToList().ForEach(p => amount += WebUtility.HtmlDecode(p));
                    weight = ParseHelpers.ParseAmount(amount);

                }

                var rest = parts[2].Trim();
                var relativeWeight = Formulas.MeasuresWeights.Keys.ToList().FirstOrDefault(s => Map.WordCheck(s, rest));
                if (relativeWeight == null) {
                    relativeWeight = Formulas.RelativeProductSize.FirstOrDefault(s => Map.WordCheck(s, rest));
                    if (relativeWeight == null) {
                        relativeWeight = Formulas.RelativeSizes.FirstOrDefault(s => Map.WordCheck(s, rest));
                        if (relativeWeight == null) {
                            relativeWeight = "";
                        }
                    }
                }
                ingredients.Add(new Tuple<string, double, string>(ingredient, weight, relativeWeight));
            }
            return ingredients;
        }

        public TimeSpan GetPrepTime(string page) {
            return new TimeSpan(0, 10, 0);
        }
       
        public FoodParser() {
            ServingSplitter = new string[1] { "Servings Per Recipe:" };
        }

        public int GetServings(string page) {
            var servingParts = page.Split(ServingSplitter, StringSplitOptions.None);
            var servingStr = new String(servingParts[1].TakeWhile(a => a != '<').ToArray());
            return int.Parse(servingStr);

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

    }
}
