using System;
using System.Globalization;
using System.IO;
using System.Web.UI;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers
{
	public abstract class BaseUserControl<T>: UserControl where T : BaseWebPresenter, new()
	{
	    private BasePageHelper _BasePageHelper; // TODO : --> zie BasePage

        private T _Presenter;

        protected abstract void RestoreViewState();
        protected abstract void FormToPresenter();
        protected abstract void PresenterToForm();

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

	    public SessionHelper SessionHelper { get; set; }

	    #endregion readonly session properties

        private void BuildPresenter()
        {
            if (!_BasePageHelper.IsAssigned())
            {
                try
                {
                    string urlReferer = string.Empty;

                    if (Request.UrlReferrer.IsAssigned())
                    {
                        urlReferer = Request.UrlReferrer.ToString();
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
                _Presenter = new T {IsLoaded = false};
            }

            if (!_Presenter.PageName.IsAssigned())
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
			SessionHelper = new SessionHelper(Context, UniqueID);

			base.OnInit(e);
		}

        protected void SwitchLanguagePanel(Control languageNl, Control languageDe, Control languageUk)
        {
            if (languageDe != null && languageNl != null && languageUk != null)
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