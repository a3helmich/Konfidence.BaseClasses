using System.IO;
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
        private string _LogFile;
        private StreamWriter _LogStream;
        private bool _Disposed; //  = false; // default :(

        private StreamWriter LogStream
        {
            get
            {
                if (_LogStream == null)
                {
                    _LogStream = File.AppendText(_LogFile);
                }
                return _LogStream;
            }
        }

        public string LogFile
        {
            get { return _LogFile; }
            set
            {
                if (_LogFile != null)
                {
                    LogStream.Close();
                    _LogStream = null;
                }
                _LogFile = value;
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
            if (_Disposed)
            {
                LogStream.Flush();
                LogStream.Close();
            }
        }

        protected virtual void Dispose()
        {
            LogStream.Flush();
            LogStream.Close();
            _Disposed = true;
        }


    }

    sealed public class ApplicationLoggerFactory : SingletonFactory
    {
        static public IApplicationLogger ApplicationLogger(string logFile)
        {
            var applicationLogger = GetInstance(typeof(ApplicationLogger)) as IApplicationLogger;

            if (applicationLogger != null)
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
