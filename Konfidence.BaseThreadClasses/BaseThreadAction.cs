using Konfidence.Base;

namespace Konfidence.BaseThreadClasses
{
    public abstract class BaseThreadAction : BaseItem 
    {
        protected internal abstract void Execute();

        public BaseThreadAction()
        {
            IsTerminating = false;
        }

        internal bool IsTerminating { get; set; }

        protected void Finished()
        {
            IsTerminating = true;
        }

        //protected void Sleep(int milliseconds)
        //{
        //    Thread.Sleep(milliseconds);
        //}
    }
}
