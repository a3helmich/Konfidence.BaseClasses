using System;
using System.Collections.Generic;
using System.Text;

namespace Konfidence.Base
{
    public class BaseItemList<T> : List<T> where T : BaseItem, new()
    {
    }
}
