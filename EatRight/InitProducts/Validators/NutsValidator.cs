using System.Collections.Generic;

namespace InitProducts {
    internal class NutsValidator : BasicValidator {
        public NutsValidator() {
            MainParts = new List<string> {"chia seeds","peanuts", "almonds", "pecans", "walnuts","seeds","cashew nuts"};
            SecondParts = new List<string> { "english", "glazed" ,"coconut milk","sesame seeds"};
            ThirdParts = new List<string> { "whole" };
        }
    }
}