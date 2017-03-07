using System.Collections.Generic;

namespace InitProducts {
    internal class LegumesValidator : BasicValidator {
        public LegumesValidator() {
            MainParts = new List<string> {"Peanuts", "Soy sauce", "Soymilk", "Beans","Refried beans"};
            SecondParts = new List<string> {"all types","white","french","pink","navy","black turtle", "black","original and vanilla","made from hydrolyzed vegetable protein" ,"traditional style (includes USDA commodity)"};
            ThirdParts = new List<string> { "mature seeds" };
        }
    }
}