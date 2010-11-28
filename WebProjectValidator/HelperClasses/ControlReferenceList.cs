using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace WebProjectValidator.HelperClasses
{
    public class ControlReferenceList :  List<ControlReference>
    {
        public bool FindReference()
        {
            return false;
        }
    }
}
