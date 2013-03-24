using System.Data.SqlClient;

namespace Konfidence.BaseData
{
    public class SqlServerDataItem: BaseDataItem
    {
        private readonly int _SqlServerId;

        private const string SQLSERVER_GET = "SqlServer_Get";

        private const string SQLSERVERID = "SqlServerId";

        private const string AVAILABLE = "Available";

        private bool _IsAvailable = true;

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
        #endregion properties

        public SqlServerDataItem() 
        {
            SqlServerGetItem();
        }

        public SqlServerDataItem(int sqlServerId)
        {
            _SqlServerId = sqlServerId;

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

                SetErrorMessage(sqlException.Message);
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
            GetField(AVAILABLE, out _IsAvailable);
        }
    }
}
