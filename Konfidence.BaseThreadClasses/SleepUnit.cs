using JetBrains.Annotations;

namespace Konfidence.BaseThreadClasses
{
    public enum SleepUnit
    {
        Daily = 1,
        Hourly = 2,
        Minutes = 3,
        Seconds = 4,
        [UsedImplicitly]
        Unknown = 0
    }
}
