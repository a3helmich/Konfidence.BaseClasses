using System.Data;

namespace Konfidence.BaseDataInterfaces
{
    public interface IDbParameterObject 
    {
        string Field { get; set; }

        DbType DbType { get; set; }

        object Value { get; set; }
    }
}
