using RestModel.Validators;
using System.Collections.Generic;

namespace RestModel {
    internal class SweetsValidator : BasicValidator {
        public SweetsValidator() {
            MainParts = new List<string> {"Sugars","Honey","Cocoa","Syrups"};
            SecondParts = new List<string> { "brown","granulated","maple" };
        }
    }
}