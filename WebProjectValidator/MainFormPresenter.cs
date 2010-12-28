using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;
using System.IO;
using WebProjectValidator.HelperClasses;
using WebProjectValidator.FileListChecker;
using WebProjectValidator.EnumTypes;

namespace WebProjectValidator
{
    class MainFormPresenter: BaseItem
    {
        private string _SolutionFolder = string.Empty;
        private string _ProjectName = string.Empty;
        private LanguageType _LanguageType = LanguageType.Unknown;
        private TabPageType _TabPageType = TabPageType.Unknown;

        private bool _IsWebProjectCheck;

        private bool _IsUserControlValidCheck;
        private bool _IsUserControlInvalidCheck;
        private bool _IsUserControlMissingCheck;
        private bool _IsUserControlUnusedCheck;

        public bool IsUserControlValidCheck
        {
            get { return _IsUserControlValidCheck; }
            set { _IsUserControlValidCheck = value; }
        }

        public bool IsUserControlInvalidCheck
        {
            get { return _IsUserControlInvalidCheck; }
            set { _IsUserControlInvalidCheck = value; }
        }

        public bool IsUserControlMissingCheck
        {
            get { return _IsUserControlMissingCheck; }
            set { _IsUserControlMissingCheck = value; }
        }

        public bool IsUserControlUnusedCheck
        {
            get { return _IsUserControlUnusedCheck; }
            set { _IsUserControlUnusedCheck = value; }
        }

        private int _ProjectFileCount = 0;
        private int _ProjectFileValidCount = 0;
        private int _ProjectFileInvalidCount = 0;
        private int _ProjectFileListCount = 0;
        private List<DesignerFileItem> _ProjectTypeValidationList;

        private int _UserControlMissingCount = 0;
        private int _UserControlMissingValidCount = 0;
        private int _UserControlMissingInvalidCount = 0;
        private int _UserControlMissingListCount = 0;
        private List<DesignerFileItem> _MissingUserControlList;

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

        public TabPageType TabPageType
        {
            get { return _TabPageType; }
            set { _TabPageType = value; }
        }

        public bool IsWebProjectCheck
        {
            get { return _IsWebProjectCheck; }
            set { _IsWebProjectCheck = value; }
        }

        // project counters text
        public string ProjectFileCountText
        {
            get { return "Total: " + _ProjectFileCount; }
        }

        public string ProjectFileValidCountText
        {
            get { return "Valid: " + _ProjectFileValidCount; }
        }

        public string ProjectFileInvalidCountText
        {
            get { return "Invalid: " + _ProjectFileInvalidCount; }
        }

        public string ProjectFileListCountText
        {
            get { return "RowCount: " + _ProjectFileListCount; }
        }
        
        // user control counters text
        public string UserControlMissingCountText
        {
            get { return "Total: " + _UserControlMissingCount; }
        }

        public string UserControlMissingValidCountText
        {
            get { return "Valid: " + _UserControlMissingValidCount; }
        }

        public string UserControlMissingInvalidCountText
        {
            get { return "Invalid: " + _UserControlMissingInvalidCount; }
        }

        public string UserControlMissingListCountText
        {
            get { return "RowCount: " + _UserControlMissingListCount; }
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

        public ListFilterType FilterType
        {
            get
            {
                switch (_TabPageType)
                {
                    case TabPageType.ProjectTypeValidation:
                        {
                            return ListFilterType.Unknown;
                        }
                    case TabPageType.DesignerFileMissing:
                        {
                            if (IsWebProjectCheck)
                            {
                                return ListFilterType.WebProject;
                            }

                            return ListFilterType.WebApplication;
                        }
                    case TabPageType.UserControlMissing:
                        {
                            if (IsUserControlInvalidCheck)
                            {
                                return ListFilterType.UserControlInvalid;
                            }

                            if (IsUserControlValidCheck)
                            {
                                return ListFilterType.UserControlValid;
                            }
                            
                            if (IsUserControlMissingCheck)
                            {
                                return ListFilterType.UserControlMissing;
                            }
                            
                            if (IsUserControlUnusedCheck)
                            {
                                return ListFilterType.UserControlUnused;
                            }

                            return ListFilterType.All;
                        }
                }

                return ListFilterType.Unknown;
            }
        }

        public string ProjectFile
        {
            get
            {
                switch (LanguageType)
                {
                    case LanguageType.cs:
                        {
                            return ProjectFolder + @"\" + ProjectName + ".csproj";
                        }
                    case LanguageType.vb:
                        {
                            return ProjectFolder + @"\" + ProjectName + ".vbproj";
                        }
                }

                return "Unknown projectfile";
            }
        }

        public List<DesignerFileItem> ProjectTypeValidationList
        {
            get
            {
                return _ProjectTypeValidationList;
            }
        }

        public List<DesignerFileItem> MissingUserControlList
        {
            get
            {
                return _MissingUserControlList;
            }
        }
        
        public FileList WebFileList
        {
            get
            {
                return new FileList(ProjectFolder, LanguageType, DeveloperFileType.WebFile);
            }
        }

        public FileList SourceFileList
        {
            get
            {
                return new FileList(ProjectFolder, LanguageType, DeveloperFileType.SourceFile);
            }
        }

        public FileList DesignerFileList
        {
            get
            {
                return new FileList(ProjectFolder, LanguageType, DeveloperFileType.DesignerFile);
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

        public bool ConvertButtonsEnabled ()
        {
            if (TabPageType.Equals(TabPageType.ProjectTypeValidation))
            {
                return true;
            }

            return false;
        }

        public bool ProjectTypeValidationItemVisible()
        {
            if (TabPageType.Equals(TabPageType.ProjectTypeValidation))
            {
                return true;
            }
            return false;
        }

        public bool UserControlMissingValidationItemVisible()
        {
            if (TabPageType.Equals(TabPageType.UserControlMissing))
            {
                return true;
            }
            return false;
        }

        public void ConvertToWebApplication()
        {
            ListProcessor processor = new ListProcessor(ProjectName, ProjectFolder, LanguageType, ProjectFile);

            // TODO : DesignerFileItem -> doesn't feel right
            // TODO : property?
            // web application uses no projectfile -> all files must be converted
            DesignerFileItemList webFileItemList = new DesignerFileItemList(ProjectFolder, WebFileList);

            processor.ConvertToWebApplication(webFileItemList);
        }

        public void ConvertToWebProject()
        {
            ListProcessor processor = new ListProcessor(ProjectName, ProjectFolder, LanguageType, ProjectFile);

            // TODO : just get a list of all projectFiles without any processing
            List<DesignerFileItem> webProjectFileItemList = processor.processCodeFileCheck(WebFileList, FilterType);

            // web project uses a projectfile -> only files included in the project file must be converted
            processor.ConvertToWebProject(webProjectFileItemList);
        }

        internal void ResetCalculatedProperties()
        {
            _ProjectFileCount = 0;
            _ProjectFileValidCount = 0;
            _ProjectFileInvalidCount = 0;
            _ProjectFileListCount = 0;

            _ProjectTypeValidationList = null;

            _UserControlMissingCount = 0;
            _UserControlMissingValidCount = 0;
            _UserControlMissingInvalidCount = 0;
            _UserControlMissingListCount = 0;

            _MissingUserControlList = null;
        }

        public void ProjectTypeValidation()
        {
            ResetCalculatedProperties();

            ListProcessor processor = new ListProcessor(ProjectName, ProjectFolder, LanguageType, ProjectFile);

            // TODO : diferentiate between application and project
            if (IsWebProjectCheck)
            {
                _ProjectTypeValidationList = processor.processCodeFileCheck(WebFileList, FilterType);
            }
            else
            {
                _ProjectTypeValidationList = processor.processCodeFileCheck(WebFileList, FilterType);
            }

            _ProjectFileCount = processor.Count;
            _ProjectFileValidCount = processor.ValidCount;
            _ProjectFileInvalidCount = processor.InvalidCount;
            _ProjectFileListCount = _ProjectTypeValidationList.Count;
        }

        public void UserControlValidation()
        {
            ListProcessor processor = new ListProcessor(ProjectName, ProjectFolder, LanguageType, ProjectFile);

            List<DesignerFileItem> missingUserControlList = processor.processUserControlMissing(WebFileList, FilterType);

            _UserControlMissingCount = processor.Count;
            _UserControlMissingValidCount = processor.ValidCount;
            _UserControlMissingInvalidCount = processor.InvalidCount;
            _UserControlMissingListCount = missingUserControlList.Count;
        }
    }
}
