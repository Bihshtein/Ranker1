using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestModel
{
    class Mapping
    {
        public static void ReplaceWord(List<List<string>> keyWordList, ref string ingredient)
        {
            var copy = ingredient;
            keyWordList.ForEach(list => list.ForEach(item => copy = copy.Replace(item + " ", "")));
            ingredient = copy;
        }

        public static void ReplaceLastWord(List<List<string>> keyWordList, ref string ingredient)
        {
            var copy = ingredient;
            keyWordList.ForEach(list => list.ForEach(item => copy = copy.Replace(item + "", "")));
            ingredient = copy;
        }
        public static Dictionary<string, string> RecipeToNutrient = new Dictionary<string, string> {
            { "white sugar","granulated sugar"},
            { "bread flour","bread wheat flour"},
            { "all-purpose flour","all-purpose wheat flour"},
            { "whole wheat flour","whole-grain wheat flour"},
            { "kosher salt","table salt"},
            { "dry milk powder","dry milk"},
            { "skinless chicken thighs","meat only chicken thighs"},
            { "marsala wine","sweet wine"},
            { "port wine","sweet wine"},
            { "white vinegar","distilled vinegar"},
            { "ketchup","catsup"},
            { "sirloin steak","top sirloin steak"},
        
        };
        public static List<string> StartCutDetails = new List<string> {
            "melted","sifted", "sprig", "sprigs", "ground", "shredded", "cubed", "rolled",
            "head", "heads", "sliced", "stalk", "stalks", "diced", "minced", "chopped",
            "grated","mashed","crushed", "ripe", "steaks"};

        public static List<string> EndCutDetails = new List<string> {
            "steaks", "- peeled","halves"
        };
        public static List<string> ServeDetails = new List<string> { "cold", "warm", "fresh" };
        public static List<string> PackDetails = new List<string> { "for frying", "prepared", "package", "packages" };
        public static List<string> NeedlesInfo = new List<string> { "panko ", "for frying" };
    }
}
