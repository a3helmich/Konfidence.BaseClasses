using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Konfidence.Base;
using System.Configuration;

namespace Konfidence.BaseUserControlHelpers
{
    public abstract class BaseMasterPage<T> : MasterPage where T : BaseWebPresenter, new()
    {
        private BasePageHelper _BasePageHelper = null;

        private T _Presenter = null;

        public T Presenter
        {
            get
            {
                return _Presenter;
            }
        }

        protected abstract void FormToPresenter();
        protected abstract void PresenterToForm();

        #region readonly session properties
        protected string CurrentDomainExtension
        {
            get { return _BasePageHelper.CurrentDomainExtension; }
        }

        protected string CurrentLanguage
        {
            get { return _BasePageHelper.CurrentLanguage; }
        }

        protected string CurrentDnsName
        {
            get { return _BasePageHelper.CurrentDnsName; }
        }

        protected string CurrentPagePath
        {
            get { return _BasePageHelper.CurrentPagePath; }
        }

        protected string CurrentPageName
        {
            get { return _BasePageHelper.CurrentPageName; }
        }
        #endregion readonly session properties

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsAssigned(_BasePageHelper))
            {
                _BasePageHelper = new BasePageHelper(this.Request.Url.ToString());
            }

            if (!IsAssigned(_Presenter))
            {
                _Presenter = new T();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FormToPresenter();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            PresenterToForm();
        }

        protected void Redirect(string url)
        {
            if (!BaseItem.IsEmpty(url))
            {
                Response.Redirect(url, false);

                Response.End();
            }
        }

        protected bool IsAssigned(object assignedObject)
        {
            return BaseItem.IsAssigned(assignedObject);
        }
    }
}
