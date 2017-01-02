using InitDB.Validators;
using System.Collections.Generic;

namespace InitDB {
    internal class NutsValidator : BasicValidator {
        public NutsValidator() {
            MainParts = new List<string> { "almonds", "pecans", "walnuts" };
            SecondParts = new List<string> { "english", "glazed" };
        }
    }
}