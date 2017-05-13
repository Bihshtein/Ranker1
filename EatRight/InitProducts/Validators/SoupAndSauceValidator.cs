using System;
using System.Collections.Generic;

namespace InitProducts {
    internal class SoupAndSauceValidator : BasicValidator {
         public SoupAndSauceValidator() {
            MainParts = new List<string> {"Sauce","Soup","Gravy"};
            SecondParts = new List<string> {
                "barbecue", "horseradish","fish","duck","hot chile","cheese","cream of mushroom","cream of chicken",
                "oyster","pesto","pizza","plum","salsa","teriyaki","tomato chili sauce","tartar","tomato",
                "sweet and sour","sofrito","pepper or hot","pasta","hoisin",
                "bean with bacon", "bean with frankfurters", "bean with pork",
                "beef and mushroom", "beef and vegetables","beef barley","beef broth","beef","vegetable broth",
                "beef broth or bouillon","beef mushroom","black bean","chicken","chicken broth","worcestershire",
            };
            ThirdParts = new List<string> {
                "spaghetti/marinara","sriracha", "ready to serve","stock", "ready-to-serve" ,
                "cubed","bouillon","consomme", 
            };
        }

       
        public override Tuple<string, string> SimpleSecondPart(string item) {
            if (item.Contains("cream of mushroom"))
                item = "cream mushroom";
            else if (item.Contains("cream of chicken"))
                item = "cream chicken";
            else if (item.Contains("barbecue"))
                item = "bbq";
            return base.SimpleSecondPart(item);
        }
    }
}
