﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Konfidence.BaseWebsiteClasses
{
    public class BaseMasterPage : MasterPage
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
            _BasePageHelper = new BasePageHelper(this);

            AfterPage_Load();
        }

        protected virtual void AfterPage_Load()
        {
            // NOP
        }
    }
}