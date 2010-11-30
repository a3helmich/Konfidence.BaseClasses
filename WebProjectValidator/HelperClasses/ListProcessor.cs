using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebProjectValidator.FileListChecker;
using System.IO;
using Konfidence.Base;

namespace WebProjectValidator.HelperClasses
{
    class ListProcessor: BaseItem
    {
        private string _Project = string.Empty;
        private string _Folder = string.Empty;
        private LanguageType _LanguageType = LanguageType.cs;

        private List<string> _ExtensionFilter = new List<string>();

        private string _ProjectFileName = string.Empty;

        private int _Count = 0;
        private int _ValidCount = 0;
        private int _InvalidCount = 0;

        private string _DesignerReplace = string.Empty;
        private string _DesignerSearch = string.Empty;

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

        public string ProjectFileName
        {
            get { return _ProjectFileName; }
        }
        #endregion simple properties

        public ListProcessor(string project, string folder, LanguageType language)
        {
            _Project = project;
            _Folder = folder;

            _LanguageType = language;

            _ExtensionFilter.Add(".aspx");
            _ExtensionFilter.Add(".ascx");
            _ExtensionFilter.Add(".master");
            _ExtensionFilter.Add(".asax");

            switch (language)
            {
                case LanguageType.cs:
                    _DesignerSearch = ".cs";
                    _DesignerReplace = ".designer.cs";
                    break;
                case LanguageType.vb:
                    _DesignerSearch = ".vb";
                    _DesignerReplace = ".designer.vb";
                    break;
            }
        }

        public List<DesignerFileItem> processDesignerFileMissing(FileList fileList, FileList searchList, ListFilterType filter)
        {
            List<DesignerFileItem> resultList = new List<DesignerFileItem>();

            _Count = 0;
            _ValidCount = 0;
            _InvalidCount = 0;

            ProjectFileProcessor projectFileProcessor = new ProjectFileProcessor(_Project, _Folder, _LanguageType);

            List<string> projectFileList = projectFileProcessor.GetProjectFileNameList(_Folder, _ExtensionFilter);

            _ProjectFileName = projectFileProcessor.ProjectFileName;

            foreach (string fileName in fileList)
            {
                if (projectFileList.Contains(fileName.Substring(0, fileName.Length - 3), StringComparer.CurrentCultureIgnoreCase))
                {
                    DesignerFileItem designerFileItem = new DesignerFileItem(_Project, _Folder, fileName);

                    string findName = this.ReplaceIgnoreCase(fileName, _DesignerSearch, _DesignerReplace);

                    if (searchList.Contains(findName))
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

                    if (MustAddDesignerFileItem(designerFileItem, filter))
                    {
                        resultList.Add(designerFileItem);
                    }
                }
            }

            return resultList;
        }

        public List<DesignerFileItem> processCodeFileCheck(FileList fileList, ListFilterType filter)
        {
            List<DesignerFileItem> resultList = new List<DesignerFileItem>();

            _Count = 0;
            _ValidCount = 0;
            _InvalidCount = 0;
            ProjectFileProcessor projectFileProcessor = new ProjectFileProcessor(_Project, _Folder, _LanguageType);

            List<string> projectFileList = projectFileProcessor.GetProjectFileNameList(_Folder, _ExtensionFilter);

            _ProjectFileName = projectFileProcessor.ProjectFileName;

            foreach (string fileName in fileList)
            {
                if (projectFileList.Contains(fileName, StringComparer.CurrentCultureIgnoreCase))
                {
                    List<string> fileLines = FileReader.ReadLines(fileName);

                    DesignerFileItem designerFileItem = new DesignerFileItem(_Project, _Folder, fileName);

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

                    if (MustAddDesignerFileItem(designerFileItem, filter))
                    {
                        resultList.Add(designerFileItem);
                    }
                }
            }

            return resultList;
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

        public List<DesignerFileItem> processUserControlMissing(FileList fileList, ListFilterType filter)
        {
            List<DesignerFileItem> resultList = new List<DesignerFileItem>();
            ControlReferenceList allUserControlReferences = new ControlReferenceList();
            List<DesignerFileItem> userControlReferences = new List<DesignerFileItem>();

            ProjectFileProcessor projectFileProcessor = new ProjectFileProcessor(_Project, _Folder, _LanguageType);

            List<string> projectFileList = projectFileProcessor.GetProjectFileNameList(_Folder, _ExtensionFilter);

            _ProjectFileName = projectFileProcessor.ProjectFileName;

            fileList.Sort();

            foreach (string fileName in fileList)
            {
                string fileFolder = fileName.Replace(_Folder, "");

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
                DesignerFileItem designerFileItem = new DesignerFileItem(_Project, _Folder, controlReference);

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
                if (MustAddDesignerFileItem(designerFileItem, filter))
                {
                    resultList.Add(designerFileItem);
                }
            }

            return resultList;
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

        public bool MustAddDesignerFileItem(DesignerFileItem designerFileItem, ListFilterType filter)
        {
            switch (filter)
            {
                case ListFilterType.Valid:
                    if (designerFileItem.Valid)
                    {
                        return true;
                    }
                    return false;
                case ListFilterType.Invalid:
                    if (!designerFileItem.Valid)
                    {
                        return true;
                    }
                    return false;
                case ListFilterType.Exists:
                    if (designerFileItem.Exists)
                    {
                        return true;
                    }
                    return false;
                case ListFilterType.Missing:
                    if (!designerFileItem.Exists)
                    {
                        return true;
                    }
                    return false;
                case ListFilterType.Unused:
                    if (!designerFileItem.IsUsed)
                    {
                        return true;
                    }
                    return false;
                default:
                    return true;
            }
        }

        public void repairCodeFileToApplication(List<DesignerFileItem> repairList)
        {
            foreach (DesignerFileItem fileItem in repairList)
            {
                if (!fileItem.Valid)
                {
                    List<string> fileLines = FileReader.ReadLines(fileItem.FullFileName);

                    List<string> newFileLines = fixCodeFileToApplication(fileLines);

                    if (!Directory.Exists(fileItem.ProjectFolder + @"\fileBackup"))
                    {
                        Directory.CreateDirectory(fileItem.ProjectFolder + @"\fileBackup");
                    }

                    if (!File.Exists(fileItem.ProjectFolder + @"\fileBackup" + @"\" + fileItem.FileName))
                    {
                        File.Copy(fileItem.FullFileName, fileItem.ProjectFolder + @"\fileBackup" + @"\" + fileItem.FileName, true);
                    }

                    if (IsAssigned(newFileLines))
                    {
                        using (TextWriter textWriter = new StreamWriter(fileItem.FullFileName, false, Encoding.Default))
                        {
                            foreach (string line in newFileLines)
                            {
                                textWriter.WriteLine(line);
                            }
                        }
                    }
                }
            }
        }

        public void repairCodeFileToProject(List<DesignerFileItem> repairList)
        {
            foreach (DesignerFileItem fileItem in repairList)
            {
                if (fileItem.Valid) // even omgekeerde logica (2 keer)
                {
                    List<string> fileLines = FileReader.ReadLines(fileItem.FullFileName);

                    List<string> newFileLines = fixCodeFileToProject(fileLines);

                    if (!Directory.Exists(fileItem.ProjectFolder + @"\fileBackup"))
                    {
                        Directory.CreateDirectory(fileItem.ProjectFolder + @"\fileBackup");
                    }

                    if (!File.Exists(fileItem.ProjectFolder + @"\fileBackup" + @"\" + fileItem.FileName))
                    {
                        File.Copy(fileItem.FullFileName, fileItem.ProjectFolder + @"\fileBackup" + @"\" + fileItem.FileName, true);
                    }

                    if (IsAssigned(newFileLines))
                    {
                        using (TextWriter textWriter = new StreamWriter(fileItem.FullFileName, false, Encoding.Default))
                        {
                            foreach (string line in newFileLines)
                            {
                                textWriter.WriteLine(line);
                            }
                        }
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
