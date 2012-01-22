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

            After_OnInit();
        }

        protected virtual void After_OnInit()
        {
            // NOP
        }
    }
}
