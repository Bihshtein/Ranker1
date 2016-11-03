﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitDB {
    class PorkValidator {
        public static bool IsPorkParameter(string _param) {
            _param = _param.Trim();
            return (CommonValidator.IsCommonParameter(_param) ||
                    PorkSecondParts.Contains(_param) ||
                    PorkMainParts.Contains(_param)
                    );
        }

        public static List<string> PorkSecondParts = new List<string>() {
            "leg cap steak","blade", "tenderloin", "country-style ribs", "rump", "shank", "slice", "whole", "spiral slice" , "arm picnic","rump half", "shank half", "steak","blade roll","blade (chops or roasts)","center rib (chops or roasts)",
            "blade (chops)","top loin (chops)", "sirloin (chops)", "center rib (chops)", "center loin (chops)","top loin (chops)", "top loin (chops)","sirloin (chops or roasts)",
            "blade (roasts)","top loin (roasts)", "sirloin (roasts)", "center rib (roasts)", "center loin (roasts)","center slice"};
        public static List<string> PorkMainParts = new List<string>() { "carcass", "belly", "salt pork", "backfat", "backribs", "leg (ham)", "ham", "feet", "bacon", "loin", "shoulder", "spareribs", "ham -- water added", "ham and water product", "ham with natural juices" };

    }
}
