using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konfidence.BaseClassInterfaces
{
    public interface IBaseItemList<T> : IList<T> where T : IBaseItem
    {
    }
}
