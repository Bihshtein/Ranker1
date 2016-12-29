using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class ParseHelpers
    {
        public static string GetWithoutLast_ES_letters(string str)
        {
            var length = str.Length;
            if (str[length - 1] == 's' && str[length - 2] == 'e')
                return str.Remove(length - 2, 2);
            return str;
        }

        public static string GetWithoutLast_S_letter(string str)
        {
            var length = str.Length;
            if (str[length - 1] == 's')
                return str.Remove(length - 1);
            return str;
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

            if (!fraction.Contains('('))
                return GetFractionedNumber(fraction);
            else
            {
                var parts = fraction.Split('(');
                var fractioned = GetFractionedNumber(parts[0]);
                double relative;
                if (parts[1].Contains('.'))
                    relative = double.Parse("0" + parts[1]);
                else
                {
                    var size = parts[1].Split(new string[1] { "to" }, StringSplitOptions.RemoveEmptyEntries)[0]; // range of two numbers
                    relative = double.Parse(size);
                }
                return fractioned * relative;
            }

        }

        private static double GetFractionedNumber(string fraction)
        {
            fraction = fraction.Replace("whole", string.Empty);
            fraction = fraction.Trim();

            if (!fraction.Contains('/'))
                return double.Parse(fraction);
            else
            {
                var wholeAndFract = fraction.Split(' ');
                var num = 0;
                var fractIndex = 0;
                if (wholeAndFract.Length > 1)
                {
                    num = int.Parse(wholeAndFract[0]);
                    fractIndex = 1;
                }

                var fract = wholeAndFract[fractIndex].Split('/');
                return num + (double.Parse(fract[0]) / double.Parse(fract[1]));
            }
        }
    }
}
