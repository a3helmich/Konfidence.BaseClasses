using System.Web.Services;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers
{
    [UsedImplicitly]
    public class BaseWebService<T> : WebService where T : BaseWebPresenter, new()
    {
        private T _presenter;

        [UsedImplicitly]
        public T Presenter
        {
            get
            {
                if (!_presenter.IsAssigned())
                {
                    _presenter = new T();
                }

                return _presenter;
            }
        }
    }
}
