using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitDB {
    public class BeefValidator {
        public static List<string> BeefMainParts = new List<string>() { "liver", "heart", "kidney", "tongue", "loin", "flank", "chuck", "rib", "round", "brisket", "plate", "short loin" };
        public static List<string> BeefSecondParts = new List<string>()
        {"small end rib","striploin","strip loin","rump centre","ribs prepared","oyster blade","flat","hind shin", "eye round","cube roll","chuck eye roll","bolar blade","chuck eye Country-Style ribs","clod","ribeye","top loin petite", "ribeye petite" ,"ribeye  petite", "top loin petite", "top round","tri - tip","tri-tip", "arm pot", "mock tender","blade","top blade",
            "chuck eye", "short ribs","bottom round","point half","whole", "flat half","knuckle","eye of round","outside round",
            "tip round","top round","tenderloin","shoulder top blade","eye","small end (ribs 10-12)","whole (ribs 6-12)", "shoulder pot","large end (ribs 6-9)",
            "shortribs","back ribs","rib eye","back ribs", "porterhouse",  "t-bone","retail cuts","bottom sirloin butt","top sirloin petite",
            "top loin", "top sirloin", "bottom sirloin", "chuck for stew", "composite of trimmed retail cuts" };


      
        public static List<string> BeefCuts = new List<string>() { "cap-off steak/roast", "navel end", "point end", "pot roast or steak", "pot roast", "center steak", "filet", "cap steak", "roast/fillet", "roast/filet", "roast or steak", "steak/roast", "steak/fillet", "fillet", "cap", "roast", "full cut", "steak", "roast" };

        public static List<string> BeefDescription = new List<string>() { "grass-fed", "Wagyu", "variety meats and by-products", "America's Beef Roast", "Denver Cut", "imported" };
        public static List<string> BeefImportDescription = new List<string>() { "New Zealand", "Australian" };
        public static List<string> BeefCutDetails = new List<string>() { "under blade", "lip off", "lip-on", "lip-off", "outside skirt", "inside skirt", };

        public static List<string> BeefGrades = new List<string>() { "Aust. marble score 4/5", "Aust. marble score 9", "prime", "all grades", "choice", "select", };
        public static List<string> BeefTrimmedDetails = new List<string>() { "trimmed to 1/4\" fat", "trimmed to 1 / 4\" fat", "trimmed to 1/8\" fat", "trimmed to 1 / 8\" fat", "trimmed to 0\" fat", "trimmed to 1/8\"fat", };

        public static List<string> GroundBeefTypes = new List<string>() { "loaf", "patty cooked", "patty", "crumbles" };
        public static bool CheckWithoutCut(string _param) {
            return
            BeefCuts.Any((cut) => BeefMainParts.Contains(_param.Replace(cut, string.Empty).Trim())) ||
            BeefCuts.Any((cut) => BeefSecondParts.Contains(_param.Replace(cut, string.Empty).Trim())) ||
            BeefCuts.Any((cut) => BeefCutDetails.Contains(_param.Replace(cut, string.Empty).Trim()));
        }
        public static bool IsBeefParameter(string _param) {
            _param = _param.Trim();
            return (
                    CommonValidator.IsCommonParameter(_param) ||
                    BeefSecondParts.Contains(_param) ||
                    BeefMainParts.Contains(_param) ||
                    BeefImportDescription.Contains(_param) ||
                    CheckWithoutCut(_param) ||
                    BeefDescription.Contains(_param) ||
                    BeefCuts.Contains(_param) ||
                    BeefCutDetails.Contains(_param) ||
                    BeefGrades.Contains(_param) ||
                    BeefTrimmedDetails.Contains(_param) ||
                    GroundBeefTypes.Contains(_param)
                    );
        }
    }
}
