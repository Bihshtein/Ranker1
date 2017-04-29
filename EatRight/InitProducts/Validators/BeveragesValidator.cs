using System;
using System.Collections.Generic;

namespace InitProducts {
    internal class BeveragesValidator : BasicValidator {
        public BeveragesValidator() {
            MainParts = new List<string>() {
            "beer","coffee","liqueur", "wine","tea" ,"drink","whiskey","pina colada","cola","rum","vodka"
            };

            SecondParts = new List<string>() {
           "all", "dry","sweet","orange", "mocha","regular","black", "green","herb","hibiscus","red","white","brewed","instant","generic","rice (sake)"
            };
            ThirdParts = new List<string> {
            "chocolate", "breakfast type","dessert","cooking",  "regular",  "cooking","light","dessert", "lemon", "ready to drink","ready-to-drink","table","breakfast blend","espresso","chicory",
            "80 proof"
            };
        }

        public override bool IsMainPart(string part) {
            if (part == string.Empty)
                return true;
            return MainParts.Contains(part) ||
                (part.Contains("drink") && !part.Contains("ready")) ||
                 (part.Contains("milk") && !part.Contains("based") && !part.Contains("with")) ||
                part.Contains("soda") || part.Contains("flavor")||
             (char.IsUpper(part[0]) && !part.ToLower().Contains("beverage"));
        }

        public override bool IsValidPart(string part) {
            return part.ToLower() == "alcoholic beverage" ||
            base.IsValidPart(part);
        }

        public override Tuple<string, string> GetNameAndDescription(string item) {
            if (item.Contains("rice (sake)"))
                item = "sake";
            return base.GetNameAndDescription(item);
        }
    }
}