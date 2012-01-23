using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;
using System.Web.UI;
using System.Configuration;

namespace Konfidence.BaseUserControlHelpers
{
    public class BaseWebPresenter : BaseItem
    {
        private string _MenuPage = string.Empty;

        public string MenuPage
        {
            get { return ConfigurationManager.AppSettings["MenuPage"] as string; }
        }
    }
}
