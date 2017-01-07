using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitDB.Validators {
    public class BeefValidator : BasicValidator {
        public BeefValidator() {
            MainParts = new List<string>(){
                "liver", "heart", "kidney", "tongue", "loin", "flank", "chuck",
                "rib", "round", "brisket", "plate", "short loin" }; ;

            SecondParts = new List<string>(){
                "small end rib","striploin","strip loin","rump centre","ribs prepared","oyster blade","flat","hind shin", 
                "eye round","cube roll","chuck eye roll", "bolar blade","chuck eye Country-Style ribs","clod",
                "ribeye","top loin petite", "ribeye petite" ,"ribeye  petite", "top loin petite", "top round",
                "tri - tip","tri-tip", "arm pot", "mock tender","blade","top blade", "chuck eye", "short ribs","bottom round",
                "point half","whole", "flat half","knuckle","eye of round","outside round", "tip round","top round",
                "tenderloin","shoulder top blade","eye","small end (ribs 10-12)","whole (ribs 6-12)", "shoulder pot",
                "large end (ribs 6-9)", "shortribs","back ribs","rib eye","back ribs", "porterhouse",
                "t-bone","retail cuts","bottom sirloin butt","top sirloin petite", "top loin", "top sirloin",
                "bottom sirloin", "chuck for stew", "composite of trimmed retail cuts"
            };

            ThirdParts = new List<string>()
            {  "steak", "roast","under blade steak", "under blade pot roast", "under blade pot roast or steak",
                "under blade center steak", "outside skirt steak",
                "inside skirt steak", "cap-off steak/roast", "navel end", "point end", "pot roast or steak",
                "pot roast", "center steak", "filet", "cap steak", "roast/fillet", "roast/filet", "roast or steak",
                "steak/roast", "steak/fillet", "fillet", "cap", "roast", "full cut",

            };
        }


        public static List<string> BeefDescription = new List<string>() {
            "grass-fed", "Wagyu", "variety meats and by-products", "America's Beef Roast", "Denver Cut", "imported", "lip off", "lip-on", "lip-off",
        };
        public static List<string> BeefImportDescription = new List<string>() { "New Zealand", "Australian" };


        public static List<string> BeefGrades = new List<string>() {
            "Aust. marble score 4/5", "Aust. marble score 9", "prime", "all grades", "choice", "select"
        };

        public static List<string> GroundBeefTypes = new List<string>() { "loaf", "patty cooked", "patty", "crumbles" };        
      

        public override bool IsValidPart(string part) {
            part = part.Trim();
            return (
                    base.IsValidPart(part) || 
                    BeefImportDescription.Contains(part) ||
                    BeefDescription.Contains(part) ||
                    BeefGrades.Contains(part) ||
                    GroundBeefTypes.Contains(part)
                    );
        }

    }
}
