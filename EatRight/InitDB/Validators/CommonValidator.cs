using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitDB {
    public class CommonValidator {


        private static List<string> PeelingOptions = new List<string>() { "skin", "peel" };
        private static List<string> AdditionalProductsOptions = new List<string>() {
        "glucose","vitamin A", "lactose", "salted","sugared","nuts","fruit", "vegetables", "protein","lowfat milk","nonfat milk","skim milk","whole milk", "vitamin D","concentrate","sucralose","sugar", "undiluted","calcium", "sulfured", "ascorbic acid","sodium", "milk","salt","water","butter","margarine","green chilies","onions","mushrooms","herbs and cheese","dill","artificial sweetener","sweetened","unsweetened"
        };

        private static List<string> ConnectWordOptions = new List<string>() {
            "reduced","per","with","without","added","in","and","using","or","from","low","reduced"
        };
        private static List<string> PackOptions = new List<string>()
            {"condensed","pressurized","extra heavy syrup", "heavy syrup", "extra light syrup","light syrup","drained", "puree","paste","sauce" , "in tomato juice","all styles", "regular", "brine", "vacuum",
            "jellied","juice","water","cream style" ,"syrup","in oil","mixed", "solids and liquids", "drained solids"};

        public static List<string> CookingOptions = new List<string>() 
            {"cultured","poached","whipped","pasteurized process","pasteurized","raw or frozen","uncooked","french fried","home-prepared","or baked", "oven heated","pan-fried in canola oil","pan fried in canola oil","candied","as purchased","oven-heated","sauteed", "breaded","stir-fried","steamed","fried", "slow roasted",
             "boiled", "fast roasted", "fast fried", "grilled", "pan-browned", "crumbles",
             "par fried", "pickled", "microwaved","microwave", "baked", "pickled", "roasted", "pan-broiled", "pan-broil",
             "heated in oven" ,"scalloped", "broiled", "raw", "pan-fried", "braised","stewed","rotisserie","BBQ","simmered","glazed", "cooked simmered","heated (conventional oven)","heated (microwave)"};

        public static List<string> StorageOptions = new List<string>() 
        {"low moisture","chilled","riped","ripe", "bottled","dehydrated (low-moisture)","dehydrated (low moisture)","canned or bottled","frozen concentrate","rehydrated","dry","sprouted","dehydrated","seasoned","freshly harvest", "stored" ,"dry mix","dry form"
        ,"refrigerated","exposed to ultraviolet light","sun-dried", "dried","frozen as packaged","freeze-dried","unprepared","prepared", "ground", "pre-sliced", "frozen", "cooked", "canned", "heated", "unheated" };
        public static List<string> FatOptions = new List<string>() {
        "fat-free","fat free","3.7% milkfat","3.25% milkfat","dietetic,","1% fat","2% fat","nonfat or fat free","fat free","1% milkfat","2% milkfat" ,  "reduced fat", "low fat","non-fat", "lowfat","nonfat", "97% lean meat / 3% fat", "70% lean meat / 30% fat", "85% lean meat / 15% fat", "93% lean meat /7% fat", "93% lean meat / 7% fat", "97% lean meat /3% fat", "95% lean meat / 5% fat", "93% lean meat / 7% fat", "90% lean meat / 10% fat", "80% lean meat / 20% fat", "75% lean meat / 25% fat", "72% lean / 28% fat", "84% lean / 16% fat", "96% lean / 4% fat", "extra lean", "separable fat", "extra lean (approximately 5% fat)", "regular (approximately 13% fat)", "regular (approximately 11% fat)", "extra lean (approximately 4% fat)", "extra lean and regular", "rendered fat", "separable lean and fat", "separable lean only" };
        public static List<string> BoneOptions = new List<string>() { "bone removed", "bone-in", "boneless" };
        public static List<string> PreservationOptions = new List<string>() { "fresh", "cured" };
        public static List<string> FryingOptions = new List<string>() { "barbecue flavored", "batter", "flour" };
        public static bool IsCommonParameter(string part) {
            part = part.Trim();
            return (CookingOptions.Contains(part) ||
                    StorageOptions.Contains(part) ||
                    FatOptions.Contains(part) ||
                    PreservationOptions.Contains(part) ||
                    BoneOptions.Contains(part) ||
                    FryingOptions.Contains(part) ||
                    (PackOptions.Contains(part) ||(PackOptions.Any((item) => part.Contains(item) && part.Contains("pack"))))||
                    CheckWithWord(PeelingOptions, part) ||
                    CheckWithWord(AdditionalProductsOptions, part) ||
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
