using System;
using System.Collections.Generic;

namespace InitProducts {
    public class FruitsValidator : BasicValidator {
        public FruitsValidator() {
            SecondParts = new List<string> {
                    "tropical" ,"(west indian cherry)","(pineapple and papaya and banana and guava)","ripe",
                    "(cape-gooseberries or poha)","(jambolan)","single strength","tart",
                    "canned (small-extra large)","all commercial varieties","valencias","navels","(granadilla)",
                    "(sweetsop)","(surinam-cherry)", "(mamey)", "mamey","golden seedless", "all varieties",
                    "extra sweet variety","traditional varieties","chunks","native","grape and pear blend","apple and grape blend",
                    "thompson seedless","strawberry banana", "(peach and pear and apricot and pineapple and cherry)","european black",
                    "zante","grape", "dried (prunes)","(natal-plum)", "japanese","(peach and pineapple and pear and grape and cherry)",
                    "medjool","deglet noor", "(mandarin oranges)","honeydew","casaba","cantaloupe","apple","jambolan)","strawberry",
                    "muscadine","common","bosc","green anjou","red anjou","bartlett","spiced","asian","golden delicious", "granny smith",
                    "red delicious","wild","fuji","gala","pink and red and white","yellow","green","purple", "sweet","sour","white","pink",
                    "red","pink or red", "pink and red","red and white","California", "Florida",
                "REAL LEMON","CONCORD","includes from concentrate"
            };
            ThirdParts = new List<string> {
                "sections", "all areas", "seeded", "seedless", "pitted", "sliced",
                "whole","canned (jumbo-super colossal)","chilled"
            };
        }

        public override string SimpleMainPart(string part) {
            if (part.ToLower() == "lemon juice from concentrate")
                return "concentrate lemon juice";
            return base.SimpleMainPart(part);
        }

        public override Tuple<string, string> SimpleSecondPart(string item) {
            item = item.Replace("(", "");
            item = item.Replace(")", "");
            if (item.ToLower() == "includes from concentrate")
                item = "concentrate";
            return base.SimpleSecondPart(item);
        }
    }
}