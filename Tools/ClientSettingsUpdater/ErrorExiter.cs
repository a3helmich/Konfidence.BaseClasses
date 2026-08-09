using System;
using System.Diagnostics.CodeAnalysis;

namespace ClientSettingsUpdater;

[ExcludeFromCodeCoverage]
public class ErrorExiter : IErrorExiter
{
    public void Exit(int errorCode)
    {
        Environment.Exit(errorCode);
    }
}
