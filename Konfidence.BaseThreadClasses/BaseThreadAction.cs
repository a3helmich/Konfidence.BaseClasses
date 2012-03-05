﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;

namespace Konfidence.BaseThreadClasses
{
    public abstract class BaseThreadAction : BaseItem
    {
        internal abstract void Execute();

        private bool _IsTerminating = false;

        internal bool IsTerminating
        {
            get { return _IsTerminating; }
            set { _IsTerminating = value; }
        }
    }
}
