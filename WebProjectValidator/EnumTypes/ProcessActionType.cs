﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebProjectValidator.EnumTypes
{
    enum ProcessActionType
    {
        None = 0,
        DesignerFileAll = 1,
        DesignerFileExists = 2,
        DesignerFileMissing = 3,
        UserControlUnused = 4,
        WebProject = 5,
        WebApplication = 6,
        UserControlValid = 7,
        UserControlInvalid = 8,
        UserControlMissing = 9,
        UserControlAll = 10,
    }
}
