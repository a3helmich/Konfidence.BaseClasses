using System.ServiceProcess;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.BaseWindowForms
{
    public class BaseServiceBase : ServiceBase
    {
        [ContractAnnotation("assignedObject:null => false")]
        public static bool IsAssigned(object assignedObject)
        {
            return BaseItem.IsAssigned(assignedObject);
        }
    }
}
