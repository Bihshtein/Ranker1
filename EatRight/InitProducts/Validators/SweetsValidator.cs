﻿using System.Collections.Generic;

namespace InitProducts {
    internal class SweetsValidator : BasicValidator {
        public SweetsValidator() {
            MainParts = new List<string> {"Sugars","Honey","Cocoa","Syrups","Cocoa","Candies"};
            SecondParts = new List<string> { "brown","granulated","maple", "dry powder","sweet chocolate","powdered"};
        }
    }
}