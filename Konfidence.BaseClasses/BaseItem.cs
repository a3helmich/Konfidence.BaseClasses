using System;
using System.Diagnostics;
using System.Globalization;
using JetBrains.Annotations;

namespace Konfidence.Base
{
	public class BaseItem
	{
	    public static bool UnitTest = false;

        public string ErrorMessage { get; private set; } = string.Empty;

	    // TODO : convert to errorlist 
        public bool SetErrorMessage(string errorMessage)
        {
            ErrorMessage = errorMessage;

            return false;
        }

        public bool HasErrors()
        {
            return ErrorMessage.IsAssigned();
        }

        public void ClearErrorMessage()
        {
            ErrorMessage = string.Empty;
        }

        // string extender van maken
        public string ReplaceIgnoreCase(string fromString, string oldValue, string newValue)
        {
            var toString = fromString;

            var codeBehindIndex = fromString.IndexOf(oldValue, StringComparison.OrdinalIgnoreCase);

            if (codeBehindIndex > -1)
            {
                toString = fromString.Substring(0, codeBehindIndex);
                toString += newValue;
                toString += fromString.Substring(codeBehindIndex + oldValue.Length);
            }

            return toString;
        }

        public decimal ToDecimal(string decimalString,  decimal defaultValue)
        {
            decimal returnValue;

            if (decimalString.IndexOf('.') < 0 && decimalString.IndexOf(',') >= 0)
            {
                decimalString = decimalString.Replace(',', '.');
            }

            if (decimal.TryParse(decimalString, NumberStyles.Currency, CultureInfo.InvariantCulture, out returnValue))
            {
                return returnValue;
            }

            decimalString = decimalString.Replace(',', 'k');
            decimalString = decimalString.Replace('.', ',');
            decimalString = decimalString.Replace('k', '.');

            if (decimal.TryParse(decimalString, NumberStyles.Currency, CultureInfo.InvariantCulture, out returnValue))
            {
                return returnValue;
            }

            return returnValue; // default teruggeven
        }
    }
}
