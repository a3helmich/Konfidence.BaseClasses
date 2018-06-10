using System.Globalization;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace Konfidence.Base
{
    /// <summary>
    /// Summary description for AllowedFileItem
    /// </summary>
    public class AllowedFileItem
    {
        [UsedImplicitly]
        public virtual string AllowedExtensions => "all";

        protected virtual string[] GetAllowedExtensions()
        {
            return null;
        }

        [UsedImplicitly]
        public bool IsAllowedExtension(string fileName)
        {
            var allowedExtensions = GetAllowedExtensions();

            var fileExtension = Path.GetExtension(fileName);

            if (!allowedExtensions.IsAssigned())
            {
                return true;
            }

            return allowedExtensions.Any(allowedExtension => string.Compare(fileExtension, allowedExtension, true, CultureInfo.InvariantCulture) == 0);
        }	
    }
}
