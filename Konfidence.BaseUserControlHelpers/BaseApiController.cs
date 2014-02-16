using Konfidence.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;

namespace Konfidence.BaseUserControlHelpers
{
    public class BaseApiController : ApiController
    {
        protected bool IsEmpty(string assignedString)
        {
            return BaseItem.IsEmpty(assignedString);
        }

        protected bool IsAssigned(object assignedObject)
        {
            return BaseItem.IsAssigned(assignedObject);
        }
    }
}
