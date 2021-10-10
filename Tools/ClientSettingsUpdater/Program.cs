using System;
using JetBrains.Annotations;

namespace ClientSettingsUpdater
{
    class Program
    {
        static void Main([NotNull] string[] args)
        {
            var clientSettingsManager = new ClientSettingsManager(args, new ErrorExiter());

            clientSettingsManager.Execute();
        }
    }
}
