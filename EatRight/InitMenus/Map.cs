using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitRecipes {
    class Map {

        public static Dictionary<string, string> SimpleNames = new Dictionary<string, string>() {
            { "skinless, boneless chicken","chicken"},
            { "fresh mozzarella, cut into small cubes", "mozzarella" },
            { "leaves", "leaf" },
            { "greek-style", "greek" },
            { "cooking oil", "salad or cooking oil" },
        };

        public static List<string> RelativeSizes = new List<string> {
            // relative to the weight stated by the USDA for the product
            // order matter keep the more inclusicve delimiters first
            "slices",
            "slice ",
            "large",
            "small",
            "medium",
            "cup packed",
            "cup unpacked",
            "cups",
            "cup",
            "cup sliced",
            "cup small",
            
        };


        public static Dictionary<string, string> RecipeToUSDAMeasure = new Dictionary<string, string> {
            { "cup sliced","cup chopped" },
            { "cup","glass" },
            { "chicken breasts","piece" },
            { "chicken breast","piece" },
            { "salmon steak","fillet" },
            { "chicken breast halve","piece" },
            { "chicken breast halves","piece" },
            { "cornish game hens", "bird"}
        };


        public static Dictionary<string, double> MeasuresWeights = new Dictionary<string, double>(){
            {"bunch",60 },
            {"cloves",3 },//garlic
            {"pinch",0.25 },//green onion or whatever
            {"teaspoon ",5 },
            {"teaspoons",5 },
            {"tablespoons",14 },
            {"tablespoon ",14 },
            {"pound ",453.5},
            {"pound)",453.5},
            {"pounds ",453.5},
            {"ounces",28.3 },
            {"ounce ",28.3 },
            {"ounce)",28.3 },
            {"fluid ounce)",28.3 },
            {"fluid ounce ",28.3 },
            {"fluid ounces",28.3 },
        };

        public static string GetPrettyItem(string item) {
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
          
            innerpart = innerpart.Replace("(optional)", "");
            innerpart = innerpart.Replace("(110 degrees f/45 degrees c)", "");
            return innerpart;
        }
    }
}
