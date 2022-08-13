using System.Data;

namespace Konfidence.DatabaseInterface
{
    public interface ISpParameterData 
    {
        string ParameterName { get; set; }

        DbType DbType { get; set; }

        object? Value { get; set; }
    }
}
