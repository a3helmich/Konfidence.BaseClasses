using System;
using System.Collections.Generic;
using System.Text;

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
