using System.Globalization;
using System.IO;

namespace Konfidence.Base
{
    /// <summary>
    /// Summary description for AllowedFileItem
    /// </summary>
    public class AllowedFileItem: BaseItem
    {
        public virtual string AllowedExtensions
        {
            get
            {
                return "all";
            }
        }

        protected virtual string[] GetAllowedExtensions()
        {
            return null;
        }

        public bool IsAllowedExtension(string fileName)
        {
            string[] allowedExtensions = GetAllowedExtensions();

            string fileExtension = Path.GetExtension(fileName);

            if (!IsAssigned(allowedExtensions))
            {
                return true;
            }

            foreach (string allowedExtension in allowedExtensions)
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
