using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;
using System.IO;
using WebProjectValidator.HelperClasses;
using WebProjectValidator.FileListChecker;

namespace WebProjectValidator
{
    class MainFormPresenter: BaseItem
    {
        private string _SolutionFolder = string.Empty;
        private string _ProjectName = string.Empty;
        private LanguageType _LanguageType = LanguageType.Unknown;
        private TabPageType _TabPageType = TabPageType.Unknown;

#region simple properties
        public string SolutionFolder
        {
            get  {  return _SolutionFolder;  }
            set 
            {
                _SolutionFolder = value;

                if (!_SolutionFolder.EndsWith(@"\"))
                {
                    _SolutionFolder += @"\";
                }
            }
        }

        public string ProjectFolder
        {
            get { return SolutionFolder + ProjectName; }
        }

        public string ProjectName
        {
            get { return _ProjectName; }
            set{ _ProjectName = value; }
        }

        public LanguageType LanguageType
        {
            get { return _LanguageType; }
            set { _LanguageType = value; }
        }

        public LanguageFileType LanguageFileType
        {
            get
            {
                switch (LanguageType)
                {
                    case LanguageType.cs:
                        return LanguageFileType.cs;
                    case LanguageType.vb:
                        return LanguageFileType.vb;
                }
                return LanguageFileType.Unknown;
            }
        }

        public TabPageType TabPageType
        {
            get { return _TabPageType; }
            set { _TabPageType = value; }
        }

#endregion simple properties

        public bool IsCS
        {
            get {
                if (LanguageType == LanguageType.cs)
                {
                    return true;
                }
                return false; 
            }
            set 
            {
                LanguageType = LanguageType.vb;

                if (value)
                {
                    LanguageType = LanguageType.cs;
                }

            }
        }

        public bool IsVB
        {
            get { return !IsCS; }
        }

        public string ProjectFile
        {
            get
            {
                switch (LanguageType)
                {
                    case LanguageType.cs:
                        return ProjectFolder + @"\" + ProjectName + ".csproj";
                    case LanguageType.vb:
                        return ProjectFolder + @"\" + ProjectName + ".vbproj";
                }

                return "Unknown projectfile";
            }
        }

        public MainFormPresenter()
        {
            LoadDefaults();
        }

        public bool Validate()
        {
            ClearErrorMessage();

            if (IsEmpty(_SolutionFolder))
            {
                SetErrorMessage("Selecteer een solution folder!");
            }
            else
            {
                if (!Directory.Exists(ProjectFolder))
                {
                    SetErrorMessage("Folder: '" + ProjectFolder + "' bestaat niet!");
                }
                else
                {
                    if (!File.Exists(ProjectFile))
                    {
                        SetErrorMessage("Projectfile: '" + ProjectFile + "' niet gevonden!");
                    }
                }
            }

            return !HasErrors();
        }

        public void Close()
        {
            SaveDefaults();
        }

        private void LoadDefaults()
        {
            ConfigurationStore configurationStore = new ConfigurationStore();

            string getText = string.Empty;

            configurationStore.GetProperty("ProjectName", out getText);

            ProjectName = getText;

            configurationStore.GetProperty("ProjectFolder", out getText);

            SolutionFolder = getText;

            configurationStore.GetProperty("rbCSChecked", out getText);

            LanguageType = LanguageType.vb;
            if (getText.Equals("1"))
            {
                LanguageType = LanguageType.cs;
            }
        }

        private void SaveDefaults()
        {
            ConfigurationStore configurationStore = new ConfigurationStore();

            configurationStore.SetProperty("ProjectName", ProjectName);
            configurationStore.SetProperty("ProjectFolder", SolutionFolder);

            string rbCSText = "0";
            if (LanguageType == LanguageType.cs)
            {
                rbCSText = "1";
            }

            configurationStore.SetProperty("rbCSChecked", rbCSText);

            configurationStore.Save();
        }



        public bool ConvertButtonsEnabled 
        {
            get
            {
                if (TabPageType.Equals(TabPageType.CodeFileCheck))
                {
                    return true;
                }

                return false;
            }
        }
    }
}
