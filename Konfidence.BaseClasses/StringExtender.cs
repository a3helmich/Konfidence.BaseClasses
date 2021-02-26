using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;

namespace Konfidence.Base
{
    [UsedImplicitly]
    public static class StringExtender
    {
        [NotNull]
        [UsedImplicitly]
        public static string TrimStart(this string line, [NotNull] string trimPart, bool leaveWhiteSpace = false)
        {
            if (trimPart.IsAssigned() && line.Length >= trimPart.Length && line.StartsWith(trimPart))
            {
                line = line.Substring(trimPart.Length);

                if (!leaveWhiteSpace)
                {
                    line = line.TrimStart();
                }
            }

            return line;
        }

        [NotNull]
        [UsedImplicitly]
        public static string TrimStartIgnoreCase(this string line, [NotNull] string trimPart, bool leaveWhiteSpace = false)
        {
            if (trimPart.IsAssigned() && line.Length >= trimPart.Length && line.StartsWith(trimPart, StringComparison.CurrentCultureIgnoreCase))
            {
                line = line.Substring(trimPart.Length);

                if (!leaveWhiteSpace)
                {
                    line = line.TrimStart();
                }
            }

            return line;
        }

        [NotNull]
        [UsedImplicitly]
        public static string TrimEnd(this string line, [NotNull] string trimPart, bool leaveWhiteSpace = false)
        {
            if (trimPart.IsAssigned() && line.Length >= trimPart.Length && line.EndsWith(trimPart))
            {
                line = line.Substring(0, line.Length - trimPart.Length);

                if (!leaveWhiteSpace)
                {
                    line = line.TrimEnd();
                }
            }

            return line;
        }

        [NotNull]
        [UsedImplicitly]
        public static string TrimEndIgnoreCase(this string line, [NotNull] string trimPart, bool leaveWhiteSpace = false)
        {
            if (trimPart.IsAssigned() && line.Length >= trimPart.Length && line.EndsWith(trimPart, StringComparison.InvariantCultureIgnoreCase))
            {
                line = line.Substring(0, line.Length - trimPart.Length);

                if (!leaveWhiteSpace)
                {
                    line = line.TrimEnd();
                }
            }

            return line;
        }

        [NotNull]
        [UsedImplicitly]
        public static List<string> TrimList([NotNull] this IEnumerable<string> lines)
        {
            return lines.Select(x => x.Trim()).ToList();
        }

        [NotNull]
        [UsedImplicitly]
        public static string ReplaceIgnoreCase([NotNull] this string fromString, [NotNull] string oldValue, string newValue)
        {
            var replaceFromIndex = fromString.IndexOf(oldValue, StringComparison.OrdinalIgnoreCase);

            return replaceFromIndex > -1
                ? $"{fromString.Substring(0, replaceFromIndex)}{newValue}{fromString.Substring(replaceFromIndex + oldValue.Length)}"
                : fromString;
        }

        [UsedImplicitly]
        public static decimal ToDecimal(this string decimalString, decimal defaultValue = 0)
        {
            if (decimalString.IndexOf('.') < 0 && decimalString.IndexOf(',') >= 0)
            {
                decimalString = decimalString.Replace(',', '.');
            }

            if (decimal.TryParse(decimalString, NumberStyles.Currency, CultureInfo.InvariantCulture, out var returnValue))
            {
                return returnValue;
            }

            decimalString = decimalString.Replace(',', 'k');
            decimalString = decimalString.Replace('.', ',');
            decimalString = decimalString.Replace('k', '.');

            return decimal.TryParse(decimalString, NumberStyles.Currency, CultureInfo.InvariantCulture, out returnValue)
                ? returnValue
                : defaultValue;
        }
    }
}
