using System;
using System.Globalization;
using JetBrains.Annotations;

namespace Konfidence.Base
{
    [UsedImplicitly]
    public static class StringExtender
    {
        [NotNull]
        public static string ReplaceIgnoreCase([NotNull] this string fromString, [NotNull] string oldValue, string newValue)
        {
            var fromStringIndex = fromString.IndexOf(oldValue, StringComparison.OrdinalIgnoreCase);

            if (fromStringIndex > -1)
            {
                var toString = fromString.Substring(0, fromStringIndex);

                toString += newValue;
                toString += fromString.Substring(fromStringIndex + oldValue.Length);

                return toString;
            }

            return fromString;
        }

        public static decimal ToDecimal(this string decimalString, decimal defaultValue)
        {
            if (decimalString.IndexOf('.') < 0 && decimalString.IndexOf(',') >= 0)
            {
                decimalString = decimalString.Replace(',', '.');
            }

            if (decimal.TryParse(decimalString, NumberStyles.Currency, CultureInfo.InvariantCulture, out var returnValue1))
            {
                return returnValue1;
            }

            decimalString = decimalString.Replace(',', 'k');
            decimalString = decimalString.Replace('.', ',');
            decimalString = decimalString.Replace('k', '.');

            if (decimal.TryParse(decimalString, NumberStyles.Currency, CultureInfo.InvariantCulture, out var returnValue2))
            {
                return returnValue2;
            }

            return 0; // return default
        }
    }
}
