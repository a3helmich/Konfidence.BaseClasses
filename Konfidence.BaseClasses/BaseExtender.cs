using System;
using JetBrains.Annotations;

namespace Konfidence.Base
{
    public static class BaseExtender
    {
        [ContractAnnotation("assignedObject:null => false")]
        public static bool IsAssigned(this object assignedObject)
        {
            var assignedString = assignedObject as string;

            if (assignedObject == null)
            {
                return false;
            }

            if (assignedString != null)
            {
                if (string.IsNullOrWhiteSpace(assignedString))
                {
                    return false;
                }
            }

            return true;
        }

        [ContractAnnotation("assignedGuid:null => false")]
        public static bool IsGuid(this string assignedGuid)
        {
            Guid isGuid;

            if (Guid.TryParse(assignedGuid, out isGuid))
            {
                return true;
            }

            return false;
        }
    }
}
