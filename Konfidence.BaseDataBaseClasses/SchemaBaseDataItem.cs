using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Konfidence.BaseData
{
    public partial class SchemaBaseDataItem : BaseDataItem
    {
        protected DataTable GetSchemaObject(string objectType)
        {
            BaseHost dataHost = GetHost();

            return dataHost.GetSchemaObject(objectType);
        }
    }
}
