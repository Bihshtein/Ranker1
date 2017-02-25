using System.Collections.Generic;

namespace InitProducts {
    internal class LegumesValidator : BasicValidator {
        public LegumesValidator() {
            MainParts = new List<string> { "Soy sauce", "Soymilk" };
            SecondParts = new List<string> { "original and vanilla","made from hydrolyzed vegetable protein" };

        }
    }
}