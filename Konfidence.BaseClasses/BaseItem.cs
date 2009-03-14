namespace Konfidence.Base
{
	public class BaseItem
	{
		public static bool IsAssigned(object assignedObject)
		{
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
                return true;
            }

            return false;
        }
	}
}
