using InitDB.Validators;
using System.Collections.Generic;

namespace InitDB {
    internal class OilsAndFatsValidator : BasicValidator {
        public OilsAndFatsValidator() {
            MainParts = new List<string> { "Oil" };
            SecondParts = new List<string> { "vegetable","olive" };
            ThirdParts = new List<string> { "salad or cooking","Natreon canola", "high stability", "non trans", "high oleic (70%)" };
        }
    }
}