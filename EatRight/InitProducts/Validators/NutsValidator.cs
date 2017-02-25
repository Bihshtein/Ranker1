using System.Collections.Generic;

namespace InitProducts {
    internal class NutsValidator : BasicValidator {
        public NutsValidator() {
            MainParts = new List<string> {"peanuts", "almonds", "pecans", "walnuts","seeds"};
            SecondParts = new List<string> { "english", "glazed" ,"coconut milk","sesame seeds"};
            ThirdParts = new List<string> { "whole" };
        }
    }
}