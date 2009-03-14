
namespace Konfidence.Base
{
    public class BaseControlHelper: BaseItem
    {
        private bool _isDebug = false;

        public bool IsDebug
        {
            get { return _isDebug; }
        }

        public BaseControlHelper()
        {
#if DEBUG
            _isDebug = true;
#endif
        }
    }
}
