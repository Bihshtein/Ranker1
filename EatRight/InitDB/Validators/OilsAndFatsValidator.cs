using InitDB.Validators;
using System.Collections.Generic;

namespace InitDB {
    internal class OilsAndFatsValidator : BasicValidator {
        public OilsAndFatsValidator() {
            MainParts = new List<string> { "Oil" ,"Mayonnaise","Salad dressing" };
            SecondParts = new List<string> {"made with tofu", "vegetable","olive","mayonnaise" };
            ThirdParts = new List<string> { "salad or cooking","Natreon canola", "high stability", "non trans", "high oleic (70%)" ,"light","with olive oil"};
        }
    }
}