using System.Collections.Generic;

namespace InitProducts {
    internal class SweetsValidator : BasicValidator {
        public SweetsValidator() {
            MainParts = new List<string> {
                "Sugars","Honey","Cocoa","Syrups","Cocoa","Candies","Sweeteners","Sweetener", "Jams and preserves","Snacks"
            };
            SecondParts = new List<string> {
                "brown","granulated","maple", "dry powder", "semisweet chocolate", "sweet chocolate","powdered","corn",
                "fruit leather","marshmallows","syrup"
            };
            ThirdParts = new List<string> {
                "sugar substitute","light","dark","pieces","agave"
            };
        }

        public override string SimpleMainPart(string part) {
            if (part.ToLower().Contains("jams"))
                return "jam";
            return base.SimpleMainPart(part);
        }
    }
}