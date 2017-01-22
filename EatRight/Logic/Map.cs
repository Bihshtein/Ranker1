using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic {
    public class Map {

        
        public static string AdjustNames(string item) {
            SimpleNames.Keys.ToList().ForEach(key => {
                if (item.Contains(key))
                    item = item.Replace(key, Map.SimpleNames[key]);
            });
            return item;
        }


        public static string AdjustIngredient(string ingredient) {
            ingredient = ingredient.ToLower();
            Map.StartPharseRemove.ForEach(item => ingredient = ingredient.Replace(item + " ", ""));
            Map.OtherPhraseRemove.ForEach(item => ingredient = ingredient.Replace(item, ""));
            ingredient = ingredient.Trim();
            foreach (var prefix in PrefixRemove)
            {
                if (ingredient.StartsWith(prefix))
                {
                    ingredient = ingredient.Substring(prefix.Length);
                    break;
                }
            }

            return ingredient;
        }

        public static string AdjustInnerPart(string innerpart, ref string unifiedInnerpart) {
            var split = innerpart.Split(',');
            if (split[0] != string.Empty)
                innerpart = split[0];
            else
                innerpart = split[1];

            if (split.Length > 1)
            {
                unifiedInnerpart = split[0] + " " + split[1];
                unifiedInnerpart = unifiedInnerpart.Trim();
            }
      
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
            { "boneless, skinless chicken","chicken"},
            { "fresh mozzarella, cut into small cubes", "mozzarella" },
            { "leaves", "leaf" },
            { "greek-style", "greek" },
            { "cooking oil", "salad or cooking oil" },
            { "package elbow macaroni", "macaroni"},
            { "dijon mustard", "mustard"},
            { "fine bread crumbs", "bread crumbs" },
            { "can anchovy fillets", "anchovy"},
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
            { "prime rib", "rib eye" },
            { "whole chicken", "chicken" },
            { "links pork sausage links", "frankfurter pork sausage"},
            { "packages refrigerated crescent roll dough (such as pillsbury&#174;", "pillsbury refrigerated dough"},
            { "italian parsley", "parsley"},
            { "salmon fillets", "salmon"},
            { "red pepper", "red peppers" },
            { "pepper (any color)", "peppers" },
            { "pumpkin puree", "pumpkin" },
            { "breakfast sausage", "pork sausage" },
            { "strips bacon", "bacon" },
            { "chorizo sausage", "pork sausage" },
            { "flat-leaf parsley", "parsley" },
            { "frozen hash brown potatoes", "hash brown potatoes" },
            { "half-and-half cream", "half and half cream" },
            { "dry sherry", "dry dessert wine" },
            { "cilantro", "coriander (cilantro) leaves" },
            { "rib celery", "celery" },
            { "confectioners' sugar", "granulated sugars" },
            { "french bread", "french or vienna bread" },
            { "shredded pepperjack cheese", "monterey cheese" },
            { "parmigiano-reggiano cheese", "parmesan cheese" },
            { "low-fat cottage cheese", "cottage 1% milkfat|cheese" },
            { "monterey jack cheese", "monterey cheese" },
            { "green chile peppers", "green peppers" },
            { "canola oil", "corn and canola oil" },
            { "unsweetened applesauce", "applesauce" },
            { "whole wheat pastry flour", "wheat flour" },
            { "barbeque", "barbecue" },
            { "spinach leaf", "chopped or leaf spinach" }
        };

        public static List<string> StartPharseRemove = new List<string> {
             "prepared", "package", "packages","blanched","slivered",
            "melted","sifted", "sprig", "sprigs", "ground", "shredded", "cubed", "rolled",
            "head", "heads", "thick sliced", "sliced", "stalk", "stalks", "finely diced",
            "diced", "dried minced", "minced", "finely chopped", "chopped", "grated","mashed","crushed",
            "ripe", "steaks", "cold", "warm", "fresh"};

        public static List<string> OtherPhraseRemove = new List<string> {
           "sprigs", "(optional)", "asiago", "extra virgin", "panko" , "steaks", "- peeled","halves",
            "for frying", "herb-seasoned", "steel cut", "peeled"
        };

        public static List<string> PrefixRemove = new List<string>
        {
            "and "
        };

    }
}
