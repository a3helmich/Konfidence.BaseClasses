using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.BaseThreadClasses
{
    public delegate void BeforeExecute<in T>(T baseThreadAction);
    public delegate void AfterExecute<in T>(T baseThreadAction);
    public delegate void InitializeAction<in T>(T baseThreadAction);

    public class ThreadManager<TAction> where TAction : ThreadAction, new()
    {
        public InitializeAction<TAction> InitializeAction;
        public BeforeExecute<TAction> BeforeExecuteAction;
        public AfterExecute<TAction> AfterExecuteAction;

        private ThreadRunner<TAction> ThreadRunner { get; }

        public bool IsRunning => ThreadRunner.IsAssigned() && ThreadRunner.IsRunning;

        public ThreadManager()
        {
            ThreadRunner = new ThreadRunner<TAction>(this);
        }

        [UsedImplicitly]
        public ThreadManager<TAction> SetInitializeAction(InitializeAction<TAction> initializeAction)
        {
            InitializeAction = initializeAction;

            return this;
        }

        [UsedImplicitly]
        public ThreadManager<TAction> SetBeforeExecuteAction(BeforeExecute<TAction> beforeExecuteAction)
        {
            BeforeExecuteAction = beforeExecuteAction;

            return this;
        }

        [UsedImplicitly]
        public ThreadManager<TAction> SetAfterExecuteAction(AfterExecute<TAction> afterExecuteAction)
        {
            AfterExecuteAction = afterExecuteAction;

            return this;
        }

        [UsedImplicitly]
        public void StartThread(int sleepTime = 1, SleepUnit sleepUnit = SleepUnit.Seconds)
        {
            if (IsRunning)
            {
                return;
            }

            ThreadRunner.StartThreadRunner(sleepTime, sleepUnit);
        }

        [UsedImplicitly]
        public void StopThread()
        {
            if (!IsRunning)
            {
                return;
            }

            ThreadRunner.StopThreadRunner();
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
