using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitDB {
    public class CommonValidator {


        public static List<string> PeelingOptions = new List<string>{"meat only", "skin", "peel" };
        public static List<string> AdditionalFoodsData = new List<string>() {
            "egg","nuts","fruit", "vegetables", "milk","butter","margarine",
            "green chilies","onions","mushrooms","herbs and cheese","dill"
        };

        public static List<string> AdditionalHealthData = new List<string>{
            "dietetic","unenriched","gluten","enriched","glucose","vitamin A","lactose", "salted",
            "sugared","protein","vitamin D","concentrate","sucralose","sugar","undiluted","calcium",
            "sulfured","ascorbic acid","sodium","salt","water","sweeten","original seasoning"
        };

        private static List<string> ConnectWordOptions = new List<string>{
          "free", "reduced","per","with","without","added","in","and","using","or","from","low","reduced"
        };
        public static List<string> PackOptions = new List<string>{
           "condensed","pressurized","syrup pack","drained solids","all styles", "regular pack", "brine pack", "vacuum pack",
           "drained", "jellied","juice pack","water","cream style" ,"in oil", "solids and liquids" };

        public static List<string> CookingOptions = new List<string>(){
            "cultured","poached","whipped","pasteurized","candied","sauteed", "breaded","steamed","fried","heated",
             "boiled", "grilled", "pan-browned", "crumbles", "pickled", "microwave", "baked", "pickled", "roasted",
             "oven","broil","scalloped", "raw", "braised","stewed","rotisserie","BBQ","simmered","glazed","cooked",
            "prepared","batter", "flour"
        };

        public static List<string> StorageOptions = new List<string>{
            "fresh", "cured","moisture","chilled","ripe", "bottled", "hydrated","dry","sprouted","hydrated",
            "seasoned","freshly harvest", "stored","refrigerated","exposed to ultraviolet light",
            "dried", "ground", "pre-sliced", "frozen", "canned", "as purchased"
        };
       
        public static List<string> FatOptions = new List<string>{
            "milkfat","nonfat","lowfat","nonfat", "lean","fat"
        };

        public static List<string> BoneOptions = new List<string>{ "bone removed", "bone-in", "boneless" };
        public static bool IsCommonParameter(string part) {
            part = part.Trim();
            return (CookingOptions.Any((item) => part.Contains(item)) ||
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
