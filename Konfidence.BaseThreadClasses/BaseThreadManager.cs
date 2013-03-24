using Konfidence.Base;

namespace Konfidence.BaseThreadClasses
{
    public abstract class BaseThreadManager<T, TK> : BaseItem
        where T : BaseThreadRunner<TK>, new() where TK : BaseThreadAction, new() 
    {
        protected abstract void BeforeStart();
        protected abstract void AfterStop();

        private readonly T _ThreadRunner;

        private int _SleepMilliSeconds;
        private int _WaitMilliSeconds = 100;

        public int SleepMilliSeconds
        {
            get { return _SleepMilliSeconds; }
            set { _SleepMilliSeconds = value; }
        }

        public int WaitMilliSeconds
        {
            get { return _WaitMilliSeconds; }
            set { _WaitMilliSeconds = value; }
        }

        protected T ThreadRunner
        {
            get { return _ThreadRunner; }
        }

        public bool IsRunning
        {
            get
            {
                if (IsAssigned(_ThreadRunner))
                {
                    return _ThreadRunner.IsRunning;
                }

                return false;
            }
        }

        protected BaseThreadManager()
        {
            _ThreadRunner = new T();
        }

        public void StartThread()
        {
            if (!IsRunning)
            {
                BeforeStart();

                ThreadRunner.SleepTime = _SleepMilliSeconds;
                ThreadRunner.WaitTime = _WaitMilliSeconds;

                ThreadRunner.StartThreadRunner();
            }
        }

        public void StopThread()
        {
            if (IsRunning)
            {
                ThreadRunner.StopThreadRunner();

                AfterStop();
            }
        }
    }
}
