using RestModel.Validators;
using System.Collections.Generic;

namespace RestModel {
    internal class SoupAndSauceValidator : BasicValidator {
         public SoupAndSauceValidator() {
            MainParts = new List<string> {"Sauce","Soup"};
            SecondParts = new List<string> {
                "barbecue", "horseradish","fish","duck","hot chile","cheese",
                "oyster","pesto","pizza","plum","salsa","teriyaki","tomato chili sauce","tartar",
                "sweet and sour","sofrito","pepper or hot",
                "bean with bacon", "bean with frankfurters", "bean with pork",
                "beef and mushroom", "beef and vegetables","beef barley","beef broth","beef","vegetable broth",
                "beef broth or bouillon","beef mushroom","black bean","chicken broth","worcestershire",
            };
            ThirdParts = new List<string> { "ready to serve","stock", "ready-to-serve" ,"cubed","bouillon","consomme"};
        }
    }
}
