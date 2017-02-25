using System.Collections.Generic;

namespace InitProducts {
    internal class OilsAndFatsValidator : BasicValidator {
        public OilsAndFatsValidator() {
            MainParts = new List<string> {
                "Shortening", "Oil" ,"Mayonnaise","Salad dressing","Margarine"
            };
            SecondParts = new List<string> {
                "corn","corn and canola","made with tofu", "vegetable","olive","mayonnaise" ,"regular",
                "fractionated palm","lard and vegetable oil","sesame","coconut","PAM cooking spray"
            };
            ThirdParts = new List<string> {
                "composite", "stick","tub", "salad or cooking","Natreon canola",
                "industrial and retail", "all purpose salad or cooking",
                "high stability", "non trans", "high oleic (70%)" ,"light","with olive oil",
                "industrial","confectionery","household","original"
            };
        }

        public override string GetPrettyName(string part) {
            if (part.ToLower().Contains("PAM cooking spray"))
                return "cooking spray";
            return part;
        }

        public override bool IsSecondPart(string part) {
            return  part.Contains("soy ")||
            base.IsSecondPart(part);
        }
    }
}