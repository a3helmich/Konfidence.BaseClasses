using System;

namespace Konfidence.BaseData
{
    public class SqlHostException : Exception
    {
        public SqlHostException(string description)
            : base(description)
        {
        }
    }
}
