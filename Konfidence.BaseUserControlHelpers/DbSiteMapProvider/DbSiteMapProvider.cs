using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers.DbSiteMapProvider
{
    //public class DbSiteMapProviderLocal : DbSiteMapProvider
    //{
    //    public DbSiteMapProviderLocal()
    //    {
    //        LoggedOn = false;
    //        Administrator = false;
    //        IsLocal = true;
    //    }
    //}

    //public class DbSiteMapProviderExternal : DbSiteMapProvider
    //{
    //    public DbSiteMapProviderExternal()
    //    {
    //        LoggedOn = false;
    //        Administrator = false;
    //        IsLocal = false;
    //    }
    //}

    //public class DbSiteMapProviderInternal : DbSiteMapProvider
    //{
    //    public DbSiteMapProviderInternal()
    //    {
    //        LoggedOn = true;
    //        Administrator = false;
    //        IsLocal = false;
    //    }
    //}

    //public class DbSiteMapProviderAdministrator : DbSiteMapProvider
    //{
    //    public DbSiteMapProviderAdministrator()
    //    {
    //        LoggedOn = true;
    //        Administrator = true;
    //        IsLocal = false;
    //    }
    //}

    //	[AspNetHostingPermission(SecurityAction.Demand, Level=AspNetHostingPermissionLevel.Minimal)]
    public class DbSiteMapProvider : StaticSiteMapProvider
    {
        private bool _LoggedOn = false;
        private bool _Administrator = false;
        private bool _IsLocal = false;

        private SiteMapNode _RootNode = null;

        // Implement a default constructor.
        //public DbSiteMapProvider()
        //{
        //  // NOP
        //}
        #region properties

        // Return the root node of the current site map.
        public override SiteMapNode RootNode // !!! base niet aanroepen, is by design !!!
        {
            get
            {
                if (!IsAssigned(_RootNode))
                {
                    _RootNode = BuildSiteMap();
                }
                return _RootNode;
            }
        }

        public bool LoggedOn
        {
            get
            {
                return _LoggedOn;
            }
            set
            {
                _LoggedOn = value;
            }
        }

        public bool Administrator
        {
            get
            {
                return _Administrator;
            }
            set
            {
                _Administrator = value;
            }
        }

        public bool IsLocal
        {
            get
            {
                return _IsLocal;
            }
            set
            {
                _IsLocal = value;
            }
        }

        #endregion

        protected override SiteMapNode GetRootNodeCore() // !!! base niet aanroepen, is by design !!!
        {
            return RootNode;
        }

        // Initialize is used to initialize the properties and any state that the
        // AccessProvider holds, but is not used to build the site map.
        // The site map is built when the BuildSiteMap method is called.
        public override void Initialize(string name, NameValueCollection attributes) // !!! base niet aanroepen, is by design !!!
        {
            base.Initialize(name, attributes);
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

                if (!IsAssigned(_RootNode)) // als eenmaal gebouwd, niet verder naar kijken
                {
                    MenuItemTree menuItemTree;

                    // Start with a clean slate
                    Clear();

                    menuItemTree = new MenuItemTree(1);

                    rootNode = BuildMenuNode(menuItemTree);

                    if (menuItemTree.ChildNodes.Count > 0)
                    {
                        // Copy the menuItems from the tree to the sitemap
                        BuildChildNodes(menuItemTree.ChildNodes, rootNode);
                    }
                }

                return rootNode;
            }
        }

        private void BuildChildNodes(List<MenuItem> childNodes, SiteMapNode parentNode)
        {
            foreach (MenuItem childItem in childNodes)
            {
                bool showItem = false;

                if (IsLocal)
                {
                    if (childItem.LocalVisible)
                    {
                        showItem = true;
                    }
                }
                else
                {
                    // show when not logged in
                    if (childItem.NotLogonVisible && !LoggedOn)
                    {
                        showItem = true;
                    }

                    // show when logged in
                    if (childItem.LogonVisible && LoggedOn)
                    {
                        showItem = true;
                    }

                    // only visible for administrators
                    if (childItem.Administrators && !Administrator)
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

                    if (IsAssigned(childItem.ChildNodes) && childItem.ChildNodes.Count > 0)
                    {
                        // Copy the menuItems from the tree to the sitemap
                        BuildChildNodes(childItem.ChildNodes, childNode);
                    }
                }
            }
        }

        private SiteMapNode BuildMenuNode(MenuItem menuItem)
        {
            string menuUrl = string.Empty;
            string menuMenuText = string.Empty;

            if (menuItem.Visible)
            {
                menuUrl = menuItem.Url;
                menuMenuText = menuItem.MenuText;
            }

            SiteMapNode menuNode = new SiteMapNode(this,
                                                             menuItem.NodeId.ToString(),
                                                             menuUrl,
                                                             menuMenuText);

            return menuNode;
        }

        private static bool IsAssigned(object assignedObject)
        {
            return BaseItem.IsAssigned(assignedObject);
        }
    }
}