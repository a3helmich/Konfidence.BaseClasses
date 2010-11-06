using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebProjectValidator.FileListChecker;

namespace WebProjectValidator.HelperClasses
{
    class ListProcessor
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

        public void processCodeFileCheck(FileList fileList)
        {
            
        }

        public List<DesignerFileItem> processDesignerFile(FileList fileList, ListFilterType filter)
        {
            List<string> findList = new List<string>(fileList);
            List<DesignerFileItem> resultList = new List<DesignerFileItem>();

            foreach (string fileName in fileList)
            {
                DesignerFileItem designerFileItem = new DesignerFileItem();

                designerFileItem.Project = _Project;
                designerFileItem.ProjectFolder = _Folder.ToLower();

                designerFileItem.FileName = fileName.Replace(designerFileItem.ProjectFolder, string.Empty);

                int deviderIndex = designerFileItem.FileName.LastIndexOf(@"\");
                if (deviderIndex > 0)
                {
                    designerFileItem.ControlFolder = designerFileItem.FileName.Substring(1, deviderIndex) ;
                    designerFileItem.FileName = designerFileItem.FileName.Substring(deviderIndex);
                }

                designerFileItem.FileName = designerFileItem.FileName.Replace(@"\", string.Empty);

                string findName = fileName.Replace(_DesignerSearch, _DesignerReplace);

                if (findList.Contains(findName))
                {
                    designerFileItem.Valid = true;
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
                            _ValidCount++;
                        }
                        break;
                    case ListFilterType.Invalid:
                        if (!designerFileItem.Valid)
                        {
                            resultList.Add(designerFileItem);
                            _InvalidCount++;
                        }
                        break;
                }

            }

            _Count = findList.Count;

            if (_InvalidCount > _ValidCount)
            {
                _ValidCount = _Count - _InvalidCount;
            }
            else
            {
                _InvalidCount = _Count - _ValidCount;
            }

            return resultList;
        }

        public void processUserControl(FileList fileList)
        {
        }
    }
}
