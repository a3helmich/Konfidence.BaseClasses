using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebProjectValidator.EnumTypes
{
    public enum DeveloperFileType
    {
        Unknown = 0,
        DesignerFile = 1,   // .designer.
        SourceFile = 2,     // .cs, .vb
        WebFile = 3         // .aspx, .ascx, .master, asax
    }
}
