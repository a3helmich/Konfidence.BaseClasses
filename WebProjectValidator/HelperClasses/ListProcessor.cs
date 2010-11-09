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
            List<string> fileLines = null;

            _Count = fileList.Count;
            _ValidCount = fileList.Count;
            _InvalidCount = 0;

            foreach (string fileName in fileList)
            {
                fileLines = new List<string>();

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

                switch (filter)
                {
                    case ListFilterType.All:
                        resultList.Add(designerFileItem);
                        break;
                    case ListFilterType.Valid:
                        if (designerFileItem.Valid)
                        {
                            resultList.Add(designerFileItem);
                        }
                        break;
                    case ListFilterType.Invalid:
                        if (!designerFileItem.Valid)
                        {
                            resultList.Add(designerFileItem);
                        }
                        break;
                }
            }

            return resultList;
        }

        private bool IsValidCodeFile(List<string> fileLines)
        {
            foreach (string line in fileLines)
            {
                if (line.IndexOf(" codefile=", StringComparison.InvariantCultureIgnoreCase) > 0)
                {
                    return true;
                }

                if (line.IndexOf(" codebehind=", StringComparison.InvariantCultureIgnoreCase) > 0)
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

                switch (filter)
                {
                    case ListFilterType.All:
                        resultList.Add(designerFileItem);
                        break;
                    case ListFilterType.Valid:
                        if (designerFileItem.Valid)
                        {
                            resultList.Add(designerFileItem);
                        }
                        break;
                    case ListFilterType.Invalid:
                        if (!designerFileItem.Valid)
                        {
                            resultList.Add(designerFileItem);
                        }
                        break;
                }

            }

            return resultList;
        }

        public void processUserControl(FileList fileList)
        {
        }
    }
}
