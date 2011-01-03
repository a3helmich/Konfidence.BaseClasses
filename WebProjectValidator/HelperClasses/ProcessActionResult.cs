using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebProjectValidator.HelperClasses
{
    public class ProcessActionResult
    {
        private int _Count = 0;
        private int _ValidCount = 0;
        private int _InvalidCount = 0;

        private List<DesignerFileItem> _DesignerFileDesignerFileItemList = new List<DesignerFileItem>();
        private List<DesignerFileItem> _ProjectTypeDesignerFileItemList = new List<DesignerFileItem>();
        private List<DesignerFileItem> _UserControlDesignerFileItemList = new List<DesignerFileItem>();

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

        public List<DesignerFileItem> DesignerFileDesignerFileItemList
        {
            get { return _DesignerFileDesignerFileItemList; }
            set { _DesignerFileDesignerFileItemList = value; }
        }

        public List<DesignerFileItem> ProjectTypeDesignerFileItemList
        {
            get { return _ProjectTypeDesignerFileItemList; }
            set { _ProjectTypeDesignerFileItemList = value; }
        }

        public List<DesignerFileItem> UserControlDesignerFileItemList
        {
            get { return _UserControlDesignerFileItemList; }
            set { _UserControlDesignerFileItemList = value; }
        }

        #endregion simple properties
    }
}
