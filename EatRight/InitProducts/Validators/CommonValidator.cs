using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitDB {
    public class CommonValidator {

        public static List<string> PeelingOptionsRaw = new List<string>{"with skin","without skin",
            "american type (slip skin)","Lemon peel","without peel","Orange peel","with peel",
            "flesh and skin","skin only","skin","cooked in skin","cooked without skin","in skin",
            "skin with salt","includes skin","baked in skin","skinless","meat only","back meat and skin",
            "breast meat and skin","drumstick meat and skin","thigh meat and skin","wing meat and skin",
            "meat and skin","meat and skin and giblets and neck",
        };

        public static List<string> PeelingOptions = new List<string>{"meat only", "skin", "peel" };

        public static List<string> AdditionalFoodsDataRaw = new List<string>{"egg","plain or buttermilk",
            "made with margarine","prepared with 2% milk","80% margarine","and eggs","made with low fat (2%) milk",
            "salvadoran sweet cheese (quesadilla salvadorena)","made with nonfat dry milk",
            "butter (includes fresh and frozen)","fruit","butter or sugar","with peanut butter filling",
            "butter","made with butter","peanut butter sandwich","sandwich-type with peanut butter filling",
            "egg and onion","milk","complete (includes buttermilk)","incomplete (includes buttermilk)",
            "prepared with whole milk","whey and milk based","citrus fruit juice drink",
            "Fruit flavored drink containing less than 3% fruit juice","fruit juice drink",
            "fruit punch juice drink","fruit punch-flavor drink","fruit-flavored drink",
            "Lemonade fruit juice drink light","pineapple and grapefruit juice drink","fruit-flavored",
            "fruit flavored","beef and vegetables","prepared with equal volume milk","margarine-like vegetable oil spread",
            "multiuse for non-dairy butter flavor","principal uses popcorn and flavoring vegetables",
            "made with egg","made without egg","with fruit","with vegetables","15 ounce prepared with 1/2 cup milk",
            "made from lowfat milk","with added nonfat milk solids","with added nonfat milk solids and vitamin A and vitamin D",
            "non-butterfat","nonfat milk","lowfat milk","butterhead (includes boston and bibb types)",
            "Peas and onions","dill or kosher dill","dill","sweet (includes bread and butter pickles)",
            "low sodium (includes bread and butter pickles)","whole milk and butter","home-prepared from recipe using butter",
            "home-prepared from recipe using margarine","with butter sauce","flakes without milk",
            "granules with milk","granules without milk","prepared from flakes without milk",
            "whole milk and butter added","whole milk and margarine added","prepared from granules with milk",
            "water and margarine added","prepared from granules without milk","whole milk added",
            "without milk","whole milk and margarine","home-prepared with butter","home-prepared with margarine",
            "with green chilies",
        };

        public static List<string> AdditionalFoodsData = new List<string>() {
            "egg","nuts","fruit", "vegetables", "milk","butter","margarine",
            "green chilies","onions","mushrooms","herbs and cheese","dill",
          
        };

        public static List<string> NoiseWords = new List<string>() {
            "Strawberry"
        };

        public static List<string> AdditionalHealthDataRaw = new List<string>{
            "enriched (includes corn muffin mix)",
            "unenriched (includes corn muffin mix)","gluten-free","enriched","unenriched",
            "protein (includes gluten)","(includes gluten)","reduced-calorie","low sodium",
            "no salt","low sodium no salt","special dietary (includes lemon-flavored)","butter or sugar",
            "sugar free","special dietary","sugar wafers with creme filling","sugar","without salt",
            "saltines","low-sodium","low salt","unenriched (includes apple","sugared or glazed",
            "enriched (includes honey buns)","unenriched (includes honey buns)","without calcium propionate(includes sourdough)",
            "with calcium propionate (includes sourdough)","without calcium propionate (includes sourdough)",
            "sodium aluminum sulfate","made with enriched flour","without added salt","without added calcium",
            "sweetened","unsweetened","fortified with vitamin C","without caffeine","no caffeine",
            "reduced sugar","contains caffeine and sweeteners","no sugar added","fortified with calcium",
            "vitamins A","decaffeinated","half the caffeine","low calorie","with vitamin C added",
            "fortified with vitamins C","sugar-free","with high vitamin C","with vitamin E added",
            "without added sodium","with aspartame","with high vitamin C with other added vitamins",
            "fortified with vitamin E and C","flavored and sweetened","added calcium","vitamin A and vitamin D",
            "with added vitamin C","diet","with added ascorbic acid","sweetened with sodium saccharin",
            "sugar-free with guarana","fortified with vitamins B6 and B12","non-alcoholic",
            "yellow green colored citrus soft drink with caffeine","caffeine free","with calcium",
            "saccharin and corn sweetener","with corn syrup and/or sugar and low calorie sweetener",
            "with added potassium and sodium","reduced sodium","with salt","with salt added",
            "without salt added","with added vitamin D","low calorie or diet","alcohol",
            "no alcohol","dried and salted","salted","with added salt","calcium-fortified",
            "unbleached","without added vitamin D","vitamin D fortified","fortified with vitamin D",
            "lactose reduced","no sodium added","with lauric acid oil and sodium caseinate",
            "glucose reduced","sugared","with added vitamin A and vitamin D","with added vitamin A",
            "with added vitamin D and without added vitamin A","without added vitamin A and vitamin D",
            "calcium reduced","calcium fortified (fat free or skim)","vitamin A and vitamin D (fat free or skim)",
            "with added vitamin A and vitamin D (fat free or skim)","without added vitamin A and vitamin D (fat free or skim)",
            "with added nonfat milk solids and vitamin A and vitamin D","without added vitamin A",
            "with low-calorie sweetener","with low calorie sweetener","sweetened with low calorie sweetener",
            "sweetened with low-calorie sweetener","without added ascorbic acid","diluted with 3 volume water without added ascorbic acid",
            "undiluted","sulfured","with added sugar","without added sugar","without salt (includes USDA commodity)",
            "without added ascorbic acid (includes USDA commodity)","with added vitamin C and calcium",
            "with added ascorbic acid and calcium","with added calcium","with sucralose",
            "Lemon juice from concentrate","includes from concentrate","with added calcium and vitamin D",
            "with added calcium and vitamins A","with sugar","no salt added","drained without salt",
            "with artificial sweetener","low sodium (includes bread and butter pickles)",
            "salt added in processing","salt not added in processing","skin with salt","original seasoning",
            "salt pork",
        };

        public static List<string> AdditionalHealthData = new List<string>{
            "reduced-calorie","sweetened","diet","caffeine","decaffeinated","dietetic","unenriched","gluten","enriched","glucose","lactose", "salted",
            "sugared","concentrate","sucralose","sugar","undiluted","calcium","low calorie","vitamin",
            "sulfured","ascorbic acid","sodium","salt","sweeten","original seasoning","aspartame","special dietary",
            "alcohol","no alcohol","unsweetened", "unbleached",
        };

        private static List<string> ConnectWordOptions = new List<string>{
          "free", "reduced","per","with","without","added","in","and","using","or","from","low","reduced","no"
        };

        public static List<string> PackOptionsRaw = new List<string>{
            "water biscuits","prepared with water",
            "Coconut water","prepared with tap water","Energy Drink with carbonated water and high fructose corn syrup",
            "Gerolsteiner naturally sparkling mineral water","Glacial Natural spring water",
            "prepared with distilled water","water","condensed","prepared with equal volume water",
            "canned in oil","drained solids","fresh water","freshwater","drained solids with bone",
            "canned in water","pressurized","diluted with 3 volume water without added ascorbic acid",
            "diluted with 3 volume water","drained","extra heavy syrup pack","solids and liquids",
            "extra light syrup pack","heavy syrup pack","juice pack","light syrup pack",
            "water pack","solids and liquids (includes USDA commodity red tart cherries",
            "syrup pack","regular pack","all styles","drained without salt","boiled drained",
            "cream style","vacuum pack","brine pack","rinsed with tap water","rinsed in tap water",
            "water and margarine added","with cream style corn","packed in oil","ham -- water added",
            "ham and water product",
        };

        public static List<string> PackOptions = new List<string>{
           "condensed","pressurized","syrup pack","drained solids","all styles", "regular pack", "brine pack", "vacuum pack",
           "drained", "jellied","juice pack","water","cream style" ,"in oil", "solids and liquids" };

        public static List<string> PreperationOptionsRaw = new List<string>{
            "toasted","prepared","baked","flour","raw","glazed","fried","carbonated","roasted",
            "cooked","breaded","moist heat","dry heat","smoked","pickled","boiled","steamed",
            "whipped","pasteurized","cultured","poached","heated","stewed","microwave","grilled",
            "sauteed","scalloped","candied","braised","rotisserie","BBQ","batter","simmered",
            "broiled",
        };

       private static List<string> PreparationOptions = new List<string>(){
         "moist heat", "dry heat", "smoked", "carbonated","cultured","poached","whipped","pasteurized","candied","sauteed", "breaded","steamed","fried","heated",
         "toasted","boiled", "grilled", "pan-browned", "crumbles", "pickled", "microwave", "baked", "pickled", "roasted",
             "oven","broiled","scalloped", "raw", "braised","stewed","rotisserie","BBQ","simmered","glazed","cooked",
            "prepared","batter", "flour",
        };

        public static List<string> StorageOptionsRaw = new List<string>{"refrigerated dough",
            "dry mix","frozen","dry","seasoned","canned","refrigerated","sprouted","made with nonfat dry milk",
            "butter (includes fresh and frozen)","baking powder","active dry","chilled",
            "ready-to-bake","ready-to-bake or -fry","shelf stable","EAS soy protein powder",
            "EAS whey protein powder","powder","frozen concentrate","bottled","dry powdered mix",
            "KELLOGG'S SPECIAL K20 protein powder","chocolate malt powder","Classic Malt powder",
            "Protein powder soy based","Protein powder whey based","Whey protein powder isolate",
            "dry powder","powdered","beef broth or bouillon canned","dry roasted","fresh",
            "dried","ground","chili powder","curry powder","garlic powder","onion powder",
            "cooked (not previously frozen)","moist heat (may have been previously frozen)",
            "raw (may have been previously frozen)","raw (not previously frozen)","canned in oil",
            "fresh water","freshwater","dry heat","striped","dried and salted","dry heat (may have been previously frozen)",
            "canned in tomato sauce","canned in water","fresh-refrigerated","as purchased",
            "dry white","low moisture","dry grated","liquid or frozen","frozen mixture",
            "canned or bottled","dehydrated (low moisture)","dehydrated (low-moisture)",
            "dehydrated","or banana powder","canned)","raw or frozen","ripe","canned (jumbo-super colossal)",
            "canned (small-extra large)","dried (prunes)","freeze-dried","(dried gourd strips)",
            "exposed to ultraviolet light","dehydrated flakes","sun-dried","dry form","cured",
            "pre-sliced",
        };

        public static List<string> StorageOptions = new List<string>{
            "powder","fresh", "cured","moisture","chilled", "bottled", "hydrated","dry","sprouted","hydrated",
            "seasoned","freshly harvest", "stored","refrigerated","exposed to ultraviolet light",
            "shelf stable","dried", "ground", "pre-sliced", "frozen", "canned", "as purchased","ready-to-bake"
        };

        public static List<string> FatOptionsRaw = new List<string>{"higher fat",
            "lower fat","made with low fat (2%) milk","made with nonfat dry milk","low fat",
            "fat-free","low-fat","reduced fat","sodium aluminum sulfate","67-70% fat","80% fat",
            "canola (partially hydrogenated) oil for deep fat frying","KRAFT MIRACLE WHIP FREE Nonfat Dressing",
            "nonfat or fat free","lowfat","1% milkfat","2% milkfat","fat free","nonfat",
            "made from lowfat milk","2% fat","1% fat","calcium fortified (fat free or skim)",
            "with added nonfat milk solids","vitamin A and vitamin D (fat free or skim)",
            "with added vitamin A and vitamin D (fat free or skim)","without added vitamin A and vitamin D (fat free or skim)",
            "37% milkfat","with added nonfat milk solids and vitamin A and vitamin D","325% milkfat",
            "non-butterfat","nonfat milk","lowfat milk","non-fat","separable fat","rendered fat",
            "separable lean and fat","separable lean only","extra lean (approximately 5% fat)",
            "extra lean and regular","regular (approximately 11% fat)","extra lean (approximately 4% fat)",
            "lean and fat","regular (approximately 13% fat)","extra lean","backfat","separable lean and fat only",
            "boneless separable lean only","trimmed to 0\" fat","trimmed to 1/8\" fat","trimmed to 1/4\" fat",
            "small end (ribs 10- 12) separable lean only","trimmed to 1/8\"fat","boneless separable lean and fat",
        };

        public static List<string> FatOptions = new List<string>{
            "milkfat","nonfat","lowfat","nonfat", "lean","fat","low-fat","lower fat","higher fat"
        };

        public static bool IsPreparationOption(string part) {
            return PreparationOptions.Any((item) => part.Contains(item)) &&
                     !NoiseWords.Any((item) => part.Contains(item));
        }


        public static string GetPreparationOption(string part) {
            return PreparationOptions.Find(item => part.Contains(item));
        }


        public static List<string> BoneOptions = new List<string>{ "bone removed", "bone-in", "boneless" };
        public static bool IsCommonParameter(string part) {
            part = part.Trim();
            return ((IsPreparationOption(part)) ||
                    StorageOptions.Any((item) => part.Contains(item)) ||
                    FatOptions.Any((item) =>part.Contains(item)) ||
                    PackOptions.Any((item) => part.Contains(item)) ||
                    BoneOptions.Contains(part) ||
                    CheckWithWord(PeelingOptions, part) ||
                    CheckWithWord(AdditionalFoodsData, part) ||
                    CheckWithWord(AdditionalHealthData, part) ||
                    InitDB.FoodGroups.Keys.Contains(part)
                    );
        }

        public static bool CheckWithWord(List<string> list, string part) {
            return list.Contains(part) ||
                    (list.Any((option) => part.Contains(option)) &&
                    ConnectWordOptions.Any((option) => part.Contains(option)));
        }
    }
}
