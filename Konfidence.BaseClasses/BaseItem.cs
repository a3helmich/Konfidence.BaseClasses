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

        public static bool IsString(string newString) // ToDo : back to protected 
        {
            if (string.IsNullOrEmpty(newString))
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
