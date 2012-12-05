using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using Konfidence.Base;

namespace Konfidence.BaseWindowForms
{
    public class BaseServiceBase : ServiceBase
    {
        public static bool IsAssigned(object assignedObject)
        {
            return BaseItem.IsAssigned(assignedObject);
        }
    }
}
