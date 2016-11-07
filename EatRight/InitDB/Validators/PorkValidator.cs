using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitDB.Validators {
    class PorkValidator : BasicValidator {

        public List<string> Cuts { get; }
        public List<string> SecondParts { get; }
        public List<string> MainParts { get; }


        public PorkValidator() {
            MainParts = new List<string>() { "carcass", "belly", "salt pork", "backfat", "backribs", "leg (ham)", "ham", "feet", "bacon", "loin", "shoulder", "spareribs", "ham -- water added", "ham and water product", "ham with natural juices" };

            Cuts = new List<string>() {"slice", "whole","spiral slice","center slice"};

            SecondParts = new List<string>() {
            "leg cap steak","blade", "tenderloin", "country-style ribs", "rump", "shank","arm picnic","rump half", "shank half", "steak","blade roll","blade (chops or roasts)","center rib (chops or roasts)",
            "blade (chops)","top loin (chops)", "sirloin (chops)", "center rib (chops)", "center loin (chops)","top loin (chops)", "top loin (chops)","sirloin (chops or roasts)",
            "blade (roasts)","top loin (roasts)", "sirloin (roasts)", "center rib (roasts)", "center loin (roasts)"};
        }

        public bool IsValidPart(string part) {
            part = part.Trim();
            return (CommonValidator.IsCommonParameter(part) ||
                    SecondParts.Contains(part) ||
                    MainParts.Contains(part) ||
                    Cuts.Contains(part)
                    );
        }
     

        public  Tuple<string, string> GetNameAndCut(string item) {
            var split = item.Split('(', ')');
            var name = split[0];
            var cut = string.Empty;
            if (split.Length > 1)
                cut = split[1];
            return new Tuple<string, string>(name, cut);
        }

        public bool IsSecondPart(string item) {
            return SecondParts.Contains(item);
        } 
        public string GetPrettyName(string name) {
            if (name.Contains("ham"))
                name = "ham";
            return name;
        }
    }
}
