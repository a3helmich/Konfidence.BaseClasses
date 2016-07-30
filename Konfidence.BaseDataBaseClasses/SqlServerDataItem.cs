using System.Data.SqlClient;

namespace Konfidence.BaseData
{
    public class SqlServerDataItem: BaseDataItem
    {
        private readonly int _sqlServerId;

        private const string SqlserverGet = "SqlServer_Get";

        private const string Sqlserverid = "SqlServerId";

        private const string Available = "Available";

        private bool _isAvailable = true;

        #region properties
        public int SqlServerId => Id;

        public bool IsAvailable => _isAvailable;

        #endregion properties

        public SqlServerDataItem() 
        {
            SqlServerGetItem();
        }

        public SqlServerDataItem(int sqlServerId)
        {
            _sqlServerId = sqlServerId;

            SqlServerGetItem();
        }

        private void SqlServerGetItem()
        {
            try
            {
                GetItem(SqlserverGet, 1);
            }
            catch(SqlException sqlException)
            {
                _isAvailable = false;

                SetErrorMessage(sqlException.Message);
            }
        }

        //public SqlServerDataItem(List<BaseDataItem.ParameterObject> ParameterList)
        //{
        //    GetItem(ParameterList);
        //}

        protected internal override void InitializeDataItem()
        {
            AutoIdField = Sqlserverid;

            ServiceName = "SqlServerDataItemService";
        }

        protected internal override void GetData()
        {
            GetField(Available, out _isAvailable);
        }
    }
}
