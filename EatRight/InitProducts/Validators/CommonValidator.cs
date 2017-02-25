using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitProducts {
    public class CommonValidator {

        public static List<string> PeelingOptions = new List<string>{"meat only", "skin", "peel" };

  
        public static List<string> AdditionalFoodsData = new List<string>() {
            "egg","nuts","fruit", "vegetables", "milk","butter","margarine",
            "green chilies","onions","mushrooms","herbs and cheese","dill",
          
        };
        public static List<string> NoiseWords = new List<string>() {
            "Strawberry"
        };

        public static List<string> AdditionalHealthData = new List<string>{
            "reduced-calorie","sweetened","diet","caffeine","decaffeinated","dietetic","unenriched","gluten","enriched","glucose","lactose", "salted",
            "sugared","concentrate","sucralose","sugar","undiluted","calcium","low calorie","vitamin",
            "sulfured","ascorbic acid","sodium","salt","sweeten","original seasoning","aspartame","special dietary",
            "alcohol","no alcohol","unsweetened", "unbleached","unfortified"
        };

        private static List<string> ConnectWordOptions = new List<string>{
          "free", "reduced","per","with","without","added","in","and","using","or","from","low","reduced","no"
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


        public static List<string> StorageOptions = new List<string>{
            "powder","fresh", "cured","moisture","chilled", "bottled", "hydrated","dry","sprouted","hydrated",
            "seasoned","freshly harvest", "stored","refrigerated","exposed to ultraviolet light",
            "shelf stable","dried", "ground", "pre-sliced", "frozen", "canned", "as purchased","ready-to-bake"
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
                    USDA.FoodGroups.Keys.Contains(part)
                    );
        }

        public static bool CheckWithWord(List<string> list, string part) {
            return list.Contains(part) ||
                    (list.Any((option) => part.Contains(option)) &&
                    ConnectWordOptions.Any((option) => part.Contains(option)));
        }
    }
}
