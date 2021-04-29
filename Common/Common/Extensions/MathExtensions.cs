using System;
using System.Collections.Generic;
using System.Text;

namespace Zero99Lotto.SRC.Common.Extensions
{
    public static class MathExtensions
    {
        public static double RoundToSignificantDigits(this double value, int digits)
        {
            if (value == 0)
                return 0;

            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(value))) + 1);
            return scale * Math.Round(value / scale, digits);
        }

        public static decimal RoundToSignificantDigits(this decimal value, int digits)
        {
            double dValue = (double)value;
            double dRound = dValue.RoundToSignificantDigits(digits);
            return (decimal) dRound;
        }
    }
}
