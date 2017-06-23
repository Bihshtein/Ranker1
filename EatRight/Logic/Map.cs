using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic {
    public class Map {

        public static string AdjustRelativeWeight(string origWeight)
        {
            foreach (var entry in WeightAdjustment)
            {
                if (entry.Key.Equals(origWeight))
                {
                    return entry.Value;
                }
            }

            return origWeight;
        }

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
                if (list.All(word => WordCheck(word, item)))
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
            if (ingredient == string.Empty)
                return "";
            ingredient = ingredient.ToLower();
            if (ingredient.StartsWith(") "))
                ingredient = ingredient.Replace(") ", "");

            if (ingredient.Length > 0 && ingredient[ingredient.Length - 1] == ')') {
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
            
            // Remove "or" that are left after removing words
            if (ingredient.StartsWith("or "))
            {
                ingredient = ingredient.Substring(3);
            }

            // If we have an example in the ingredient name ("some fruit such as apple") we want this example
            var exampleParts = ingredient.Split(new string[] { "such as" }, StringSplitOptions.None);
            if (exampleParts.Length > 1 && exampleParts[1].Length > 0)
            {
                ingredient = exampleParts[1];
            }

            return ingredient;
        }

        public static string AdjustInnerPart(string innerpart) {
            var split = innerpart.Split(',');
            if (split[0] != string.Empty)
            {
                innerpart = split[0];
            }
            else
            {
                innerpart = split[1];
            }
            split = innerpart.Split(new string[1] { " - " }, StringSplitOptions.None);
            if (split[0] != string.Empty) {
                innerpart = split[0];
            }
            else {
                innerpart = split[1];
            }
            return innerpart;
        }

        public static Dictionary<string, string> RecipeToUSDAMeasure = new Dictionary<string, string> {
            { "cup sliced","cup chopped" },
            { "cup","glass" },
            { "rib","strip" },
            { "chicken breasts","piece" },
            { "chicken breast","piece" },
            { "salmon steak","fillet" },
            { "chicken breast halve","piece" },
            { "chicken breast halves","piece" },
            { "cornish game hens", "bird"},
            { "teaspoon", "tsp"},
            { "tablespoon", "tbsp"},
        };


        public static Dictionary<string, Tuple<string,double>> MeasureToMeasure = new Dictionary<string, Tuple<string, double>> {
            { "tsp", new Tuple<string,double>("tbsp",0.5)},
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
            { new List<string> {"flour"},new Tuple<List<string>,string>(new List<string> {"whole","coconut","almond", "tortilla"}, "all-purpose wheat flour")},
            { new List<string> {"pasta"},new Tuple<List<string>,string>(new List<string> {"whole","grain"}, "pasta")},
            { new List<string> {"onion"},new Tuple<List<string>,string>(new List<string> {"soup", "dehydrated", "scallion","spring","green","scalions","yellow"}, "onion")},
            { new List<string> {"onions"},new Tuple<List<string>,string>(new List<string> {"soup", "dehydrated", "scallion","spring","green","scalions","yellow"}, "onion")},

            { new List<string> {"lettuce"},new Tuple<List<string>,string>(new List<string> { "iceberg", "romain",}, "lettuce")},
        };
        public static Dictionary<List<string>, string> WordsMap = new Dictionary<List<string>, string>() {
               { new List<string> {"pillsbury","dough"},  "pillsbury dough"},
               { new List<string> {"egg", "substitute" }, "egg substitute"},
               { new List<string> {"pancake","mix" }, "pancakes dry mix"},
               { new List<string> {"almond","milk" }, "almond milk"},
               { new List<string> {"jam"}, "jam"},
               { new List<string> {"turmeric"}, "turmeric"},
               { new List<string> { "capsicum"}, "hot chili green peppers"},
               { new List<string> {"green","chiles" }, "hot chili green peppers"},
               { new List<string> {"green","chili" }, "hot chili green peppers"},
               { new List<string> {"red","chili" }, "hot chili red peppers"},
               { new List<string> {"red","chilli" }, "hot chili red peppers"},
               { new List<string> {"red","chiles" }, "hot chili red peppers"},
               { new List<string> {"green","chilies" }, "hot chili green peppers"},
               { new List<string> {"green","chile" }, "hot chili green peppers"},
               { new List<string> {"green","chilli" }, "hot chili green peppers"},
               { new List<string> {"mustard" }, "prepared mustard"},// can be also dry mustard in the case the ration is 1tsp(dry)=1tbsp(prepared)
               { new List<string> {"prosciutto"}, "cured ham"},
               { new List<string> {"bacon"}, "bacon"},
               { new List<string> {"rocket"}, "arugula"},
               { new List<string> {"sugar","substitute"}, "sugar substitute"},
               { new List<string> {"splenda", "granular"}, "sugar substitute"},
               { new List<string> {"artificial", "sweetener"}, "sugar substitute"},
               { new List<string> {"stevia"}, "sugar substitute"},
               { new List<string> {"pancetta"}, "bacon"},
               { new List<string> {"celery"}, "celery"},
               { new List<string> {"pizza", "dough"}, "biscuits dough"},
               { new List<string> {"pizza", "dough"}, "biscuits dough"},
               { new List<string> {"noodles"}, "noodles"},
               { new List<string> {"bisquick", "baking", "mix"}, "biscuits dough"},
               { new List<string> {"berries"}, "blue berries"},
               { new List<string> {"brown", "sugar"}, "brown sugar"},
               { new List<string> {"demerara", "sugar"}, "brown sugar"},
               { new List<string> {"muscovado sugar"}, "brown sugar"},
               { new List<string> {"marshmallows"}, "marshmallows"},
               { new List<string> {"tomatoes"}, "tomatoes"},
               { new List<string> {"roma","tomato"}, "tomatoes"},
               { new List<string> {"cherry","tomato"}, "tomatoes"},
               { new List<string> {"popcorn"}, "popcorn"},
               { new List<string> {"scallion"}, "green onion"},
               { new List<string> {"chicken", "breast" }, "chicken breast"},
               { new List<string> {"salt" }, "table salt"},// very hardcore, should exclude thigs like :"with salt"
               { new List<string> {"butter" }, "butter"},// very hardcore, should exclude peanut butter
               { new List<string> {"parsley" }, "parsley"},
               { new List<string> { "breakfast", "sausage" } ,"pork sausage" },
               { new List<string> { "ground", "beef" }, "ground beef"},
               { new List<string> { "hamburger", "meat" }, "ground beef"},
               { new List<string> { "water",  }, "water"},
               { new List<string> { "honey",  }, "honey"},
               { new List<string> { "ice", "cubes" }, "water"},
               { new List<string> { "basil",  }, "dried basil"},
               { new List<string> { "spring", "onion" }, "green onion"},
               { new List<string> { "spinach",  }, "spinach"},
               { new List<string> { "bread crumbs",  }, "bread crumbs"},
               { new List<string> { "vegetables", "oil",  }, "olive oil"},
               { new List<string> { "olive", "oil", }, "olive oil"},
               { new List<string> { "evoo" }, "olive oil"},
               { new List<string> { "olives",  }, "olives"},
               { new List<string> { "chia",}, "chia seeds"},
               { new List<string> { "jalapeֳ±o", }, "jalapeno"},
               { new List<string> { "pomelo", }, "pumello"},
               { new List<string> { "vermicelli", }, "pasta"},
               { new List<string> { "coconut", "milk", }, "coconut milk"},
               { new List<string> { "penne", }, "pasta"},
               { new List<string> { "orzo", }, "pasta"},
               { new List<string> { "anise", }, "anise seed"},
               { new List<string> { "yoghurt", }, "yogurt"},
               { new List<string> { "spaghetti", }, "pasta"},
               { new List<string> { "orzo", }, "pasta"},
               { new List<string> { "cheerios", }, "cheerios"},
               { new List<string> { "rigatoni", }, "pasta"},
               { new List<string> { "cumin",}, "cumin seed"},
               { new List<string> { "mayo"}, "mayonnaise"},
               { new List<string> { "mint"}, "peppermint"},
               { new List<string> { "prawn"}, "shrimp"},
               { new List<string> { "oats"}, "oats"},
               { new List<string> { "hard","eggs"}, "boiled eggs"},
               { new List<string> { "crescent","rolls"}, "dinner rolls"},
               { new List<string> { "buns"}, "dinner rolls"},
               { new List<string> { "crescent","roll","dough"}, "sweet rolls dough"},
               { new List<string> { "tofu"}, "tofu"},
               { new List<string> { "stick","cinnamon"}, "ground cinnamon"},
               { new List<string> { "sticks","cinnamon"}, "ground cinnamon"},
               { new List<string> { "soy", "sauce"}, "soy sauce"},
               { new List<string> { "artichoke"}, "artichoke"},
               { new List<string> { "orange","rind"}, "orange peel"},
               { new List<string> { "orange","peel"}, "orange peel"},
               { new List<string> { "lemon","rind"}, "lemon peel"},
               { new List<string> { "lime","rind"}, "lemon peel"},
               { new List<string> { "lime","peel"}, "lemon peel"},
               { new List<string> { "lemon", "peel"}, "lemon peel"},
               { new List<string> { "apple","cider"}, "apple juice"},
               { new List<string> { "seaweed"}, "seaweed"},
               { new List<string> { "mozzarella"}, "mozzarella cheese"},
               { new List<string> { "mozarella"}, "mozzarella cheese"},
               { new List<string> { "shrimp"}, "shrimp"},
               { new List<string> { "mushroom","soup"}, "cream mushroom soup"},
               { new List<string> { "agave"}, "agave syrup"},
               { new List<string> { "chicken", "thighs",  }, "chicken thighs"},
               { new List<string> { "marinara", "sauce",  }, "pasta sauce"},
               { new List<string> { "pepperjack", "cheese",  }, "monterey cheese"},
               { new List<string> { "colby ", "cheese",  }, "colby  cheese"},
               { new List<string> { "havarti", "cheese",  }, "monterey cheese"},
               { new List<string> { "monterey", "cheese",  }, "monterey cheese"},
               { new List<string> { "cheddar"}, "cheddar cheese"},
               { new List<string> { "chedder","cheese"}, "cheddar cheese"},
               { new List<string> { "velveeta"}, "cheddar cheese"},
               { new List<string> {"black", "pepper" },"black pepper"},
               { new List<string> {"peppercorns" },"black pepper"},
               { new List<string> { "whipping", "cream" },"cream heavy"},
               { new List<string> { "double", "cream" },"cream heavy"},
            { new List<string> { "cilantro"},"raw coriander"},
            { new List<string> { "fresh", "coriander" },"raw coriander"},
            { new List<string> { "coriander", "leaves" },"raw coriander"},
            { new List<string> { "dry", "yeast" },"active dry yeast"},
            { new List<string> { "sweet", "chocolate","candies" },"sweet chocolate candies"},
            { new List<string> { "corn", "muffin", "mix" },"corn muffins dry mix"},
            { new List<string> { "savoy", "cabbage"},"savoy cabbage"},
            { new List<string> {"sirloin"},"top sirloin"},
            { new List<string> { "powdered", "milk"},"dry milk"},
            { new List<string> { "powder", "milk"},"dry milk"},
            { new List<string> { "granola"},"granola bars"},
            { new List<string> { "low-fat", "milk"},"lowfat milk"},
            { new List<string> { "1%", "milk"},"lowfat milk"},
            { new List<string> { "2%", "milk"},"lowfat milk"},
            { new List<string> { "thyme", "leave"},"fresh thyme"},
            { new List<string> { "thyme", "leaf"},"fresh thyme"},
            { new List<string> { "thyme", "leaves"},"fresh thyme"},
            { new List<string> { "thyme", "dried"},"dried thyme"},
            { new List<string> { "wine", "vinegar"},"red wine vinegar"},
            { new List<string> { "confectioners%27", "sugar"},"powdered sugars"},
            { new List<string> { "caster", "sugar"},"powdered sugars"},
            { new List<string> { "icing", "sugar"},"powdered sugars"},
            { new List<string> { "dried", "sage"},"ground sage"},
            { new List<string> { "kaiser", "rolls"},"hard rolls"},
            { new List<string> { "sriracha"},"hot chile sriracha"},
            { new List<string> { "harissa"},"hot chile sriracha"},
            { new List<string> { "cooking","spray"},"cooking spray"},
            { new List<string> { "lemon%2c"},"concentrate lemon juice"},
            { new List<string> { "lime%2c"},"concentrate lemon juice"},
            { new List<string> { "lemons%2c"},"concentrate lemon juice"},
            { new List<string> { "limes%2c"},"concentrate lemon juice"},
            { new List<string> { "orange%2c"},"concentrate orange juice"},
            { new List<string> { "oranges%2c"},"concentrate orange juice"},
            { new List<string> { "chili","sauce"},"tomato chili sauce"},
            { new List<string> { "corn", "kernels"},"corn whole kernel"},
            { new List<string> { "baguette"},"french or vienna bread"},
            { new List<string> { "baguettes"},"french or vienna bread"},
            { new List<string> { "maple", "syrup"},"maple syrup"},
            { new List<string> { "vanilla", "extract"},"vanilla extract"},
            { new List<string> { "ranch", "dressing"},"ranch dressing"},
            { new List<string> { "flax", "seed"},"flax seed"},    
            { new List<string> { "thousand","island" },"thousand island"},
            { new List<string> { "creme", "fraiche" },"cream sour"},
            { new List<string> { "crֳ¨me", "fraֳ®che" },"cream sour"},
            { new List<string> { "ginger", "root" },"ginger root"},
            { new List<string> { "courgettes"},"zucchini"},
            { new List<string> { "courgette"},"zucchini"},
            { new List<string> { "lemon", "juice"},"lemon juice"},
            { new List<string> { "lime", "juice"},"lemon juice"},
            { new List<string> { "sherry", "vinegar"},"red wine vinegar"},
            { new List<string> { "lamb"},"lamb"},
            { new List<string> { "parmesan"},"parmesan"},
            { new List<string> { "almonds"},"almonds"},
            { new List<string> { "natural","yogurt"},"plain yogurt"},
            { new List<string> { "basmati","rice"},"white long-grain rice"},
            { new List<string> { "risotto","rice"},"white medium-grain rice"},
            { new List<string> { "chicken","stock"},"chicken stock"},
            { new List<string> { "breadcrumb"},"bread crumbs"},
            { new List<string> { "panko"},"bread crumbs"},
            { new List<string> { "stew","meat"},"chuck roast"},
            { new List<string> { "stew","beef"},"chuck roast"},
            { new List<string> { "passata"},"tomato puree"},
            { new List<string> { "tomato", "purֳ©e"},"tomato puree"},
            { new List<string> { "beetroot"},"beets"},
            { new List<string> { "groundnut", "oil"},"peanut oil"},
            { new List<string> { "vanilla", "pod"},"vanilla extract"},
            { new List<string> { "full-fat", "milk"},"milk"},
            { new List<string> { "rotisserie", "chicken"},"roasted chicken"}, 
        };
        public static Dictionary<string, string> ChangePartialPhrases = new Dictionary<string, string>() {
             { "low fat","lowfat"},
            { "chilli powder", "chili powder"},
            { "worchestershire","worcestershire"},
            { "long grain rice","long-grain rice"},
             { "aubergine","eggplant"},
             { "new potato","potato"},
             { "sweetcorn","sweet corn"},
            { "low-fat" ,"lowfat"},
             { "fat-free" ,"nonfat"},
             { "fat free" ,"nonfat"},
            { "kabocha squash","winter squash"},
            { "roasted red pepper", "sauteed red pepper"},
            {"bean sprouts", "sprouted alfalfa seeds "},
            { "hashbrowns","hash browns"},
            { "dry yeast","active dry yeast"},
            { "yellow squash", "summer squash"},
            { "tater tots", "fried potatoes"},
            { "dill weed", "dill"},
            { "baking potatoes", "baked potatoes"},
            { "hard-cooked", "hard-boiled"},
            { "fresh lemon juice", "raw lemon juice"},
            { "oatmeal", "oats"},
            { "dark rum", "rum"},
            { "jeera", "cumin seeds"},
            { "cashews", "cashew nuts"},
            { "deli ham", "sliced ham"},
            { "kilishi","beef jerky" },
            { "light soy sauce", "soy sauce"},
            { "button mushroom", "white mushroom"},
            { "sliced mushrooms", "white mushroom"},
            { "prawn", "shrimp"},
            { "iceberg lattuce", "iceberg lettuce"},
            { "romain lettuce", "romaine lettuce"},
            { "beef hotdogs", "frankfurter"},
            { "spicy chorizo","chorizo"},
            { "pot roast", "arm pot roast"},
            { "chihuahua cheese", "queso chihuahua cheese"},
            { "pork roast", "pork loin roast"},            
            { "pork butt","pork shoulder blade"},
            { "bay leaves", "bay leaf"},
            { "beef mince", "ground beef"},
            { "scalions","green onion" },
            { "corn and ola oil","corn oil" },
            { "coriander powder","dried coriander leaf"},
            { "chocolate chips", "sweet chocolate candies"},
            { "cocoa powder","cocoa dry powder" },
            { "smoked paprika","paprika" },
            { "vanilla essence", "vanilla extract"},
            { "skinless, boneless chicken","chicken"},
            { "boneless, skinless chicken","chicken"},
            { "boneless skinless chicken","chicken"},
            { "boneless chicken","chicken"},
            { "greek-style", "greek" },
            { "cooking oil", "salad or cooking oil" },
            { "elbow macaroni", "macaroni"},
            { "dijon mustard", "mustard"},
            { "breadcrumbs", "bread crumbs" },
            { "bread cubes", "bread crumbs" },
            { "can anchovy fillets", "anchovy"},
            { "skim", "nonfat"},
            { "bell pepper", "pepper" },
            { "orange pepper", "pepper" },
            { "pepperoni sausage", "pepperoni" },
            { "genoa", "italian" },
            { "zest", "peel" },
            { "flaxseed", "flax seed" },
            { "gingerroot", "ginger root" },
            { "white sugar","granulated sugar"},
            { "raw sugar","granulated sugar"},
            { "whole wheat flour","whole-grain wheat flour"},
            { "dry milk powder","dry milk"},
            { "rice vinegar","cider vinegar"},
            { "marsala wine","sweet wine"},
            { "port wine","sweet wine"},
            { "white vinegar","distilled vinegar"},
            { "ketchup","catsup"},
            { "tomato catsup","catsup"},
            { "sirloin steak","top sirloin steak"},
            { "prime rib", "rib eye" },
            { "whole chicken", "chicken" },
            { "links pork sausage links", "frankfurter pork sausage"},
            { "salmon fillets", "salmon"},
            { "pepper (any color)", "peppers" },
            { "pumpkin puree", "pumpkin" },
            { "chorizo sausage", "pork sausage" },
            { "half-and-half cream", "half and half cream" },
            { "dry sherry", "dry dessert wine" },
            { "rib celery", "celery" },
            { "curry leaves", "curry powder" },           
            { "sugar powder","powdered sugars"  },
            { "confectioners' sugar", "granulated sugars" },
            { "french bread", "french or vienna bread" },
            { "crusty bread", "french or vienna bread" },
            { "half-and-half","half and half" },
            { "sourdough bread", "french or vienna bread" },
            { "parmigiano-reggiano cheese", "parmesan cheese" },
            { "low-fat cottage cheese", "cottage 1% milkfat|cheese" },          
            { "canola oil", "corn and canola oil" },
            { "unsweetened applesauce", "applesauce" },
            { "whole wheat pastry flour", "wheat flour" },
            { "barbeque", "bbq" },
            { "barbecue", "bbq" },
            { "toast", "toasted bread"},
            { "sandwich bread", "white bread"},
            { "garbanzo beans", "chickpeas"},
            { "smoked ham", "cured ham"},          
            { "crabmeat","crab"},
            { "breakfast muffins", "wheat bran muffins"}
        };
        public static List<string> ActionInfo = new List<string> {
            "thin sliced","peeled and diced","thick sliced", "sliced","slivered","melted","sifted", "ground", "shredded",
            "cubed", "rolled","mashed","crushed","boiled","grated","julienne","julienne-sliced",
            "finely cooked","fine minced","finely chopped","medium dice", "small dice","finely diced","diced", "dried minced", "minced", "finely chopped", "chopped",
            "finely shredded","cooked","finely crushed","frozen","blended","chiffonade","to taste",
            "drained", "softened", "shelled"
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
           "sprig", "sprigs","loaf", "bulk","refrigerated",
           "grind", "ripe", "steaks", "cold", "warm", "fresh","freshly",
          "(optional)", "asiago", "steaks", "- peeled","halves",
            "for frying","for frying pan", "herb-seasoned", "solid pack","beaten"
        };

        public static List<string> SkipHeadlines = new List<string> {// 
            "topping","for the","---","~~~"
        };
        public static List<string> SkipHeadlinesFullNames = new List<string> {// 
            "dressing","seasonings","seasoning", "filling","ingredients","dressing:"
        };

        public static List<string> Adjectives = new List<string>
        {
            "dark", "light", "green", "red", "french"
        };

        public static Dictionary<string, string> WeightAdjustment = new Dictionary<string, string>
        {
            { "g", "gram" }
        };
    }
}
