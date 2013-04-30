using System.Data;
using Konfidence.Base;

namespace Konfidence.BaseData
{
    internal class ReaderRepository : BaseItem, IReaderRepository
    {
        private IDataReader _DataReader;

        public ReaderRepository(IDataReader dataReader)
        {
            _DataReader = dataReader;
        }
    }
}
