using System;
using System.Diagnostics;
using System.Security.Permissions;
using Konfidence.BaseHelper;

namespace Konfidence.BaseWindowForms
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class BaseMainform : System.Windows.Forms.Form
    {
        private Type _ConfigClass;
        private BaseConfigClass _Config;
        private readonly string _ErrorHeader;

        private readonly BaseContainerFrame _BaseMainframe;
        private Type _AboutFormClass;

        private System.Windows.Forms.MainMenu _MainMenu;
        private System.Windows.Forms.MenuItem _BestandMenuItem;
        private System.Windows.Forms.MenuItem _AfsluitenMenuItem;
        private System.Windows.Forms.MenuItem _BeeldMenuItem;
        private System.Windows.Forms.MenuItem _LijstMenuItem;
        private System.Windows.Forms.MenuItem _DetailMenuItem;
        private System.Windows.Forms.MenuItem _ZoekenMenuItem;
        private System.Windows.Forms.MenuItem _RapportMenuItem;
        private System.Windows.Forms.MenuItem _InstellingenMenuItem;
        private System.Windows.Forms.MenuItem _DbConfigMenuItem;
        private System.Windows.Forms.MenuItem _InfoMenuItem;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly System.ComponentModel.Container _Components = null;

        private System.Windows.Forms.StatusBar _BaseStatusBar;
        private System.Windows.Forms.StatusBarPanel _StatusBarPanel;
        private System.Windows.Forms.MenuItem _AboutMenuItem;
        private System.Windows.Forms.MenuItem _StatusMenuItem;
        private System.Windows.Forms.Panel _MainPanel;

        public Type ConfigClass
        {
            get { return _ConfigClass; }
            set { _ConfigClass = value; }
        }

        public Type AboutFormClass
        {
            get { return _AboutFormClass; }
            set
            {
                if (BaseAboutForm.IsDerivedClass(value))
                    _AboutFormClass = value;
            }
        }

        public BaseContainerFrame BaseMainframe
        {
            get { return _BaseMainframe; }
        }


        public BaseMainform()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            BaseInitialize();

            var resources = new System.Resources.ResourceManager(typeof(BaseMainform));

            _ErrorHeader = resources.GetString("ErrorHeader.Text");

            _BaseMainframe = BuildMainContainer(null);
        }

        virtual protected void BaseInitialize()
        {
            // TODO: Supply in derived mainform the application configclass
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_Components != null)
                {
                    _Components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BaseMainform));
            this._MainMenu = new System.Windows.Forms.MainMenu();
            this._BestandMenuItem = new System.Windows.Forms.MenuItem();
            this._AfsluitenMenuItem = new System.Windows.Forms.MenuItem();
            this._BeeldMenuItem = new System.Windows.Forms.MenuItem();
            this._LijstMenuItem = new System.Windows.Forms.MenuItem();
            this._DetailMenuItem = new System.Windows.Forms.MenuItem();
            this._ZoekenMenuItem = new System.Windows.Forms.MenuItem();
            this._RapportMenuItem = new System.Windows.Forms.MenuItem();
            this._InstellingenMenuItem = new System.Windows.Forms.MenuItem();
            this._DbConfigMenuItem = new System.Windows.Forms.MenuItem();
            this._InfoMenuItem = new System.Windows.Forms.MenuItem();
            this._BaseStatusBar = new System.Windows.Forms.StatusBar();
            this._StatusBarPanel = new System.Windows.Forms.StatusBarPanel();
            this._MainPanel = new System.Windows.Forms.Panel();
            this._AboutMenuItem = new System.Windows.Forms.MenuItem();
            this._StatusMenuItem = new System.Windows.Forms.MenuItem();
            ((System.ComponentModel.ISupportInitialize)(this._StatusBarPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this._MainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                             this._BestandMenuItem,
                                                                             this._BeeldMenuItem,
                                                                             this._RapportMenuItem,
                                                                             this._InstellingenMenuItem,
                                                                             this._InfoMenuItem});
            this._MainMenu.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mainMenu.RightToLeft")));
            // 
            // BestandMenuItem
            // 
            this._BestandMenuItem.Enabled = ((bool)(resources.GetObject("BestandMenuItem.Enabled")));
            this._BestandMenuItem.Index = 0;
            this._BestandMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                    this._AfsluitenMenuItem});
            this._BestandMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("BestandMenuItem.Shortcut")));
            this._BestandMenuItem.ShowShortcut = ((bool)(resources.GetObject("BestandMenuItem.ShowShortcut")));
            this._BestandMenuItem.Text = resources.GetString("BestandMenuItem.Text");
            this._BestandMenuItem.Visible = ((bool)(resources.GetObject("BestandMenuItem.Visible")));
            // 
            // AfsluitenMenuItem
            // 
            this._AfsluitenMenuItem.Enabled = ((bool)(resources.GetObject("AfsluitenMenuItem.Enabled")));
            this._AfsluitenMenuItem.Index = 0;
            this._AfsluitenMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("AfsluitenMenuItem.Shortcut")));
            this._AfsluitenMenuItem.ShowShortcut = ((bool)(resources.GetObject("AfsluitenMenuItem.ShowShortcut")));
            this._AfsluitenMenuItem.Text = resources.GetString("AfsluitenMenuItem.Text");
            this._AfsluitenMenuItem.Visible = ((bool)(resources.GetObject("AfsluitenMenuItem.Visible")));
            this._AfsluitenMenuItem.Click += new System.EventHandler(this.AfsluitenMenuItem_Click);
            // 
            // BeeldMenuItem
            // 
            this._BeeldMenuItem.Enabled = ((bool)(resources.GetObject("BeeldMenuItem.Enabled")));
            this._BeeldMenuItem.Index = 1;
            this._BeeldMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                  this._LijstMenuItem,
                                                                                  this._DetailMenuItem,
                                                                                  this._ZoekenMenuItem});
            this._BeeldMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("BeeldMenuItem.Shortcut")));
            this._BeeldMenuItem.ShowShortcut = ((bool)(resources.GetObject("BeeldMenuItem.ShowShortcut")));
            this._BeeldMenuItem.Text = resources.GetString("BeeldMenuItem.Text");
            this._BeeldMenuItem.Visible = ((bool)(resources.GetObject("BeeldMenuItem.Visible")));
            // 
            // LijstMenuItem
            // 
            this._LijstMenuItem.Enabled = ((bool)(resources.GetObject("LijstMenuItem.Enabled")));
            this._LijstMenuItem.Index = 0;
            this._LijstMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("LijstMenuItem.Shortcut")));
            this._LijstMenuItem.ShowShortcut = ((bool)(resources.GetObject("LijstMenuItem.ShowShortcut")));
            this._LijstMenuItem.Text = resources.GetString("LijstMenuItem.Text");
            this._LijstMenuItem.Visible = ((bool)(resources.GetObject("LijstMenuItem.Visible")));
            // 
            // DetailMenuItem
            // 
            this._DetailMenuItem.Enabled = ((bool)(resources.GetObject("DetailMenuItem.Enabled")));
            this._DetailMenuItem.Index = 1;
            this._DetailMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("DetailMenuItem.Shortcut")));
            this._DetailMenuItem.ShowShortcut = ((bool)(resources.GetObject("DetailMenuItem.ShowShortcut")));
            this._DetailMenuItem.Text = resources.GetString("DetailMenuItem.Text");
            this._DetailMenuItem.Visible = ((bool)(resources.GetObject("DetailMenuItem.Visible")));
            // 
            // ZoekenMenuItem
            // 
            this._ZoekenMenuItem.Enabled = ((bool)(resources.GetObject("ZoekenMenuItem.Enabled")));
            this._ZoekenMenuItem.Index = 2;
            this._ZoekenMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("ZoekenMenuItem.Shortcut")));
            this._ZoekenMenuItem.ShowShortcut = ((bool)(resources.GetObject("ZoekenMenuItem.ShowShortcut")));
            this._ZoekenMenuItem.Text = resources.GetString("ZoekenMenuItem.Text");
            this._ZoekenMenuItem.Visible = ((bool)(resources.GetObject("ZoekenMenuItem.Visible")));
            // 
            // RapportMenuItem
            // 
            this._RapportMenuItem.Enabled = ((bool)(resources.GetObject("RapportMenuItem.Enabled")));
            this._RapportMenuItem.Index = 2;
            this._RapportMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("RapportMenuItem.Shortcut")));
            this._RapportMenuItem.ShowShortcut = ((bool)(resources.GetObject("RapportMenuItem.ShowShortcut")));
            this._RapportMenuItem.Text = resources.GetString("RapportMenuItem.Text");
            this._RapportMenuItem.Visible = ((bool)(resources.GetObject("RapportMenuItem.Visible")));
            // 
            // InstellingenMenuItem
            // 
            this._InstellingenMenuItem.Enabled = ((bool)(resources.GetObject("InstellingenMenuItem.Enabled")));
            this._InstellingenMenuItem.Index = 3;
            this._InstellingenMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                         this._DbConfigMenuItem});
            this._InstellingenMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("InstellingenMenuItem.Shortcut")));
            this._InstellingenMenuItem.ShowShortcut = ((bool)(resources.GetObject("InstellingenMenuItem.ShowShortcut")));
            this._InstellingenMenuItem.Text = resources.GetString("InstellingenMenuItem.Text");
            this._InstellingenMenuItem.Visible = ((bool)(resources.GetObject("InstellingenMenuItem.Visible")));
            // 
            // DbConfigMenuItem
            // 
            this._DbConfigMenuItem.Enabled = ((bool)(resources.GetObject("DbConfigMenuItem.Enabled")));
            this._DbConfigMenuItem.Index = 0;
            this._DbConfigMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("DbConfigMenuItem.Shortcut")));
            this._DbConfigMenuItem.ShowShortcut = ((bool)(resources.GetObject("DbConfigMenuItem.ShowShortcut")));
            this._DbConfigMenuItem.Text = resources.GetString("DbConfigMenuItem.Text");
            this._DbConfigMenuItem.Visible = ((bool)(resources.GetObject("DbConfigMenuItem.Visible")));
            this._DbConfigMenuItem.Click += new System.EventHandler(this.DbConfigMenuItem_Click);
            // 
            // infoMenuItem
            // 
            this._InfoMenuItem.Enabled = ((bool)(resources.GetObject("infoMenuItem.Enabled")));
            this._InfoMenuItem.Index = 4;
            this._InfoMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                 this._AboutMenuItem,
                                                                                 this._StatusMenuItem});
            this._InfoMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("infoMenuItem.Shortcut")));
            this._InfoMenuItem.ShowShortcut = ((bool)(resources.GetObject("infoMenuItem.ShowShortcut")));
            this._InfoMenuItem.Text = resources.GetString("infoMenuItem.Text");
            this._InfoMenuItem.Visible = ((bool)(resources.GetObject("infoMenuItem.Visible")));
            // 
            // baseStatusBar
            // 
            this._BaseStatusBar.AccessibleDescription = resources.GetString("baseStatusBar.AccessibleDescription");
            this._BaseStatusBar.AccessibleName = resources.GetString("baseStatusBar.AccessibleName");
            this._BaseStatusBar.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("baseStatusBar.Anchor")));
            this._BaseStatusBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("baseStatusBar.BackgroundImage")));
            this._BaseStatusBar.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("baseStatusBar.Dock")));
            this._BaseStatusBar.Enabled = ((bool)(resources.GetObject("baseStatusBar.Enabled")));
            this._BaseStatusBar.Font = ((System.Drawing.Font)(resources.GetObject("baseStatusBar.Font")));
            this._BaseStatusBar.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("baseStatusBar.ImeMode")));
            this._BaseStatusBar.Location = ((System.Drawing.Point)(resources.GetObject("baseStatusBar.Location")));
            this._BaseStatusBar.Name = "_BaseStatusBar";
            this._BaseStatusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
                                                                                     this._StatusBarPanel});
            this._BaseStatusBar.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("baseStatusBar.RightToLeft")));
            this._BaseStatusBar.ShowPanels = true;
            this._BaseStatusBar.Size = ((System.Drawing.Size)(resources.GetObject("baseStatusBar.Size")));
            this._BaseStatusBar.TabIndex = ((int)(resources.GetObject("baseStatusBar.TabIndex")));
            this._BaseStatusBar.Text = resources.GetString("baseStatusBar.Text");
            this._BaseStatusBar.Visible = ((bool)(resources.GetObject("baseStatusBar.Visible")));
            // 
            // statusBarPanel
            // 
            this._StatusBarPanel.Alignment = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("statusBarPanel.Alignment")));
            this._StatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this._StatusBarPanel.Icon = ((System.Drawing.Icon)(resources.GetObject("statusBarPanel.Icon")));
            this._StatusBarPanel.MinWidth = ((int)(resources.GetObject("statusBarPanel.MinWidth")));
            this._StatusBarPanel.Text = resources.GetString("statusBarPanel.Text");
            this._StatusBarPanel.ToolTipText = resources.GetString("statusBarPanel.ToolTipText");
            this._StatusBarPanel.Width = ((int)(resources.GetObject("statusBarPanel.Width")));
            // 
            // mainPanel
            // 
            this._MainPanel.AccessibleDescription = resources.GetString("mainPanel.AccessibleDescription");
            this._MainPanel.AccessibleName = resources.GetString("mainPanel.AccessibleName");
            this._MainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mainPanel.Anchor")));
            this._MainPanel.AutoScroll = ((bool)(resources.GetObject("mainPanel.AutoScroll")));
            this._MainPanel.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("mainPanel.AutoScrollMargin")));
            this._MainPanel.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("mainPanel.AutoScrollMinSize")));
            this._MainPanel.BackColor = System.Drawing.SystemColors.Info;
            this._MainPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mainPanel.BackgroundImage")));
            this._MainPanel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mainPanel.Dock")));
            this._MainPanel.Enabled = ((bool)(resources.GetObject("mainPanel.Enabled")));
            this._MainPanel.Font = ((System.Drawing.Font)(resources.GetObject("mainPanel.Font")));
            this._MainPanel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mainPanel.ImeMode")));
            this._MainPanel.Location = ((System.Drawing.Point)(resources.GetObject("mainPanel.Location")));
            this._MainPanel.Name = "_MainPanel";
            this._MainPanel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mainPanel.RightToLeft")));
            this._MainPanel.Size = ((System.Drawing.Size)(resources.GetObject("mainPanel.Size")));
            this._MainPanel.TabIndex = ((int)(resources.GetObject("mainPanel.TabIndex")));
            this._MainPanel.Text = resources.GetString("mainPanel.Text");
            this._MainPanel.Visible = ((bool)(resources.GetObject("mainPanel.Visible")));
            // 
            // aboutMenuItem
            // 
            this._AboutMenuItem.Enabled = ((bool)(resources.GetObject("aboutMenuItem.Enabled")));
            this._AboutMenuItem.Index = 0;
            this._AboutMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("aboutMenuItem.Shortcut")));
            this._AboutMenuItem.ShowShortcut = ((bool)(resources.GetObject("aboutMenuItem.ShowShortcut")));
            this._AboutMenuItem.Text = resources.GetString("aboutMenuItem.Text");
            this._AboutMenuItem.Visible = ((bool)(resources.GetObject("aboutMenuItem.Visible")));
            this._AboutMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
            // 
            // statusMenuItem
            // 
            this._StatusMenuItem.Enabled = ((bool)(resources.GetObject("statusMenuItem.Enabled")));
            this._StatusMenuItem.Index = 1;
            this._StatusMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("statusMenuItem.Shortcut")));
            this._StatusMenuItem.ShowShortcut = ((bool)(resources.GetObject("statusMenuItem.ShowShortcut")));
            this._StatusMenuItem.Text = resources.GetString("statusMenuItem.Text");
            this._StatusMenuItem.Visible = ((bool)(resources.GetObject("statusMenuItem.Visible")));
            // 
            // BaseMainform
            // 
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AccessibleName = resources.GetString("$this.AccessibleName");
            this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
            this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
            this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
            this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
            this.Controls.Add(this._MainPanel);
            this.Controls.Add(this._BaseStatusBar);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
            this.Menu = this._MainMenu;
            this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
            this.Name = "BaseMainform";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            ((System.ComponentModel.ISupportInitialize)(this._StatusBarPanel)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion


        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            if (_AboutFormClass != null)
            {
                var aboutForm = Activator.CreateInstance(_AboutFormClass) as BaseAboutForm;

                if (aboutForm != null)
                {
                    aboutForm.ShowDialog();
                }
            }
        }

        private void AfsluitenMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        protected virtual BaseContainerFrame BuildMainContainer(BaseContainerFrame mainframe)
        {
            try
            {
                //      var
                //        FrameFactory: TFrameFactory;
                //      begin
                //        FrameFactory := TFrameFactory.Create;
                //        FrameFactory.RegisterFrame(FrmMainFrame, TDSPMainContainerFrame);
                //        FMainFrame := FrameFactory.GetFrame(FrmMainFrame, MainPanel);
                //        FrameFactory.Free;

                // TODO: same construction as in aboutform --> do the same in delphi.
                if (mainframe == null)
                {
                    mainframe = new BaseContainerFrame();
                }

                mainframe.Height = _MainPanel.Height;
                mainframe.Width = _MainPanel.Width;
                mainframe.Anchor = _MainPanel.Anchor;

                if (_Config == null)
                {
                    if (_ConfigClass != null)
                    {
                        _Config = Activator.CreateInstance(_ConfigClass) as BaseConfigClass;
                    }
                }

                mainframe.Config = _Config;  // this is not the way to make this a singleton!!!  ---> must be an applicationsettings class

                _MainPanel.Controls.Add(mainframe);

                mainframe.AfterCreate();
            }
            catch (Exception e)
            {
                string errorString = _ErrorHeader + e;

                if (IsAssigned(_Config))
                {
                    if (_Config != null)
                    {
                        _Config.EventLog.WriteEntry(errorString, EventLogEntryType.Error);
                    }

                    throw;
                }

                throw;
            }

            return mainframe;
        }

        private void DbConfigMenuItem_Click(object sender, EventArgs e)
        {
            var databaseForm = new BaseDatabaseConfigForm();

            databaseForm.ShowDialog();
        }

        #region helperCode

        private bool IsAssigned(object newObject)
        {
            if (newObject == null)
            {
                return false;
            }

            return true;
        }
        #endregion helperCode
    }
}
