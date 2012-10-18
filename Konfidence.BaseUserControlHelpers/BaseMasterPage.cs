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

        private Dictionary<string, string> _MasterPageDictionaryIn = null;
        private Dictionary<string, string> _MasterPageDictionaryOut = null;

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

        protected string RefererDnsName
        {
            get
            {
                if (IsAssigned(_BasePageHelper))
                {
                    return _BasePageHelper.RefererDnsName;
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
                    _BasePageHelper = new BasePageHelper(this.Request.Url.ToString(), this.Request.UrlReferrer.ToString());
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

        protected Dictionary<string, string> MasterPageDictionaryIn
        {
            get
            {
                if (!IsAssigned(_MasterPageDictionaryIn))
                {
                    if (!IsAssigned(Session["MasterPageDictionaryIn"]))
                    {
                        Session["MasterPageDictionaryIn"] = new Dictionary<string, string>();
                    }

                    _MasterPageDictionaryIn = Session["MasterPageDictionaryIn"] as Dictionary<string, string>;
                }

                return _MasterPageDictionaryIn;
            }
        }

        protected Dictionary<string, string> MasterPageDictionaryOut
        {
            get
            {
                if (!IsAssigned(_MasterPageDictionaryOut))
                {
                    if (!IsAssigned(Session["MasterPageDictionaryOut"]))
                    {
                        Session["MasterPageDictionaryOut"] = new Dictionary<string, string>();
                    }

                    _MasterPageDictionaryOut = Session["MasterPageDictionaryOut"] as Dictionary<string, string>;
                }

                return _MasterPageDictionaryOut;
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

        private void BuildMasterPageDictionaries()
        {
            if (MasterPageDictionaryOut.Count > 0)
            {
                MasterPageDictionaryIn.Clear();

                foreach (KeyValuePair<string, string> kvp in MasterPageDictionaryOut)
                {
                    MasterPageDictionaryIn.Add(kvp.Key, kvp.Value);
                }

                MasterPageDictionaryOut.Clear();
            }
        }

        private List<string> _MasterPageFileList = null;

        private List<string> MasterPageFileList
        {
            get
            {
                if (!IsAssigned(_MasterPageFileList))
                {
                    _MasterPageFileList = new List<string>();

                    AddMasterPageFile(Page.Master, Page.MasterPageFile);
                }

                return _MasterPageFileList;
            }
        }

        private void AddMasterPageFile(MasterPage masterPage, string masterPageFile)
        {
            if (!IsEmpty(masterPageFile))
            {
                _MasterPageFileList.Add(masterPageFile);
            }

            if (IsAssigned(masterPage))
            {
                AddMasterPageFile(masterPage.Master, masterPage.MasterPageFile);
            }
        }

        private void CheckIsMasterPagePostBack()
        {
            foreach (string masterPageFile in MasterPageFileList)
            {
                if (MasterPageDictionaryIn.ContainsKey(masterPageFile))
                {
                    _IsMasterPagePostBack = true;
                }
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            BuildMasterPageDictionaries();

            if (!IsPostBack && !Presenter.LogonPageName.Equals(Presenter.PageName, StringComparison.InvariantCultureIgnoreCase))
            {
                Presenter.FromUrl = CurrentPagePath;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckIsMasterPagePostBack();

            if (IsRestoreViewState)
            {
                RestoreViewState();
            }

            FormToPresenter();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Presenter.IsLoggedIn)
            {
                Redirect(Presenter.SignInUrl);
            }

            PresenterToForm();

            ViewState["IsRestoreViewState"] = "IsRestoreViewState";

            foreach (string masterPageFile in MasterPageFileList)
            {
                if (!MasterPageDictionaryOut.ContainsKey(masterPageFile))
                {
                    MasterPageDictionaryOut.Add(masterPageFile, masterPageFile);
                }
            }
        }

        protected bool IsMasterPagePostBack
        {
            get { return _IsMasterPagePostBack; }
        }

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
