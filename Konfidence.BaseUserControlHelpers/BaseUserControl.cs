using System;
using System.Web.UI;
using Konfidence.Base;
using System.IO;
using System.Globalization;

namespace Konfidence.BaseUserControlHelpers
{
	public abstract class BaseUserControl<T>: UserControl where T : BaseWebPresenter, new()
	{
		private SessionHelper _SessionHelper;

        private BasePageHelper _BasePageHelper = null; // TODO : --> zie BasePage

        private T _Presenter = null;

        protected abstract void RestoreViewState();
        protected abstract void FormToPresenter();
        protected abstract void PresenterToForm();

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

        protected void Page_Init(object sender, EventArgs e)
        {
            BuildPresenter();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsRestoreViewState && IsPostBack)
            {
                RestoreViewState();
            }

            FormToPresenter();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            PresenterToForm();

            ViewState["IsRestoreViewState"] = "IsRestoreViewState";
        }

        protected bool IsEmpty(string assignedString)
        {
            return BaseItem.IsEmpty(assignedString);
        }

		protected static bool IsAssigned(object assignedObject)
		{
			return BaseItem.IsAssigned(assignedObject);
		}

        protected void Redirect(string url)
        {
            if (!BaseItem.IsEmpty(url))
            {
                Response.Redirect(url, false);

                Response.End();
            }
        }

        protected Control FindUserControlByType(ControlCollection controlCollection, Type findType)
        {
            return BasePageHelper.FindUserControlByType(controlCollection, findType);
        }

		protected override void OnInit(EventArgs e)
		{
			_SessionHelper = new SessionHelper(Context, UniqueID);

			base.OnInit(e);
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