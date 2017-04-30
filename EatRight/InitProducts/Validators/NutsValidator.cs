using System;
using System.Collections.Generic;

namespace InitProducts {
    internal class NutsValidator : BasicValidator {
        public NutsValidator() {
            MainParts = new List<string> {"pine nuts","peanuts", "almonds", "pecans", "walnuts","seeds","cashew nuts"};
            SecondParts = new List<string> {"tahini","sunflower seed kernels","chia seeds","coconut meat", "english", "glazed" ,"coconut milk","sesame seeds","flaxseed"};
            ThirdParts = new List<string> {"sesame butter", "whole", "flaked", "shredded" };
        }

        public override bool IsThirdPart(string part) {
            return part.ToLower().Contains("kernels") ||
                base.IsThirdPart(part);
        }


        public override Tuple<string, string> SimpleSecondPart(string item) {
            if (item.Contains("sunflower"))
                item = "sunflower";
            if (item.Contains("coconut meat"))
                item = "coconut";
            if (item.Contains("flaxseed"))
                item = "flax seed";
            return base.SimpleSecondPart(item);
        }
    }
}