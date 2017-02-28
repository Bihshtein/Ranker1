using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic {
    public class Map {

        
        public static string AdjustNames(string item) {
            ChangePartialPhrases.Keys.ToList().ForEach(key => {
                if (item.Contains(key))
                    item = item.Replace(key, Map.ChangePartialPhrases[key]);
            });

            ChangeWholePhrases.Keys.ToList().ForEach(key => {
                if (item == key)
                    item = Map.ChangeWholePhrases[item];
            });
            return item;
        }


        public static string AdjustIngredient(string ingredient) {
            ingredient = ingredient.ToLower();
            if (ingredient.StartsWith(") "))
                ingredient = ingredient.Replace(") ", "");
            Map.PharsesToRemove.ForEach(item => {
                if (ingredient.StartsWith(item + " ") || ingredient.EndsWith(" " + item))
                    ingredient = ingredient.Replace(item, "");
                ingredient = ingredient.Trim();
            });
            return ingredient;
        }

        public static string AdjustInnerPart(string innerpart) {
            var split = innerpart.Split(',');
            if (split[0] != string.Empty)
                innerpart = split[0];
            else
                innerpart = split[1];
            split = innerpart.Split(new string[1] { " - " }, StringSplitOptions.None);
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
        public static Dictionary<string, string> ChangeWholePhrases = new Dictionary<string, string>() {
            { "flour","all-purpose wheat flour"},
            { "cayenne" , "cayenne pepper"},
            { "spaghetti" , "pasta"}

        };
        public static Dictionary<string, string> ChangePartialPhrases = new Dictionary<string, string>() {
            { "light soy sauce", "soy sauce"},
            {"mint leaves" ,"peppermint"},
            { "mozzarella string cheese", "mozzarella cheese" },
            { "tomato juice or v8", "tomato juice"},
            { "button mushroom", "white mushroom"},
            { "prawn", "shrimp"},
            { "iceberg lattuce", "iceberg lettuce"},
            { "lettuce leaves", "lettuce"},
            { "romain lettuce", "romaine lettuce"},
            { "beef hotdogs", "frankfurter"},
            { "spaghetti pasta" , "pasta"},
            { "spicy chorizo","chorizo"},
            { "macaroni pasta" , "pasta"},
            { "penne pasta" , "pasta"},
            { "an onion", "onion"},
            { "tamatos","tomatoes"},
            { "chihuahua cheese", "queso chihuahua cheese"},
            { "pancakes mix", "pancakes dry mix"},
            { "spaghetti noodles", "noodles"},
            { "macaroni noodles", "noodles"},
            { "lasagna noodles", "noodles"},
            { "pork roast", "pork loin roast"},
            { "extra virgin olive oil","olive oil"},
            { "freshly ground black pepperl","black pepper"},
            { "hard eggs","boiled eggs"},
            { "hard boiled eggs","boiled eggs"},
            { "roma tomatoes","tomato"},
            { "grape tomatoes","tomato"},
            { "dried basil leaves", "dried basil" },
            { "basil leaves", "dried basil" },
            { "basil leaf", "dried basil"},
            { "bay leaves", "bay leaf"},
            { "salt to taste","table salt" },
            { "salt pepper","table salt" },
            { "beef mince", "ground beef"},
            { "scallion","green onion" },
            { "scallions","green onion" },
            { "frozen shrimps", "shrimps"},
            { "green spring onion", "green onion"},
            { "spring onions", "green onion"},
            { "spring onion", "green onion"},
            { "light brown sugar", "brown sugar"},
            { "corn and ola oil","corn oil" },
            { "baby spinach","spinach" },
            { "chocolate chips", "sweet chocolate candies"},
            { "cocoa powder","cocoa dry powder" },
            { "smoked paprika","paprika" },
            { "hot water","water"},
            { "vanilla essence", "vanilla extract"},
            { "cilantro", "coriander" },
            { "butter or margarine","butter" },
            { "margarine or butter","butter" },
            { "boiling water","water"},
            { "skinless, boneless chicken","chicken"},
            { "boneless, skinless chicken","chicken"},
            { "boneless skinless chicken","chicken"},
            { "fresh mozzarella, cut into small cubes", "mozzarella" },
            { "greek-style", "greek" },
            { "cooking oil", "salad or cooking oil" },
            { "package elbow macaroni", "macaroni"},
            { "dijon mustard", "mustard"},
            { "fine bread crumbs", "bread crumbs" },
            { "breadcrumbs", "bread crumbs" },
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
            { "white flour","all-purpose wheat flour"},
            { "all-purpose flour","all-purpose wheat flour"},
            { "whole wheat flour","whole-grain wheat flour"},
            { "all purpose flour","all-purpose wheat flour"},
            { "pinch table salt and black pepper","table salt" },
            { "kosher salt","table salt"},
            { "cumin","cumin seed"},
            { "ground cumin","cumin"},           
            { "salt and pepper","table salt" },
            { "salt &amp; pepper","table salt" },
            { "sea salt and black pepper","table salt" },
            { "sea salt","table salt" },
            { "garlic salt","table salt" },
            { "dry milk powder","dry milk"},
            { "heavy cream","cream heavy whipping"},
            { "unsalted butter","butter without salt"},
            { "butter (salted)","butter salted"},
            { "skinless chicken thighs","meat only chicken thighs"},
            { "marsala wine","sweet wine"},
            { "port wine","sweet wine"},
            { "white vinegar","distilled vinegar"},
            { "ketchup","catsup"},
            { "sirloin steak","top sirloin steak"},
            { "red onion","onion"},
            { "white onion", "onion" },
            { "white wine vinegar","red wine vinegar"},
            { "seashell pasta", "pasta" },
            { "prime rib", "rib eye" },
            { "yellow squash", "summer squash"},
            { "whole chicken", "chicken" },
            { "links pork sausage links", "frankfurter pork sausage"},
            { "packages refrigerated crescent roll dough (such as pillsbury&#174;", "pillsbury refrigerated dough"},
            { "italian parsley", "parsley"},
            { "parsley flakes", "parsley"},
            { "parsley leaves", "parsley"},
            { "salmon fillets", "salmon"},
            { "pepper (any color)", "peppers" },
            { "pumpkin puree", "pumpkin" },
            { "breakfast sausage", "pork sausage" },
            { "strips bacon", "bacon" },
            { "chorizo sausage", "pork sausage" },
            { "flat-leaf parsley", "parsley" },
            { "parsley for garnish", "parsley"},
            { "frozen hash brown potatoes", "hash brown potatoes" },
            { "half-and-half cream", "half and half cream" },
            { "dry sherry", "dry dessert wine" },            
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
            { "spinach leaf", "chopped or leaf spinach" },
            { "spinach leaves", "spinach" },
            { "cherry tomatoes", "tomato" },
            { "vine ripened tomatoes", "tomato"},
            { "cocktail tomatoes", "tomato"},
            { "toast", "toasted bread"},
            { "smoked ham", "cured ham"},
            { "lean ground beef", "ground beef"},
            { "extra-lean ground beef", "ground beef"},
            { "extra ground ground beef", "ground beef"},
            { "extra lean ground beef", "ground beef"}
        };

        public static List<string> PharsesToRemove = new List<string> {
         "some","fillets", "prepared", "package", "packages","blanched","slivered","florets",
         "(peeled)","(sliced)","(chopped)",
            "melted","sifted", "sprig", "sprigs", "ground", "shredded", "cubed", "rolled",
            "head", "heads", "thick sliced", "sliced", "stalk", "stalks", "finely diced",
            "diced", "dried minced", "minced", "finely chopped", "chopped", "grated","mashed","crushed",
            "ripe", "steaks", "cold", "warm", "fresh", "cut into",
            "canned", "cans","can","boiled", "(optional)", "asiago",  "panko" , "steaks", "- peeled","halves",
           "and cut into circles", "for frying", "herb-seasoned", "steel cut", "peeled","solid pack","small dice",";"
        };



    }
}
