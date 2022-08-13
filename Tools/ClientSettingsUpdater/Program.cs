namespace ClientSettingsUpdater
{
    static class Program
    {
        static void Main(string[] args)
        {
            var clientSettingsManager = new ClientSettingsManager(args, new ErrorExiter());

            clientSettingsManager.Execute();
        }
    }
}
