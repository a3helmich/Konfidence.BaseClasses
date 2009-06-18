using Konfidence.BaseData;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;

namespace Konfidence.BaseData
{
    public class SqlServerDataItem: BaseDataItem
    {
        private const string SQLSERVER_GET = "SqlServer_Get";

        private const string SQLSERVERID = "SqlServerId";

        private const string AVAILABLE = "Available";

        private bool _IsAvailable = true;
        private string _ErrorMessage = string.Empty;

        #region properties
        public int SqlServerId
        {
            get
            {
                return Id;
            }
        }

        public bool IsAvailable
        {
            get { return _IsAvailable; }
        }

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
        }
        #endregion properties

        public SqlServerDataItem() 
        {
            SqlServerGetItem();
        }

        public SqlServerDataItem(int sqlServerId)
        {
            SqlServerGetItem();
        }

        private void SqlServerGetItem()
        {
            try
            {
                GetItem(SQLSERVER_GET, 1);
            }
            catch(SqlException sqlException)
            {
                _IsAvailable = false;
                _ErrorMessage = sqlException.Message;
            }
        }

        //public SqlServerDataItem(List<BaseDataItem.ParameterObject> ParameterList)
        //{
        //    GetItem(ParameterList);
        //}

        protected override void InitializeDataItem()
        {
            base.InitializeDataItem();

            AutoIdField = SQLSERVERID;

            ServiceName = "SqlServerDataItemService";
        }

        protected internal override void GetData()
        {
            _IsAvailable = GetFieldBool(AVAILABLE);
        }
    }
}
