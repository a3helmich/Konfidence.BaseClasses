using System.Web.Services;
using JetBrains.Annotations;
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
                if (!IsAssigned(_Presenter))
                {
                    _Presenter = new T();
                }

                return _Presenter;
            }
        }

        [ContractAnnotation("assignedObject:null => false")]
        protected static bool IsAssigned(object assignedObject)
        {
            return BaseItem.IsAssigned(assignedObject);
        }

        protected static bool IsEmpty(string assignedString)
        {
            return BaseItem.IsEmpty(assignedString);
        }
        
    }
}
