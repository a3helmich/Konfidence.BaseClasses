using System;

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
