using System.Diagnostics;
using System.Security.Permissions;

namespace Konfidence.BaseHelper
{
    /// <summary>
    /// Summary description for BaseConfigClass.
    /// </summary>
    /// 

    // TODO: this class has to be removed at some point

    // TODO: make this a singleton class;
    [EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted = true)]
    public class BaseConfigClass
    {
        // statics
        private static string _ApplicationEventLogName = "FrameWork";
        private static EventLog _EventLog;

        private string _EventLogNamePrefix = "Kit_";
        private string _EventLogName;

        #region ApplicationName
        public static string ApplicationName
        {
            get
            {
                return _ApplicationEventLogName;
            }
        }
        #endregion


        public EventLog EventLog
        {
            get
            {
                if (_EventLog == null)
                {
                    _EventLogName = _EventLogNamePrefix + _ApplicationEventLogName;

                    // the next thing works when we are not deiling with a webapp or service
                    if (!EventLog.SourceExists(_EventLogName))
                        EventLog.CreateEventSource(_EventLogName, "Application");
                    _EventLog = new EventLog();
                    _EventLog.Source = _EventLogName;
                }
                return _EventLog;
            }
        }

        public BaseConfigClass(string applicationName)
        {
            if (applicationName != null)
            {
                if (applicationName.Length == 0 && !_ApplicationEventLogName.Equals("FrameWork"))
                {
                    _ApplicationEventLogName = "FrameWork";
                }
                if (applicationName.Length != 0 && !_ApplicationEventLogName.Equals(applicationName))
                {
                    _ApplicationEventLogName = applicationName;
                }
            }
        }

        public BaseConfigClass()
            : this(string.Empty)
        {

        }
    }
}
