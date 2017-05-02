using System;
using System.Collections.Generic;

namespace InitProducts {
    internal class SausagesValidator : BasicValidator {
        public SausagesValidator() {
            SecondParts = new List<string> {
                "turkey", "beef" , "beef and pork", "pork and beef", "chicken","meat", "meat and poultry",
                "chicken liver","goose liver","liver", "beef","pork", "Berliner","Polish","Vienna",
                "smoked link sausage", "prepackaged or deli"
            };
            ThirdParts = new List<string> {"Italian style", "sliced" ,"Italian","luncheon meat",};
        }

        public override Tuple<string, string> SimpleSecondPart(string item) {
            if (item == "prepackaged or deli")
                item = "deli";
            return base.SimpleSecondPart(item);
        }

        public override string SimpleMainPart(string part) {
            if (part.ToLower() == "turkey breast")
                return "tukey";
            return base.SimpleMainPart(part);
        }
    }
}