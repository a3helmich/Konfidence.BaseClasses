using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers
{
	public abstract class BasePage<T>: Page where T: BaseWebPresenter, new()
	{
        private BasePageHelper _BasePageHelper;  // TODO : aanpassen conform nieuwe werkwijze -> naar BaseWebPresenter verplaatsen

        private T _Presenter;

        protected abstract void DataInitialize();
        protected abstract void RestoreViewState();
        protected abstract void LoadDefaults();
        protected abstract void FormToPresenter();
        protected abstract void PresenterToForm();
        protected abstract void SaveDefaults();

        private bool _IsExpired; 
		private bool _IsRefreshed;

		#region properties
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

		public bool IsRefreshed
		{
			get { return _IsRefreshed; }
		}

        public bool IsExpired
        {
            get { return _IsExpired; }
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

		#endregion

	    protected BasePage()
        {
            _IsExpired = false;
            _IsRefreshed = false;
        }

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
                    string urlReferer = string.Empty;

                    if (IsAssigned(Request.UrlReferrer))
                    {
                        var urlReferrer = Request.UrlReferrer;

                        if (urlReferrer != null)
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

            if (!IsAssigned(_Presenter))
            {
                _Presenter = new T();

                DataInitialize();
            }

            if (IsEmpty(_Presenter.PageName))
            {
                _Presenter.SetPageName(CurrentPageName);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            BuildPresenter();
        }

        protected string GetParameter(string name)
        {
            string param = Request.QueryString[name];

            if (IsEmpty(param))
            {
                return string.Empty;
            }

            return param;
        }

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
            if (!IsEmpty(url))
            {
                Response.Redirect(url, false);

                Response.End();
            }
        }

        protected Control FindUserControlByType(ControlCollection controlCollection, Type findType)
        {
            return BasePageHelper.FindUserControlByType(controlCollection, findType);
        }

        protected bool IsEmpty(string assignedString)
        {
            return BaseItem.IsEmpty(assignedString);
        }

		protected bool IsAssigned(object assignedObject)
		{
			return BaseItem.IsAssigned(assignedObject);
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
			string pageId = Guid.NewGuid().ToString("N");

			ClientScript.RegisterHiddenField(SessionHelper.PAGE_ID_VALUE, pageId);
			Context.Items[SessionHelper.PAGE_ID_VALUE] = pageId;
		}

		private void CreatePageIdentifier()
		{
			string pageId;

			if (!IsPostBack)
			{
				pageId = Guid.NewGuid().ToString("N");
			}
			else
			{
				pageId = Request[SessionHelper.PAGE_ID_VALUE];
			}

			ClientScript.RegisterHiddenField(SessionHelper.PAGE_ID_VALUE, pageId);
			Context.Items[SessionHelper.PAGE_ID_VALUE] = pageId;
		}

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
			bool asyncPostBack;
			ScriptManager scriptManager = GetScriptManager();

			if (IsAssigned(scriptManager))
			{
				asyncPostBack = scriptManager.IsInAsyncPostBack;
			}
			else
			{
				asyncPostBack = IsAsync;
			}

			_IsRefreshed = PageRefreshHelper.Check(Page, Context, asyncPostBack);
		}

		private void UpdateSessionState()
		{
			string currentSessionId = Request[SessionHelper.SESSION_ID_VALUE];

             //check of de sessie expired is.
             //1. Om te kunnen verlopen, moet er een currentSessionId zijn. is die
             //er niet, dan is de sessie niet expired.
             //2. Is er een currentSessionId, dan moet deze vergeleken worden met de sessionId
             //van de sessie waar we in zitten, deze moet gelijk zijn.
             //3. Als er een currentSessionId is, en het is een nieuwe sessie. Dan is de sessie 
             //opnieuw opgebouwd en de sessie expired.

			if (String.IsNullOrEmpty(currentSessionId) && currentSessionId == Session.SessionID && Session.IsNewSession)
			{
				_IsExpired = true;
			}

             //bewaar de currentSessionId, zodat deze voor UpdateSessionState() bij de 
             //volgende check beschikbaar is.
			ClientScript.RegisterHiddenField(SessionHelper.SESSION_ID_VALUE, Session.SessionID);
		}
	}
}