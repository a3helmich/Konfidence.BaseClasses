using System;
using System.Globalization;
using System.IO;
using System.Web.UI;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers
{
	public abstract class BaseUserControl<T>: UserControl where T : BaseWebPresenter, new()
	{
	    private BasePageHelper _basePageHelper; // TODO : --> zie BasePage

        private T _presenter;

        protected abstract void RestoreViewState();
        protected abstract void FormToPresenter();
        protected abstract void PresenterToForm();

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

		public bool IsRefreshed
		{
			get
			{
                var refreshPage = Page as BasePage<T>;

                if (refreshPage.IsAssigned())
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

            if (viewState.IsAssigned())
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
                if (_basePageHelper.IsAssigned())
                {
                    return _basePageHelper.CurrentDomainExtension;
                }

                return string.Empty;
            }
        }

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

	    public SessionHelper SessionHelper { get; set; }

	    #endregion readonly session properties

        private void BuildPresenter()
        {
            if (!_basePageHelper.IsAssigned())
            {
                try
                {
                    string urlReferer = string.Empty;

                    if (Request.UrlReferrer.IsAssigned())
                    {
                        urlReferer = Request.UrlReferrer.ToString();
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
                _presenter = new T {IsLoaded = false};
            }

            if (!_presenter.PageName.IsAssigned())
            {
                _presenter.SetPageName(CurrentPageName);
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
                Presenter.IsLoaded = true;

                RestoreViewState();
            }

            FormToPresenter();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            PresenterToForm();

            ViewState["IsRestoreViewState"] = "IsRestoreViewState";
        }

        protected void Redirect(string url)
        {
            if (url.IsAssigned())
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
			SessionHelper = new SessionHelper(Context, UniqueID);

			base.OnInit(e);
		}

        protected void SwitchLanguagePanel(Control languageNl, Control languageDe, Control languageUk)
        {
            if (languageDe.IsAssigned() && languageNl.IsAssigned() && languageUk.IsAssigned())
            {
                languageNl.Visible = true;
                languageDe.Visible = true;
                languageUk.Visible = true;

                switch (CurrentLanguage)
                {
                    case "nl":
                        languageDe.Visible = false;
                        languageUk.Visible = false;
                        break;
                    case "de":
                        languageNl.Visible = false;
                        languageUk.Visible = false;
                        break;
                    case "uk":
                        languageDe.Visible = false;
                        languageNl.Visible = false;
                        break;
                }
            }
        }

        protected string LastUpdateDate()
        {
            // TODO: lastupdate moet afhankelijk zijn van de content!!!
            string filePath = Page.Request.PhysicalPath;
            string oldFilePath = Page.Request.FilePath.Replace("/", @"\");
            string exeFilePath = Page.Request.CurrentExecutionFilePath.Replace("/", @"\");

            filePath = filePath.Replace(oldFilePath, exeFilePath);

            if (File.Exists(filePath))
            {
                return File.GetLastWriteTime(filePath).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + "/" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }

            return "file not found";
        }
    }
}