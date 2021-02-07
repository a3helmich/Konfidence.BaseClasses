using System.Data;

namespace Konfidence.DataBaseInterface
{
    public interface ISpParameterData 
    {
        string ParameterName { get; set; }

        DbType DbType { get; set; }

        object Value { get; set; }
    }
}
