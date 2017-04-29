﻿using System.Collections.Generic;
using System.Linq;
using System;

namespace InitProducts {
    public class VegsValidator : BasicValidator {


        public VegsValidator() {
            SecondParts = new List<string>() {
                "hash brown","brown", "green", "yellow", "white","orange","red", "tahitian",
                "hungarian","scotch","chinese", "japanese style", "swiss", "italian, or crimini","paste",
                "italian","fordhook","hawaii","hawaiian style","scallop","crookneck and straightneck",
                "all varieties", "zucchini","acorn","au gratin","cucumber", "chowchow","hamburger","hot dog",
                "serrano", "pasilla","jalapeno","hot pickled","hot-chile", "year round average", "butternut", "hubbard",
                "spaghetti",  "russet",  "o'brien", "banana", "green (includes baby and lesuer types)", "young green",
                "(skunk cabbage)",   "(corn and limas)", "(vegetable oyster)","agar", "white icicle", "oriental" ,"hawaiian style",
                "yellow fleshed", "spring or scallions (includes tops and bulb)", "(tendergreen)",
                "hot chile", "ripe", "(bulb and lower leaf-portion)","(dried gourd strips)", "(pepeao)",
                "italian style", "kelp", "kave","laver","spirulina", "wakame", "flowers","white-flowered (calabash)",
                "dishcloth (towelgourd)", "garden", "mustard", "(globe or french)", "snap", "harvard","flower clusters","stalks",
                "irishmoss",  "common","stalks","(fuki)", "red leaf","iceberg (includes crisphead types)",
                "green leaf", "cos or romaine","butterhead (includes boston and bibb types)",
                "potherb","ancho","(poke)","edible-podded", "common (danish, domestic, and pointed types)",
                "(chinese preserving melon)",  "enoki", "maitake","morel","oyster","straw", "shiitake", "portabella","fruit",
                 "(matai)", "root",  "witloof","baby","garland",  "napa","savoy", "kimchi","chili",
                "all-types", "chinese (pak-choi)","chinese (pe-tsai)","kidney", "fava","mung","navy","pinto","shellie","puree"
            };

            ThirdParts = new List<string>(){
                "flesh","shredded","crushed","crosscut","chopped or leaf","steak cut","summer", "winter",
                "shoestring","all types","whole", "chopped", "spears","mashed","sweet",
                "crinkle or regular cut","cross cut", "extruded", "cottage-cut" ,"steak fries",
                "wedge cut", "tops only","(bulb and lower-leaf portion)","cut off cob","excluding seeds",
                "young pods with seeds", "kernels","kernels on cob", "bulb","kernels cut off cob",
                "whole kernel", "in pod","immature seeds","shoots", "mature seeds","leaves",
                "leafy tips", "pods", "young pods with seed","hot chili"
            };
        }

        public override string SimpleMainPart(string part) {
            part = part.ToLower();
            if (part !="tomato sauce" && part.ToLower().Contains("tomato"))
                return "tomato";
            else if (part.Contains("coriander (cilantro) leaves"))
                return  "coriander";
            else if (part.Contains("lemon grass (citronella)"))
                return "lemongrass";
            else if (part.Contains("yambean (jicama)"))
                return "jicama";
            return part;
        }

        public override Tuple<string, string> SimpleSecondPart(string item) {
            if (item.Contains("spring"))
                item = "green";
            else if (item.ToLower().Contains("cos or romaine"))
                item = "romaine";
            else if (item.ToLower().Contains("iceberg (includes crisphead types)"))
                item = "iceberg";
            return base.SimpleSecondPart(item);
        }

    }
}