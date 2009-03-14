using System;
using System.Web.UI;
using Konfidence.Base;

namespace Konfidence.UserControlHelpers
{
	public class BaseUserControl: UserControl
	{
		private SessionHelper _SessionHelper;
		private bool _IsForeignKeyChanged = false;
		private bool _IsPrimaryKeyChanged = false;
		private SessionAccount _SessionAccount = null;

		public bool IsRefreshed
		{
			get
			{
				BasePage refreshPage = Page as BasePage;

				if (IsAssigned(refreshPage))
				{
					return refreshPage.IsRefreshed;
				}

				return false;
			}
		}

		public SessionAccount SessionAccount
		{
			get
			{
				if (!IsAssigned(_SessionAccount))
				{
					_SessionAccount = Session[KitSessionAccount.AccountObject] as SessionAccount;
				}

				return _SessionAccount;
			}
		}

		protected virtual void LogOff()
		{
			BasePage topPage = NamingContainer as BasePage;

			if (IsAssigned(topPage))
			{
				topPage.LogOff();
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

		protected static bool IsAssigned(object assignedObject)
		{
			return BaseItem.IsAssigned(assignedObject);
		}

		protected override void OnInit(EventArgs e)
		{
			_SessionHelper = new SessionHelper(Context, UniqueID);

			base.OnInit(e);
		}

		protected void RebuildParent()
		{
			BaseUserControl topControl = NamingContainer as BaseUserControl;

			if (IsAssigned(topControl))
			{
				topControl.TopRebuild(this);
			}
		}

		public int PrimaryKey
		{
			get
			{
				return _SessionHelper.SessionParameterObject.PrimaryKey;
			}
			set
			{
				if (_SessionHelper.SessionParameterObject.PrimaryKey != value)
				{
					if (_SessionHelper.SessionParameterObject.PrimaryKey != 0)
					{
						_IsPrimaryKeyChanged = true;
					}
				}
				_SessionHelper.SessionParameterObject.PrimaryKey = value;
			}
		}

		public bool IsPrimaryKeyAssigned
		{
			get
			{
				return _SessionHelper.SessionParameterObject.IsPrimaryKeyAssigned;
			}
		}

		public bool IsPrimaryKeyChanged
		{
			get
			{
				return _IsPrimaryKeyChanged;
			}
		}

		public int ForeignKey
		{
			get
			{
				return _SessionHelper.SessionParameterObject.ForeignKey;
			}
			set
			{
				if (_SessionHelper.SessionParameterObject.ForeignKey != value)
				{
					if (_SessionHelper.SessionParameterObject.ForeignKey != 0)
					{
						_IsForeignKeyChanged = true;
					}
				}
				_SessionHelper.SessionParameterObject.ForeignKey = value;
			}
		}

		public bool IsForeignKeyAssigned
		{
			get
			{
				return _SessionHelper.SessionParameterObject.IsForeignKeyAssigned;
			}
		}

		public bool IsForeignKeyChanged
		{
			get
			{
				return _IsForeignKeyChanged;
			}
		}

		public virtual void Rebuild()
		{
			// NOP : 
		}

		public virtual void Hide()
		{
			Visible = false;
		}

		/// <summary>
		/// De base.Show method voert een Rebuild uit.
		/// </summary>
		public virtual void Show()
		{
			Visible = true;

			Rebuild();
		}

		protected virtual void TopRebuild(BaseUserControl childControl)
		{
			RebuildParent();
		}
	}
}