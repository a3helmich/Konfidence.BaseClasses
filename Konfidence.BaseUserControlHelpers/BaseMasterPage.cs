﻿using System;
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
        private bool _IsMasterPagePostBack = false;

        private T _Presenter = null;

        public T Presenter
        {
            get
            {
                if (!IsAssigned(_Presenter))
                {
                    BuildPresenter();
                }

                return _Presenter;
            }
        }

        protected abstract void RestoreViewState();
        protected abstract void FormToPresenter();
        protected abstract void PresenterToForm();

        #region readonly session properties
        protected string CurrentDomainExtension
        {
            get
            {
                if (IsAssigned(_BasePageHelper))
                {
                    return _BasePageHelper.CurrentDomainExtension;
                }

                return string.Empty;
            }
        }

        protected string CurrentLanguage
        {
            get
            {
                if (IsAssigned(_BasePageHelper))
                {
                    return _BasePageHelper.CurrentLanguage;
                }

                return string.Empty;
            }
        }

        protected string CurrentDnsName
        {
            get
            {
                if (IsAssigned(_BasePageHelper))
                {
                    return _BasePageHelper.CurrentDnsName;
                }

                return string.Empty;
            }
        }

        protected string CurrentPagePath
        {
            get
            {
                if (IsAssigned(_BasePageHelper))
                {
                    return _BasePageHelper.CurrentPagePath;
                }

                return string.Empty;
            }
        }

        protected string CurrentPageName
        {
            get
            {
                if (IsAssigned(_BasePageHelper))
                {
                    return _BasePageHelper.CurrentPageName;
                }

                return string.Empty;
            }
        }
        #endregion readonly session properties

        private void BuildPresenter()
        {
            if (!IsAssigned(_BasePageHelper))
            {
                try
                {
                    _BasePageHelper = new BasePageHelper(this.Request.Url.ToString());
                }
                catch (NullReferenceException)
                {
                    // jammer dan
                }
            }

            if (!IsAssigned(_Presenter))
            {
                _Presenter = new T();
            }

            if (IsEmpty(_Presenter.PageName))
            {
                _Presenter.SetPageName(CurrentPageName);
            }
        }

        protected bool IsRestoreViewState
        {
            get
            {
                if (IsPostBack)
                {
                    if (GetViewState("IsRestoreViewState").Equals("IsRestoreViewState"))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        protected string GetViewState(string fieldName)
        {
            object viewState = ViewState[fieldName];

            if (viewState != null)
            {
                return viewState.ToString();
            }

            return string.Empty;
        }


        protected string GetSessionState(string fieldName)
        {
            object sessionState = Session[fieldName];

            if (sessionState != null)
            {
                return sessionState.ToString();
            }

            return string.Empty;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !Presenter.LogonPageName.Equals(Presenter.PageName, StringComparison.InvariantCultureIgnoreCase))
            {
                Presenter.FromUrl = CurrentPagePath;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsRestoreViewState)
            {
                RestoreViewState();
            }

            FormToPresenter();
        }

        private void CheckIsMasterPagePostBack()
        {
            if (IsPostBack)
            {
                if (GetSessionState(Page.MasterPageFile + "_FromMasterPage").Equals(Page.MasterPageFile))
                {
                    _IsMasterPagePostBack = true;

                    Session.Remove(Page.MasterPageFile + "_FromMasterPage");
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Presenter.IsLoggedIn)
            {
                Redirect(Presenter.SignInUrl);
            }

            PresenterToForm();

            ViewState["IsRestoreViewState"] = "IsRestoreViewState";
            Session[Page.MasterPageFile + "_FromMasterPage"] = Page.MasterPageFile;
        }

        protected bool IsMasterPagePostBack
        {
            get { return _IsMasterPagePostBack; }
        }

        //protected bool IsMasterPagePostBack
        //{
        //    get
        //    {
        //        if (Session["FromMasterPage"] != null)
        //        {
        //            if (Session["FromMasterPage"].Equals(Page.MasterPageFile))
        //            {
        //                return true;
        //            }
        //        }

        //        return false;
        //    }
        //}

        protected void Redirect(string url)
        {
            if (!BaseItem.IsEmpty(url))
            {
                Response.Redirect(url, false);

                Response.End();
            }
        }

        protected bool IsEmpty(string assignedString)
        {
            return BaseItem.IsEmpty(assignedString);
        }

        protected bool IsAssigned(object assignedObject)
        {
            return BaseItem.IsAssigned(assignedObject);
        }
    }
}
