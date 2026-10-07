using System;
using System.Diagnostics.CodeAnalysis;

namespace PackageCleaner;

[ExcludeFromCodeCoverage]
public class ErrorExiter : IErrorExiter
{
    public void Exit(int errorCode)
    {
        Environment.Exit(errorCode);
    }
}
