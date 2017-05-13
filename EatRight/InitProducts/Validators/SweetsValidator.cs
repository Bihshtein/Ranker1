using System.Collections.Generic;

namespace InitProducts {
    internal class SweetsValidator : BasicValidator {
        public SweetsValidator() {
            MainParts = new List<string> {
                "Sugars","Honey","Cocoa","Syrups","Cocoa","Candies","Sweeteners","Sweetener",
                "Jams and preserves","Snacks","Marmalade","Chocolate"
            };
            SecondParts = new List<string> {
                "brown","granulated","maple", "dry powder", "semisweet chocolate", "sweet chocolate",
                "powdered","corn", "fruit leather","marshmallows","syrup","orange","dark"
            };
            ThirdParts = new List<string> {
                "sugar substitute","light","pieces","agave",
            };
        }

        public override string SimpleMainPart(string part) {
            if (part.ToLower().Contains("jams"))
                return "jam";
            return base.SimpleMainPart(part);
        }

        public override bool IsThirdPart(string part) {
            if (part.Contains("cacao solids"))
                return true;
            else 
            return base.IsThirdPart(part);
        }
    }
}