using InitDB.Validators;
using System.Collections.Generic;
using System.Linq;

namespace InitDB.Validators {
    public class VegsValidator : BasicValidator {

        public override bool IsMainPart(string part) {
            return part != part.ToLower();
        }



        public VegsValidator() {
            SecondParts = new List<string>() {
        "summer", "winter", "sweet","hash brown","brown", "green", "yellow", "white","orange","red", "tahitian", "hungarian","scotch","chinese", "japanese style", "swiss", "italian, or crimini",
            "italian","fordhook","hawaii","hawaiian style","scallop","crookneck and straightneck","all varieties", "zucchini","acorn","au gratin","cucumber", "chowchow","hamburger","hot dog", "serrano", "pasilla","jalapeno","hot pickled","hot-chile",
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
            "all-types", "chinese (pak-choi)","chinese (pe-tsai)",
            "kidney", "fava","mung","navy","pinto","shellie","chili"};

            Cuts = new List<string>()
            {"flesh","shredded","crushed","crosscut","chopped or leaf","steak cut",
            "shoestring","all types","whole", "chopped", "spears","mashed",
         "crinkle or regular cut","cross cut", "extruded", "cottage-cut" ,"steak fries","wedge cut",
            "tops only","(bulb and lower-leaf portion)","cut off cob","excluding seeds","young pods with seeds",
            "kernels","kernels on cob", "bulb","kernels cut off cob", "whole kernel",
            "in pod","immature seeds","shoots", "mature seeds","leaves", "leafy tips", "pods",
            "young pods with seed"};
        }

    }
}