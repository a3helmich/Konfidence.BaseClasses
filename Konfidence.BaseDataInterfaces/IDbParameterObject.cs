using System.Data;

namespace Konfidence.BaseDataInterfaces
{
    public interface IDbParameterObject : IBaseDataItem
    {
        string Field { get; set; }

        DbType DbType { get; set; }

        object Value { get; set; }
    }
}
