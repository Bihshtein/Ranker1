﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic {
    public class Map {

        public static void ReplaceWord(List<List<string>> keyWordList, ref string ingredient) {
            var copy = ingredient;
            keyWordList.ForEach(list => list.ForEach(item => copy = copy.Replace(item + " ", "")));
            ingredient = copy;
        }

        public static void ReplaceLastWord(List<List<string>> keyWordList, ref string ingredient) {
            var copy = ingredient;
            keyWordList.ForEach(list => list.ForEach(item => copy = copy.Replace(item + "", "")));
            ingredient = copy;
        }
      
        public static string AdjustNames(string item) {
            SimpleNames.Keys.ToList().ForEach(key => {
                if (item.Contains(key))
                    item = item.Replace(key, Map.SimpleNames[key]);
            });
            return item;
        }

        public static string AdjustInnerPart(string innerpart) {
            var split = innerpart.Split(',');
            if (split[0] != string.Empty)
                innerpart = split[0];
            else
                innerpart = split[1];         
      
            return innerpart;
        }

        public static Dictionary<string, string> RecipeToUSDAMeasure = new Dictionary<string, string> {
            { "cup sliced","cup chopped" },
            { "cup","glass" },
            { "chicken breasts","piece" },
            { "chicken breast","piece" },
            { "salmon steak","fillet" },
            { "chicken breast halve","piece" },
            { "chicken breast halves","piece" },
            { "cornish game hens", "bird"},
            { "teaspoon", "tsp"},
            { "tablespoon", "tbsp"},
        };


        public static Dictionary<string, string> SimpleNames = new Dictionary<string, string>() {
            { "skinless, boneless chicken","chicken"},
            { "fresh mozzarella, cut into small cubes", "mozzarella" },
            { "leaves", "leaf" },
            { "greek-style", "greek" },
            { "cooking oil", "salad or cooking oil" },
            { "package elbow macaroni", "macaroni"},
            { "dijon mustard", "mustard"},
            { "fine bread crumbs", "bread crumbs" },
            { "can anchovy fillets", "anchovy"},
            { " sprigs",""},
            { "fresh dill","fresh dill weed"},
            { "skim", "nonfat"},
            { "bell pepper", "pepper" },
            { "black olives", "ripe olives" },
            { "pepperoni sausage", "pepperoni" },
            { "genoa", "italian" },
            { "zest", "peel" },
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
            { "red onion","onions"},
            { "white wine vinegar","red wine vinegar"},
            { "seashell pasta", "pasta" },

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
        public static List<string> NeedlesInfo = new List<string> { "(optional)", "asiago", "extra virgin", "panko ", "for frying" };



    }
}
