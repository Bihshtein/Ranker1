using RestModel.Validators;
using System.Collections.Generic;

namespace RestModel {
    internal class NutsValidator : BasicValidator {
        public NutsValidator() {
            MainParts = new List<string> { "almonds", "pecans", "walnuts" };
            SecondParts = new List<string> { "english", "glazed" };
        }
    }
}