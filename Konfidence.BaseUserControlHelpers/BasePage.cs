using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers
{
	public abstract class BasePage<T>: Page where T: BaseWebPresenter, new()
	{
        private BasePageHelper _BasePageHelper = null;  // TODO : aanpassen conform nieuwe werkwijze -> naar BaseWebPresenter verplaatsen

        private T _Presenter = null;

        protected abstract void FormToPresenter();
        protected abstract void RestoreViewState();
        protected abstract void PresenterToForm();

        private bool _IsExpired = false; 
		private bool _IsRefreshed = false;

		#region properties
        public T Presenter
        {
            get { return _Presenter; }
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
                    if (GetViewState("IsViewStateRestore").Equals("IsViewStateRestore"))
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

                _Presenter.SetPageName(CurrentPageName);
            }
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

            FormToPresenter();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MaintainScrollPositionOnPostBack = true;

            PresenterToForm();

            ViewState["IsViewStateRestore"] = "IsViewStateRestore";
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
			string PageId = Guid.NewGuid().ToString("N");

			ClientScript.RegisterHiddenField(SessionHelper.PageIdValue, PageId);
			Context.Items[SessionHelper.PageIdValue] = PageId;
		}

		private void CreatePageIdentifier()
		{
			string PageId;

			if (!IsPostBack)
			{
				PageId = Guid.NewGuid().ToString("N");
			}
			else
			{
				PageId = Request[SessionHelper.PageIdValue];
			}

			ClientScript.RegisterHiddenField(SessionHelper.PageIdValue, PageId);
			Context.Items[SessionHelper.PageIdValue] = PageId;
		}

		private ScriptManager getScriptManager()
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
			ScriptManager scriptManager = getScriptManager();

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
			string currentSessionId = Request[SessionHelper.SessionIdValue];

			/// check of de sessie expired is.
			/// 1. Om te kunnen verlopen, moet er een currentSessionId zijn. is die
			/// er niet, dan is de sessie niet expired.
			/// 2. Is er een currentSessionId, dan moet deze vergeleken worden met de sessionId
			/// van de sessie waar we in zitten, deze moet gelijk zijn.
			/// 3. Als er een currentSessionId is, en het is een nieuwe sessie. Dan is de sessie 
			/// opnieuw opgebouwd en de sessie expired.

			if (String.IsNullOrEmpty(currentSessionId) && currentSessionId == Session.SessionID && Session.IsNewSession)
			{
				_IsExpired = true;
			}

			/// bewaar de currentSessionId, zodat deze voor UpdateSessionState() bij de 
			/// volgende check beschikbaar is.
			ClientScript.RegisterHiddenField(SessionHelper.SessionIdValue, Session.SessionID);
		}
	}
}