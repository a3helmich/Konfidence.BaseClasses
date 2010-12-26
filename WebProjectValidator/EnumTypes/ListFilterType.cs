using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebProjectValidator.EnumTypes
{
    enum ListFilterType
    {
        Unknown = 0,
        All = 1,
        Valid = 2,
        Invalid = 3,
        DesignerFileExists = 4,
        DesignerFileMissing = 5,
        Unused = 6,
        WebProject,
        WebApplication
    }
}
