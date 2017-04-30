using System.Collections.Generic;

namespace InitProducts {
    internal class LegumesValidator : BasicValidator {
        public LegumesValidator() {
            MainParts = new List<string> {"Miso","Tofu","Peanuts", "Soy sauce", "Soymilk", "Beans","Refried beans","Hummus"};
            SecondParts = new List<string> {
                "all types","white","french","pink","navy","black turtle", "black","original and vanilla",
                "made from hydrolyzed vegetable protein" ,"traditional style (includes USDA commodity)",
                "regular","commercial"};
            ThirdParts = new List<string> { "mature seeds" };
        }

        public override bool IsMainPart(string part) {
            if (part.Contains("Chickpeas"))
                return true;
            else 
            return base.IsMainPart(part);
        }

        public override bool IsSecondPart(string part) {
            if (part.Contains("bengal gram"))
                return true;
            else
                return base.IsSecondPart(part);
        }

        public override string SimpleMainPart(string part) {
            if (part.Contains("Chickpeas"))
                return "chickpeas";
            else
                return base.SimpleMainPart(part);
        }


    }
}