using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Konfidence.BaseWindowForms
{
    /// <summary>
    /// Summary description for NzbUtilHelper.
    /// </summary>
    public abstract class BaseUtilHelper
    {
        public static bool IsValidEmail([NotNull] string eMail)
        {
            var regex = new Regex(".+@.+\\.[a-z]+");
            var match = regex.Match(eMail);

            return match.Success;
        }
    }
}
