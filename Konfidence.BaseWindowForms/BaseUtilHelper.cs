using System.Text.RegularExpressions;


namespace Konfidence.BaseWindowForms
{
    /// <summary>
    /// Summary description for NzbUtilHelper.
    /// </summary>
    abstract public class BaseUtilHelper
    {
        public static bool IsValidEmail(string eMail)
        {
            var regex = new Regex(".+@.+\\.[a-z]+");
            var match = regex.Match(eMail);

            return match.Success;
        }
    }
}
