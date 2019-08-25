using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers
{
	public abstract class BasePage<T>: Page where T: BaseWebPresenter, new()
	{
        private BasePageHelper _basePageHelper;  // TODO : aanpassen conform nieuwe werkwijze -> naar BaseWebPresenter verplaatsen

        private T _presenter;

        protected abstract void DataInitialize();
        protected abstract void RestoreViewState();
        protected abstract void LoadDefaults();
        protected abstract void FormToPresenter();
        protected abstract void PresenterToForm();
        protected abstract void SaveDefaults();

	    private bool _isRefreshed;

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

		public bool IsRefreshed => _isRefreshed;

	    public bool IsExpired { get; private set; }

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

	    protected BasePage()
        {
            IsExpired = false;
            _isRefreshed = false;
        }

	    [UsedImplicitly]
        [NotNull]
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
        [NotNull]
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
        [NotNull]
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
        [NotNull]
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

	    [UsedImplicitly]
        [NotNull]
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

        [NotNull]
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

                DataInitialize();
            }

            if (!_presenter.PageName.IsAssigned())
            {
                _presenter.SetPageName(CurrentPageName);
            }
        }

	    [UsedImplicitly]
        protected void Page_Init(object sender, EventArgs e)
        {
            BuildPresenter();
        }

	    [UsedImplicitly]
        [NotNull]
        protected string GetParameter(string name)
        {
            var param = Request.QueryString[name];

            if (!param.IsAssigned())
            {
                return string.Empty;
            }

            return param;
        }

	    [UsedImplicitly]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Presenter.IsLoggedIn && Presenter.IsLogonRequired)
            {
                Redirect(Presenter.LogonUrl);
            }

            if (IsRestoreViewState && IsPostBack)
            {
                RestoreViewState();
            }

            if (!IsPostBack)
            {
                LoadDefaults();
            }

            FormToPresenter();
        }

	    [UsedImplicitly]
        protected void Page_PreRender(object sender, EventArgs e)
        {
            MaintainScrollPositionOnPostBack = true;

            PresenterToForm();

            ViewState["IsRestoreViewState"] = "IsRestoreViewState";

            if (IsPostBack)
            {
                SaveDefaults();
            }
        }

        protected void Redirect(string url)
        {
            if (url.IsAssigned())
            {
                Response.Redirect(url, false);

                Response.End();
            }
        }

	    [UsedImplicitly]
        [CanBeNull]
        protected Control FindUserControlByType([NotNull] ControlCollection controlCollection, Type findType)
        {
            return BasePageHelper.FindUserControlByType(controlCollection, findType);
        }

		protected override void OnPreLoad(EventArgs e)
		{
			base.OnPreLoad(e);

            // TODO : pageidentifier updatesessionstate and updaterefreshstate -> move to basemasterpage
			CreatePageIdentifier();

			UpdateSessionState();
			UpdateRefreshState();

			// IsRefreshed: kan ook betekenen dat de huidige page geopend is als 
			// een 'New Page', dit ziet er hetzelfde uit als een refresh
			// in dat geval moet er een nieuwe PageId worden gebruikt, zodat 
			// vanaf dat moment duidelijk is dat er sprake is van een andere page.
			if (IsRefreshed)
			{
				UpdatePageIdentifier();
			}

			AfterOnPreLoadPage();
		}

		protected virtual void AfterOnPreLoadPage()
		{
			// NOP
		}

		private void UpdatePageIdentifier()
		{
			var pageId = Guid.NewGuid().ToString("N");

			ClientScript.RegisterHiddenField(SessionHelper.PAGE_ID_VALUE, pageId);
			Context.Items[SessionHelper.PAGE_ID_VALUE] = pageId;
		}

		private void CreatePageIdentifier()
		{
		    var pageId = IsPostBack ? Request[SessionHelper.PAGE_ID_VALUE] : Guid.NewGuid().ToString("N");

            ClientScript.RegisterHiddenField(SessionHelper.PAGE_ID_VALUE, pageId);
			Context.Items[SessionHelper.PAGE_ID_VALUE] = pageId;
		}

		[CanBeNull]
        private ScriptManager GetScriptManager()
		{
			ScriptManager scriptManager = null;
			
			foreach (Control parentControl in Controls)
			{
				if (parentControl is HtmlForm) // only one in a body
				{
					foreach (Control control in parentControl.Controls)
					{
						if (control is ScriptManager) // only one in a form
						{
							if ((control as ScriptManager).IsInAsyncPostBack)
							{
								scriptManager = control as ScriptManager;
							}
							break;
						}
					}
					break;
				}
			}
			return scriptManager;
		}

		private void UpdateRefreshState()
		{
		    var scriptManager = GetScriptManager();

			var asyncPostBack = scriptManager.IsAssigned() ? scriptManager.IsInAsyncPostBack : IsAsync;

			_isRefreshed = PageRefreshHelper.Check(Page, Context, asyncPostBack);
		}

		private void UpdateSessionState()
		{
			var currentSessionId = Request[SessionHelper.SESSION_ID_VALUE];

             //check of de sessie expired is.
             //1. Om te kunnen verlopen, moet er een currentSessionId zijn. is die
             //er niet, dan is de sessie niet expired.
             //2. Is er een currentSessionId, dan moet deze vergeleken worden met de sessionId
             //van de sessie waar we in zitten, deze moet gelijk zijn.
             //3. Als er een currentSessionId is, en het is een nieuwe sessie. Dan is de sessie 
             //opnieuw opgebouwd en de sessie expired.

			if (!currentSessionId.IsAssigned() && currentSessionId == Session.SessionID && Session.IsNewSession)
			{
				IsExpired = true;
			}

             //bewaar de currentSessionId, zodat deze voor UpdateSessionState() bij de 
             //volgende check beschikbaar is.
			ClientScript.RegisterHiddenField(SessionHelper.SESSION_ID_VALUE, Session.SessionID);
		}
	}
}