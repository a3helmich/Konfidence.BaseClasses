using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Konfidence.Base;
using System.Globalization;

namespace Konfidence.BaseWebsiteClasses
{
    // TODO : alle functionaliteit naar helperclass overzetten, IsAssigned mag hier niet nodig zijn.

    public partial class BaseUserControl: UserControl
    {
        private BasePageHelper _BasePageHelper = null;

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

        protected void Page_Load(object sender, EventArgs e)
        {
            AfterPage_Load();
        }

        protected virtual void AfterPage_Load()
        {
            // NOP
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _BasePageHelper = new BasePageHelper(Page);

            After_OnInit();
        }

        protected virtual void After_OnInit()
        {
            // NOP
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
