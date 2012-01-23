using System;
using System.Web.UI;
using Konfidence.Base;
using Konfidence.BaseWebsiteClasses;
using System.IO;
using System.Globalization;

namespace Konfidence.BaseUserControlHelpers
{
	public abstract class BaseUserControl<T>: UserControl where T : BaseWebPresenter, new()
	{
		private SessionHelper _SessionHelper;
		private bool _IsForeignKeyChanged = false;
		private bool _IsPrimaryKeyChanged = false;
		private SessionAccount _SessionAccount = null;

        private BasePageHelper _BasePageHelper = null; // TODO : --> zie BasePage

        private T _Presenter = null;

        protected T Presenter
        {
            get
            {
                return _Presenter;
            }
        }

		public bool IsRefreshed
		{
			get
			{
                BasePage<T> refreshPage = Page as BasePage<T>;

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

        protected abstract void FormToPresenter();
        protected abstract void PresenterToForm();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsAssigned(_Presenter))
            {
                _Presenter = new T();
            }

            FormToPresenter();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            PresenterToForm();
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

            _BasePageHelper = new BasePageHelper(Page.Request.Url.ToString());

			base.OnInit(e);
		}

        protected virtual void RebuildParent()
		{
            BaseUserControl<BaseWebPresenter> topControl = NamingContainer as BaseUserControl<BaseWebPresenter>;

			if (IsAssigned(topControl))
			{
                topControl.RebuildParent();
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

        private  void TopRebuild(BaseUserControl<BaseWebPresenter> childControl)
		{
			RebuildParent();
		}

        protected void SwitchLanguagePanel(Control languageNL, Control languageDE, Control languageUK)
        {
            if (languageDE != null && languageNL != null && languageUK != null)
            {
                languageNL.Visible = true;
                languageDE.Visible = true;
                languageUK.Visible = true;

                switch (CurrentLanguage)
                {
                    case "nl":
                        languageDE.Visible = false;
                        languageUK.Visible = false;
                        break;
                    case "de":
                        languageNL.Visible = false;
                        languageUK.Visible = false;
                        break;
                    case "uk":
                        languageDE.Visible = false;
                        languageNL.Visible = false;
                        break;
                }
            }
        }

        protected string LastUpdateDate()
        {
            // TODO: lastupdate moet afhankelijk zijn van de content!!!
            string FilePath = Page.Request.PhysicalPath;
            string OldFilePath = Page.Request.FilePath.Replace("/", @"\");
            string ExeFilePath = Page.Request.CurrentExecutionFilePath.Replace("/", @"\");

            FilePath = FilePath.Replace(OldFilePath, ExeFilePath);

            if (File.Exists(FilePath))
            {
                return File.GetLastWriteTime(FilePath).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + "/" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }

            return "file not found";
        }
    }
}