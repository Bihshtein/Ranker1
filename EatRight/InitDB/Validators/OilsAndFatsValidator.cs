using InitDB.Validators;
using System.Collections.Generic;

namespace InitDB {
    internal class OilsAndFatsValidator : BasicValidator {
        public OilsAndFatsValidator() {
            MainParts = new List<string> { "Oil" ,"Mayonnaise","Salad dressing","Margarine" };
            SecondParts = new List<string> {"corn","corn and canola","made with tofu", "vegetable","olive","mayonnaise" ,"regular"};
            ThirdParts = new List<string> {
                "composite", "stick","tub", "salad or cooking","Natreon canola",
                "industrial and retail", "all purpose salad or cooking",
                "high stability", "non trans", "high oleic (70%)" ,"light","with olive oil"
            };
        }
    }
}