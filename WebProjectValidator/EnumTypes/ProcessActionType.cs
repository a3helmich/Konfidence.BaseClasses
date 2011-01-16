using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebProjectValidator.EnumTypes
{
    public enum ProcessActionType
    {
        None = 0,
        DesignerFileExists = 2,
        InProjectFile = 3,
        UserControlUnused = 4,
        WebProject = 5,
        Website = 6,
        UserControlValid = 7,
        UserControlInvalid = 8,
        UserControlMissing = 9,
        UserControlAll = 10,
    }
}
