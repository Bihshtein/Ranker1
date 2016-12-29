using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Expressions
    {

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
