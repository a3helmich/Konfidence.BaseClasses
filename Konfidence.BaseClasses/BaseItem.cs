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
            if (assignedObject is string)
            {
                throw new Exception("Een string mag niet met IsAssigned getest worden!");
            }

            if (assignedObject == null)
            {
                return false;
            }

			return true;
		}

        public static bool IsEmpty(string newString) // ToDo : back to protected 
        {
            if (string.IsNullOrEmpty(newString))
            {
                return false;
            }

            return true;
        }

        public void SetErrorMessage(string errorMessage)
        {
            _ErrorMessage = errorMessage;
        }

        public bool HasErrors()
        {
            return IsAssigned(_ErrorMessage);
        }
	}
}
