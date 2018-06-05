using Konfidence.Base;

namespace Konfidence.BaseThreadClasses
{
    public delegate void BeforeExecute<in T>(T baseThreadAction);
    public delegate void AfterExecute<in T>(T baseThreadAction);
    public delegate void InitializeAction<in T>(T baseThreadAction);

    public delegate void AfterStart();
    public delegate void AfterStop();

    public class ThreadManager<TAction> where TAction : ThreadAction, new()
    {
        public BeforeExecute<TAction> BeforeExecuteAction;
        public AfterExecute<TAction> AfterExecuteAction;
        public InitializeAction<TAction> InitializeAction;

        public AfterStart AfterStart;
        public AfterStop AfterStop;

        private ThreadRunner<TAction> ThreadRunner { get; }

        public bool IsRunning => ThreadRunner.IsAssigned() && ThreadRunner.IsRunning;

        public ThreadManager()
        {
            ThreadRunner = new ThreadRunner<TAction>(this);
        }

        public void StartThread(int sleepTime = 1, SleepUnit sleepUnit = SleepUnit.Seconds)
        {
            if (IsRunning)
            {
                return;
            }

            ThreadRunner.StartThreadRunner(sleepTime, sleepUnit);

            if (AfterStart.IsAssigned())
            {
                AfterStart();
            }
        }

        public void StopThread()
        {
            if (!IsRunning)
            {
                return;
            }

            ThreadRunner.StopThreadRunner();

            if (AfterStop.IsAssigned())
            {
                AfterStop();
            }
        }

        internal void InternalInitializeAction(TAction threadAction)
        {
            if (InitializeAction.IsAssigned())
            {
                InitializeAction(threadAction);
            }
        }

        internal void InternalBeforeExecuteAction(TAction threadAction)
        {
            if (BeforeExecuteAction.IsAssigned())
            {
                BeforeExecuteAction(threadAction);
            }
        }

        internal void InternalAfterExecuteAction(TAction threadAction)
        {
            if (AfterExecuteAction.IsAssigned())
            {
                AfterExecuteAction(threadAction);
            }
        }
    }
}
