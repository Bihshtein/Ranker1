﻿using System;
using System.Collections.Generic;

namespace InitProducts {
    internal class NutsValidator : BasicValidator {
        public NutsValidator() {
            MainParts = new List<string> {"peanuts", "almonds", "pecans", "walnuts","seeds","cashew nuts"};
            SecondParts = new List<string> {"sunflower seed kernels","chia seeds","coconut meat", "english", "glazed" ,"coconut milk","sesame seeds"};
            ThirdParts = new List<string> { "whole", "flaked", "shredded" };
        }


        public override Tuple<string, string> GetNameAndDescription(string item) {
            if (item.Contains("sunflower"))
                item = "sunflower";
            if (item.Contains("coconut meat"))
                item = "coconut";
            return base.GetNameAndDescription(item);
        }
    }
}