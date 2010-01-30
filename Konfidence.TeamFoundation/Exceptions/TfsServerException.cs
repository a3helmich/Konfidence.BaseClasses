using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Konfidence.TeamFoundation.Exceptions
{
    public class TfsServerException : Exception
    {
        public TfsServerException(string message) : base(message) { }
    }
}
