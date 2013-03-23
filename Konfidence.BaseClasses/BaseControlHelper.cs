
namespace Konfidence.Base
{
    public class BaseControlHelper: BaseItem
    {
        private readonly bool _IsDebug;

        public bool IsDebug
        {
            get { return _IsDebug; }
        }

        public BaseControlHelper()
        {
#if DEBUG
            _IsDebug = true;
#else
            _IsDebug = false;
#endif
        }
    }
}
