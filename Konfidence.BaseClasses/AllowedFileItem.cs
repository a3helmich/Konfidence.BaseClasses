using System.Globalization;
using System.IO;

namespace Konfidence.Base
{
    /// <summary>
    /// Summary description for AllowedFileItem
    /// </summary>
    public class AllowedFileItem: BaseItem
    {
        public virtual string AllowedExtensions => "all";

        protected virtual string[] GetAllowedExtensions()
        {
            return null;
        }

        public bool IsAllowedExtension(string fileName)
        {
            var allowedExtensions = GetAllowedExtensions();

            var fileExtension = Path.GetExtension(fileName);

            if (!allowedExtensions.IsAssigned())
            {
                return true;
            }

            foreach (var allowedExtension in allowedExtensions)
            {
                if (string.Compare(fileExtension, allowedExtension, true, CultureInfo.InvariantCulture) == 0)
                {
                    return true;
                }
            }

            return false;
        }	
    }
}
