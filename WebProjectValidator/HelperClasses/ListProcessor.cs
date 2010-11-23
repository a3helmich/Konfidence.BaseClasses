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
        #endregion simple properties

        public ListProcessor(string project, string folder, LanguageType language)
        {
            _Project = project;
            _Folder = folder;

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

            _Count = fileList.Count;
            _ValidCount = fileList.Count;
            _InvalidCount = 0;

            foreach (string fileName in fileList)
            {
                DesignerFileItem designerFileItem = new DesignerFileItem(_Project, _Folder, fileName);

                string findName = this.ReplaceIgnoreCase(fileName, _DesignerSearch, _DesignerReplace);

                if (searchList.Contains(findName))
                {
                    designerFileItem.Exists = true;
                }

                if (!designerFileItem.Exists)
                {
                    _ValidCount--;
                    _InvalidCount++;
                }

                if (MustAddDesignerFileItem(designerFileItem, filter))
                {
                    resultList.Add(designerFileItem);
                }
            }

            return resultList;
        }

        public List<DesignerFileItem> processCodeFileCheck(FileList fileList, ListFilterType filter)
        {
            List<DesignerFileItem> resultList = new List<DesignerFileItem>();
            List<string> fileLines = new List<string>();

            _Count = fileList.Count;
            _ValidCount = fileList.Count;
            _InvalidCount = 0;

            foreach (string fileName in fileList)
            {
                fileLines.Clear();

                using (TextReader textReader = new StreamReader(fileName, Encoding.Default))
                {
                    string line = textReader.ReadLine();

                    while (IsAssigned(line))
                    {
                        fileLines.Add(line);
                        line = textReader.ReadLine();
                    }
                }

                DesignerFileItem designerFileItem = new DesignerFileItem(_Project, _Folder, fileName);

                designerFileItem.Valid = IsValidCodeFile(fileLines);

                if (!designerFileItem.Valid)
                {
                    _ValidCount--;
                    _InvalidCount++;
                }

                if (MustAddDesignerFileItem(designerFileItem, filter))
                {
                    resultList.Add(designerFileItem);
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

            return false;
        }

        public List<DesignerFileItem> processUserControlMissing(FileList fileList, ListFilterType filter)
        {
            List<DesignerFileItem> resultList = new List<DesignerFileItem>();
            List<string> fileLines = new List<string>();
            List<string> allUserControlReferences = new List<string>();
            List<DesignerFileItem> userControlReferences = new List<DesignerFileItem>();

            fileList.Sort();

            foreach (string fileName in fileList)
            {
                fileLines.Clear();

                using (TextReader textReader = new StreamReader(fileName, Encoding.Default))
                {
                    string line = textReader.ReadLine();

                    while (IsAssigned(line))
                    {
                        fileLines.Add(line);
                        line = textReader.ReadLine();
                    }
                }

                List<string> referenceList = GetControlReferences(fileLines);
                foreach (string controlFileName in referenceList)
                {
                    if (!allUserControlReferences.Contains(controlFileName))
                    {
                        allUserControlReferences.Add(controlFileName);
                    }
                }
            }

            allUserControlReferences.Sort();

            foreach (string fileName in allUserControlReferences)
            {
                DesignerFileItem designerFileItem = new DesignerFileItem(_Project, _Folder, fileName);

                designerFileItem.Valid = true;

                userControlReferences.Add(designerFileItem);
            }


            foreach (DesignerFileItem designerFileItem in userControlReferences)
            {
                if (designerFileItem.FileName.StartsWith(".."))
                {
                    designerFileItem.Valid = false;
                    designerFileItem.SetErrorMessage("file path begint met ../ ipv ~/");
                }
                else if (!designerFileItem.FileName.StartsWith("~") && designerFileItem.FileName.Contains("/"))
                {
                    designerFileItem.Valid = false;
                    designerFileItem.SetErrorMessage("file path begint niet met ~/");
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

        private List<string> GetControlReferences(List<string> fileLines)
        {
            List<string> userControlReferences = new List<string>();
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
                string referenceName = GetReferenceFileName(referenceLine);

                if (!userControlReferences.Contains(referenceName))
                {
                    userControlReferences.Add(referenceName);
                }
            }

            return userControlReferences;
        }

        private string GetReferenceFileName(string referenceLine)
        {
            string fileName = referenceLine.Substring(referenceLine.IndexOf("src=", StringComparison.InvariantCultureIgnoreCase) + 5);
            fileName = fileName.Substring(0, fileName.IndexOf("\""));
            return fileName;
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

        public void repairCodeFile(List<DesignerFileItem> repairList)
        {
            List<string> fileLines = new List<string>();

            foreach (DesignerFileItem fileItem in repairList)
            {
                fileLines.Clear();

                if (!fileItem.Valid)
                {
                    using (TextReader textReader = new StreamReader(fileItem.FullFileName, Encoding.Default))
                    {
                        string line = textReader.ReadLine();

                        while (IsAssigned(line))
                        {
                            fileLines.Add(line);
                            line = textReader.ReadLine();
                        }
                    }

                    List<string> newFileLines = fixCodeFile(fileLines);

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

        private List<string> fixCodeFile(List<string> fileLines)
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

                        if (foundPage)
                        {
                            newLine = ReplaceIgnoreCase(line, " codebehind=", " CodeFile=");
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
