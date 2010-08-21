using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Konfidence.Base;

namespace Konfidence.UserControlHelpers
{
	public class BasePage: Page
	{
		private bool _IsExpired;  // false by default
		private bool _IsRefreshed;  // false by default
		private string _PageTitle = string.Empty;

		#region properties
		public bool IsRefreshed
		{
			get
			{
				return _IsRefreshed;
			}
		}

		public bool IsExpired
		{
			get
			{
				return _IsExpired;
			}
		}

		public string PageTitle
		{
			get
			{
				return _PageTitle;
			}
			set
			{
				_PageTitle = value;
			}
		}

		public SessionAccount SessionAccount
		{
			get
			{
				return Session[KitSessionAccount.AccountObject] as SessionAccount;
			}
		}

		public string MenuPage
		{
			get
			{
				if (IsAssigned(Session[KitSessionContext.UrlMenuPage]))
				{
					return Session[KitSessionContext.UrlMenuPage] as string;
				}

				return string.Empty;
			}
			set
			{
				Session[KitSessionContext.UrlMenuPage] = value;
			}
		}

		#endregion

		protected static bool IsAssigned(object assignedObject)
		{
			return BaseItem.IsAssigned(assignedObject);
		}

		protected override void OnPreLoad(EventArgs e)
		{
			base.OnPreLoad(e);

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

			Title = PageTitle + " - " + ConfigurationManager.AppSettings["applicationTitle"];
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

			if (IsAssigned(currentSessionId) && currentSessionId == Session.SessionID && Session.IsNewSession)
			{
				_IsExpired = true;
			}

			/// bewaar de currentSessionId, zodat deze voor UpdateSessionState() bij de 
			/// volgende check beschikbaar is.
			ClientScript.RegisterHiddenField(SessionHelper.SessionIdValue, Session.SessionID);
		}

		public virtual void LogOff()
		{
			if (IsAssigned(Session[KitSessionAccount.AccountObject]))
			{
				Session.Remove(KitSessionAccount.AccountObject);
			}
		}
	}
}