using InitDB.Validators;
using System.Collections.Generic;

namespace InitDB.Validators {
    public class VegsValidator : BasicValidator {
        public override bool IsValidPart(string part) {
            part = part.Trim();
            var aleg = ((part != part.ToLower()) ||
                        StorageOptions.Contains(part) ||
                        ServingOptions.Contains(part) ||
                        PreparationOptions.Contains(part) ||
                        PodsOptions.Contains(part) ||
                        CutOptions.Contains(part) ||
                        ColorsOptions.Contains(part) ||
                        PackOptions.Contains(part) ||
                        PeelingOptions.Contains(part) ||
                        PlacesOptions.Contains(part) ||
                        PlacesOptions.Contains(part) ||
                        PotatoesCookingOptions.Contains(part) ||
                        TomatoStuff.Contains(part)||
                        SeasonOptions.Contains(part)||
                        TasteOptionsOptions.Contains(part) ||
                        PickleOptions.Contains(part) ||
                         base.IsValidPart(part));
            return aleg;

        }
        public VegsValidator() {
            SecondParts = new List<string>() {
 "scallop","crookneck and straightneck","all varieties", "zucchini","acorn","au gratin","cucumber", "chowchow","hamburger","hot dog", "serrano", "pasilla","jalapeno","hot pickled","hot-chile",
       "year round average", "butternut", "hubbard",  "spaghetti",  "russet",  "o'brien", "banana", "green (includes baby and lesuer types)", "young green",
      "(skunk cabbage)",   "(corn and limas)", "(vegetable oyster)","agar", "white icicle",  "oriental" ,"hawaiian style", "yellow fleshed", "spring or scallions (includes tops and bulb)", "(tendergreen)",
         "hot chile",    "ripe",   "(bulb and lower leaf-portion)","(dried gourd strips)", "(pepeao)",
          "italian style",  "kelp",  "kave","laver","spirulina", "wakame", "flowers","white-flowered (calabash)",
                "dishcloth (towelgourd)", "garden", "mustard", "(globe or french)", "snap", "harvard","flower clusters","stalks",
              "irishmoss",  "common","stalks","(fuki)", "red leaf","iceberg (includes crisphead types)",
                "green leaf", "cos or romaine","butterhead (includes boston and bibb types)",
                "potherb","ancho","(poke)", "hot chili","edible-podded", "common (danish, domestic, and pointed types)",
              "(chinese preserving melon)",  "enoki", "maitake","morel","oyster","straw", "shiitake", "portabella","fruit",
             "(matai)", "root",  "witloof","baby","garland",  "napa","savoy", "kimchi",
                "chinese (pak-choi)","chinese (pe-tsai)",
            "kidney", "fava","mung","navy","pinto","shellie","chili"};
        }
        private static List<string> TasteOptionsOptions = new List<string>() {
            "sour","sweet"
        };
        private static List<string> PickleOptions = new List<string>() {
            "dill or kosher dill","dill"
        };
        private static List<string> TomatoStuff = new List<string>() {"puree","paste","sauce" };
        private static List<string> SeasonOptions = new List<string>() {"summer","winter" };
        private static List<string> PlacesOptions = new List<string>() 
        { "tahitian", "hungarian","scotch","chinese", "japanese style", "swiss", "italian, or crimini",
            "italian","fordhook","hawaii","hawaiian style", };
        private static List<string> PeelingOptions = new List<string>() {"includes skin","peeled","with peel" };
        private static List<string> StorageOptions = new List<string>()
        {"rehydrated","dry","sprouted","dehydrated","seasoned","freshly harvest", "stored" ,"dry mix","dry form",
        "ready-to-eat","refrigerated","exposed to ultraviolet light","home prepared","home-prepared","sun-dried", "dried"};
        private static List<string> ServingOptions = new List<string>()
        {"with butter sauce","salted", "with salt added","without salt added","plain","drained",
            "solids and liquids" ,
         "reduced sodium",   "drained solids","drained without salt","with artificial sweetener",};
        private static List<string> PreparationOptions = new List<string>()
        {"without milk","prepared from granules without milk","prepared from granules with milk",
            "whole milk and margarine added","home-prepared from recipe using margarine",
            "home-prepared from recipe using butter","prepared with water","whole milk and margarine added",
            "water and margarine added","home-prepared with butter", "home-prepared with margarine",
            "whole milk and margarine",
         "whole milk added", "whole milk and butter",  "whole milk and butter added","low sodium",
            "no salt added","with salt", "without salt","salt added in processing",
            "salt not added in processing" };
        private static List<string> PodsOptions = new List<string>()
        {"tops only","(bulb and lower-leaf portion)","cut off cob","excluding seeds","young pods with seeds",
            "kernels","kernels on cob", "bulb","kernels cut off cob", "whole kernel",
            "in pod","immature seeds","shoots", "mature seeds","leaves", "leafy tips", "pods",
            "young pods with seed" };
        private static List<string> CutOptions = new List<string>()
        {"shredded","crushed","crosscut","chopped or leaf","steak cut",
            "shoestring","all types","whole", "chopped", "spears","mashed",
         "crinkle or regular cut","cross cut", "extruded", "cottage-cut" ,"steak fries","wedge cut"};
        private static List<string> PotatoesCookingOptions = new List<string>()
        {"skin with salt","without skin","baked in skin","in skin","prepared from granules","without",
            "prepared from flakes without milk","granules without milk","granules with milk",
           "flakes without milk","skin only", "cooked without skin", "cooked in skin","all-types",
            "flesh and skin","french fried", "skin", "flesh", "young pods with seed" };
        private static List<string> ColorsOptions = new List<string>() 
        {"hash brown","brown", "green", "yellow", "white","orange","red"};
        private static List<string> PackOptions = new List<string>()
        { "with green chilies","with onions","with mushrooms","with herbs and cheese","spanish style"
            ,"packed in tomato juice","all styles", "regular pack", "brine pack", "vacuum pack",
            "cream style" ,"syrup pack","packed in oil","mixed"};
    }
}