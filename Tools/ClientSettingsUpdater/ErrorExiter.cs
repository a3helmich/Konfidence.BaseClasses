using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSettingsUpdater
{
    public class ErrorExiter : IErrorExiter
    {
        public void Exit(int errorCode)
        {
            Environment.Exit(errorCode);
        }
    }
}
