using System;
using System.Globalization;

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
            var codeBehindIndex = fromString.IndexOf(oldValue, StringComparison.OrdinalIgnoreCase);

            if (codeBehindIndex > -1)
            {
                var toString = fromString.Substring(0, codeBehindIndex);

                toString += newValue;
                toString += fromString.Substring(codeBehindIndex + oldValue.Length);

                return toString;
            }

            return fromString;
        }

        public decimal ToDecimal(string decimalString,  decimal defaultValue)
        {
            if (decimalString.IndexOf('.') < 0 && decimalString.IndexOf(',') >= 0)
            {
                decimalString = decimalString.Replace(',', '.');
            }

            decimal returnValue;

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
