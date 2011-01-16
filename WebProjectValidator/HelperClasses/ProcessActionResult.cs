using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebProjectValidator.HelperClasses
{
    public class ProcessActionResult
    {
        private int _DesignerFileCount = 0;
        private int _DesignerFileValidCount = 0;
        private int _DesignerFileInvalidCount = 0;

        private int _ProjectTypeCount = 0;
        private int _ProjectTypeValidCount = 0;
        private int _ProjectTypeInvalidCount = 0;

        private int _UserControlCount = 0;
        private int _UserControlValidCount = 0;
        private int _UserControlInvalidCount = 0;

        private List<ApplicationFileItem> _DesignerFileDeveloperItemList = new List<ApplicationFileItem>();
        private List<ApplicationFileItem> _ProjectTypeDeveloperItemList = new List<ApplicationFileItem>();
        private List<ApplicationFileItem> _UserControlDeveloperItemList = new List<ApplicationFileItem>();

        #region simple properties
        public int DesignerFileCount
        {
            get { return _DesignerFileCount; }
            set { _DesignerFileCount = value; }
        }

        public int DesignerFileValidCount
        {
            get { return _DesignerFileValidCount; }
            set { _DesignerFileValidCount = value; }
        }

        public int DesignerFileInvalidCount
        {
            get { return _DesignerFileInvalidCount; }
            set { _DesignerFileInvalidCount = value; }
        }

        public List<ApplicationFileItem> DesignerFileDeveloperItemList
        {
            get { return _DesignerFileDeveloperItemList; }
            set { _DesignerFileDeveloperItemList = value; }
        }
        #endregion simple properties

        public int ProjectTypeCount
        {
            get { return _ProjectTypeCount; }
            set { _ProjectTypeCount = value; }
        }

        public int ProjectTypeValidCount
        {
            get { return _ProjectTypeValidCount; }
            set { _ProjectTypeValidCount = value; }
        }

        public int ProjectTypeInvalidCount
        {
            get { return _ProjectTypeInvalidCount; }
            set { _ProjectTypeInvalidCount = value; }
        }

        public List<ApplicationFileItem> ProjectTypeDeveloperItemList
        {
            get { return _ProjectTypeDeveloperItemList; }
            set { _ProjectTypeDeveloperItemList = value; }
        }
        public int UserControlCount
        {
            get { return _UserControlCount; }
            set { _UserControlCount = value; }
        }

        public int UserControlValidCount
        {
            get { return _UserControlValidCount; }
            set { _UserControlValidCount = value; }
        }

        public int UserControlInvalidCount
        {
            get { return _UserControlInvalidCount; }
            set { _UserControlInvalidCount = value; }
        }

        public List<ApplicationFileItem> UserControlDeveloperItemList
        {
            get { return _UserControlDeveloperItemList; }
            set { _UserControlDeveloperItemList = value; }
        }
    }
}
