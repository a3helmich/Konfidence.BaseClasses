using System;
using System.Collections.Generic;
using System.Web.UI;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers
{
    [UsedImplicitly]
    public abstract class BaseMasterPage<T> : MasterPage where T : BaseWebPresenter, new()
    {
        private BasePageHelper _basePageHelper;

        private Dictionary<string, string> _masterPageDictionaryIn;
        private Dictionary<string, string> _masterPageDictionaryOut;

        private bool _isMasterPagePostBack;

        private T _presenter;

        public T Presenter
        {
            get
            {
                if (!_presenter.IsAssigned())
                {
                    BuildPresenter();
                }

                return _presenter;
            }
        }

        protected BaseMasterPage ()
        {
            _isMasterPagePostBack = false;
        }

        protected abstract void RestoreViewState();
        protected abstract void FormToPresenter();
        protected abstract void PresenterToForm();

        [UsedImplicitly]
        protected string CurrentDomainExtension
        {
            get
            {
                if (_basePageHelper.IsAssigned())
                {
                    return _basePageHelper.CurrentDomainExtension;
                }

                return string.Empty;
            }
        }

        [UsedImplicitly]
        protected string CurrentLanguage
        {
            get
            {
                if (_basePageHelper.IsAssigned())
                {
                    return _basePageHelper.CurrentLanguage;
                }

                return string.Empty;
            }
        }

        [UsedImplicitly]
        protected string CurrentDnsName
        {
            get
            {
                if (_basePageHelper.IsAssigned())
                {
                    return _basePageHelper.CurrentDnsName;
                }

                return string.Empty;
            }
        }

        [UsedImplicitly]
        protected string RefererDnsName
        {
            get
            {
                if (_basePageHelper.IsAssigned())
                {
                    return _basePageHelper.RefererDnsName;
                }

                return string.Empty;
            }
        }

        protected string CurrentPagePath
        {
            get
            {
                if (_basePageHelper.IsAssigned())
                {
                    return _basePageHelper.CurrentPagePath;
                }

                return string.Empty;
            }
        }

        protected string CurrentPageName
        {
            get
            {
                if (_basePageHelper.IsAssigned())
                {
                    return _basePageHelper.CurrentPageName;
                }

                return string.Empty;
            }
        }
        
        private void BuildPresenter()
        {
            if (!_basePageHelper.IsAssigned())
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

                    _basePageHelper = new BasePageHelper(Request.Url.ToString(), urlReferer);
                }
                catch (NullReferenceException)
                {
                    // jammer dan
                }
            }

            if (!_presenter.IsAssigned())
            {
                _presenter = new T();
            }

            if (!_presenter.PageName.IsAssigned())
            {
                _presenter.SetPageName(CurrentPageName);
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

        [CanBeNull]
        protected Dictionary<string, string> MasterPageDictionaryIn
        {
            get
            {
                if (!_masterPageDictionaryIn.IsAssigned())
                {
                    if (!Session["MasterPageDictionaryIn"].IsAssigned())
                    {
                        Session["MasterPageDictionaryIn"] = new Dictionary<string, string>();
                    }

                    _masterPageDictionaryIn = Session["MasterPageDictionaryIn"] as Dictionary<string, string>;
                }

                return _masterPageDictionaryIn;
            }
        }

        [CanBeNull]
        protected Dictionary<string, string> MasterPageDictionaryOut
        {
            get
            {
                if (!_masterPageDictionaryOut.IsAssigned())
                {
                    if (!Session["MasterPageDictionaryOut"].IsAssigned())
                    {
                        Session["MasterPageDictionaryOut"] = new Dictionary<string, string>();
                    }

                    _masterPageDictionaryOut = Session["MasterPageDictionaryOut"] as Dictionary<string, string>;
                }

                return _masterPageDictionaryOut;
            }
        }

        [NotNull]
        protected string GetViewState([NotNull] string fieldName)
        {
            var viewState = ViewState[fieldName];

            if (viewState.IsAssigned())
            {
                return viewState.ToString();
            }

            return string.Empty;
        }

        [UsedImplicitly]
        [NotNull]
        protected string GetSessionState(string fieldName)
        {
            var sessionState = Session[fieldName];

            if (sessionState.IsAssigned())
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

        private List<string> _masterPageFileList;

        private IEnumerable<string> MasterPageFileList
        {
            get
            {
                if (!_masterPageFileList.IsAssigned())
                {
                    _masterPageFileList = new List<string>();

                    AddMasterPageFile(Page.Master, Page.MasterPageFile);
                }

                return _masterPageFileList;
            }
        }

        private void AddMasterPageFile(MasterPage masterPage, string masterPageFile)
        {
            if (masterPageFile.IsAssigned())
            {
                _masterPageFileList.Add(masterPageFile);
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
                    _isMasterPagePostBack = true;
                }
            }
        }

        [UsedImplicitly]
        protected void Page_Init(object sender, EventArgs e)
        {
            BuildMasterPageDictionaries();

            if (!IsPostBack && !Presenter.LogonPageName.Equals(Presenter.PageName, StringComparison.InvariantCultureIgnoreCase))
            {
                Presenter.FromUrl = CurrentPagePath;
            }
        }

        [UsedImplicitly]
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckIsMasterPagePostBack();

            if (IsRestoreViewState)
            {
                RestoreViewState();
            }

            FormToPresenter();
        }

        [UsedImplicitly]
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

        [UsedImplicitly]
        protected bool IsMasterPagePostBack => _isMasterPagePostBack;

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
