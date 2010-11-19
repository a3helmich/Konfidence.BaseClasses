using System;
namespace Konfidence.Base
{
	public class BaseItem
	{
        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
        }

		public static bool IsAssigned(object assignedObject)
		{
            if (assignedObject == null)
            {
                return false;
            }

			return true;
		}

        public static bool IsAssigned(string newString) // ToDo : back to protected 
        {
            if (newString == null)
            {
                return false;
            }

            return true;
        }

        public static bool IsEmpty(string newString) // ToDo : back to protected 
        {
            if (string.IsNullOrEmpty(newString))
            {
                return true;
            }

            return false;
        }

        public void SetErrorMessage(string errorMessage)
        {
            _ErrorMessage = errorMessage;
        }

        public bool HasErrors()
        {
            return IsAssigned(_ErrorMessage);
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
	}
}
