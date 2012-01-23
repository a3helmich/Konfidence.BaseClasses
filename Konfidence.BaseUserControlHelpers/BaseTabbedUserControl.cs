using System.Collections.Generic;
using System.Web.UI.WebControls;
using System;
using System.Web.UI;

namespace Konfidence.BaseUserControlHelpers
{
    public abstract class BaseTabbedUserControl<T> : BaseUserControl<T>  where T : BaseWebPresenter, new()
	{
		private TabEntryList _TabEntryList = new TabEntryList();

		internal class TabEntry
		{
			public string TabId = string.Empty;				
			public Button TabButton;									// default value null
            public UserControl TabbedUserControl; // default value null
		}

		internal class TabEntryList: List<TabEntry>
		{
			public void ShowTab(string tabId)
			{
				foreach (TabEntry tabEntry in this)
				{
					if (tabEntry.TabId == tabId)
					{
						tabEntry.TabButton.Font.Bold = true;
						tabEntry.TabbedUserControl.Visible = true;
					}
					else
					{
						tabEntry.TabButton.Font.Bold = false;
						tabEntry.TabbedUserControl.Visible = false;
					}
				}
			}

            public string RegisterTab(Button tabButton, UserControl tabbedUserControl)
			{
				TabEntry tabEntry	= new TabEntry();

				tabEntry.TabId = tabbedUserControl.ClientID;
				tabEntry.TabButton = tabButton;
				tabEntry.TabbedUserControl = tabbedUserControl;

				Add(tabEntry);

				return tabEntry.TabId;
			}
		}

        public BaseTabbedUserControl()
        {
        }

		protected void ShowTab(string tabId)
		{
			_TabEntryList.ShowTab(tabId);

			AfterShowTab();
		}

        protected string RegisterTab(Button tabButton, BaseUserControl<BaseWebPresenter> tabbedUserControl)
		{
			return _TabEntryList.RegisterTab(tabButton, tabbedUserControl);
		}

		protected virtual void AfterShowTab()
		{
			// NOP
		}
    }
}
