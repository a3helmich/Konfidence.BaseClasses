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

        private List<DesignerFileItem> _DesignerFileItemList = new List<DesignerFileItem>();

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

        public List<DesignerFileItem> DesignerFileItemList
        {
            get { return _DesignerFileItemList; }
            set { _DesignerFileItemList = value; }
        }
        #endregion simple properties
    }
}
