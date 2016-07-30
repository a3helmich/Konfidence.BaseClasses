using System;
using System.ComponentModel;
using System.Windows.Forms;
using Konfidence.Base;

namespace Konfidence.BaseWindowForms
{
    public class BaseDatabaseConfigForm : BaseDialogForm
    {
        private TextBox _dbServerTextBox;
        private Label _dbServerLabel;
        private TextBox _dbUserTextBox;
        private Label _dbUserLabel;
        private readonly IContainer Components = null;

        private readonly string _currentDbServer;
        private readonly string _currentDatabase;
        private readonly string _currentUser;
        private Label _databaseLabel;
        private TextBox _databaseTextBox;
        private TextBox _dbPasswordTextBox;
        private Label _dbPasswordLabel;
        private readonly string _currentPassword;


        public BaseDatabaseConfigForm()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
            _currentDbServer = ApplicationSettings.GetStringValue("SQLServer");
            _currentDatabase = ApplicationSettings.GetStringValue("SQLDatabase");
            _currentUser = ApplicationSettings.GetStringValue("SQLUsername");
            _currentPassword = ApplicationSettings.GetStringValue("SQLPassword");

            _dbServerTextBox.Text = _currentDbServer;
            _databaseTextBox.Text = _currentDatabase;
            _dbUserTextBox.Text = _currentUser;
            _dbPasswordTextBox.Text = _currentPassword;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Components.IsAssigned())
                {
                    Components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._dbServerTextBox = new System.Windows.Forms.TextBox();
            this._dbServerLabel = new System.Windows.Forms.Label();
            this._dbUserTextBox = new System.Windows.Forms.TextBox();
            this._dbUserLabel = new System.Windows.Forms.Label();
            this._databaseTextBox = new System.Windows.Forms.TextBox();
            this._databaseLabel = new System.Windows.Forms.Label();
            this._dbPasswordTextBox = new System.Windows.Forms.TextBox();
            this._dbPasswordLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.ButtonCancel.Location = new System.Drawing.Point(204, 144);
            // 
            // buttonOK
            // 
            this.ButtonOk.Location = new System.Drawing.Point(124, 144);
            this.ButtonOk.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // DBServerTextBox
            // 
            this._dbServerTextBox.AccessibleName = "First Name";
            this._dbServerTextBox.Location = new System.Drawing.Point(151, 16);
            this._dbServerTextBox.Name = "_dbServerTextBox";
            this._dbServerTextBox.Size = new System.Drawing.Size(128, 20);
            this._dbServerTextBox.TabIndex = 0;
            // 
            // DBServerLabel
            // 
            this._dbServerLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this._dbServerLabel.Location = new System.Drawing.Point(17, 17);
            this._dbServerLabel.Name = "_dbServerLabel";
            this._dbServerLabel.Size = new System.Drawing.Size(96, 18);
            this._dbServerLabel.TabIndex = 18;
            this._dbServerLabel.Text = "Database server";
            this._dbServerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DBUserTextBox
            // 
            this._dbUserTextBox.AccessibleName = "First Name";
            this._dbUserTextBox.Location = new System.Drawing.Point(151, 72);
            this._dbUserTextBox.Name = "_dbUserTextBox";
            this._dbUserTextBox.Size = new System.Drawing.Size(128, 20);
            this._dbUserTextBox.TabIndex = 2;
            this._dbUserTextBox.TextChanged += new System.EventHandler(this.DBUserTextBox_TextChanged);
            // 
            // DBUserLabel
            // 
            this._dbUserLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this._dbUserLabel.Location = new System.Drawing.Point(17, 72);
            this._dbUserLabel.Name = "_dbUserLabel";
            this._dbUserLabel.Size = new System.Drawing.Size(96, 18);
            this._dbUserLabel.TabIndex = 20;
            this._dbUserLabel.Text = "Username";
            this._dbUserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DatabaseTextBox
            // 
            this._databaseTextBox.AccessibleName = "First Name";
            this._databaseTextBox.Location = new System.Drawing.Point(151, 44);
            this._databaseTextBox.Name = "_databaseTextBox";
            this._databaseTextBox.Size = new System.Drawing.Size(128, 20);
            this._databaseTextBox.TabIndex = 1;
            // 
            // DatabaseLabel
            // 
            this._databaseLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this._databaseLabel.Location = new System.Drawing.Point(17, 44);
            this._databaseLabel.Name = "_databaseLabel";
            this._databaseLabel.Size = new System.Drawing.Size(96, 18);
            this._databaseLabel.TabIndex = 22;
            this._databaseLabel.Text = "Database";
            this._databaseLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DBPasswordTextBox
            // 
            this._dbPasswordTextBox.AccessibleName = "First Name";
            this._dbPasswordTextBox.Location = new System.Drawing.Point(151, 100);
            this._dbPasswordTextBox.Name = "_dbPasswordTextBox";
            this._dbPasswordTextBox.Size = new System.Drawing.Size(128, 20);
            this._dbPasswordTextBox.TabIndex = 3;
            // 
            // DBPasswordLabel
            // 
            this._dbPasswordLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this._dbPasswordLabel.Location = new System.Drawing.Point(17, 100);
            this._dbPasswordLabel.Name = "_dbPasswordLabel";
            this._dbPasswordLabel.Size = new System.Drawing.Size(96, 18);
            this._dbPasswordLabel.TabIndex = 24;
            this._dbPasswordLabel.Text = "Password";
            this._dbPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BaseDatabaseConfigForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(296, 174);
            this.Controls.Add(this._dbPasswordTextBox);
            this.Controls.Add(this._dbPasswordLabel);
            this.Controls.Add(this._databaseTextBox);
            this.Controls.Add(this._databaseLabel);
            this.Controls.Add(this._dbUserTextBox);
            this.Controls.Add(this._dbUserLabel);
            this.Controls.Add(this._dbServerTextBox);
            this.Controls.Add(this._dbServerLabel);
            this.Name = "BaseDatabaseConfigForm";
            this.Controls.SetChildIndex(this.ButtonOk, 0);
            this.Controls.SetChildIndex(this.ButtonCancel, 0);
            this.Controls.SetChildIndex(this._dbServerLabel, 0);
            this.Controls.SetChildIndex(this._dbServerTextBox, 0);
            this.Controls.SetChildIndex(this._dbUserLabel, 0);
            this.Controls.SetChildIndex(this._dbUserTextBox, 0);
            this.Controls.SetChildIndex(this._databaseLabel, 0);
            this.Controls.SetChildIndex(this._databaseTextBox, 0);
            this.Controls.SetChildIndex(this._dbPasswordLabel, 0);
            this.Controls.SetChildIndex(this._dbPasswordTextBox, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void DBUserTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (_dbServerTextBox.Text != _currentDbServer)
                ApplicationSettings.SetStringValue("SQLServer", _dbServerTextBox.Text);
            if (_databaseTextBox.Text != _currentDatabase)
                ApplicationSettings.SetStringValue("SQLDatabase", _databaseTextBox.Text);
            if (_dbUserTextBox.Text != _currentUser)
                ApplicationSettings.SetStringValue("SQLUsername", _dbUserTextBox.Text);
            if (_dbPasswordTextBox.Text != _currentPassword)
                ApplicationSettings.SetStringValue("SQLPassword", _dbPasswordTextBox.Text);

            ApplicationSettings.Flush();

            Close();
        }
    }
}

