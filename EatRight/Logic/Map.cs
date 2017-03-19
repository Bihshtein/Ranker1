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
            WordsMap.Keys.ToList().ForEach(list => {
                if (list.All(word => item.Contains(word)))
                    item = Map.WordsMap[list];
            });
            return item;
        }

        public static bool ShouldSkip(string ingredient) {
            ingredient = ingredient.ToLower();
            return SkipIngredients.Contains(ingredient) || ingredient.Contains("optional");
        }

        public static string RemoveWord(List<string> wordsList, string phrase,bool adjust=true) {
            wordsList.ForEach(item => {
                if (phrase.Split(' ').Length > 1 && item.Split(' ').Length == 1) {
                    var newPhrase = "";
                    phrase.Split(' ').ToList().ForEach(word => {
                        word = word.Trim();
                        if (word != item && word != "("+item+")")
                            newPhrase += word + " ";
                    });
                    phrase = newPhrase.Trim();
                }
                else if (adjust)
                    AdjustPhrase(item, ref phrase);
                else
                    phrase = phrase.Split(new string[1] { item }, StringSplitOptions.None)[0];
            });
            return phrase;
        }

        public static void AdjustPhrase (string item, ref string phrase) {
           if (WordCheck(item, phrase)) {
                phrase = phrase.Replace(item, "");
                phrase = phrase.Replace("(", "");
                phrase = phrase.Replace(")", "");
                phrase = phrase.Replace(";", "");
            }
            phrase = phrase.Trim();
        }

        public static bool HasWord(List<string> wordsList, string phrase) {
            return wordsList.Any(item => WordCheck(item, phrase));
        }

        public static string GetWord(List<string> wordsList, string phrase) {
            return wordsList.First(item => WordCheck(item, phrase));
        }

        public static bool WordCheck(string word, string phrase) {
            return (phrase.StartsWith(word + " ") || phrase.EndsWith(" " + word) || phrase.Contains(" " + word + " ") || phrase.Contains(" " + word + ".") || phrase.Contains(" " + word + ";") ||
                phrase == word || phrase.Contains("(" + word + ")") || phrase.Contains(" " + word + ")") || phrase.Contains("(" + word + " "));
        }

        public static string AdjustIngredient(string ingredient) {
            ingredient = ingredient.ToLower();
            if (ingredient.StartsWith(") "))
                ingredient = ingredient.Replace(") ", "");

            if (ingredient[ingredient.Length - 1] == ')') {
                var index = ingredient.LastIndexOf('(');
                if (index > 0)
                    ingredient = ingredient.Remove(index, ingredient.Length - index);
            }
            ingredient = RemoveWord(Map.StateInfo, ingredient).Trim();
            ingredient = RemoveWord(Map.ActionInfo, ingredient).Trim();
            ingredient = RemoveWord(Map.ConnectingWords, ingredient).Trim();
            ingredient = RemoveWord(Map.EndActionInfo, ingredient,false).Trim();
            ingredient = ingredient.Replace(";", "");
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
            { "spaghetti" , "pasta"},
            { "season salt","table salt"},
            { "seasoned salt","table salt"},
            { "seasoning salt","table salt"},
            { "celery salt","table salt"},
            { "onion salt","table salt"},
             { "cumin","cumin seed"},
            { "ground cumin","cumin"},
               { "tamatos","tomatoes"},
               { "jalapeֳ±o","jalapeno"},
            { "liquid milk","milk"},

        };
        public static Dictionary<List<string>, string> WordsMap = new Dictionary<List<string>, string>() {
               { new List<string> {"pancake","mix" }, "pancakes dry mix"},
               { new List<string> {"bacon"}, "bacon"},
               { new List<string> {"noodles"}, "noodles"},
               { new List<string> {"berries"}, "blue berries"},
               { new List<string> {"brown sugar"}, "brown sugar"},
               { new List<string> {"tomatoes"}, "tomatoes"},
               { new List<string> { "scallion"}, "green onion"},
               { new List<string> {"all","purpose", "flour"}, "all-purpose wheat flour"},
               { new List<string> {"chicken", "breast" }, "chicken breast"},
               { new List<string> {"salt", "pepper" }, "table salt"},
               { new List<string> { "table", "salt" }, "table salt"},
               { new List<string> { "breakfast", "sausage" } ,"pork sausage" },

               { new List<string> { "spaghetti", "pasta" }, "pasta"},
                { new List<string> { "macaroni", "pasta" }, "pasta"},
               { new List<string> { "penne", "pasta" }, "pasta"},
               { new List<string> { "rotini", "pasta" }, "pasta"},
               { new List<string> { "bow", "tie", "pasta" }, "pasta"},
               { new List<string> { "cappelini",  "pasta" }, "pasta"},
               { new List<string> { "fettuccine",  "pasta" }, "pasta"},
               { new List<string> { "farfalle",  "pasta" }, "pasta"},
               { new List<string> { "your",  "pasta" }, "pasta"},
               { new List<string> { "elbow",  "macaroni" }, "pasta"},
                { new List<string> { "whole","grain", "pasta" }, "whole-wheat pasta"},
                { new List<string> { "fresh", "dill" }, "fresh dill weed"},
        };
        public static Dictionary<string, string> ChangePartialPhrases = new Dictionary<string, string>() {
            { "cashews", "cashew nuts"},
            { "kilishi","beef jerky" },
            { "light soy sauce", "soy sauce"},
            {"mint leaves" ,"peppermint"},
            { "mozzarella string cheese", "mozzarella cheese" },
            { "tomato juice or v8", "tomato juice"},
            { "button mushroom", "white mushroom"},
            { "prawn", "shrimp"},
            { "lukewarm water", "water"},
            { "iceberg lattuce", "iceberg lettuce"},
            { "lettuce leaves", "lettuce"},
            { "romain lettuce", "romaine lettuce"},
            { "beef hotdogs", "frankfurter"},
            { "spicy chorizo","chorizo"},
            { "an onion", "onion"},
            { "pot roast", "arm pot roast"},
            { "chihuahua cheese", "queso chihuahua cheese"},         
            { "pork roast", "pork loin roast"},
            { "extra virgin olive oil","olive oil"},
            { "freshly ground black pepperl","black pepper"},
            { "hard eggs","boiled eggs"},
            { "hard boiled eggs","boiled eggs"},
            { "pork butt","pork shoulder blade"},
            { "dried basil leaves", "dried basil" },
            { "basil leaves", "dried basil" },
            { "basil leaf", "dried basil"},
            { "bay leaves", "bay leaf"},
            { "salt to taste","table salt" },
            { "salt pepper","table salt" },
            { "black peppet and table salt","table salt" },
            { "beef mince", "ground beef"},
            { "scalions","green onion" },
            { "green spring onion", "green onion"},
            { "spring onions", "green onion"},
            { "spring onion", "green onion"},   
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
            { "boneless chicken","chicken"},
            { "fresh mozzarella, cut into small cubes", "mozzarella" },
            { "greek-style", "greek" },
            { "cooking oil", "salad or cooking oil" },
            { "package elbow macaroni", "macaroni"},
            { "dijon mustard", "mustard"},
            { "fine bread crumbs", "bread crumbs" },
            { "breadcrumbs", "bread crumbs" },
            { "bread cubes", "bread crumbs" },
            { "Italian-seasoned bread crumbs" ,"seasoned bread crumbs"},
            {"Italian-style seasoned bread crumbs" , "seasoned bread crumbs"},
            { "dried bread crumbs", "bread crumbs" },
            { "can anchovy fillets", "anchovy"},
            { "skim", "nonfat"},
            { "bell pepper", "pepper" },
            { "black olives", "ripe olives" },
            { "pepperoni sausage", "pepperoni" },
            { "genoa", "italian" },
            { "zest", "peel" },
            { "white sugar","granulated sugar"},
            { "bread flour","bread wheat flour"},
            { "white flour","all-purpose wheat flour"},
            { "ap flour","all-purpose wheat flour"},
            { "plain flour","all-purpose wheat flour"},
            { "whole wheat flour","whole-grain wheat flour"},
            { "pinch table salt and black pepper","table salt" },
            { "kosher salt","table salt"},
            { "oil/butter", "butter"},
            { "salt and pepper","table salt" },
            { "salt and black pepper","table salt" },
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
            { "chorizo sausage", "pork sausage" },
            { "flat-leaf parsley", "parsley" },
            { "parsley for garnish", "parsley"},
            { "half-and-half cream", "half and half cream" },
            { "dry sherry", "dry dessert wine" },
            { "rib celery", "celery" },
            { "curry leaves", "curry powder" },
            { "icing sugar" ,"powdered sugars"},
            { "sugar powder","powdered sugars"  },
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
            { "toast", "toasted bread"},
            { "smoked ham", "cured ham"},
            { "lean ground beef", "ground beef"},
            { "extra-lean ground beef", "ground beef"},
            { "extra ground ground beef", "ground beef"},
            { "extra lean ground beef", "ground beef"},
            { "breakfast muffins", "wheat bran muffins"}
        };
        public static List<string> ActionInfo = new List<string> {
            "peeled and diced","thick sliced", "sliced","slivered","melted","sifted", "ground", "shredded",
            "cubed", "rolled","mashed","crushed"  ,"boiled","grated","julienne",
            "fine minced","finely chopped","medium dice", "small dice","finely diced","diced", "dried minced", "minced", "finely chopped", "chopped",
            "finely shredded","cooked","finely crushed","frozen","blended","chiffonade"
        };

        public static List<string> EndActionInfo = new List<string> {
            "cut into"
        };

        public static List<string> ConnectingWords = new List<string> {
           "a","of"
        };
        public static List<string> StateInfo = new List<string> {
          "canned", "cans","can","some","fillets", "prepared", "package","as needed",
           "packages","blanched","florets","steel cut", "peeled","small dice",
           "sprig", "sprigs","loaf", "head", "heads",  "stalk", "stalks",
           "grind", "ripe", "steaks", "cold", "warm", "fresh","freshly",
          "(optional)", "asiago",  "panko" , "steaks", "- peeled","halves",
            "for frying","for frying pan", "herb-seasoned", "solid pack",
        };

        public static List<string> SkipIngredients = new List<string> {
            "for topping","toppings"
        };
    }
}
