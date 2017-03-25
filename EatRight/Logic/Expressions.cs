﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class ParseHelpers
    {
        public static Dictionary<string, string> WeightsShortcuts = new Dictionary<string, string> {
            { "c." ,"cup"},
            { "c" ,"cup"},
            { "C" ,"cup"},
            { "t" ,"teaspoon"},
            { "T" ,"tablespoon"},
        };
        public static string GetWithoutLast_ES_letters(string str)
        {
            var length = str.Length;
            if (length > 3 && str[length - 1] == 's' && str[length - 2] == 'e')
                return str.Remove(length - 2, 2);
            return str;
        }

        public static string GetWithoutLast_S_letter(string str)
        {
            var length = str.Length;
            if (length > 2 && str[length - 1] == 's')
                return str.Remove(length - 1);
            return str;
        }
        public static string GetWeightFullName(string weight) {
            var splitBySpace = weight.Split(' ');
            if (splitBySpace.Length > 1 && WeightsShortcuts.Keys.Any(k => k == splitBySpace[1])) {
                weight = weight.Replace(splitBySpace[1], WeightsShortcuts[splitBySpace[1]]);
            }
            return weight;
        }
        public static string FixIllegalCharacters(string name) {
            name = name.Replace(".", "");
            name = name.Replace(">", "&gt;");
            name = name.Replace("<", "&lt;");
            name = name.Replace("\"", "&quot;");
            name = name.Replace("'", "&apos;");
            name = name.Replace("\b", "");
            return name;
        }

        public static TimeSpan ParseTime(string time)
        {
            var hours = GetTimeUnit(ref time, 'h');
            return new TimeSpan(hours, GetTimeUnit(ref time, 'm'), 0);
        }

        public static int GetTimeUnit(ref string time, char timeUnit)
        {
            var parts = time.Split(timeUnit);
            if (parts.Length > 1)
            {
                time = parts[1];
                return int.Parse(parts[0]);
            }
            else
                return 0;
        }

        public static double ParseAmount(string fraction)
        {
            fraction = fraction.Trim();
            if (fraction.Contains("stick")) {
                fraction = fraction.Replace("stick", "");
            }
            if (fraction.Contains("g")) {
                fraction = fraction.Replace("g", "");
            }
            if (fraction.Contains("inch")) {
                fraction = fraction.Replace("inch", "");
            }

            if (!fraction.Contains('('))
                return GetFractionedNumber(fraction);
            else
            {
                var parts = fraction.Split('(');
                var fractioned = GetFractionedNumber(parts[0]);
                double relative;
                var relativeStr = parts[1].Split(')')[0];//  inCase the number comes in branches
                if (parts[1].Contains('.') && parts[1].Split('.').Length == 1)
                    relative = double.Parse("0" + relativeStr);
                else if (relativeStr.Contains(" ") || relativeStr.Contains("/"))
                    relative  = GetFractionedNumber(relativeStr);
                else if (relativeStr.Contains("to") || relativeStr.Contains("or") )
                {
                    var size = parts[1].Split(new string[2] { "to","or" }, StringSplitOptions.RemoveEmptyEntries)[0]; // range of two numbers
                    
                    relative = double.Parse(size);
                }
                else {
                    relative = double.Parse(relativeStr);
                }
                return fractioned * relative;
            }

        }

        private static double GetFractionedNumber(string fraction) {
            fraction = fraction.Replace("whole", string.Empty);
            fraction = fraction.Trim();

            if (!fraction.Contains('/')) {
                try {
                    return double.Parse(fraction);
                }
                catch(FormatException) {
                    return 1;
                }
                
            }
            else {
                var wholeAndFract = fraction.Split(' ');
                if (wholeAndFract.Length > 2)
                    return 1;
                var num = 0;
                var fractIndex = 0;
                if (wholeAndFract.Length > 1) {
                    num = int.Parse(wholeAndFract[0]);
                    fractIndex = 1;
                }

                var fract = wholeAndFract[fractIndex].Split('/');
                return num + (double.Parse(fract[0]) / double.Parse(fract[1]));
            }
        }
    }
}
