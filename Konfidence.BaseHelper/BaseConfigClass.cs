using System.Diagnostics;
using System.Security.Permissions;
using JetBrains.Annotations;
using Konfidence.Base;

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
        private static string _applicationEventLogName = "FrameWork";
        private static EventLog _eventLog;

        private const string EventLogNamePrefix = "Kit_";
        private string _eventLogName;

        [UsedImplicitly]
        public static string ApplicationName => _applicationEventLogName;


        public EventLog EventLog
        {
            get
            {
                if (!_eventLog.IsAssigned())
                {
                    try
                    {
                        _eventLogName = EventLogNamePrefix + _applicationEventLogName;

                        // the next thing works when we are not deiling with a webapp or service
                        if (!EventLog.SourceExists(_eventLogName))
                        {
                            EventLog.CreateEventSource(_eventLogName, "Application");
                        }

                        _eventLog = new EventLog { Source = _eventLogName };
                    }
                    catch 
                    {
                        _eventLog = null;
                    }
                }

                return _eventLog;
            }
        }

        public BaseConfigClass(string applicationName)
        {
            if (applicationName.IsAssigned())
            {
                if (applicationName.Length == 0 && !_applicationEventLogName.Equals("FrameWork"))
                {
                    _applicationEventLogName = "FrameWork";
                }
                if (applicationName.Length != 0 && !_applicationEventLogName.Equals(applicationName))
                {
                    _applicationEventLogName = applicationName;
                }
            }
        }

        [UsedImplicitly]
        public BaseConfigClass() : this(string.Empty)
        {

        }
    }
}
