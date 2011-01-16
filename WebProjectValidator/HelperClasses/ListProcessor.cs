using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebProjectValidator.FileListChecker;
using System.IO;
using Konfidence.Base;
using WebProjectValidator.EnumTypes;

namespace WebProjectValidator.HelperClasses
{
    class ListProcessor: BaseItem
    {
        private string _Project = string.Empty;
        private string _ProjectFolder = string.Empty;
        private string _ProjectFile = string.Empty;
        private LanguageType _LanguageType = LanguageType.cs;

        private List<string> _ExtensionFilter = new List<string>();

        private int _Count = 0;
        private int _ValidCount = 0;
        private int _InvalidCount = 0;

        private string _DesignerReplace = string.Empty;
        private string _DesignerSearch = string.Empty;

        private static List<ListProcessor> _ListProcessorList = new List<ListProcessor>();
        private static ProcessActionResult _ProcessActionResult = new ProcessActionResult();


        #region simple properties
        public int Count
        {
            get { return _Count; }
            set { _Count = value; }
        }

        public int ValidCount
        {
            get { return _ValidCount; }
            set { _ValidCount = value; }
        }

        public int InvalidCount
        {
            get { return _InvalidCount; }
            set { _InvalidCount = value; }
        }
        #endregion simple properties

        private FileList WebFileList
        {
            get
            {
                return new FileList(_ProjectFolder, _LanguageType, DeveloperFileType.WebFile);
            }
        }

        private FileList SourceFileList
        {
            get
            {
                return new FileList(_ProjectFolder, _LanguageType, DeveloperFileType.SourceFile);
            }
        }

        private FileList DesignerFileList
        {
            get
            {
                return new FileList(_ProjectFolder, _LanguageType, DeveloperFileType.DesignerFile);
            }
        }

        public static ListProcessor GetProcessor(string project, string projectFolder, string projectFile, LanguageType language)
        {
            ListProcessor newProcessor = null;

            foreach (ListProcessor processor in _ListProcessorList)
            {
                if (processor._Project == project && processor._ProjectFolder == projectFolder)
                {
                    if (processor._ProjectFile == projectFile && processor._LanguageType == language)
                    {
                        return processor;
                    }
                }
            }

            newProcessor = new ListProcessor(project, projectFolder, projectFile, language);

            _ListProcessorList.Add(newProcessor);

            return newProcessor;
        }

        private ListProcessor(string project, string projectFolder, string projectFile, LanguageType languageType)
        {
            _Project = project;
            _ProjectFolder = projectFolder;
            _ProjectFile = projectFile;

            _LanguageType = languageType;

            _ExtensionFilter.Add(".aspx");
            _ExtensionFilter.Add(".ascx");
            _ExtensionFilter.Add(".master");
            _ExtensionFilter.Add(".asax");

            switch (languageType)
            {
                case LanguageType.cs:
                    {
                        _DesignerSearch = ".cs";
                        _DesignerReplace = ".designer.cs";

                        break;
                    }
                case LanguageType.vb:
                    {
                        _DesignerSearch = ".vb";
                        _DesignerReplace = ".designer.vb";

                        break;
                    }
            }
        }

        public ProcessActionResult ProcessProjectFileValidation(ProcessActionType actionType)
        {
            // 1. get a list of all files included in the projectfile
            // 2. get a list of all files in the website
            // 3. all website files not in included in the projectfile are invalid


            // 1 :
            ProjectFileProcessor projectFileProcessor = new ProjectFileProcessor(_ProjectFile);

            List<string> projectFileList = projectFileProcessor.GetProjectFileNameList(_ProjectFolder, _ExtensionFilter);

            // 2: 

            List<ApplicationFileItem> resultList = new List<ApplicationFileItem>();

            _Count = 0;
            _ValidCount = 0;
            _InvalidCount = 0;

            foreach (string fileName in WebFileList)
            {
                if (projectFileList.Contains(fileName.Substring(0, fileName.Length - 3), StringComparer.CurrentCultureIgnoreCase))
                {
                    ApplicationFileItem applicationFileItem = new ApplicationFileItem(_ProjectFolder, fileName);

                    string findName = this.ReplaceIgnoreCase(fileName, _DesignerSearch, _DesignerReplace);

                    if (DesignerFileList.Contains(findName))
                    {
                        applicationFileItem.Exists = true;
                    }

                    _Count++;

                    if (applicationFileItem.Exists)
                    {
                        _ValidCount++;
                    }
                    else
                    {
                        _InvalidCount++;
                    }

                    if (MustAddApplicationFileItem(applicationFileItem, actionType))
                    {
                        resultList.Add(applicationFileItem);
                    }
                }
            }

            return GetActionResult(resultList, TabPageType.ProjectFileValidation);
        }

        public ProcessActionResult ProcessProjectTypeValidation(ProcessActionType actionType)
        {
            List<ApplicationFileItem> resultList = new List<ApplicationFileItem>();

            _Count = 0;
            _ValidCount = 0;
            _InvalidCount = 0;

            ProjectFileProcessor projectFileProcessor = new ProjectFileProcessor(_ProjectFile);

            List<string> projectFileList = projectFileProcessor.GetProjectFileNameList(_ProjectFolder, _ExtensionFilter);

            foreach (string fileName in WebFileList)
            {
                if (projectFileList.Contains(fileName, StringComparer.CurrentCultureIgnoreCase))
                {
                    List<string> fileLines = FileReader.ReadLines(fileName);

                    ApplicationFileItem applicationFileItem = new ApplicationFileItem( _ProjectFolder, fileName);

                    applicationFileItem.Valid = IsValidCodeFile(fileLines);

                    _Count++;

                    if (applicationFileItem.Valid)
                    {
                        _ValidCount++;
                    }
                    else
                    {
                        _InvalidCount++;
                    }

                    if (MustAddApplicationFileItem(applicationFileItem, actionType))
                    {
                        resultList.Add(applicationFileItem);
                    }
                }
            }

            return GetActionResult(resultList, TabPageType.ProjectTypeValidation);
        }

        private bool IsValidCodeFile(List<string> fileLines)
        {
            foreach (string line in fileLines)
            {
                if (line.IndexOf("codefile=", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    return true;
                }

                if (line.IndexOf("codebehind=", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    return false;
                }
            }

            return true; // komt niet voor in het bestand
        }

        public ProcessActionResult ProcessUserControlValidation(ProcessActionType actionType)
        {
            List<ApplicationFileItem> resultList = new List<ApplicationFileItem>();
            ControlReferenceList allUserControlReferences = new ControlReferenceList();
            List<ApplicationFileItem> userControlReferences = new List<ApplicationFileItem>();

            ProjectFileProcessor projectFileProcessor = new ProjectFileProcessor(_ProjectFile);

            List<string> projectFileList = projectFileProcessor.GetProjectFileNameList(_ProjectFolder, _ExtensionFilter);

            WebFileList.Sort();

            foreach (string fileName in WebFileList)
            {
                string fileFolder = fileName.Replace(_ProjectFolder, "");

                if (fileFolder.Contains(@"\"))
                {
                    fileFolder = fileFolder.Substring(0, fileFolder.LastIndexOf(@"\"));
                    if (fileFolder.StartsWith(@"\"))
                    {
                        fileFolder = fileFolder.Substring(1);
                    }
                }
                else
                {
                    fileFolder = string.Empty;
                }

                if (projectFileList.Contains(fileName, StringComparer.CurrentCultureIgnoreCase))
                {
                    List<string> fileLines = FileReader.ReadLines(fileName);

                    ControlReferenceList referenceList = GetControlReferenceList(fileLines, fileFolder);

                    // TODO : controlReference Class, met daarin de filename + de originele reference
                    foreach (ControlReference controlReference in referenceList)
                    {
                        if (!allUserControlReferences.Contains(controlReference))
                        {
                            allUserControlReferences.Add(controlReference);
                        }
                    }
                }
            }

            allUserControlReferences.Sort();

            foreach (ControlReference controlReference in allUserControlReferences)
            {
                ApplicationFileItem applicationFileItem = new ApplicationFileItem( _ProjectFolder, controlReference);

                applicationFileItem.Valid = true;

                userControlReferences.Add(applicationFileItem);
            }


            foreach (ApplicationFileItem applicationFileItem in userControlReferences)
            {
                if (applicationFileItem.Reference.StartsWith(".."))
                {
                    applicationFileItem.Valid = false;
                    applicationFileItem.SetErrorMessage("file path begint met ../ ipv ~/");
                }
                else if (!applicationFileItem.Reference.StartsWith("~") && applicationFileItem.Reference.Contains("/"))
                {
                    applicationFileItem.Valid = false;
                    applicationFileItem.SetErrorMessage("file path begint niet met ~/");
                }

                if (!File.Exists(applicationFileItem.FullFileName))
                {
                    applicationFileItem.Valid = false;
                    applicationFileItem.SetErrorMessage("reference naar een niet bestaand control");
                }

                if (!projectFileList.Contains(applicationFileItem.FullFileName, StringComparer.CurrentCultureIgnoreCase))
                {
                    applicationFileItem.Valid = false;
                    applicationFileItem.SetErrorMessage("reference naar een control dat niet in het project zit");
                }
            }

            _Count = userControlReferences.Count;
            _ValidCount = userControlReferences.Count;
            _InvalidCount = 0;

            foreach (ApplicationFileItem applicationFileItem in userControlReferences)
            {
                if (!applicationFileItem.Valid)
                {
                    _ValidCount--;
                    _InvalidCount++;
                }
            }

            foreach (ApplicationFileItem applicationFileItem in userControlReferences)
            {
                if (MustAddApplicationFileItem(applicationFileItem, actionType))
                {
                    resultList.Add(applicationFileItem);
                }
            }

            return GetActionResult(resultList, TabPageType.UserControlValidation);
        }

        private ProcessActionResult GetActionResult(List<ApplicationFileItem> resultList, TabPageType tabPageType)
        {
            switch (tabPageType)
            {
                case TabPageType.ProjectFileValidation:
                    _ProcessActionResult.DesignerFileCount = Count;
                    _ProcessActionResult.DesignerFileValidCount = ValidCount;
                    _ProcessActionResult.DesignerFileInvalidCount = InvalidCount;

                    _ProcessActionResult.DesignerFileDeveloperItemList = resultList;
                    break;
                case TabPageType.ProjectTypeValidation:
                    _ProcessActionResult.ProjectTypeCount = Count;
                    _ProcessActionResult.ProjectTypeValidCount = ValidCount;
                    _ProcessActionResult.ProjectTypeInvalidCount = InvalidCount;

                    _ProcessActionResult.ProjectTypeDeveloperItemList = resultList;
                    break;
                case TabPageType.UserControlValidation:
                    _ProcessActionResult.UserControlCount = Count;
                    _ProcessActionResult.UserControlValidCount = ValidCount;
                    _ProcessActionResult.UserControlInvalidCount = InvalidCount;

                    _ProcessActionResult.UserControlDeveloperItemList = resultList;
                    break;
            }

            return _ProcessActionResult;
        }

        private ControlReferenceList GetControlReferenceList(List<string> fileLines, string fileFolder)
        {
            ControlReferenceList userControlReferences = new ControlReferenceList();
            List<string> userControlReferenceLines = new List<string>();

            string reference = string.Empty;

            foreach (string line in fileLines)
            {
                if (line.Trim().StartsWith("<%@ Register",StringComparison.InvariantCultureIgnoreCase) && line.Trim().IndexOf("src=", StringComparison.InvariantCultureIgnoreCase) > -1)
                {
                    reference = line;

                    if (line.Trim().EndsWith("%>"))
                    {
                        userControlReferenceLines.Add(reference);

                        reference = string.Empty;
                    }
                }

                if (!IsEmpty(reference))
                {
                    reference += line;

                    if (line.Trim().EndsWith("%>"))
                    {
                        userControlReferenceLines.Add(reference);

                        reference = string.Empty;
                    }
                }
            }

            foreach (string referenceLine in userControlReferenceLines)
            {
                ControlReference controlReference = GetControlReference(referenceLine, fileFolder);

                if (!userControlReferences.Contains(controlReference))
                {
                    userControlReferences.Add(controlReference);
                }
            }

            return userControlReferences;
        }

        private ControlReference GetControlReference(string fullReferenceLine, string fileFolder)
        {
            string referenceName = fullReferenceLine.Substring(fullReferenceLine.IndexOf("src=", StringComparison.InvariantCultureIgnoreCase) + 5);

            string quote = "\"";

            referenceName = referenceName.Substring(0, referenceName.IndexOf(quote));

            ControlReference controlReference = new ControlReference(referenceName, fileFolder);

            return controlReference;
        }

        // TODO : split actionType usercontrol / designer functionality
        public bool MustAddApplicationFileItem(ApplicationFileItem applicationFileItem, ProcessActionType actionType)
        {
            switch (actionType)
            {
                case ProcessActionType.UserControlValid:
                    {
                        if (applicationFileItem.Valid)
                        {
                            return true;
                        }
                        return false;
                    }
                case ProcessActionType.UserControlInvalid:
                    {
                        if (!applicationFileItem.Valid)
                        {
                            return true;
                        }
                        return false;
                    }
                case ProcessActionType.DesignerFileExists:
                    {
                        if (applicationFileItem.Exists)
                        {
                            return true;
                        }
                        return false;
                    }
                case ProcessActionType.InProjectFile:
                    {
                        if (!applicationFileItem.Exists)
                        {
                            return true;
                        }
                        return false;
                    }
                case ProcessActionType.UserControlMissing:
                    {
                        if (!applicationFileItem.Exists)
                        {
                            return true;
                        }
                        return false;
                    }
                case ProcessActionType.UserControlUnused:
                    {
                        if (!applicationFileItem.IsUsed)
                        {
                            return true;
                        }
                        return false;
                    }
            }
            return true;
        }

        public void ConvertToWebsite()
        {
            // TODO : ApplicationFileItem -> doesn't feel right
            // TODO : property?
            // website uses no projectfile -> all files must be converted
            ApplicationFileItemList websiteApplicationFileItemList = new ApplicationFileItemList(_ProjectFolder, WebFileList);

            CheckFileBackupDirectory();

            foreach (ApplicationFileItem fileItem in websiteApplicationFileItemList)
            {
                if (!fileItem.Valid)
                {
                    List<string> fileLines = FileReader.ReadLines(fileItem.FullFileName);

                    List<string> newFileLines = fixCodeFileToApplication(fileLines);

                    if (IsAssigned(newFileLines))
                    {
                        if (!File.Exists(fileItem.ProjectFolder + @"\fileBackup" + @"\" + fileItem.FileName))
                        {
                            File.Copy(fileItem.FullFileName, fileItem.ProjectFolder + @"\fileBackup" + @"\" + fileItem.FileName, true);
                        }

                        FileWriter.WriteLines(fileItem.FullFileName, newFileLines);
                    }
                }
            }
        }

        private void CheckFileBackupDirectory()
        {
            if (!Directory.Exists(_ProjectFolder + @"\fileBackup"))
            {
                Directory.CreateDirectory(_ProjectFolder + @"\fileBackup");
            }
        }

        public void ConvertToWebProject()
        {
            // TODO : just get a list of all projectFiles without any processing
            ProcessActionResult projectTypeValidationResult = ProcessProjectTypeValidation(ProcessActionType.WebProject);

            List<ApplicationFileItem> repairList = projectTypeValidationResult.ProjectTypeDeveloperItemList;

            // web project uses a projectfile -> only files included in the project file must be converted

            CheckFileBackupDirectory();

            foreach (ApplicationFileItem fileItem in repairList)
            {
                if (fileItem.Valid) // even omgekeerde logica (2 keer)
                {
                    List<string> fileLines = FileReader.ReadLines(fileItem.FullFileName);

                    List<string> newFileLines = fixCodeFileToProject(fileLines);

                    if (!File.Exists(fileItem.ProjectFolder + @"\fileBackup" + @"\" + fileItem.FileName))
                    {
                        File.Copy(fileItem.FullFileName, fileItem.ProjectFolder + @"\fileBackup" + @"\" + fileItem.FileName, true);
                    }

                    if (IsAssigned(newFileLines))
                    {
                        FileWriter.WriteLines(fileItem.FullFileName, newFileLines);
                    }
                }
            }
        }

        private List<string> fixCodeFileToApplication(List<string> fileLines)
        {
            List<string> newFileLines = null;

            if (!IsValidCodeFile(fileLines))
            {
                bool foundPage = false;
                bool finished = false;

                newFileLines = new List<string>();

                foreach (string line in fileLines)
                {
                    string newLine = line;

                    if (!finished)
                    {
                        if (line.StartsWith("<%@ page", StringComparison.InvariantCultureIgnoreCase))
                        {
                            foundPage = true;
                        }

                        if (line.StartsWith("<%@ control", StringComparison.InvariantCultureIgnoreCase))
                        {
                            foundPage = true;
                        }

                        if (line.StartsWith("<%@ master", StringComparison.InvariantCultureIgnoreCase))
                        {
                            foundPage = true;
                        }

                        if (foundPage)
                        {
                            newLine = ReplaceIgnoreCase(line, "codebehind=", "CodeFile=");
                        }

                        if (line.Contains("%>"))
                        {
                            finished = true;
                        }
                    }

                    newFileLines.Add(newLine);
                }
            }

            return newFileLines;
        }

        private List<string> fixCodeFileToProject(List<string> fileLines)
        {
            List<string> newFileLines = null;

            if (IsValidCodeFile(fileLines))  // even omgekeerde logica (2 plekken)
            {
                bool foundPage = false;
                bool finished = false;

                newFileLines = new List<string>();

                foreach (string line in fileLines)
                {
                    string newLine = line;

                    if (!finished)
                    {
                        if (line.StartsWith("<%@ page", StringComparison.InvariantCultureIgnoreCase))
                        {
                            foundPage = true;
                        }

                        if (line.StartsWith("<%@ control", StringComparison.InvariantCultureIgnoreCase))
                        {
                            foundPage = true;
                        }

                        if (line.StartsWith("<%@ master", StringComparison.InvariantCultureIgnoreCase))
                        {
                            foundPage = true;
                        }

                        if (foundPage)
                        {
                            newLine = ReplaceIgnoreCase(line, "codefile=", "Codebehind=");
                        }

                        if (line.Contains("%>"))
                        {
                            finished = true;
                        }
                    }

                    newFileLines.Add(newLine);
                }
            }

            return newFileLines;
        }
    }
}
