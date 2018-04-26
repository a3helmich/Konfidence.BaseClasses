using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Resources;
using System.Security.Permissions;
using System.Windows.Forms;
using Konfidence.Base;
using Konfidence.BaseHelper;

namespace Konfidence.BaseWindowForms
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class BaseMainform : Form
    {
        private Type _configClass;
        private BaseConfigClass _config;
        private readonly string _errorHeader;

        private Type _aboutFormClass;

        private MainMenu _mainMenu;
        private MenuItem _bestandMenuItem;
        private MenuItem _afsluitenMenuItem;
        private MenuItem _beeldMenuItem;
        private MenuItem _lijstMenuItem;
        private MenuItem _detailMenuItem;
        private MenuItem _zoekenMenuItem;
        private MenuItem _rapportMenuItem;
        private MenuItem _instellingenMenuItem;
        private MenuItem _dbConfigMenuItem;
        private MenuItem _infoMenuItem;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly Container _components = null;

        private StatusBar _baseStatusBar;
        private StatusBarPanel _statusBarPanel;
        private MenuItem _aboutMenuItem;
        private MenuItem _statusMenuItem;
        private Panel _mainPanel;

        public Type ConfigClass
        {
            get { return _configClass; }
            set { _configClass = value; }
        }

        public Type AboutFormClass
        {
            get { return _aboutFormClass; }
            set
            {
                if (BaseAboutForm.IsDerivedClass(value))
                    _aboutFormClass = value;
            }
        }

        public BaseContainerFrame BaseMainframe { get; }

        public BaseMainform()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            BaseInitialize();

            var resources = new ResourceManager(typeof(BaseMainform));

            _errorHeader = resources.GetString("ErrorHeader.Text");

            BaseMainframe = BuildMainContainer(null);
        }

        private void BaseInitialize()
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
                if (_components.IsAssigned())
                {
                    _components.Dispose();
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
            this._mainMenu = new System.Windows.Forms.MainMenu();
            this._bestandMenuItem = new System.Windows.Forms.MenuItem();
            this._afsluitenMenuItem = new System.Windows.Forms.MenuItem();
            this._beeldMenuItem = new System.Windows.Forms.MenuItem();
            this._lijstMenuItem = new System.Windows.Forms.MenuItem();
            this._detailMenuItem = new System.Windows.Forms.MenuItem();
            this._zoekenMenuItem = new System.Windows.Forms.MenuItem();
            this._rapportMenuItem = new System.Windows.Forms.MenuItem();
            this._instellingenMenuItem = new System.Windows.Forms.MenuItem();
            this._dbConfigMenuItem = new System.Windows.Forms.MenuItem();
            this._infoMenuItem = new System.Windows.Forms.MenuItem();
            this._baseStatusBar = new System.Windows.Forms.StatusBar();
            this._statusBarPanel = new System.Windows.Forms.StatusBarPanel();
            this._mainPanel = new System.Windows.Forms.Panel();
            this._aboutMenuItem = new System.Windows.Forms.MenuItem();
            this._statusMenuItem = new System.Windows.Forms.MenuItem();
            ((System.ComponentModel.ISupportInitialize)(this._statusBarPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this._mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                             this._bestandMenuItem,
                                                                             this._beeldMenuItem,
                                                                             this._rapportMenuItem,
                                                                             this._instellingenMenuItem,
                                                                             this._infoMenuItem});
            this._mainMenu.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mainMenu.RightToLeft")));
            // 
            // BestandMenuItem
            // 
            this._bestandMenuItem.Enabled = ((bool)(resources.GetObject("BestandMenuItem.Enabled")));
            this._bestandMenuItem.Index = 0;
            this._bestandMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                    this._afsluitenMenuItem});
            this._bestandMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("BestandMenuItem.Shortcut")));
            this._bestandMenuItem.ShowShortcut = ((bool)(resources.GetObject("BestandMenuItem.ShowShortcut")));
            this._bestandMenuItem.Text = resources.GetString("BestandMenuItem.Text");
            this._bestandMenuItem.Visible = ((bool)(resources.GetObject("BestandMenuItem.Visible")));
            // 
            // AfsluitenMenuItem
            // 
            this._afsluitenMenuItem.Enabled = ((bool)(resources.GetObject("AfsluitenMenuItem.Enabled")));
            this._afsluitenMenuItem.Index = 0;
            this._afsluitenMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("AfsluitenMenuItem.Shortcut")));
            this._afsluitenMenuItem.ShowShortcut = ((bool)(resources.GetObject("AfsluitenMenuItem.ShowShortcut")));
            this._afsluitenMenuItem.Text = resources.GetString("AfsluitenMenuItem.Text");
            this._afsluitenMenuItem.Visible = ((bool)(resources.GetObject("AfsluitenMenuItem.Visible")));
            this._afsluitenMenuItem.Click += new System.EventHandler(this.AfsluitenMenuItem_Click);
            // 
            // BeeldMenuItem
            // 
            this._beeldMenuItem.Enabled = ((bool)(resources.GetObject("BeeldMenuItem.Enabled")));
            this._beeldMenuItem.Index = 1;
            this._beeldMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                  this._lijstMenuItem,
                                                                                  this._detailMenuItem,
                                                                                  this._zoekenMenuItem});
            this._beeldMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("BeeldMenuItem.Shortcut")));
            this._beeldMenuItem.ShowShortcut = ((bool)(resources.GetObject("BeeldMenuItem.ShowShortcut")));
            this._beeldMenuItem.Text = resources.GetString("BeeldMenuItem.Text");
            this._beeldMenuItem.Visible = ((bool)(resources.GetObject("BeeldMenuItem.Visible")));
            // 
            // LijstMenuItem
            // 
            this._lijstMenuItem.Enabled = ((bool)(resources.GetObject("LijstMenuItem.Enabled")));
            this._lijstMenuItem.Index = 0;
            this._lijstMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("LijstMenuItem.Shortcut")));
            this._lijstMenuItem.ShowShortcut = ((bool)(resources.GetObject("LijstMenuItem.ShowShortcut")));
            this._lijstMenuItem.Text = resources.GetString("LijstMenuItem.Text");
            this._lijstMenuItem.Visible = ((bool)(resources.GetObject("LijstMenuItem.Visible")));
            // 
            // DetailMenuItem
            // 
            this._detailMenuItem.Enabled = ((bool)(resources.GetObject("DetailMenuItem.Enabled")));
            this._detailMenuItem.Index = 1;
            this._detailMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("DetailMenuItem.Shortcut")));
            this._detailMenuItem.ShowShortcut = ((bool)(resources.GetObject("DetailMenuItem.ShowShortcut")));
            this._detailMenuItem.Text = resources.GetString("DetailMenuItem.Text");
            this._detailMenuItem.Visible = ((bool)(resources.GetObject("DetailMenuItem.Visible")));
            // 
            // ZoekenMenuItem
            // 
            this._zoekenMenuItem.Enabled = ((bool)(resources.GetObject("ZoekenMenuItem.Enabled")));
            this._zoekenMenuItem.Index = 2;
            this._zoekenMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("ZoekenMenuItem.Shortcut")));
            this._zoekenMenuItem.ShowShortcut = ((bool)(resources.GetObject("ZoekenMenuItem.ShowShortcut")));
            this._zoekenMenuItem.Text = resources.GetString("ZoekenMenuItem.Text");
            this._zoekenMenuItem.Visible = ((bool)(resources.GetObject("ZoekenMenuItem.Visible")));
            // 
            // RapportMenuItem
            // 
            this._rapportMenuItem.Enabled = ((bool)(resources.GetObject("RapportMenuItem.Enabled")));
            this._rapportMenuItem.Index = 2;
            this._rapportMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("RapportMenuItem.Shortcut")));
            this._rapportMenuItem.ShowShortcut = ((bool)(resources.GetObject("RapportMenuItem.ShowShortcut")));
            this._rapportMenuItem.Text = resources.GetString("RapportMenuItem.Text");
            this._rapportMenuItem.Visible = ((bool)(resources.GetObject("RapportMenuItem.Visible")));
            // 
            // InstellingenMenuItem
            // 
            this._instellingenMenuItem.Enabled = ((bool)(resources.GetObject("InstellingenMenuItem.Enabled")));
            this._instellingenMenuItem.Index = 3;
            this._instellingenMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                         this._dbConfigMenuItem});
            this._instellingenMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("InstellingenMenuItem.Shortcut")));
            this._instellingenMenuItem.ShowShortcut = ((bool)(resources.GetObject("InstellingenMenuItem.ShowShortcut")));
            this._instellingenMenuItem.Text = resources.GetString("InstellingenMenuItem.Text");
            this._instellingenMenuItem.Visible = ((bool)(resources.GetObject("InstellingenMenuItem.Visible")));
            // 
            // DbConfigMenuItem
            // 
            this._dbConfigMenuItem.Enabled = ((bool)(resources.GetObject("DbConfigMenuItem.Enabled")));
            this._dbConfigMenuItem.Index = 0;
            this._dbConfigMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("DbConfigMenuItem.Shortcut")));
            this._dbConfigMenuItem.ShowShortcut = ((bool)(resources.GetObject("DbConfigMenuItem.ShowShortcut")));
            this._dbConfigMenuItem.Text = resources.GetString("DbConfigMenuItem.Text");
            this._dbConfigMenuItem.Visible = ((bool)(resources.GetObject("DbConfigMenuItem.Visible")));
            this._dbConfigMenuItem.Click += new System.EventHandler(this.DbConfigMenuItem_Click);
            // 
            // infoMenuItem
            // 
            this._infoMenuItem.Enabled = ((bool)(resources.GetObject("infoMenuItem.Enabled")));
            this._infoMenuItem.Index = 4;
            this._infoMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                 this._aboutMenuItem,
                                                                                 this._statusMenuItem});
            this._infoMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("infoMenuItem.Shortcut")));
            this._infoMenuItem.ShowShortcut = ((bool)(resources.GetObject("infoMenuItem.ShowShortcut")));
            this._infoMenuItem.Text = resources.GetString("infoMenuItem.Text");
            this._infoMenuItem.Visible = ((bool)(resources.GetObject("infoMenuItem.Visible")));
            // 
            // baseStatusBar
            // 
            this._baseStatusBar.AccessibleDescription = resources.GetString("baseStatusBar.AccessibleDescription");
            this._baseStatusBar.AccessibleName = resources.GetString("baseStatusBar.AccessibleName");
            this._baseStatusBar.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("baseStatusBar.Anchor")));
            this._baseStatusBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("baseStatusBar.BackgroundImage")));
            this._baseStatusBar.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("baseStatusBar.Dock")));
            this._baseStatusBar.Enabled = ((bool)(resources.GetObject("baseStatusBar.Enabled")));
            this._baseStatusBar.Font = ((System.Drawing.Font)(resources.GetObject("baseStatusBar.Font")));
            this._baseStatusBar.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("baseStatusBar.ImeMode")));
            this._baseStatusBar.Location = ((System.Drawing.Point)(resources.GetObject("baseStatusBar.Location")));
            this._baseStatusBar.Name = "_baseStatusBar";
            this._baseStatusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
                                                                                     this._statusBarPanel});
            this._baseStatusBar.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("baseStatusBar.RightToLeft")));
            this._baseStatusBar.ShowPanels = true;
            this._baseStatusBar.Size = ((System.Drawing.Size)(resources.GetObject("baseStatusBar.Size")));
            this._baseStatusBar.TabIndex = ((int)(resources.GetObject("baseStatusBar.TabIndex")));
            this._baseStatusBar.Text = resources.GetString("baseStatusBar.Text");
            this._baseStatusBar.Visible = ((bool)(resources.GetObject("baseStatusBar.Visible")));
            // 
            // statusBarPanel
            // 
            this._statusBarPanel.Alignment = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("statusBarPanel.Alignment")));
            this._statusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this._statusBarPanel.Icon = ((System.Drawing.Icon)(resources.GetObject("statusBarPanel.Icon")));
            this._statusBarPanel.MinWidth = ((int)(resources.GetObject("statusBarPanel.MinWidth")));
            this._statusBarPanel.Text = resources.GetString("statusBarPanel.Text");
            this._statusBarPanel.ToolTipText = resources.GetString("statusBarPanel.ToolTipText");
            this._statusBarPanel.Width = ((int)(resources.GetObject("statusBarPanel.Width")));
            // 
            // mainPanel
            // 
            this._mainPanel.AccessibleDescription = resources.GetString("mainPanel.AccessibleDescription");
            this._mainPanel.AccessibleName = resources.GetString("mainPanel.AccessibleName");
            this._mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mainPanel.Anchor")));
            this._mainPanel.AutoScroll = ((bool)(resources.GetObject("mainPanel.AutoScroll")));
            this._mainPanel.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("mainPanel.AutoScrollMargin")));
            this._mainPanel.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("mainPanel.AutoScrollMinSize")));
            this._mainPanel.BackColor = System.Drawing.SystemColors.Info;
            this._mainPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mainPanel.BackgroundImage")));
            this._mainPanel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mainPanel.Dock")));
            this._mainPanel.Enabled = ((bool)(resources.GetObject("mainPanel.Enabled")));
            this._mainPanel.Font = ((System.Drawing.Font)(resources.GetObject("mainPanel.Font")));
            this._mainPanel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mainPanel.ImeMode")));
            this._mainPanel.Location = ((System.Drawing.Point)(resources.GetObject("mainPanel.Location")));
            this._mainPanel.Name = "_mainPanel";
            this._mainPanel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mainPanel.RightToLeft")));
            this._mainPanel.Size = ((System.Drawing.Size)(resources.GetObject("mainPanel.Size")));
            this._mainPanel.TabIndex = ((int)(resources.GetObject("mainPanel.TabIndex")));
            this._mainPanel.Text = resources.GetString("mainPanel.Text");
            this._mainPanel.Visible = ((bool)(resources.GetObject("mainPanel.Visible")));
            // 
            // aboutMenuItem
            // 
            this._aboutMenuItem.Enabled = ((bool)(resources.GetObject("aboutMenuItem.Enabled")));
            this._aboutMenuItem.Index = 0;
            this._aboutMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("aboutMenuItem.Shortcut")));
            this._aboutMenuItem.ShowShortcut = ((bool)(resources.GetObject("aboutMenuItem.ShowShortcut")));
            this._aboutMenuItem.Text = resources.GetString("aboutMenuItem.Text");
            this._aboutMenuItem.Visible = ((bool)(resources.GetObject("aboutMenuItem.Visible")));
            this._aboutMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
            // 
            // statusMenuItem
            // 
            this._statusMenuItem.Enabled = ((bool)(resources.GetObject("statusMenuItem.Enabled")));
            this._statusMenuItem.Index = 1;
            this._statusMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("statusMenuItem.Shortcut")));
            this._statusMenuItem.ShowShortcut = ((bool)(resources.GetObject("statusMenuItem.ShowShortcut")));
            this._statusMenuItem.Text = resources.GetString("statusMenuItem.Text");
            this._statusMenuItem.Visible = ((bool)(resources.GetObject("statusMenuItem.Visible")));
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
            this.Controls.Add(this._mainPanel);
            this.Controls.Add(this._baseStatusBar);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
            this.Menu = this._mainMenu;
            this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
            this.Name = "BaseMainform";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            ((System.ComponentModel.ISupportInitialize)(this._statusBarPanel)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion


        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            if (_aboutFormClass.IsAssigned())
            {
                var aboutForm = Activator.CreateInstance(_aboutFormClass) as BaseAboutForm;

                if (aboutForm.IsAssigned())
                {
                    aboutForm.ShowDialog();
                }
            }
        }

        private void AfsluitenMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        [EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted = true)]
        private BaseContainerFrame BuildMainContainer(BaseContainerFrame mainframe)
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
                if (!mainframe.IsAssigned())
                {
                    mainframe = new BaseContainerFrame();
                }

                mainframe.Height = _mainPanel.Height;
                mainframe.Width = _mainPanel.Width;
                mainframe.Anchor = _mainPanel.Anchor;

                if (!_config.IsAssigned())
                {
                    if (_configClass.IsAssigned())
                    {
                        _config = Activator.CreateInstance(_configClass) as BaseConfigClass;
                    }
                }

                mainframe.Config = _config;  // this is not the way to make this a singleton!!!  ---> must be an applicationsettings class

                _mainPanel.Controls.Add(mainframe);

                mainframe.AfterCreate();
            }
            catch (Exception e)
            {
                string errorString = _errorHeader + e;

                if (_config.IsAssigned())
                {
                    _config.EventLog.WriteEntry(errorString, EventLogEntryType.Error);
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
    }
}
