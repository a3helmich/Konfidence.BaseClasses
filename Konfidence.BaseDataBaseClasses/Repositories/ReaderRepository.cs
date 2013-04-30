using System.Data;
using Konfidence.Base;
using Konfidence.BaseData.IRepositories;

namespace Konfidence.BaseData.Repositories
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
