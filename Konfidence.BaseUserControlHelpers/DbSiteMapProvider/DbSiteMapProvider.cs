using System.Collections.Generic;
using System.Web;
using DbMenuClasses;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers.DbSiteMapProvider
{
    public class DbSiteMapProvider : StaticSiteMapProvider
    {
        private SiteMapNode _rootNode;

        // Return the root node of the current site map.
        public override SiteMapNode RootNode // !!! base niet aanroepen, is by design !!!
        {
            get
            {
                if (!_rootNode.IsAssigned())
                {
                    _rootNode = BuildSiteMap();
                }
                return _rootNode;
            }
        }

        public bool LoggedOn { get; set; }

        public bool Administrator { get; set; }

        public bool IsLocal { get; set; }

        public DbSiteMapProvider()
        {
            LoggedOn = false;
            Administrator = false;
            IsLocal = false;
        }

        [CanBeNull]
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
                _rootNode = null;

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
                var rootNode = _rootNode; // default value is null;

                if (!_rootNode.IsAssigned()) // als eenmaal gebouwd, niet verder naar kijken
                {
                    // Start with a clean slate
                    Clear();

                    var menuItemList = Bl.MenuDataItem.GetListByMenuId(1);

                    var rootMenu = GetMenuRootNode(menuItemList);

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

        private void BuildChildNodes([NotNull] IEnumerable<Bl.MenuDataItem> childNodes, Bl.MenuDataItem rootMenu, SiteMapNode parentNode)
        {
            foreach (var childItem in childNodes)
            {
                if (childItem.NodeId != childItem.ParentNodeId && rootMenu.NodeId == childItem.ParentNodeId)
                {
                    var showItem = false;

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
                        var childNode = BuildMenuNode(childItem);

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

        [CanBeNull]
        private static Bl.MenuDataItem GetMenuRootNode([NotNull] IEnumerable<Bl.MenuDataItem> menuList)
        {
            foreach (var menuItem in menuList)
            {
                if (menuItem.NodeId == menuItem.ParentNodeId)
                {
                    return menuItem;
                }
            }

            return null;
        }

        [NotNull]
        private SiteMapNode BuildMenuNode([NotNull] Bl.MenuDataItem menuItem)
        {
            var menuUrl = string.Empty;
            var menuMenuText = string.Empty;

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