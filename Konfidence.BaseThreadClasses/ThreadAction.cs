namespace Konfidence.BaseThreadClasses
{
    public abstract class ThreadAction
    {
        private readonly object _lockObject = new();

        internal bool IsAlive { get; private set; }

        internal void ExecuteAction()
        {
            IsAlive = true;

            try
            {
                lock (_lockObject)
                {
                    Execute();
                }
            }
            finally
            {
                IsAlive = false;
            }
        }

        protected abstract void Execute();
    }
}
