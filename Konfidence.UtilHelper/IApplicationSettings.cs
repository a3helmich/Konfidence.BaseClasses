namespace Konfidence.UtilHelper
{
    public interface IApplicationSettings 
    {
        string GetStringValue(string keyName);
        void SetStringValue(string keyName, string keyValue);
        void Flush();
    }
}