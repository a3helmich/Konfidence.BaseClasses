using System;

namespace Konfidence.SqlHostProvider.Exceptions
{
    public class SqlClientException : Exception
    {
        public SqlClientException(string description) : base(description)
        {
        }
    }
}
