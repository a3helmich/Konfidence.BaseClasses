using System.Data;

namespace Konfidence.DataBaseInterface
{
    public interface IDbParameterObject 
    {
        string ParameterName { get; set; }

        DbType DbType { get; set; }

        object Value { get; set; }
    }
}
