namespace ClientSettingsUpdater
{
    static class Program
    {
        static void Main(string[] args)
        {
            ClientSettingsManager? clientSettingsManager = new ClientSettingsManager(args, new ErrorExiter());

            clientSettingsManager.Execute();
        }
    }
}
