using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.BaseThreadClasses
{
    public delegate void BeforeExecute<in T>(T baseThreadAction);
    public delegate void AfterExecute<in T>(T baseThreadAction);
    public delegate void InitializeAction<in T>(T baseThreadAction);

    public class ThreadManager<TAction> where TAction : ThreadAction, new()
    {
        private InitializeAction<TAction> _initializeAction;
        private BeforeExecute<TAction> _beforeExecuteAction;
        private AfterExecute<TAction> _afterExecuteAction;

        private ThreadRunner<TAction> ThreadRunner { get; }

        public bool IsRunning => ThreadRunner.IsAssigned() && ThreadRunner.IsRunning;

        public ThreadManager()
        {
            ThreadRunner = new ThreadRunner<TAction>(this);
        }

        [UsedImplicitly]
        public ThreadManager<TAction> SetInitializeAction(InitializeAction<TAction> initializeAction)
        {
            _initializeAction = initializeAction;

            return this;
        }

        [UsedImplicitly]
        public ThreadManager<TAction> SetBeforeExecuteAction(BeforeExecute<TAction> beforeExecuteAction)
        {
            _beforeExecuteAction = beforeExecuteAction;

            return this;
        }

        [UsedImplicitly]
        public ThreadManager<TAction> SetAfterExecuteAction(AfterExecute<TAction> afterExecuteAction)
        {
            _afterExecuteAction = afterExecuteAction;

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
            if (_initializeAction.IsAssigned())
            {
                _initializeAction(threadAction);
            }
        }

        internal void InternalBeforeExecuteAction(TAction threadAction)
        {
            if (_beforeExecuteAction.IsAssigned())
            {
                _beforeExecuteAction(threadAction);
            }
        }

        internal void InternalAfterExecuteAction(TAction threadAction)
        {
            if (_afterExecuteAction.IsAssigned())
            {
                _afterExecuteAction(threadAction);
            }
        }
    }
}
