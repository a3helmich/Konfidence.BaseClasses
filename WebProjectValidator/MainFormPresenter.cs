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
        private TabPageType _TabPageType = TabPageType.ProjectFileValidation;

        private bool _IsDesignerFileExistsChecked = false;
        private bool _IsInProjectFileChecked = false;

        private bool _IsWebProjectCheck = false;

        private bool _IsUserControlValidCheck = false;
        private bool _IsUserControlInvalidCheck = false;
        private bool _IsUserControlMissingCheck = false;
        private bool _IsUserControlUnusedCheck = false;

        private ProcessActionResult _ProcessActionResult = new ProcessActionResult();

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

        public bool IsDesignerFileExistsChecked
        {
            get { return _IsDesignerFileExistsChecked; }
            set { _IsDesignerFileExistsChecked = value; }
        }

        public bool IsInProjectFileChecked
        {
            get { return _IsInProjectFileChecked; }
            set { _IsInProjectFileChecked = value; }
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
            get { return "Total: " + _ProcessActionResult.DesignerFileCount; }
        }

        public string DesignerFileMissingCountText
        {
            get { return "Existing: " + _ProcessActionResult.DesignerFileInvalidCount; }
        }

        public string DesignerFileExistsCountText
        {
            get { return "Missing: " + _ProcessActionResult.DesignerFileValidCount; }
        }

        public string DesignerFileListCountText
        {
            get { return "RowCount: " + _ProcessActionResult.DesignerFileDeveloperItemList.Count; }
        }

        // project counters text
        public string ProjectFileCountText
        {
            get { return "Total: " + _ProcessActionResult.ProjectTypeCount; }
        }

        public string ProjectFileValidCountText
        {
            get { return "Valid: " + _ProcessActionResult.ProjectTypeValidCount; }
        }

        public string ProjectFileInvalidCountText
        {
            get { return "Invalid: " + _ProcessActionResult.ProjectTypeInvalidCount; }
        }

        public string ProjectFileListCountText
        {
            get { return "RowCount: " + _ProcessActionResult.ProjectTypeDeveloperItemList.Count; }
        }
        
        // user control counters text
        public string UserControlCountText
        {
            get { return "Total: " + _ProcessActionResult.UserControlCount; }
        }

        public string UserControlValidCountText
        {
            get { return "Valid: " + _ProcessActionResult.UserControlValidCount; }
        }

        public string UserControlInvalidCountText
        {
            get { return "Invalid: " + _ProcessActionResult.UserControlInvalidCount; }
        }

        public string UserControlListCountText
        {
            get { return "RowCount: " + _ProcessActionResult.UserControlDeveloperItemList.Count; }
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

        public List<ApplicationFileItem> ProjectFileValidationList
        {
            get
            {
                return _ProcessActionResult.DesignerFileDeveloperItemList;
            }
        }

        public List<ApplicationFileItem> ProjectTypeValidationList
        {
            get
            {
                return _ProcessActionResult.ProjectTypeDeveloperItemList;
            }
        }

        public List<ApplicationFileItem> UserControlValidationList
        {
            get
            {
                return _ProcessActionResult.UserControlDeveloperItemList;
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

        public bool ProjectFileValidationItemVisible()
        {
            if (TabPageType.Equals(TabPageType.ProjectFileValidation))
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
                    case TabPageType.ProjectFileValidation:
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

            return ProcessActionType.Website;
        }

        private ProcessActionType GetDesignerFileActionType()
        {
            if (IsDesignerFileExistsChecked)
            {
                return ProcessActionType.DesignerFileExists;
            }

            if (IsInProjectFileChecked)
            {
                return ProcessActionType.InProjectFile;
            }

            return ProcessActionType.None;
        }

        private FileList WebFileList
        {
            get
            {
                return new FileList(ProjectFolder, LanguageType, DeveloperFileType.WebFile);
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
                return SetErrorMessage("Selecteer een solution folder!");
            }

            if (!Directory.Exists(ProjectFolder))
            {
                return SetErrorMessage("Folder: '" + ProjectFolder + "' bestaat niet!");
            }

            if (!File.Exists(ProjectFile))
            {
                return SetErrorMessage("Projectfile: '" + ProjectFile + "' niet gevonden!");
            }

            return true;
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

        public bool ExecuteEvent(ExecuteEventType executeEventType)
        {
            if (Validate())
            {
                MainFormController mainFormController = new MainFormController(ProjectName, ProjectFolder, ProjectFile, LanguageType);

                switch (executeEventType)
                {
                    case ExecuteEventType.Refresh: // no specific action required
                        {
                            _ProcessActionResult = mainFormController.ExecuteRefresh(ActionType); 

                            break;
                        }
                    case ExecuteEventType.ConvertToWebProject:
                    case ExecuteEventType.ConvertToWebsite:
                        {
                            _ProcessActionResult = mainFormController.ExecuteEvent(executeEventType, ActionType);

                            break;
                        }
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
                        case EnumTypes.TabPageType.ProjectFileValidation:
                        case EnumTypes.TabPageType.ProjectTypeValidation:
                        case EnumTypes.TabPageType.UserControlValidation:
                        case EnumTypes.TabPageType.Unknown:
                            {
                            return true;
                            }
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
