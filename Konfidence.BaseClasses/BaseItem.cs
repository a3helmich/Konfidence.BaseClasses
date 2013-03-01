using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Konfidence.Base
{
	public class BaseItem
	{
        private string _ErrorMessage = string.Empty;
        public static bool UnitTest = false;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
        }

        [ContractAnnotation("assignedObject:null => false")]
		public static bool IsAssigned(object assignedObject)
		{
            if (Debugger.IsAttached || UnitTest)
            {
                // TODO : write exceptions to a logFile (introduce exclusion attribute?)
                if (assignedObject is string)
                {
                    throw new InvalidCastException("IsAssigned is invalid for strings, use IsNull, IsEmpty or string.IsNullOrEmpty");
                }
            }

            if (assignedObject == null)
            {
                return false;
            }

			return true;
		}

        [ContractAnnotation("assignedString:null => false")]
        public static bool IsNull(string assignedString) // ToDo : back to protected 
        {
            if (assignedString == null)
            {
                return true;
            }

            return false;
        }

        [ContractAnnotation("assignedString:null => false")]
        public static bool IsEmpty(string assignedString) // ToDo : back to protected 
        {
            if (string.IsNullOrEmpty(assignedString))
            {
                return true;
            }

            return false;
        }

        [ContractAnnotation("assignedGuid:null => false")]
        public static bool IsGuid(string assignedGuid)
        {
            try
            {
                // todo : tryparse in framework 4.x
                var test = new Guid(assignedGuid);
            }
            catch
            {
                return false;
            }

            return true;
        }

        // TODO : convert to errorlist 
        public bool SetErrorMessage(string errorMessage)
        {
            _ErrorMessage = errorMessage;

            return false;
        }

        public bool HasErrors()
        {
            return !IsEmpty(_ErrorMessage);
        }

        public void ClearErrorMessage()
        {
            _ErrorMessage = string.Empty;
        }

        // string extender van maken
        public string ReplaceIgnoreCase(string fromString, string oldValue, string newValue)
        {
            string toString = fromString;

            int codeBehindIndex = fromString.IndexOf(oldValue, StringComparison.InvariantCultureIgnoreCase);

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

            if (decimal.TryParse(decimalString, System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.InvariantCulture, out returnValue))
            {
                return returnValue;
            }

            decimalString = decimalString.Replace(',', 'k');
            decimalString = decimalString.Replace('.', ',');
            decimalString = decimalString.Replace('k', '.');

            if (decimal.TryParse(decimalString, System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.InvariantCulture, out returnValue))
            {
                return returnValue;
            }

            return returnValue; // default teruggeven
        }
    }
}
