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

            WordsWithExclusionMap.Keys.ToList().ForEach(list => {
                if (list.All(word => item.Contains(word)) && WordsWithExclusionMap[list].Item1.All(word => !item.Contains(word)))
                    item = Map.WordsWithExclusionMap[list].Item2;
            });
            return item;
        }

        public static bool ShouldSkip(string ingredient) {
            ingredient = ingredient.ToLower();
            return SkipHeadlines.Any(item => ingredient.Contains(item) || ingredient.Contains("optional") ||SkipHeadlinesFullNames.Contains(ingredient));
        }

        public static string RemoveWord(List<string> wordsList, string phrase,bool adjust=true) {
            wordsList.ForEach(item => {
                if (phrase.Split(' ').Length > 1 && item.Split(' ').Length == 1) {
                    var newPhrase = "";
                    phrase.Split(' ').ToList().ForEach(word => {
                        word = word.Trim();
                        if (word != item && word != "("+item+")" && word != "[" + item + "]" && word != "-" + item)
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
            if (ingredient.Length > 0 && ingredient[ingredient.Length - 1] == '-')
                ingredient = ingredient.Remove(ingredient.Length - 1, 1);
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
            { "teaspoons", "tsp"},
            { "tablespoon", "tbsp"},
            { "tablespoons", "tbsp"},


        };
        public static Dictionary<string, string> ChangeWholePhrases = new Dictionary<string, string>() {
            { "flour","all-purpose wheat flour"},
            { "cayenne" , "cayenne pepper"},            
            { "season salt","table salt"},
            { "seasoned salt","table salt"},
            { "seasoning salt","table salt"},
            { "celery salt","table salt"},
            { "onion salt","table salt"},
            { "tamatos","tomatoes"},
            { "jalapeño","jalapeno"},
            { "liquid milk","milk"},
            { "ice","water"},

        };
        public static Dictionary<List<string>,Tuple<List<string>,string>> WordsWithExclusionMap = new Dictionary<List<string>, Tuple<List<string>, string>>() {
            { new List<string> {"flour"},new Tuple<List<string>,string>(new List<string> {"whole","coconut","almond"}, "all-purpose wheat flour")},
            { new List<string> {"pasta"},new Tuple<List<string>,string>(new List<string> {"whole","grain"}, "pasta")},
            { new List<string> {"onion"},new Tuple<List<string>,string>(new List<string> {"soup", "dehydrated", "scallion","spring","green","scalions"}, "onion")},
            { new List<string> {"onions"},new Tuple<List<string>,string>(new List<string> {"soup", "dehydrated", "scallion","spring","green","scalions"}, "onion")},

            { new List<string> {"lettuce"},new Tuple<List<string>,string>(new List<string> { "iceberg", "romain",}, "lettuce")},
        };
        public static Dictionary<List<string>, string> WordsMap = new Dictionary<List<string>, string>() {
               { new List<string> {"pancake","mix" }, "pancakes dry mix"},

               { new List<string> {"prosciutto"}, "cured ham"},
               { new List<string> {"bacon"}, "bacon"},
                { new List<string> {"pancetta"}, "bacon"},
               { new List<string> {"noodles"}, "noodles"},
               { new List<string> {"berries"}, "blue berries"},
               { new List<string> {"brown sugar"}, "brown sugar"},
               { new List<string> {"tomatoes"}, "tomatoes"},
               { new List<string> { "scallion"}, "green onion"},
               { new List<string> {"chicken", "breast" }, "chicken breast"},
               { new List<string> {"salt" }, "table salt"},// very hardcore, should exclude thigs like :"with salt"
               { new List<string> {"butter" }, "butter"},// very hardcore, should exclude peanut butter
               { new List<string> {"parsley" }, "parsley"},
               { new List<string> { "breakfast", "sausage" } ,"pork sausage" },
               { new List<string> { "fresh", "dill" }, "fresh dill weed"},
               { new List<string> { "ground", "beef" }, "ground beef"},
               { new List<string> { "water",  }, "water"},
               { new List<string> { "ice", "cubes" }, "water"},
               { new List<string> { "basil",  }, "dried basil"},
               { new List<string> { "spring", "onion" }, "green onion"},
               { new List<string> { "spinach",  }, "spinach"},
               { new List<string> { "bread crumbs",  }, "bread crumbs"},
               { new List<string> { "vegetables", "oil",  }, "olive oil"},
               { new List<string> { "olive oil",  }, "olive oil"},
               { new List<string> { "chia",}, "chia seeds"},
               { new List<string> { "jalapeֳ±o", }, "jalapeno"},
               { new List<string> { "pomelo", }, "pumello"},
               { new List<string> { "vermicelli", }, "pasta"},
               { new List<string> { "orzo", }, "pasta"},
               { new List<string> { "spaghetti", }, "pasta"},
               { new List<string> { "orzo", }, "pasta"},
               { new List<string> { "rigatoni", }, "pasta"},
               { new List<string> { "cumin",}, "cumin seed"},
               { new List<string> { "mayo"}, "mayonnaise"},
               { new List<string> { "mint"}, "peppermint"},
               { new List<string> { "prawn"}, "shrimp"},
               { new List<string> { "shrimp"}, "shrimp"},
               { new List<string> { "chicken", "thighs",  }, "chicken thighs"},
               { new List<string> { "pepperjack", "cheese",  }, "monterey cheese"},
               { new List<string> { "havarti", "cheese",  }, "monterey cheese"},
               { new List<string> { "monterey", "cheese",  }, "monterey cheese"},
               { new List<string> { "velveeta"}, "cheddar cheese"},


        };
        public static Dictionary<string, string> ChangePartialPhrases = new Dictionary<string, string>() {
            { "cashews", "cashew nuts"},
            { "kilishi","beef jerky" },
            { "light soy sauce", "soy sauce"},
            { "mozzarella string cheese", "mozzarella cheese" },
            { "button mushroom", "white mushroom"},
            { "prawn", "shrimp"},
            { "iceberg lattuce", "iceberg lettuce"},
            { "romain lettuce", "romaine lettuce"},
            { "beef hotdogs", "frankfurter"},
            { "spicy chorizo","chorizo"},
            { "pot roast", "arm pot roast"},
            { "chihuahua cheese", "queso chihuahua cheese"},         
            { "pork roast", "pork loin roast"},
            { "extra virgin olive oil","olive oil"},
            { "freshly ground black pepperl","black pepper"},
            { "hard eggs","boiled eggs"},
            { "hard boiled eggs","boiled eggs"},
            { "pork butt","pork shoulder blade"},
            { "bay leaves", "bay leaf"},
            { "beef mince", "ground beef"},
            { "scalions","green onion" },
            { "corn and ola oil","corn oil" },
            { "chocolate chips", "sweet chocolate candies"},
            { "cocoa powder","cocoa dry powder" },
            { "smoked paprika","paprika" },
            { "vanilla essence", "vanilla extract"},
            { "cilantro", "coriander" },
            { "skinless, boneless chicken","chicken"},
            { "boneless, skinless chicken","chicken"},
            { "boneless skinless chicken","chicken"},
            { "boneless chicken","chicken"},
            { "fresh mozzarella, cut into small cubes", "mozzarella" },
            { "greek-style", "greek" },
            { "cooking oil", "salad or cooking oil" },
            { "package elbow macaroni", "macaroni"},
            { "dijon mustard", "mustard"},
            { "breadcrumbs", "bread crumbs" },
            { "bread cubes", "bread crumbs" },
            { "can anchovy fillets", "anchovy"},
            { "skim", "nonfat"},
            { "bell pepper", "pepper" },
            { "black olives", "ripe olives" },
            { "pepperoni sausage", "pepperoni" },
            { "genoa", "italian" },
            { "zest", "peel" },
            { "white sugar","granulated sugar"},
            { "whole wheat flour","whole-grain wheat flour"},
            { "dry milk powder","dry milk"},
            { "heavy cream","cream heavy whipping"},
            
            { "marsala wine","sweet wine"},
            { "port wine","sweet wine"},
            { "white vinegar","distilled vinegar"},
            { "ketchup","catsup"},
            { "sirloin steak","top sirloin steak"},
            { "white wine vinegar","red wine vinegar"},           
            { "prime rib", "rib eye" },
            { "yellow squash", "summer squash"},
            { "whole chicken", "chicken" },
            { "links pork sausage links", "frankfurter pork sausage"},
            { "packages refrigerated crescent roll dough (such as pillsbury&#174;", "pillsbury refrigerated dough"},
            { "salmon fillets", "salmon"},
            { "pepper (any color)", "peppers" },
            { "pumpkin puree", "pumpkin" },
            { "chorizo sausage", "pork sausage" },
            { "half-and-half cream", "half and half cream" },
            { "dry sherry", "dry dessert wine" },
            { "rib celery", "celery" },
            { "curry leaves", "curry powder" },
            { "icing sugar" ,"powdered sugars"},
            { "sugar powder","powdered sugars"  },
            { "confectioners' sugar", "granulated sugars" },
            { "french bread", "french or vienna bread" },
            
            { "parmigiano-reggiano cheese", "parmesan cheese" },
            { "low-fat cottage cheese", "cottage 1% milkfat|cheese" },
            { "green chile peppers", "green peppers" },
            { "canola oil", "corn and canola oil" },
            { "unsweetened applesauce", "applesauce" },
            { "whole wheat pastry flour", "wheat flour" },
            { "barbeque", "barbecue" },
            { "toast", "toasted bread"},
            { "smoked ham", "cured ham"},
       
            { "breakfast muffins", "wheat bran muffins"}
        };
        public static List<string> ActionInfo = new List<string> {
            "thin sliced","peeled and diced","thick sliced", "sliced","slivered","melted","sifted", "ground", "shredded",
            "cubed", "rolled","mashed","crushed"  ,"boiled","grated","julienne",
            "finely cooked","fine minced","finely chopped","medium dice", "small dice","finely diced","diced", "dried minced", "minced", "finely chopped", "chopped",
            "finely shredded","cooked","finely crushed","frozen","blended","chiffonade","to taste"
        };

        public static List<string> EndActionInfo = new List<string> {
            "cut into"
        };

        public static List<string> ConnectingWords = new List<string> {
           "an","a","of"
        };
        public static List<string> StateInfo = new List<string> {
          "canned", "cans","can","some","fillets", "prepared", "package","as needed",
           "packages","blanched","florets","steel cut", "peeled","small dice","drained",
           "sprig", "sprigs","loaf", "head", "heads",  "stalk", "stalks",
           "grind", "ripe", "steaks", "cold", "warm", "fresh","freshly",
          "(optional)", "asiago",  "panko" , "steaks", "- peeled","halves",
            "for frying","for frying pan", "herb-seasoned", "solid pack",
        };

        public static List<string> SkipHeadlines = new List<string> {// 
            "topping","for the","---","~~~"
        };
        public static List<string> SkipHeadlinesFullNames = new List<string> {// 
            "dressing","seasonings","seasoning", "filling","ingredients","dressing:"
        };
    }
}
