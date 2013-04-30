using System.Data;
using Konfidence.Base;

namespace Konfidence.BaseData
{
    internal class DatabaseReaderRepository : BaseItem, IDatabaseReaderRepository
    {
        private IDataReader _DataReader;

        public DatabaseReaderRepository(IDataReader dataReader)
        {
            _DataReader = dataReader;
        }
    }
}
