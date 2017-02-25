using System.Collections.Generic;

namespace InitProducts {
    internal class FishValidator : BasicValidator {
        public FishValidator() {
            MainParts = new List<string> {
                "yellowtail","wolffish","whiting","whitefish","turbot","trout","tuna","tilefish","tilapia","swordfish","surimi",
                "sunfish","sucker","sturgeon","spot","snapper","smelt","sheepshead",
                "shark","spot","sea bass","seatrout","scup","sardine","salmon",
                "sablefish","roughy","roe","rockfish","pout","pompano","pollock",
                "pike","perch","ocean perch","mullet", "monkfish","milkfish",
                "mahimahi","mackerel","lingcod","ling","herring","halibut",
                "haddock","grouper","gefiltefish", "flatfish","fish sticks","eel",
                "drum","cusk","croaker","cod","cisco","caviar","catfish","carp",
                "butterfish","burbot","bluefish","bass","anchovy","roughy","shad",
                 "black and red","Crustaceans","Mollusks",

            };

            SecondParts = new List<string> {
                "Atlantic","Alaska","northern","florida","Pacific","sockeye","pink",
                "coho","chum","mixed species","american","rainbow","Atlantic and Pacific",
                "freshwater","fresh water","channel","bluefin","skipjack","yellowfin","light",
                "white","european","chinook","orange","pumpkin seed","granular","commercial",
                "greenland","Greenland","jack","king","Pacific and jack","spanish","walleye",
                "mixed species","shrimp","oyster"

            };
            ThirdParts = new List<string> {
                "farmed","wild","eastern"
            };
        }
    }
}