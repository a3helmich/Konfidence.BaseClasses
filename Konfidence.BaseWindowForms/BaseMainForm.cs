using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
using System.Resources;
using System.Security.Permissions;
using System.Reflection;

using Konfidence.BaseHelper;

namespace Konfidence.BaseWindowForms
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class BaseMainform : System.Windows.Forms.Form
    {
        private Type _ConfigClass = null;
        private BaseConfigClass _Config = null;
        private string _ErrorHeader;

        private BaseContainerFrame _BaseMainframe;
        private Type _AboutFormClass = null;

        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem BestandMenuItem;
        private System.Windows.Forms.MenuItem AfsluitenMenuItem;
        private System.Windows.Forms.MenuItem BeeldMenuItem;
        private System.Windows.Forms.MenuItem LijstMenuItem;
        private System.Windows.Forms.MenuItem DetailMenuItem;
        private System.Windows.Forms.MenuItem ZoekenMenuItem;
        private System.Windows.Forms.MenuItem RapportMenuItem;
        protected System.Windows.Forms.MenuItem InstellingenMenuItem;
        private System.Windows.Forms.MenuItem DbConfigMenuItem;
        private System.Windows.Forms.MenuItem infoMenuItem;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private System.Windows.Forms.StatusBar baseStatusBar;
        private System.Windows.Forms.StatusBarPanel statusBarPanel;
        private System.Windows.Forms.MenuItem aboutMenuItem;
        private System.Windows.Forms.MenuItem statusMenuItem;
        private System.Windows.Forms.Panel mainPanel;

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

            ResourceManager resources = new System.Resources.ResourceManager(typeof(BaseMainform));

            _ErrorHeader = resources.GetString("ErrorHeader.Text");

            _BaseMainframe = BuildMainContainer(0, null);
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
                if (components != null)
                {
                    components.Dispose();
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
            this.mainMenu = new System.Windows.Forms.MainMenu();
            this.BestandMenuItem = new System.Windows.Forms.MenuItem();
            this.AfsluitenMenuItem = new System.Windows.Forms.MenuItem();
            this.BeeldMenuItem = new System.Windows.Forms.MenuItem();
            this.LijstMenuItem = new System.Windows.Forms.MenuItem();
            this.DetailMenuItem = new System.Windows.Forms.MenuItem();
            this.ZoekenMenuItem = new System.Windows.Forms.MenuItem();
            this.RapportMenuItem = new System.Windows.Forms.MenuItem();
            this.InstellingenMenuItem = new System.Windows.Forms.MenuItem();
            this.DbConfigMenuItem = new System.Windows.Forms.MenuItem();
            this.infoMenuItem = new System.Windows.Forms.MenuItem();
            this.baseStatusBar = new System.Windows.Forms.StatusBar();
            this.statusBarPanel = new System.Windows.Forms.StatusBarPanel();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.aboutMenuItem = new System.Windows.Forms.MenuItem();
            this.statusMenuItem = new System.Windows.Forms.MenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                             this.BestandMenuItem,
                                                                             this.BeeldMenuItem,
                                                                             this.RapportMenuItem,
                                                                             this.InstellingenMenuItem,
                                                                             this.infoMenuItem});
            this.mainMenu.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mainMenu.RightToLeft")));
            // 
            // BestandMenuItem
            // 
            this.BestandMenuItem.Enabled = ((bool)(resources.GetObject("BestandMenuItem.Enabled")));
            this.BestandMenuItem.Index = 0;
            this.BestandMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                    this.AfsluitenMenuItem});
            this.BestandMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("BestandMenuItem.Shortcut")));
            this.BestandMenuItem.ShowShortcut = ((bool)(resources.GetObject("BestandMenuItem.ShowShortcut")));
            this.BestandMenuItem.Text = resources.GetString("BestandMenuItem.Text");
            this.BestandMenuItem.Visible = ((bool)(resources.GetObject("BestandMenuItem.Visible")));
            // 
            // AfsluitenMenuItem
            // 
            this.AfsluitenMenuItem.Enabled = ((bool)(resources.GetObject("AfsluitenMenuItem.Enabled")));
            this.AfsluitenMenuItem.Index = 0;
            this.AfsluitenMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("AfsluitenMenuItem.Shortcut")));
            this.AfsluitenMenuItem.ShowShortcut = ((bool)(resources.GetObject("AfsluitenMenuItem.ShowShortcut")));
            this.AfsluitenMenuItem.Text = resources.GetString("AfsluitenMenuItem.Text");
            this.AfsluitenMenuItem.Visible = ((bool)(resources.GetObject("AfsluitenMenuItem.Visible")));
            this.AfsluitenMenuItem.Click += new System.EventHandler(this.AfsluitenMenuItem_Click);
            // 
            // BeeldMenuItem
            // 
            this.BeeldMenuItem.Enabled = ((bool)(resources.GetObject("BeeldMenuItem.Enabled")));
            this.BeeldMenuItem.Index = 1;
            this.BeeldMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                  this.LijstMenuItem,
                                                                                  this.DetailMenuItem,
                                                                                  this.ZoekenMenuItem});
            this.BeeldMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("BeeldMenuItem.Shortcut")));
            this.BeeldMenuItem.ShowShortcut = ((bool)(resources.GetObject("BeeldMenuItem.ShowShortcut")));
            this.BeeldMenuItem.Text = resources.GetString("BeeldMenuItem.Text");
            this.BeeldMenuItem.Visible = ((bool)(resources.GetObject("BeeldMenuItem.Visible")));
            // 
            // LijstMenuItem
            // 
            this.LijstMenuItem.Enabled = ((bool)(resources.GetObject("LijstMenuItem.Enabled")));
            this.LijstMenuItem.Index = 0;
            this.LijstMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("LijstMenuItem.Shortcut")));
            this.LijstMenuItem.ShowShortcut = ((bool)(resources.GetObject("LijstMenuItem.ShowShortcut")));
            this.LijstMenuItem.Text = resources.GetString("LijstMenuItem.Text");
            this.LijstMenuItem.Visible = ((bool)(resources.GetObject("LijstMenuItem.Visible")));
            // 
            // DetailMenuItem
            // 
            this.DetailMenuItem.Enabled = ((bool)(resources.GetObject("DetailMenuItem.Enabled")));
            this.DetailMenuItem.Index = 1;
            this.DetailMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("DetailMenuItem.Shortcut")));
            this.DetailMenuItem.ShowShortcut = ((bool)(resources.GetObject("DetailMenuItem.ShowShortcut")));
            this.DetailMenuItem.Text = resources.GetString("DetailMenuItem.Text");
            this.DetailMenuItem.Visible = ((bool)(resources.GetObject("DetailMenuItem.Visible")));
            // 
            // ZoekenMenuItem
            // 
            this.ZoekenMenuItem.Enabled = ((bool)(resources.GetObject("ZoekenMenuItem.Enabled")));
            this.ZoekenMenuItem.Index = 2;
            this.ZoekenMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("ZoekenMenuItem.Shortcut")));
            this.ZoekenMenuItem.ShowShortcut = ((bool)(resources.GetObject("ZoekenMenuItem.ShowShortcut")));
            this.ZoekenMenuItem.Text = resources.GetString("ZoekenMenuItem.Text");
            this.ZoekenMenuItem.Visible = ((bool)(resources.GetObject("ZoekenMenuItem.Visible")));
            // 
            // RapportMenuItem
            // 
            this.RapportMenuItem.Enabled = ((bool)(resources.GetObject("RapportMenuItem.Enabled")));
            this.RapportMenuItem.Index = 2;
            this.RapportMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("RapportMenuItem.Shortcut")));
            this.RapportMenuItem.ShowShortcut = ((bool)(resources.GetObject("RapportMenuItem.ShowShortcut")));
            this.RapportMenuItem.Text = resources.GetString("RapportMenuItem.Text");
            this.RapportMenuItem.Visible = ((bool)(resources.GetObject("RapportMenuItem.Visible")));
            // 
            // InstellingenMenuItem
            // 
            this.InstellingenMenuItem.Enabled = ((bool)(resources.GetObject("InstellingenMenuItem.Enabled")));
            this.InstellingenMenuItem.Index = 3;
            this.InstellingenMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                         this.DbConfigMenuItem});
            this.InstellingenMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("InstellingenMenuItem.Shortcut")));
            this.InstellingenMenuItem.ShowShortcut = ((bool)(resources.GetObject("InstellingenMenuItem.ShowShortcut")));
            this.InstellingenMenuItem.Text = resources.GetString("InstellingenMenuItem.Text");
            this.InstellingenMenuItem.Visible = ((bool)(resources.GetObject("InstellingenMenuItem.Visible")));
            // 
            // DbConfigMenuItem
            // 
            this.DbConfigMenuItem.Enabled = ((bool)(resources.GetObject("DbConfigMenuItem.Enabled")));
            this.DbConfigMenuItem.Index = 0;
            this.DbConfigMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("DbConfigMenuItem.Shortcut")));
            this.DbConfigMenuItem.ShowShortcut = ((bool)(resources.GetObject("DbConfigMenuItem.ShowShortcut")));
            this.DbConfigMenuItem.Text = resources.GetString("DbConfigMenuItem.Text");
            this.DbConfigMenuItem.Visible = ((bool)(resources.GetObject("DbConfigMenuItem.Visible")));
            this.DbConfigMenuItem.Click += new System.EventHandler(this.DbConfigMenuItem_Click);
            // 
            // infoMenuItem
            // 
            this.infoMenuItem.Enabled = ((bool)(resources.GetObject("infoMenuItem.Enabled")));
            this.infoMenuItem.Index = 4;
            this.infoMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                 this.aboutMenuItem,
                                                                                 this.statusMenuItem});
            this.infoMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("infoMenuItem.Shortcut")));
            this.infoMenuItem.ShowShortcut = ((bool)(resources.GetObject("infoMenuItem.ShowShortcut")));
            this.infoMenuItem.Text = resources.GetString("infoMenuItem.Text");
            this.infoMenuItem.Visible = ((bool)(resources.GetObject("infoMenuItem.Visible")));
            // 
            // baseStatusBar
            // 
            this.baseStatusBar.AccessibleDescription = resources.GetString("baseStatusBar.AccessibleDescription");
            this.baseStatusBar.AccessibleName = resources.GetString("baseStatusBar.AccessibleName");
            this.baseStatusBar.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("baseStatusBar.Anchor")));
            this.baseStatusBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("baseStatusBar.BackgroundImage")));
            this.baseStatusBar.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("baseStatusBar.Dock")));
            this.baseStatusBar.Enabled = ((bool)(resources.GetObject("baseStatusBar.Enabled")));
            this.baseStatusBar.Font = ((System.Drawing.Font)(resources.GetObject("baseStatusBar.Font")));
            this.baseStatusBar.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("baseStatusBar.ImeMode")));
            this.baseStatusBar.Location = ((System.Drawing.Point)(resources.GetObject("baseStatusBar.Location")));
            this.baseStatusBar.Name = "baseStatusBar";
            this.baseStatusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
                                                                                     this.statusBarPanel});
            this.baseStatusBar.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("baseStatusBar.RightToLeft")));
            this.baseStatusBar.ShowPanels = true;
            this.baseStatusBar.Size = ((System.Drawing.Size)(resources.GetObject("baseStatusBar.Size")));
            this.baseStatusBar.TabIndex = ((int)(resources.GetObject("baseStatusBar.TabIndex")));
            this.baseStatusBar.Text = resources.GetString("baseStatusBar.Text");
            this.baseStatusBar.Visible = ((bool)(resources.GetObject("baseStatusBar.Visible")));
            // 
            // statusBarPanel
            // 
            this.statusBarPanel.Alignment = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("statusBarPanel.Alignment")));
            this.statusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusBarPanel.Icon = ((System.Drawing.Icon)(resources.GetObject("statusBarPanel.Icon")));
            this.statusBarPanel.MinWidth = ((int)(resources.GetObject("statusBarPanel.MinWidth")));
            this.statusBarPanel.Text = resources.GetString("statusBarPanel.Text");
            this.statusBarPanel.ToolTipText = resources.GetString("statusBarPanel.ToolTipText");
            this.statusBarPanel.Width = ((int)(resources.GetObject("statusBarPanel.Width")));
            // 
            // mainPanel
            // 
            this.mainPanel.AccessibleDescription = resources.GetString("mainPanel.AccessibleDescription");
            this.mainPanel.AccessibleName = resources.GetString("mainPanel.AccessibleName");
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mainPanel.Anchor")));
            this.mainPanel.AutoScroll = ((bool)(resources.GetObject("mainPanel.AutoScroll")));
            this.mainPanel.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("mainPanel.AutoScrollMargin")));
            this.mainPanel.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("mainPanel.AutoScrollMinSize")));
            this.mainPanel.BackColor = System.Drawing.SystemColors.Info;
            this.mainPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mainPanel.BackgroundImage")));
            this.mainPanel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mainPanel.Dock")));
            this.mainPanel.Enabled = ((bool)(resources.GetObject("mainPanel.Enabled")));
            this.mainPanel.Font = ((System.Drawing.Font)(resources.GetObject("mainPanel.Font")));
            this.mainPanel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mainPanel.ImeMode")));
            this.mainPanel.Location = ((System.Drawing.Point)(resources.GetObject("mainPanel.Location")));
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mainPanel.RightToLeft")));
            this.mainPanel.Size = ((System.Drawing.Size)(resources.GetObject("mainPanel.Size")));
            this.mainPanel.TabIndex = ((int)(resources.GetObject("mainPanel.TabIndex")));
            this.mainPanel.Text = resources.GetString("mainPanel.Text");
            this.mainPanel.Visible = ((bool)(resources.GetObject("mainPanel.Visible")));
            // 
            // aboutMenuItem
            // 
            this.aboutMenuItem.Enabled = ((bool)(resources.GetObject("aboutMenuItem.Enabled")));
            this.aboutMenuItem.Index = 0;
            this.aboutMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("aboutMenuItem.Shortcut")));
            this.aboutMenuItem.ShowShortcut = ((bool)(resources.GetObject("aboutMenuItem.ShowShortcut")));
            this.aboutMenuItem.Text = resources.GetString("aboutMenuItem.Text");
            this.aboutMenuItem.Visible = ((bool)(resources.GetObject("aboutMenuItem.Visible")));
            this.aboutMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
            // 
            // statusMenuItem
            // 
            this.statusMenuItem.Enabled = ((bool)(resources.GetObject("statusMenuItem.Enabled")));
            this.statusMenuItem.Index = 1;
            this.statusMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("statusMenuItem.Shortcut")));
            this.statusMenuItem.ShowShortcut = ((bool)(resources.GetObject("statusMenuItem.ShowShortcut")));
            this.statusMenuItem.Text = resources.GetString("statusMenuItem.Text");
            this.statusMenuItem.Visible = ((bool)(resources.GetObject("statusMenuItem.Visible")));
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
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.baseStatusBar);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
            this.Menu = this.mainMenu;
            this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
            this.Name = "BaseMainform";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion


        private void aboutMenuItem_Click(object sender, System.EventArgs e)
        {
            if (_AboutFormClass != null)
            {
                BaseAboutForm AboutForm = Activator.CreateInstance(_AboutFormClass) as BaseAboutForm;

                AboutForm.ShowDialog();
            }
        }

        private void AfsluitenMenuItem_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void menuItem1_Click_1(object sender, System.EventArgs e)
        {
            mainPanel.Controls.Remove(_BaseMainframe);
        }

        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        protected virtual BaseContainerFrame BuildMainContainer(/*IdEnum*/int controlId, BaseContainerFrame mainframe)
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

                mainframe.Height = mainPanel.Height;
                mainframe.Width = mainPanel.Width;
                mainframe.Anchor = mainPanel.Anchor;

                if (_Config == null)
                {
                    if (_ConfigClass != null)
                    {
                        _Config = Activator.CreateInstance(_ConfigClass) as BaseConfigClass;
                    }
                }

                mainframe.Config = _Config;  // this is not the way to make this a singleton!!!  ---> must be an applicationsettings class

                mainPanel.Controls.Add(mainframe);

                mainframe.AfterCreate();
            }
            catch (Exception e)
            {
                string errorString = _ErrorHeader + e.ToString();

                if (IsAssigned(_Config))
                {
                    _Config.EventLog.WriteEntry(errorString, EventLogEntryType.Error);

                    throw;
                }

                throw e;
            }

            return mainframe;
        }

        private void DbConfigMenuItem_Click(object sender, System.EventArgs e)
        {
            BaseDatabaseConfigForm databaseForm = new BaseDatabaseConfigForm();

            databaseForm.ShowDialog();
        }

        #region helperCode
        protected bool IsAssigned(object newObject)
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
