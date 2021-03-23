namespace Konfidence.BaseThreadClasses
{
    public abstract class ThreadAction
    {
        private static readonly object LockObject = new();

        internal bool IsAlive { get; private set; }

        internal void ExecuteAction()
        {
            IsAlive = true;

            lock (LockObject)
            {
                Execute();
            }

            IsAlive = false;
        }

        protected abstract void Execute();
    }
}
