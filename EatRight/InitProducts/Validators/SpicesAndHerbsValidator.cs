using InitDB.Validators;
using System.Collections.Generic;

namespace InitDB {
    internal class SpicesAndHerbsValidator : BasicValidator {
        public SpicesAndHerbsValidator() {
            MainParts = new List<string> {
                "Basil", "Capers", "Dill weed", "Horseradish","Mustard","Peppermint",
                "Rosemary","Salt", "Seasoning mix","Spearmint","Spices","Thyme",
                "Vanilla extract","Vinegar",
            };
            SecondParts = new List<string> {
                "imitation","balsamic","cider","distilled","red wine","turmeric","thyme",
                "tarragon","savory","sage","saffron","yellow","table","chili","sazon","taco",
                "allspice","anise seed","basil","bay leaf","caraway seed","cardamom",
                "celery seed","chervil","chili powder","cinnamon","cloves","coriander leaf",
                "coriander seed","curry powder","dill seed","dill weed","fennel seed","fenugreek seed",
                "garlic powder","ginger","mace","marjoram","mustard seed","nutmeg","onio powder",
                "oregano", "paprika","parsley","pepper","poppy seed","poultry seasoning",
                "pumpkin pie spice","rosemary","cumin seed"
            };
            ThirdParts = new List<string> {
                "original","coriander & annatto","black","red or cayenne","white"
            };
        }


    }
}