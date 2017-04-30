using System.Collections.Generic;
using System;
namespace InitProducts {
    internal class OilsAndFatsValidator : BasicValidator {
        public OilsAndFatsValidator() {
            MainParts = new List<string> {
                "Shortening", "Oil" ,"Mayonnaise","Salad dressing","Margarine"
            };
            SecondParts = new List<string> {
                "corn","corn and canola","made with tofu", "vegetable","olive","mayonnaise" ,"",
                "fractionated palm","lard and vegetable oil","sesame","coconut","PAM cooking spray","peanut","ranch dressing"
            };
            ThirdParts = new List<string> {
                "composite", "stick","tub", "salad or cooking","Natreon canola",
                "industrial and retail", "all purpose salad or cooking",
                "high stability", "non trans", "high oleic (70%)" ,"light","with olive oil",
                "industrial","confectionery","household","original","regular"
            };
        }


        public override Tuple<string, string> SimpleSecondPart(string item) {
            if (item.Contains("cooking spray"))
                item = "cooking spray";
            return base.SimpleSecondPart(item);
        }

        public override bool IsSecondPart(string part) {
            return  part.Contains("soy ")||
            base.IsSecondPart(part);
        }
    }
}