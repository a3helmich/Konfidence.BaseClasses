using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Konfidence.UserControlHelpers
{
	public class BaseTabbedUserControl: BaseUserControl
	{
		private TabEntryList _TabEntryList = new TabEntryList();

		internal class TabEntry
		{
			public string TabId = string.Empty;				
			public Button TabButton;									// default value null
			public BaseUserControl TabbedUserControl; // default value null
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
						tabEntry.TabbedUserControl.Show();
					}
					else
					{
						tabEntry.TabButton.Font.Bold = false;
						tabEntry.TabbedUserControl.Hide();
					}
				}
			}
			
			public string RegisterTab(Button tabButton, BaseUserControl tabbedUserControl)
			{
				TabEntry tabEntry	= new TabEntry();

				tabEntry.TabId = tabbedUserControl.ClientID;
				tabEntry.TabButton = tabButton;
				tabEntry.TabbedUserControl = tabbedUserControl;

				Add(tabEntry);

				return tabEntry.TabId;
			}
		}

		protected void ShowTab(string tabId)
		{
			_TabEntryList.ShowTab(tabId);

			AfterShowTab();
		}

		protected string RegisterTab(Button tabButton, BaseUserControl tabbedUserControl)
		{
			return _TabEntryList.RegisterTab(tabButton, tabbedUserControl);
		}

		protected virtual void AfterShowTab()
		{
			// NOP
		}
	}
}
