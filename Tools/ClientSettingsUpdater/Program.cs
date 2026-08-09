using System.Diagnostics.CodeAnalysis;

namespace ClientSettingsUpdater;

[ExcludeFromCodeCoverage]
static class Program
{
    static void Main(string[] args)
    {
        ClientSettingsManager clientSettingsManager = new(args, new ErrorExiter());

        clientSettingsManager.Execute();
    }
}
