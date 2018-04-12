namespace Konfidence.BaseClassInterfaces
{
    public interface IBaseItem
    {
        string ErrorMessage { get; }

        bool SetErrorMessage(string errorMessage);

        bool HasErrors();

        void ClearErrorMessage();
    }
}
