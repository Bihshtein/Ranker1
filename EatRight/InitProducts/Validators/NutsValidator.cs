using System;
using System.Collections.Generic;

namespace InitProducts {
    internal class NutsValidator : BasicValidator {
        public NutsValidator() {
            MainParts = new List<string> {"peanuts", "almonds", "pecans", "walnuts","seeds","cashew nuts"};
            SecondParts = new List<string> {"sunflower seed kernels","chia seeds", "english", "glazed" ,"coconut milk","sesame seeds"};
            ThirdParts = new List<string> { "whole" };
        }


        public override Tuple<string, string> GetNameAndDescription(string item) {
            if (item.Contains("sunflower"))
                item = "sunflower";
            return base.GetNameAndDescription(item);
        }
    }
}