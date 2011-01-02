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
        private TabPageType _TabPageType = TabPageType.DesignerFileValidation;

        private bool _IsDesignerFileExistsCheck;
        private bool _IsDesignerFileMissingCheck;

        private bool _IsWebProjectCheck;

        private bool _IsUserControlValidCheck;
        private bool _IsUserControlInvalidCheck;
        private bool _IsUserControlMissingCheck;
        private bool _IsUserControlUnusedCheck;

        private List<DesignerFileItem> _EmptyDesignerFileList = new List<DesignerFileItem>();

        private int _DesignerFileCount = 0;
        private int _DesignerFileMissingCount = 0;
        private int _DesignerFileExistsCount = 0;

        private List<DesignerFileItem> _DesignerFileValidationList;

        private ProcessActionResult _ProjectTypeValidationResult = new ProcessActionResult();

        private ProcessActionResult _UserControlValidationResult = new ProcessActionResult();

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
            get { return "RowCount: " + _DesignerFileValidationList.Count; }
        }

        // project counters text
        public string ProjectFileCountText
        {
            get { return "Total: " + _ProjectTypeValidationResult.Count; }
        }

        public string ProjectFileValidCountText
        {
            get { return "Valid: " + _ProjectTypeValidationResult.ValidCount; }
        }

        public string ProjectFileInvalidCountText
        {
            get { return "Invalid: " + _ProjectTypeValidationResult.InvalidCount; }
        }

        public string ProjectFileListCountText
        {
            get { return "RowCount: " + _ProjectTypeValidationResult.DesignerFileItemList.Count; }
        }
        
        // user control counters text
        public string UserControlCountText
        {
            get { return "Total: " + _UserControlValidationResult.Count; }
        }

        public string UserControlValidCountText
        {
            get { return "Valid: " + _UserControlValidationResult.ValidCount; }
        }

        public string UserControlInvalidCountText
        {
            get { return "Invalid: " + _UserControlValidationResult.InvalidCount; }
        }

        public string UserControlListCountText
        {
            get { return "RowCount: " + _UserControlValidationResult.DesignerFileItemList.Count; }
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

        public List<DesignerFileItem> DesignerFileValidationList
        {
            get
            {
                return _DesignerFileValidationList;
            }
        }

        public List<DesignerFileItem> ProjectTypeValidationList
        {
            get
            {
                return _ProjectTypeValidationResult.DesignerFileItemList;
            }
        }

        public List<DesignerFileItem> UserControlValidationList
        {
            get
            {
                return _UserControlValidationResult.DesignerFileItemList;
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

        public bool DesignerFileValidationItemVisible()
        {
            if (TabPageType.Equals(TabPageType.DesignerFileValidation))
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

        public bool UserControlValidationItemVisible()
        {
            if (TabPageType.Equals(TabPageType.UserControlValidation))
            {
                return true;
            }
            return false;
        }

        private ProcessActionType ActionType
        {
            get
            {
                switch (TabPageType)
                {
                    case TabPageType.DesignerFileValidation:
                        {
                            return GetDesignerFileActionType();
                        }
                    case TabPageType.ProjectTypeValidation:
                        {
                            return GetProjectTypeActionType();
                        }
                    case TabPageType.UserControlValidation:
                        {
                            return GetUserControlActionType();
                        }
                }

                return ProcessActionType.None;
            }
        }

        private ProcessActionType GetUserControlActionType()
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

            return ProcessActionType.UserControlAll;
        }

        private ProcessActionType GetProjectTypeActionType()
        {
            if (IsWebProjectCheck)
            {
                return ProcessActionType.WebProject;
            }

            return ProcessActionType.WebApplication;
        }

        private ProcessActionType GetDesignerFileActionType()
        {
            if (IsDesignerFileExistsCheck)
            {
                return ProcessActionType.DesignerFileExists;
            }

            if (IsDesignerFileMissingCheck)
            {
                return ProcessActionType.DesignerFileMissing;
            }

            return ProcessActionType.DesignerFileAll;
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

            ProjectName = configurationStore.ProjectName;
            SolutionFolder = configurationStore.ProjectFolder;
            LanguageType = configurationStore.LanguageType;
        }

        private void SaveDefaults()
        {
            ConfigurationStore configurationStore = new ConfigurationStore();

            configurationStore.ProjectName = ProjectName;
            configurationStore.ProjectFolder = SolutionFolder;
            configurationStore.LanguageType = LanguageType;

            configurationStore.Save();
        }

        public void ConvertToWebApplication()
        {
            ListProcessor processor = ListProcessor.GetProcessor(ProjectName, ProjectFolder, ProjectFile, LanguageType);

            // TODO : DesignerFileItem -> doesn't feel right
            // TODO : property?
            // web application uses no projectfile -> all files must be converted
            DesignerFileItemList webFileItemList = new DesignerFileItemList(ProjectFolder, WebFileList);

            processor.ConvertToWebApplication(webFileItemList);
        }

        public void ConvertToWebProject()
        {
            ListProcessor processor = ListProcessor.GetProcessor(ProjectName, ProjectFolder, ProjectFile, LanguageType);

            // TODO : just get a list of all projectFiles without any processing
            _ProjectTypeValidationResult = processor.ProcessProjectTypeValidation(ActionType);

            // web project uses a projectfile -> only files included in the project file must be converted
            processor.ConvertToWebProject(_ProjectTypeValidationResult.DesignerFileItemList);
        }

        private void ResetCalculatedProperties()
        {
            _DesignerFileMissingCount = 0;
            _DesignerFileExistsCount = 0;
            _DesignerFileCount = 0;

            _DesignerFileValidationList = _EmptyDesignerFileList;

            _ProjectTypeValidationResult = new ProcessActionResult();
            _UserControlValidationResult = new ProcessActionResult();
        }

        private void DesignerFileValidation()
        {
            ListProcessor processor = ListProcessor.GetProcessor(ProjectName, ProjectFolder, ProjectFile, LanguageType);

            _DesignerFileValidationList = processor.ProcessDesignerFileValidation(SourceFileList, DesignerFileList, ActionType);

            _DesignerFileCount = processor.Count;
            _DesignerFileMissingCount = processor.ValidCount;
            _DesignerFileExistsCount = processor.InvalidCount;
        }

        private void ProjectTypeValidation()
        {
            MainFormController mainFormController = new MainFormController(ProjectName, ProjectFolder, ProjectFile, LanguageType);

            _ProjectTypeValidationResult = mainFormController.Execute(ActionType);
        }

        private void UserControlValidation()
        {
            MainFormController mainFormController = new MainFormController(ProjectName, ProjectFolder, ProjectFile, LanguageType);

            _ProjectTypeValidationResult = mainFormController.Execute(ActionType);
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
                    case TabPageType.DesignerFileValidation:
                        DesignerFileValidation();
                        break;
                    case TabPageType.UserControlValidation:
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
                        case EnumTypes.TabPageType.DesignerFileValidation:
                        case EnumTypes.TabPageType.ProjectTypeValidation:
                        case EnumTypes.TabPageType.UserControlValidation:
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
