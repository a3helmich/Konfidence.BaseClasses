using System.Web.Services;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers
{
    public class BaseWebService<T> : WebService where T : BaseWebPresenter, new()
    {
        private T _Presenter;

        public T Presenter
        {
            get
            {
                if (!_Presenter.IsAssigned())
                {
                    _Presenter = new T();
                }

                return _Presenter;
            }
        }
    }
}
