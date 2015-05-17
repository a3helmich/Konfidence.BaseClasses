using System.Collections.Generic;
using System.Web;
using DbSiteMapMenuClasses;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers.DbSiteMapProvider
{
    //	[AspNetHostingPermission(SecurityAction.Demand, Level=AspNetHostingPermissionLevel.Minimal)]
    public class DbSiteMapProvider : StaticSiteMapProvider
    {
        private SiteMapNode _RootNode;

        #region properties

        // Return the root node of the current site map.
        public override SiteMapNode RootNode // !!! base niet aanroepen, is by design !!!
        {
            get
            {
                if (!_RootNode.IsAssigned())
                {
                    _RootNode = BuildSiteMap();
                }
                return _RootNode;
            }
        }

        public bool LoggedOn { get; set; }

        public bool Administrator { get; set; }

        public bool IsLocal { get; set; }

        #endregion

        public DbSiteMapProvider()
        {
            LoggedOn = false;
            Administrator = false;
            IsLocal = false;
        }

        protected override SiteMapNode GetRootNodeCore() // !!! base niet aanroepen, is by design !!!
        {
            return RootNode;
        }

        ///
        /// SiteMapProvider and StaticSiteMapProvider methods that this derived class must override.
        ///
        // Clean up any collections or other state that an instance of this may hold.
        protected override void Clear() // !!! base niet aanroepen, is by design !!!
        {
            lock (this)
            {
                _RootNode = null;

                base.Clear();
            }
        }

        // Build an in-memory representation from persistent
        // storage, and return the root node of the site map.
        public override SiteMapNode BuildSiteMap() // !!! base niet aanroepen, is by design !!!
        {
            // Since the SiteMap class is static, make sure that it is
            // not modified while the site map is built.
            lock (this)
            {
                SiteMapNode rootNode = _RootNode; // default value is null;

                if (!_RootNode.IsAssigned()) // als eenmaal gebouwd, niet verder naar kijken
                {
                    // Start with a clean slate
                    Clear();

                    Bl.MenuDataItemList menuItemList = Bl.MenuDataItemList.GetListByMenuCode(1);

                    Bl.MenuDataItem rootMenu = GetMenuRootNode(menuItemList);

                    rootNode = BuildMenuNode(rootMenu);

                    if (menuItemList.Count > 1)
                    {
                        // Copy the menuItems from the tree to the sitemap
                        BuildChildNodes(menuItemList, rootMenu, rootNode);
                    }
                }

                return rootNode;
            }
        }

        private void BuildChildNodes(IEnumerable<Bl.MenuDataItem> childNodes, Bl.MenuDataItem rootMenu, SiteMapNode parentNode)
        {
            foreach (Bl.MenuDataItem childItem in childNodes)
            {
                if (childItem.MenuId != childItem.ParentMenuId && rootMenu.MenuId == childItem.ParentMenuId)
                {
                    bool showItem = false;

                    if (IsLocal)
                    {
                        if (childItem.IsLocalVisible)
                        {
                            showItem = true;
                        }
                    }
                    else
                    {
                        // show when not logged in
                        if (childItem.IsNotLogonVisible && !LoggedOn)
                        {
                            showItem = true;
                        }

                        // show when logged in
                        if (childItem.IsLogonVisible && LoggedOn)
                        {
                            showItem = true;
                        }

                        // only visible for administrators
                        if (childItem.IsAdministrator && !Administrator)
                        {
                            showItem = false;
                        }
                    }

                    if (showItem)
                    {
                        SiteMapNode childNode = BuildMenuNode(childItem);

                        // add childNode with SiteMapNode.AddNode(..) to the 
                        // ChildNodes collection of the parent
                        AddNode(childNode, parentNode);

                        // CHILD of CHILD nodes ff niet
                        //if (IsAssigned(childItem.ChildNodes) && childItem.ChildNodes.Count > 0)
                        //{
                        //    // Copy the menuItems from the tree to the sitemap
                        //    BuildChildNodes(childItem.ChildNodes, childNode);
                        //}
                    }
                }
            }
        }

        private Bl.MenuDataItem GetMenuRootNode(IEnumerable<Bl.MenuDataItem> menuList)
        {
            foreach (Bl.MenuDataItem menuItem in menuList)
            {
                if (menuItem.MenuId == menuItem.ParentMenuId)
                {
                    return menuItem;
                }
            }

            return null;
        }

        private SiteMapNode BuildMenuNode(Bl.MenuDataItem menuItem)
        {
            string menuUrl = string.Empty;
            string menuMenuText = string.Empty;

            if (menuItem.IsVisible)
            {
                menuUrl = menuItem.Url;
                menuMenuText = menuItem.MenuText.MenuText;
            }

            var menuNode = new SiteMapNode(this, menuItem.MenuId.ToString(), menuUrl, menuMenuText);

            return menuNode;
        }
    }
}