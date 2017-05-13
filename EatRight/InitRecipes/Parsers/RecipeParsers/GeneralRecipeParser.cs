﻿using RestModel;
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
    }
}
