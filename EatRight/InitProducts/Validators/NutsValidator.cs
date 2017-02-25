using System.Collections.Generic;

namespace InitProducts {
    internal class NutsValidator : BasicValidator {
        public NutsValidator() {
            MainParts = new List<string> { "almonds", "pecans", "walnuts" };
            SecondParts = new List<string> { "english", "glazed" };
        }
    }
}