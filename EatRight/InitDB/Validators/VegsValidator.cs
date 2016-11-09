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
                        PotatoesCookingOptions.Contains(part) ||
                         base.IsValidPart(part));
            return aleg;
            
        }
        public VegsValidator() {
            SecondParts = new List<string>() {
                "(globe or french)", "snap", "harvard","chinese","flower clusters","stalks",
                "common","stalks","(fuki)", "red leaf","iceberg (includes crisphead types)",
                "green leaf", "cos or romaine","butterhead (includes boston and bibb types)",
                "potherb","scotch","italian, or crimini","ancho","(poke)", "hot chili","edible-podded",
                "enoki", "maitake","morel","oyster","straw", "shiitake", "portabella","sweet","fruit",
                "witloof","baby","garland", "red", "napa","savoy", "kimchi", "japanese style", "swiss",
                "chinese (pak-choi)","chinese (pe-tsai)","common (danish, domestic, and pointed types)",
            "kidney", "fava","mung","navy","pinto","shellie","chili"};
        }

        private static List<string> StorageOptions = new List<string>() {"sprouted","dehydrated","seasoned","freshly harvest", "stored" };
        private static List<string> ServingOptions = new List<string>() {"drained","solids and liquids" ,"drained solids","drained without salt"};
        private static List<string> PreparationOptions = new List<string>() { "no salt added","with salt", "without salt","salt added in processing","salt not added in processing" };
        private static List<string> PodsOptions = new List<string>() {"kernels on cob", "kernels cut off cob", "whole kernel", "in pod","immature seeds", "mature seeds","leaves", "leafy tips", "pods", "young pods with seed" };
        private static List<string> CutOptions = new List<string>() {"chopped", "spears","mashed","crinkle or regular cut","cross cut", "extruded", "cottage-cut" ,"steak fries","wedge cut"};
        private static List<string> PotatoesCookingOptions = new List<string>() { "cooked without skin", "cooked in skin","all-types", "flesh and skin","french fried", "skin", "flesh", "young pods with seed" };
        private static List<string> ColorsOptions = new List<string>() {"hash brown","brown", "green", "yellow", "white" };
        private static List<string> PackOptions = new List<string>() { "all styles", "regular pack", "brine pack", "vacuum pack", "cream style" };
    }
}