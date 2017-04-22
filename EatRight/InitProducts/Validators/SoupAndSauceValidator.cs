using System;
using System.Collections.Generic;

namespace InitProducts {
    internal class SoupAndSauceValidator : BasicValidator {
         public SoupAndSauceValidator() {
            MainParts = new List<string> {"Sauce","Soup"};
            SecondParts = new List<string> {
                "barbecue", "horseradish","fish","duck","hot chile","cheese","cream of mushroom","cream of chicken",
                "oyster","pesto","pizza","plum","salsa","teriyaki","tomato chili sauce","tartar","tomato",
                "sweet and sour","sofrito","pepper or hot",
                "bean with bacon", "bean with frankfurters", "bean with pork",
                "beef and mushroom", "beef and vegetables","beef barley","beef broth","beef","vegetable broth",
                "beef broth or bouillon","beef mushroom","black bean","chicken","chicken broth","worcestershire",
            };
            ThirdParts = new List<string> { "ready to serve","stock", "ready-to-serve" ,"cubed","bouillon","consomme"};
        }


        public override Tuple<string, string> GetNameAndDescription(string item) {
            if (item.Contains("cream of mushroom"))
                item = "cream mushroom";
            else if (item.Contains("cream of chicken"))
                item = "cream chicken";
            return base.GetNameAndDescription(item);
        }
    }
}
