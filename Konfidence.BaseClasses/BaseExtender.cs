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
                if (String.IsNullOrWhiteSpace(assignedString))
                {
                    return false;
                }
            }

            return true;
        }

        //[ContractAnnotation("assignedString:null => false")]
        //public static bool IsEmpty(this string assignedString) 
        //{
        //    if (String.IsNullOrWhiteSpace(assignedString))
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        //[ContractAnnotation("assignedObject:null => false")]
        //public static bool IsNull(this string assignedObject)
        //{
        //    return BaseItem.IsNull(assignedObject);
        //}

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
