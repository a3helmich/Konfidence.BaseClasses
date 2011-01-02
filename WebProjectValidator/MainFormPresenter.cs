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
        private TabPageType _TabPageType = TabPageType.DesignerFileMissing;

        private bool _IsDesignerFileExistsCheck;
        private bool _IsDesignerFileMissingCheck;

        private bool _IsWebProjectCheck;

        private bool _IsUserControlValidCheck;
        private bool _IsUserControlInvalidCheck;
        private bool _IsUserControlMissingCheck;
        private bool _IsUserControlUnusedCheck;

        private int _DesignerFileCount = 0;
        private int _DesignerFileMissingCount = 0;
        private int _DesignerFileExistsCount = 0;
        private int _DesignerFileListCount = 0;

        private List<DesignerFileItem> _DesignerFileMissingList;

        private int _ProjectFileCount = 0;
        private int _ProjectFileValidCount = 0;
        private int _ProjectFileInvalidCount = 0;
        private int _ProjectFileListCount = 0;

        private List<DesignerFileItem> _ProjectTypeValidationList;

        private int _UserControlMissingCount = 0;
        private int _UserControlMissingValidCount = 0;
        private int _UserControlMissingInvalidCount = 0;
        private int _UserControlMissingListCount = 0;

        private List<DesignerFileItem> _UserControlMissingList;

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

        public bool IsDesignerFileExistsCheck
        {
            get { return _IsDesignerFileExistsCheck; }
            set { _IsDesignerFileExistsCheck = value; }
        }

        public bool IsDesignerFileMissingCheck
        {
            get { return _IsDesignerFileMissingCheck; }
            set { _IsDesignerFileMissingCheck = value; }
        }

        public bool IsWebProjectCheck
        {
            get { return _IsWebProjectCheck; }
            set { _IsWebProjectCheck = value; }
        }

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

        // DesignerFile counters text
        public string DesignerFileCountText
        {
            get { return "Total: " + _DesignerFileCount; }
        }

        public string DesignerFileMissingCountText
        {
            get { return "Existing: " + _DesignerFileMissingCount; }
        }

        public string DesignerFileExistsCountText
        {
            get { return "Missing: " + _DesignerFileExistsCount; }
        }

        public string DesignerFileListCountText
        {
            get { return "RowCount: " + _DesignerFileListCount; }
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

        public List<DesignerFileItem> DesignerFileMissingList
        {
            get
            {
                return _DesignerFileMissingList;
            }
        }

        public List<DesignerFileItem> ProjectTypeValidationList
        {
            get
            {
                return _ProjectTypeValidationList;
            }
        }

        public List<DesignerFileItem> UserControlMissingList
        {
            get
            {
                return _UserControlMissingList;
            }
        }

        public bool ConvertButtonsEnabled()
        {
            if (TabPageType.Equals(TabPageType.ProjectTypeValidation))
            {
                return true;
            }

            return false;
        }

        public bool DesignerFileMissingItemVisible()
        {
            if (TabPageType.Equals(TabPageType.DesignerFileMissing))
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

        private ProcessActionType ActionType
        {
            get
            {
                switch (_TabPageType)
                {
                    case TabPageType.DesignerFileMissing:
                        {
                            if (IsDesignerFileExistsCheck)
                            {
                                return ProcessActionType.DesignerFileExists;
                            }

                            if (IsDesignerFileMissingCheck)
                            {
                                return ProcessActionType.DesignerFileMissing;
                            }

                            return ProcessActionType.All;
                        }
                    case TabPageType.ProjectTypeValidation:
                        {
                            if (IsWebProjectCheck)
                            {
                                return ProcessActionType.WebProject;
                            }

                            return ProcessActionType.WebApplication;
                        }
                    case TabPageType.UserControlMissing:
                        {
                            if (IsUserControlInvalidCheck)
                            {
                                return ProcessActionType.UserControlInvalid;
                            }

                            if (IsUserControlValidCheck)
                            {
                                return ProcessActionType.UserControlValid;
                            }

                            if (IsUserControlMissingCheck)
                            {
                                return ProcessActionType.UserControlMissing;
                            }

                            if (IsUserControlUnusedCheck)
                            {
                                return ProcessActionType.UserControlUnused;
                            }

                            return ProcessActionType.All;
                        }
                }

                return ProcessActionType.Unknown;
            }
        }
        
        private FileList WebFileList
        {
            get
            {
                return new FileList(ProjectFolder, LanguageType, DeveloperFileType.WebFile);
            }
        }

        private FileList SourceFileList
        {
            get
            {
                return new FileList(ProjectFolder, LanguageType, DeveloperFileType.SourceFile);
            }
        }

        private FileList DesignerFileList
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

        private bool Validate()
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
            List<DesignerFileItem> webProjectFileItemList = processor.processCodeFileCheck(WebFileList, ActionType);

            // web project uses a projectfile -> only files included in the project file must be converted
            processor.ConvertToWebProject(webProjectFileItemList);
        }

        private void ResetCalculatedProperties()
        {
            _DesignerFileListCount = 0;
            _DesignerFileMissingCount = 0;
            _DesignerFileExistsCount = 0;
            _DesignerFileCount = 0;

            _DesignerFileMissingList = null;

            _ProjectFileCount = 0;
            _ProjectFileValidCount = 0;
            _ProjectFileInvalidCount = 0;
            _ProjectFileListCount = 0;

            _ProjectTypeValidationList = null;

            _UserControlMissingCount = 0;
            _UserControlMissingValidCount = 0;
            _UserControlMissingInvalidCount = 0;
            _UserControlMissingListCount = 0;

            _UserControlMissingList = null;
        }

        private void DesignerFileValidation()
        {
            ListProcessor processor = new ListProcessor(ProjectName, ProjectFolder, LanguageType, ProjectFile);

            _DesignerFileMissingList = processor.processDesignerFileMissing(SourceFileList, DesignerFileList, ActionType);

            _DesignerFileCount = processor.Count;
            _DesignerFileMissingCount = processor.ValidCount;
            _DesignerFileExistsCount = processor.InvalidCount;
            _DesignerFileListCount = _DesignerFileMissingList.Count;
        }

        private void ProjectTypeValidation()
        {
            ListProcessor processor = new ListProcessor(ProjectName, ProjectFolder, LanguageType, ProjectFile);

            // TODO : diferentiate between application and project
            if (IsWebProjectCheck)
            {
                _ProjectTypeValidationList = processor.processCodeFileCheck(WebFileList, ActionType);
            }
            else
            {
                _ProjectTypeValidationList = processor.processCodeFileCheck(WebFileList, ActionType);
            }

            _ProjectFileCount = processor.Count;
            _ProjectFileValidCount = processor.ValidCount;
            _ProjectFileInvalidCount = processor.InvalidCount;
            _ProjectFileListCount = _ProjectTypeValidationList.Count;
        }

        private void UserControlValidation()
        {
            ListProcessor processor = new ListProcessor(ProjectName, ProjectFolder, LanguageType, ProjectFile);

            _UserControlMissingList = processor.processUserControlMissing(WebFileList, ActionType);

            _UserControlMissingCount = processor.Count;
            _UserControlMissingValidCount = processor.ValidCount;
            _UserControlMissingInvalidCount = processor.InvalidCount;
            _UserControlMissingListCount = _UserControlMissingList.Count;
        }

        public bool Execute()
        {
            ResetCalculatedProperties();

            if (Validate())
            {
                switch (TabPageType)
                {
                    case TabPageType.ProjectTypeValidation:
                        ProjectTypeValidation();
                        break;
                    case TabPageType.DesignerFileMissing:
                        DesignerFileValidation();
                        break;
                    case TabPageType.UserControlMissing:
                        UserControlValidation();
                        break;
                }
                return true;
            }
            return false;
        }

        public bool IsValidTag(object tabTag)
        {
            if (IsAssigned(tabTag))
            {
                try
                {
                    switch ((TabPageType)tabTag)
                    {
                        case EnumTypes.TabPageType.DesignerFileMissing:
                        case EnumTypes.TabPageType.ProjectTypeValidation:
                        case EnumTypes.TabPageType.UserControlMissing:
                        case EnumTypes.TabPageType.Unknown:
                            return true;
                    }
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }
    }
}
