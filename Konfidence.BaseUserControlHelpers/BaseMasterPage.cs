using System;
using System.Collections.Generic;
using System.Web.UI;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers
{
    public abstract class BaseMasterPage<T> : MasterPage where T : BaseWebPresenter, new()
    {
        private BasePageHelper _BasePageHelper;

        private Dictionary<string, string> _MasterPageDictionaryIn;
        private Dictionary<string, string> _MasterPageDictionaryOut;

        private bool _IsMasterPagePostBack;

        private T _Presenter;

        public T Presenter
        {
            get
            {
                if (!_Presenter.IsAssigned())
                {
                    BuildPresenter();
                }

                return _Presenter;
            }
        }

        protected BaseMasterPage ()
        {
            _IsMasterPagePostBack = false;
        }

        protected abstract void RestoreViewState();
        protected abstract void FormToPresenter();
        protected abstract void PresenterToForm();

        #region readonly session properties
        protected string CurrentDomainExtension
        {
            get
            {
                if (_BasePageHelper.IsAssigned())
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
                if (_BasePageHelper.IsAssigned())
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
                if (_BasePageHelper.IsAssigned())
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
                if (_BasePageHelper.IsAssigned())
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
                if (_BasePageHelper.IsAssigned())
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
                if (_BasePageHelper.IsAssigned())
                {
                    return _BasePageHelper.CurrentPageName;
                }

                return string.Empty;
            }
        }
        #endregion readonly session properties

        private void BuildPresenter()
        {
            if (!_BasePageHelper.IsAssigned())
            {
                try
                {
                    var urlReferer = string.Empty;

                    if (Request.UrlReferrer.IsAssigned())
                    {
                        var urlReferrer = Request.UrlReferrer;

                        if (urlReferrer.IsAssigned())
                        {
                            urlReferer = urlReferrer.ToString();
                        }
                    }

                    _BasePageHelper = new BasePageHelper(Request.Url.ToString(), urlReferer);
                }
                catch (NullReferenceException)
                {
                    // jammer dan
                }
            }

            if (!_Presenter.IsAssigned())
            {
                _Presenter = new T();
            }

            if (!_Presenter.PageName.IsAssigned())
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
                if (!_MasterPageDictionaryIn.IsAssigned())
                {
                    if (!Session["MasterPageDictionaryIn"].IsAssigned())
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
                if (!_MasterPageDictionaryOut.IsAssigned())
                {
                    if (!Session["MasterPageDictionaryOut"].IsAssigned())
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
            var viewState = ViewState[fieldName];

            if (viewState != null)
            {
                return viewState.ToString();
            }

            return string.Empty;
        }


        protected string GetSessionState(string fieldName)
        {
            var sessionState = Session[fieldName];

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

                foreach (var kvp in MasterPageDictionaryOut)
                {
                    MasterPageDictionaryIn.Add(kvp.Key, kvp.Value);
                }

                MasterPageDictionaryOut.Clear();
            }
        }

        private List<string> _MasterPageFileList;

        private IEnumerable<string> MasterPageFileList
        {
            get
            {
                if (!_MasterPageFileList.IsAssigned())
                {
                    _MasterPageFileList = new List<string>();

                    AddMasterPageFile(Page.Master, Page.MasterPageFile);
                }

                return _MasterPageFileList;
            }
        }

        private void AddMasterPageFile(MasterPage masterPage, string masterPageFile)
        {
            if (masterPageFile.IsAssigned())
            {
                _MasterPageFileList.Add(masterPageFile);
            }

            if (masterPage.IsAssigned())
            {
                AddMasterPageFile(masterPage.Master, masterPage.MasterPageFile);
            }
        }

        private void CheckIsMasterPagePostBack()
        {
            foreach (var masterPageFile in MasterPageFileList)
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

            foreach (var masterPageFile in MasterPageFileList)
            {
                if (!MasterPageDictionaryOut.ContainsKey(masterPageFile))
                {
                    MasterPageDictionaryOut.Add(masterPageFile, masterPageFile);
                }
            }
        }

        protected bool IsMasterPagePostBack => _IsMasterPagePostBack;

        protected void Redirect(string url)
        {
            if (url.IsAssigned())
            {
                Response.Redirect(url, false);

                Response.End();
            }
        }
    }
}
