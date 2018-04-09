using Konfidence.BaseInterfaces;

namespace Konfidence.Base
{
	public class BaseItem : IBaseItem
    {
	    protected static bool UnitTest = false;

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
    }
}
