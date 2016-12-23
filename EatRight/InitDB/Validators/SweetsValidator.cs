using InitDB.Validators;
using System.Collections.Generic;

namespace InitDB {
    internal class SweetsValidator : BasicValidator {
        public SweetsValidator() {
            MainParts = new List<string> {"Sugars","Honey","Cocoa"};
            SecondParts = new List<string> { "brown","granulated" };
        }
    }
}