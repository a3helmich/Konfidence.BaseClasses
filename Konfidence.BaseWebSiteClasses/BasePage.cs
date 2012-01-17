using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using Konfidence.Base;

namespace Konfidence.BaseWebsiteClasses
{
    // TODO : alle functionaliteit naar helperclass overzetten, IsAssigned mag hier niet nodig zijn.
    public partial class BasePage : Page
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

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);

            _BasePageHelper = new BasePageHelper(Page.Request.Url.ToString());

            After_Preload();
        }

        protected virtual void AfterPage_Load()
        {
            // NOP
        }

        protected virtual void After_Preload()
        {
            // NOP
        }
    }
}
