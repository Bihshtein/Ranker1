﻿using System;
using System.Collections.Generic;

namespace InitProducts {
    internal class BakedValidator : BasicValidator {
        public BakedValidator() {
            MainParts = new List<string> {
                "Bagels","Biscuits","Pie","Pie crust","Rolls","Pancakes", "Phyllo dough",
                "Muffins","English muffins","Crackers","Cookies","Cookie","Sweet rolls",
                "Coffeecake","Cake","Bread","Doughnuts","Croutons","Puff pastry",
                "Croissants","Cream puff","Danish pastry","Tortillas","Breakfast tart",
                "Leavening agents","Bread crumbs", "Bread stuffing","PILLSBURY","PEPPERIDGE FARM","NABISCO"
            };

            SecondParts = new List<string> {
                "apple","banana cream","blueberry","cherry","cheese","banana","hamburger bun",
                "Dutch Apple","yeast","baking soda","baking powder",
                "egg custard","peach","pecan","pumpkin","mince","pecan",
                "egg","oat bran", "rye","sweet","wheat","whole-wheat","french",
                "hamburger or hot dog", "hamburger or hotdog","pumpernickel","dinner","corn",
                "pound","snack cakes","sugar","shortbread","peanut butter",
                "brownies","boston brown","chapati or roti","cornbread",
                "sponge","pita","yellow","gingerbread","angelfood","boston cream pie",
                "deep dish","irish","italian","hard (includes kaiser)",
                "naan","pan dulce","paratha","potato","pound cake type","raisin",
                "protein","pumpernickel","french or vienna","shortcake","fortune", "fig bars",
                "fudge","gingersnaps","ladyfingers","Marie biscuit","marshmallow","molasses",
                "saltines","melba toast","matzo","irish soda","fruitcake","carrot","marble",
                "Crusty French Loaf","cream of tartar", "NABISCO RITZ Crackers"
            };

            ThirdParts = new List<string> {
                "white","plain","whole grain","whole wheat","mixed-grain","mixed grain","dry mix",
                "whole grain white","wheat bran","oat bran","oatmeal","cracked-wheat","active dry",
                "compressed","double-acting","seasoned","grated","ready-to-bake","refrigerated dough",
                "plain or buttermilk","multigrain","wheat germ","buckwheat","buttermilk"
            };
        }
        
        
        public override Tuple<string, string> SimpleSecondPart(string item) {
            if (item.Contains("french or vienna"))
                item = "french";
            else if (item == "cream of tartar")
                item = "cream tartar";
            else if (item == "hard (includes kaiser)")
                item = "hard";
            else if (item.ToLower().Contains("ritz crackers"))
                item = "ritz crackers";
            return base.SimpleSecondPart(item);
        }

        public override string SimpleThirdPart(string part) {
            if (part == "refrigerated dough")
                return "dough";
            return base.SimpleThirdPart(part);
        }


        public override bool IsValidPart(string part) {
            return part.Trim() == "baker's" ||
            base.IsValidPart(part);
        }

        public override bool IsSecondPart(string part) {
            return part.ToLower().Contains("chocolate") ||                
                part.ToLower().Contains("cinnamon") ||
                part.ToLower().Contains("coconut") ||
                part.ToLower().Contains("lemon") ||
                part.ToLower().Contains("wafer") ||
                part.ToLower().Contains("with") ||
                part.ToLower().Contains("-type") ||
                base.IsSecondPart(part);
        }

        public override bool IsThirdPart(string part) {
            return part.ToLower().Contains("filling") ||
                part.ToLower().Contains("filled") ||
                part.ToLower().Contains("rice") ||
                part.ToLower().Contains("yeast") ||
                part.ToLower().Contains("regular") ||
                part.ToLower().Contains("sandwich") ||

                base.IsThirdPart(part);

        }
    }
}