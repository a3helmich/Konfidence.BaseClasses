using System;
using System.Globalization;
using System.IO;
using System.Web.UI;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers
{
    [UsedImplicitly]
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

	    [UsedImplicitly]
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

	    public SessionHelper SessionHelper { get; set; }


        private void BuildPresenter()
        {
            if (!_basePageHelper.IsAssigned())
            {
                try
                {
                    var urlReferer = string.Empty;

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

	    [UsedImplicitly]
        protected void Page_Init(object sender, EventArgs e)
        {
            BuildPresenter();
        }

	    [UsedImplicitly]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsRestoreViewState && IsPostBack)
            {
                Presenter.IsLoaded = true;

                RestoreViewState();
            }

            FormToPresenter();
        }

	    [UsedImplicitly]
        protected void Page_PreRender(object sender, EventArgs e)
        {
            PresenterToForm();

            ViewState["IsRestoreViewState"] = "IsRestoreViewState";
        }

	    [UsedImplicitly]
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

		protected override void OnInit(EventArgs e)
		{
			SessionHelper = new SessionHelper(Context, UniqueID);

			base.OnInit(e);
		}

	    [UsedImplicitly]
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

	    [UsedImplicitly]
        [NotNull]
        protected string LastUpdateDate()
        {
            // TODO: lastupdate moet afhankelijk zijn van de content!!!
            var filePath = Page.Request.PhysicalPath;
            var oldFilePath = Page.Request.FilePath.Replace("/", @"\");
            var exeFilePath = Page.Request.CurrentExecutionFilePath.Replace("/", @"\");

            filePath = filePath.Replace(oldFilePath, exeFilePath);

            if (File.Exists(filePath))
            {
                return File.GetLastWriteTime(filePath).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + "/" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }

            return "file not found";
        }
    }
}