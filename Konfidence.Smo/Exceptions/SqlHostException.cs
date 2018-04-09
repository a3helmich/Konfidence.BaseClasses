using System;

namespace Konfidence.SqlHostProvider.Exceptions
{
    public class SqlHostException : Exception
    {
        public SqlHostException(string description)
            : base(description)
        {
        }
    }
}
