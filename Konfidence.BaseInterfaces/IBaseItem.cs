namespace Konfidence.BaseInterfaces
{
    public interface IBaseItem
    {
        string ErrorMessage { get; }

        bool SetErrorMessage(string errorMessage);

        bool HasErrors();

        void ClearErrorMessage();
    }
}
