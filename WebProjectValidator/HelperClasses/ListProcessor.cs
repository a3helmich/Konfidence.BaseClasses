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

        public List<DesignerFileItem> processCodeFileCheck(FileList fileList, ListFilterType filter)
        {
            List<DesignerFileItem> resultList = new List<DesignerFileItem>();
            List<string> fileLines = new List<string>();

            _Count = fileList.Count;
            _ValidCount = fileList.Count;
            _InvalidCount = 0;

            foreach (string fileName in fileList)
            {
                using (TextReader textReader = new StreamReader(fileName))
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

        public List<DesignerFileItem> processDesignerFile(FileList fileList, FileList searchList, ListFilterType filter)
        {
            List<DesignerFileItem> resultList = new List<DesignerFileItem>();

            _Count = fileList.Count;
            _ValidCount = fileList.Count;
            _InvalidCount = 0;

            foreach (string fileName in fileList)
            {
                DesignerFileItem designerFileItem = new DesignerFileItem(_Project, _Folder, fileName);

                string findName = fileName.Replace(_DesignerSearch, _DesignerReplace);

                if (searchList.Contains(findName))
                {
                    designerFileItem.Valid = true;
                }

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

        public List<DesignerFileItem> processUserControlMissing(FileList fileList, ListFilterType filter)
        {
            List<DesignerFileItem> resultList = new List<DesignerFileItem>();
            List<string> fileLines = new List<string>();
            List<string> userControlReferences = new List<string>();

            _Count = fileList.Count;
            _ValidCount = fileList.Count;
            _InvalidCount = 0;

            foreach (string fileName in fileList)
            {
                fileLines.Clear();

                using (TextReader textReader = new StreamReader(fileName))
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
                    if (!userControlReferences.Contains(controlFileName))
                    {
                        userControlReferences.Add(controlFileName);
                    }
                }
            }

            userControlReferences.Sort();

            foreach (string fileName in userControlReferences)
            {
                DesignerFileItem designerFileItem = new DesignerFileItem(_Project, _Folder, fileName);

                designerFileItem.Valid = true;

                if (MustAddDesignerFileItem(designerFileItem, filter))
                {
                    resultList.Add(designerFileItem);
                }
            }

            foreach (DesignerFileItem designerFileItem in resultList)
            {
                if (designerFileItem.FileName.StartsWith(".."))
                {
                    designerFileItem.Valid = false;
                    designerFileItem.SetErrorMessage("file path begint met ../ ipv ~/");
                }
            }

            return resultList;
        }

        private List<string> GetControlReferences(List<string> fileLines)
        {
            List<string> userControlReferences = new List<string>();
            List<string> userControlReferenceLines = new List<string>();

            foreach (string line in fileLines)
            {
                string reference = string.Empty;

                if (line.Trim().StartsWith("<%@ Register"))
                {
                    reference = line;

                    if (line.Trim().EndsWith("%>"))
                    {
                        userControlReferenceLines.Add(reference);

                        reference = string.Empty;
                    }
                }

                if (IsString(reference))
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
                string reference = GetFileName(referenceLine);

                if (!userControlReferences.Contains(reference))
                {
                    userControlReferences.Add(reference);
                }
            }

            return userControlReferences;
        }

        private string GetFileName(string referenceLine)
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
                default:
                    return true;
            }
        }
    }
}
