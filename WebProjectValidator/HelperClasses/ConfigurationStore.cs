using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebProjectValidator.EnumTypes;

namespace WebProjectValidator.HelperClasses
{
    class ConfigurationStore : BaseConfigurationStore
    {
        string _ProjectName = string.Empty;
        string _ProjectFolder = string.Empty;
        LanguageType _LanguageType = LanguageType.cs;

        public string ProjectName
        {
            get
            {
                GetProperty("ProjectName", out _ProjectName);

                return _ProjectName;
            }
            set
            {
                SetProperty("ProjectName", value);
            }
        }

        public string ProjectFolder
        {
            get
            {
                GetProperty("ProjectFolder", out _ProjectFolder);

                return _ProjectFolder;
            }
            set
            {
                SetProperty("ProjectFolder", value);
            }
        }

        public LanguageType LanguageType
        {
            get
            {
                GetProperty("LanguageType", out _LanguageType);

                return _LanguageType;
            }
            set
            {
                SetProperty("LanguageType", value.ToString());
            }
        }

        private void GetProperty(string name, out LanguageType value)
        {
            string nodeValue = GetNodeValue(name);

            try
            {
                value = (LanguageType)Enum.Parse(typeof(LanguageType), nodeValue);
            }
            catch
            {
                value = LanguageType.cs;
            }
        }
    }
}
