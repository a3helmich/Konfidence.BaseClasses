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

        public ProcessActionResult ProcessDesignerFileValidation(ProcessActionType actionType)
        {
            List<DesignerFileItem> resultList = new List<DesignerFileItem>();

            _Count = 0;
            _ValidCount = 0;
            _InvalidCount = 0;

            ProjectFileProcessor projectFileProcessor = new ProjectFileProcessor(_ProjectFile);

            List<string> projectFileList = projectFileProcessor.GetProjectFileNameList(_ProjectFolder, _ExtensionFilter);

            foreach (string fileName in SourceFileList)
            {
                if (projectFileList.Contains(fileName.Substring(0, fileName.Length - 3), StringComparer.CurrentCultureIgnoreCase))
                {
                    DesignerFileItem designerFileItem = new DesignerFileItem(_ProjectFolder, fileName);

                    string findName = this.ReplaceIgnoreCase(fileName, _DesignerSearch, _DesignerReplace);

                    if (DesignerFileList.Contains(findName))
                    {
                        designerFileItem.Exists = true;
                    }

                    _Count++;

                    if (designerFileItem.Exists)
                    {
                        _ValidCount++;
                    }
                    else
                    {
                        _InvalidCount++;
                    }

                    if (MustAddDesignerFileItem(designerFileItem, actionType))
                    {
                        resultList.Add(designerFileItem);
                    }
                }
            }

            return GetActionResult(resultList, TabPageType.DesignerFileValidation);
        }

        public List<DesignerFileItem> GetWebApplicationFileList()
        {
            List<DesignerFileItem> resultList = new List<DesignerFileItem>();

            return resultList;
        }

        public List<DesignerFileItem> GetWebProjectFileList()
        {
            List<DesignerFileItem> resultList = new List<DesignerFileItem>();

            return resultList;
        }

        public ProcessActionResult ProcessProjectTypeValidation(ProcessActionType actionType)
        {
            List<DesignerFileItem> resultList = new List<DesignerFileItem>();

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

                    DesignerFileItem designerFileItem = new DesignerFileItem( _ProjectFolder, fileName);

                    designerFileItem.Valid = IsValidCodeFile(fileLines);

                    _Count++;

                    if (designerFileItem.Valid)
                    {
                        _ValidCount++;
                    }
                    else
                    {
                        _InvalidCount++;
                    }

                    if (MustAddDesignerFileItem(designerFileItem, actionType))
                    {
                        resultList.Add(designerFileItem);
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
            List<DesignerFileItem> resultList = new List<DesignerFileItem>();
            ControlReferenceList allUserControlReferences = new ControlReferenceList();
            List<DesignerFileItem> userControlReferences = new List<DesignerFileItem>();

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
                DesignerFileItem designerFileItem = new DesignerFileItem( _ProjectFolder, controlReference);

                designerFileItem.Valid = true;

                userControlReferences.Add(designerFileItem);
            }


            foreach (DesignerFileItem designerFileItem in userControlReferences)
            {
                if (designerFileItem.Reference.StartsWith(".."))
                {
                    designerFileItem.Valid = false;
                    designerFileItem.SetErrorMessage("file path begint met ../ ipv ~/");
                }
                else if (!designerFileItem.Reference.StartsWith("~") && designerFileItem.Reference.Contains("/"))
                {
                    designerFileItem.Valid = false;
                    designerFileItem.SetErrorMessage("file path begint niet met ~/");
                }

                if (!File.Exists(designerFileItem.FullFileName))
                {
                    designerFileItem.Valid = false;
                    designerFileItem.SetErrorMessage("reference naar een niet bestaand control");
                }

                if (!projectFileList.Contains(designerFileItem.FullFileName, StringComparer.CurrentCultureIgnoreCase))
                {
                    designerFileItem.Valid = false;
                    designerFileItem.SetErrorMessage("reference naar een control dat niet in het project zit");
                }
            }

            _Count = userControlReferences.Count;
            _ValidCount = userControlReferences.Count;
            _InvalidCount = 0;

            foreach (DesignerFileItem designerFileItem in userControlReferences)
            {
                if (!designerFileItem.Valid)
                {
                    _ValidCount--;
                    _InvalidCount++;
                }
            }

            foreach (DesignerFileItem designerFileItem in userControlReferences)
            {
                if (MustAddDesignerFileItem(designerFileItem, actionType))
                {
                    resultList.Add(designerFileItem);
                }
            }

            return GetActionResult(resultList, TabPageType.UserControlValidation);
        }

        private ProcessActionResult GetActionResult(List<DesignerFileItem> resultList, TabPageType tabPageType)
        {
            switch (tabPageType)
            {
                case TabPageType.DesignerFileValidation:
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
        public bool MustAddDesignerFileItem(DesignerFileItem designerFileItem, ProcessActionType actionType)
        {
            switch (actionType)
            {
                case ProcessActionType.UserControlValid:
                    {
                        if (designerFileItem.Valid)
                        {
                            return true;
                        }
                        return false;
                    }
                case ProcessActionType.UserControlInvalid:
                    {
                        if (!designerFileItem.Valid)
                        {
                            return true;
                        }
                        return false;
                    }
                case ProcessActionType.DesignerFileExists:
                    {
                        if (designerFileItem.Exists)
                        {
                            return true;
                        }
                        return false;
                    }
                case ProcessActionType.DesignerFileMissing:
                    {
                        if (!designerFileItem.Exists)
                        {
                            return true;
                        }
                        return false;
                    }
                case ProcessActionType.UserControlMissing:
                    {
                        if (!designerFileItem.Exists)
                        {
                            return true;
                        }
                        return false;
                    }
                case ProcessActionType.UserControlUnused:
                    {
                        if (!designerFileItem.IsUsed)
                        {
                            return true;
                        }
                        return false;
                    }
            }
            return true;
        }

        public void ConvertToWebApplication()
        {
            // TODO : DesignerFileItem -> doesn't feel right
            // TODO : property?
            // web application uses no projectfile -> all files must be converted
            DesignerFileItemList webApplicationDesigenerFileItemList = new DesignerFileItemList(_ProjectFolder, WebFileList);

            CheckFileBackupDirectory();

            foreach (DesignerFileItem fileItem in webApplicationDesigenerFileItemList)
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

            List<DesignerFileItem> repairList = projectTypeValidationResult.ProjectTypeDeveloperItemList;

            // web project uses a projectfile -> only files included in the project file must be converted

            CheckFileBackupDirectory();

            foreach (DesignerFileItem fileItem in repairList)
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
