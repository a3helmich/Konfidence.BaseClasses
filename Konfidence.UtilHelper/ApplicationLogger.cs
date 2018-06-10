using System.IO;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.DesignPatterns.Singleton;

namespace Konfidence.UtilHelper
{
    /// <summary>
    /// Summary description for ApplicationLogger.
    /// </summary>
    public interface IApplicationLogger : ISingleton
    {
        string LogFile { get; set; }

        void Log(string logMessage);
    }

    internal class ApplicationLogger : IApplicationLogger
    {
        private string _logFile;
        private StreamWriter _logStream;
        private bool _disposed; //  = false; // default :(

        private StreamWriter LogStream
        {
            get
            {
                if (!_logStream.IsAssigned())
                {
                    _logStream = File.AppendText(_logFile);
                }
                return _logStream;
            }
        }

        public string LogFile
        {
            get => _logFile;
            set
            {
                if (_logFile.IsAssigned())
                {
                    LogStream.Close();
                    _logStream = null;
                }
                _logFile = value;
            }
        }

        public void Log(string logMessage)
        {
            WriteLog(logMessage);
            LogStream.Flush();
        }

        protected virtual void WriteLog(string logMessage)
        {
            LogStream.WriteLine(logMessage);
        }

        ~ApplicationLogger()
        {
            if (_disposed)
            {
                LogStream.Flush();
                LogStream.Close();
            }
        }

        [UsedImplicitly]
        protected virtual void Dispose()
        {
            LogStream.Flush();
            LogStream.Close();
            _disposed = true;
        }


    }

    [UsedImplicitly]
    public sealed class ApplicationLoggerFactory : SingletonFactory
    {
        [UsedImplicitly]
        public static IApplicationLogger ApplicationLogger(string logFile)
        {
            var applicationLogger = GetInstance(typeof(ApplicationLogger)) as IApplicationLogger;

            if (applicationLogger.IsAssigned())
            {
                applicationLogger.LogFile = logFile;

                return applicationLogger;
            }

            return null;
        }

        private ApplicationLoggerFactory()
        {
        }
    }
}
