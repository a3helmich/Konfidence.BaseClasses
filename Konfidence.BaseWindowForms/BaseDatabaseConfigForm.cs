namespace Konfidence.BaseWindowForms
{
    public class BaseDatabaseConfigForm : BaseDialogForm
    {
        private System.Windows.Forms.TextBox _DbServerTextBox;
        private System.Windows.Forms.Label _DbServerLabel;
        private System.Windows.Forms.TextBox _DbUserTextBox;
        private System.Windows.Forms.Label _DbUserLabel;
        private readonly System.ComponentModel.IContainer components = null;

        private readonly string _CurrentDbServer;
        private readonly string _CurrentDatabase;
        private readonly string _CurrentUser;
        private System.Windows.Forms.Label _DatabaseLabel;
        private System.Windows.Forms.TextBox _DatabaseTextBox;
        private System.Windows.Forms.TextBox _DbPasswordTextBox;
        private System.Windows.Forms.Label _DbPasswordLabel;
        private readonly string _CurrentPassword;


        public BaseDatabaseConfigForm()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
            _CurrentDbServer = ApplicationSettings.GetStringValue("SQLServer");
            _CurrentDatabase = ApplicationSettings.GetStringValue("SQLDatabase");
            _CurrentUser = ApplicationSettings.GetStringValue("SQLUsername");
            _CurrentPassword = ApplicationSettings.GetStringValue("SQLPassword");

            _DbServerTextBox.Text = _CurrentDbServer;
            _DatabaseTextBox.Text = _CurrentDatabase;
            _DbUserTextBox.Text = _CurrentUser;
            _DbPasswordTextBox.Text = _CurrentPassword;
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

        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._DbServerTextBox = new System.Windows.Forms.TextBox();
            this._DbServerLabel = new System.Windows.Forms.Label();
            this._DbUserTextBox = new System.Windows.Forms.TextBox();
            this._DbUserLabel = new System.Windows.Forms.Label();
            this._DatabaseTextBox = new System.Windows.Forms.TextBox();
            this._DatabaseLabel = new System.Windows.Forms.Label();
            this._DbPasswordTextBox = new System.Windows.Forms.TextBox();
            this._DbPasswordLabel = new System.Windows.Forms.Label();
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
            this._DbServerTextBox.AccessibleName = "First Name";
            this._DbServerTextBox.Location = new System.Drawing.Point(151, 16);
            this._DbServerTextBox.Name = "_DbServerTextBox";
            this._DbServerTextBox.Size = new System.Drawing.Size(128, 20);
            this._DbServerTextBox.TabIndex = 0;
            // 
            // DBServerLabel
            // 
            this._DbServerLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this._DbServerLabel.Location = new System.Drawing.Point(17, 17);
            this._DbServerLabel.Name = "_DbServerLabel";
            this._DbServerLabel.Size = new System.Drawing.Size(96, 18);
            this._DbServerLabel.TabIndex = 18;
            this._DbServerLabel.Text = "Database server";
            this._DbServerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DBUserTextBox
            // 
            this._DbUserTextBox.AccessibleName = "First Name";
            this._DbUserTextBox.Location = new System.Drawing.Point(151, 72);
            this._DbUserTextBox.Name = "_DbUserTextBox";
            this._DbUserTextBox.Size = new System.Drawing.Size(128, 20);
            this._DbUserTextBox.TabIndex = 2;
            this._DbUserTextBox.TextChanged += new System.EventHandler(this.DBUserTextBox_TextChanged);
            // 
            // DBUserLabel
            // 
            this._DbUserLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this._DbUserLabel.Location = new System.Drawing.Point(17, 72);
            this._DbUserLabel.Name = "_DbUserLabel";
            this._DbUserLabel.Size = new System.Drawing.Size(96, 18);
            this._DbUserLabel.TabIndex = 20;
            this._DbUserLabel.Text = "Username";
            this._DbUserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DatabaseTextBox
            // 
            this._DatabaseTextBox.AccessibleName = "First Name";
            this._DatabaseTextBox.Location = new System.Drawing.Point(151, 44);
            this._DatabaseTextBox.Name = "_DatabaseTextBox";
            this._DatabaseTextBox.Size = new System.Drawing.Size(128, 20);
            this._DatabaseTextBox.TabIndex = 1;
            // 
            // DatabaseLabel
            // 
            this._DatabaseLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this._DatabaseLabel.Location = new System.Drawing.Point(17, 44);
            this._DatabaseLabel.Name = "_DatabaseLabel";
            this._DatabaseLabel.Size = new System.Drawing.Size(96, 18);
            this._DatabaseLabel.TabIndex = 22;
            this._DatabaseLabel.Text = "Database";
            this._DatabaseLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DBPasswordTextBox
            // 
            this._DbPasswordTextBox.AccessibleName = "First Name";
            this._DbPasswordTextBox.Location = new System.Drawing.Point(151, 100);
            this._DbPasswordTextBox.Name = "_DbPasswordTextBox";
            this._DbPasswordTextBox.Size = new System.Drawing.Size(128, 20);
            this._DbPasswordTextBox.TabIndex = 3;
            // 
            // DBPasswordLabel
            // 
            this._DbPasswordLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this._DbPasswordLabel.Location = new System.Drawing.Point(17, 100);
            this._DbPasswordLabel.Name = "_DbPasswordLabel";
            this._DbPasswordLabel.Size = new System.Drawing.Size(96, 18);
            this._DbPasswordLabel.TabIndex = 24;
            this._DbPasswordLabel.Text = "Password";
            this._DbPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BaseDatabaseConfigForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(296, 174);
            this.Controls.Add(this._DbPasswordTextBox);
            this.Controls.Add(this._DbPasswordLabel);
            this.Controls.Add(this._DatabaseTextBox);
            this.Controls.Add(this._DatabaseLabel);
            this.Controls.Add(this._DbUserTextBox);
            this.Controls.Add(this._DbUserLabel);
            this.Controls.Add(this._DbServerTextBox);
            this.Controls.Add(this._DbServerLabel);
            this.Name = "BaseDatabaseConfigForm";
            this.Controls.SetChildIndex(this.ButtonOk, 0);
            this.Controls.SetChildIndex(this.ButtonCancel, 0);
            this.Controls.SetChildIndex(this._DbServerLabel, 0);
            this.Controls.SetChildIndex(this._DbServerTextBox, 0);
            this.Controls.SetChildIndex(this._DbUserLabel, 0);
            this.Controls.SetChildIndex(this._DbUserTextBox, 0);
            this.Controls.SetChildIndex(this._DatabaseLabel, 0);
            this.Controls.SetChildIndex(this._DatabaseTextBox, 0);
            this.Controls.SetChildIndex(this._DbPasswordLabel, 0);
            this.Controls.SetChildIndex(this._DbPasswordTextBox, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void DBUserTextBox_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            if (_DbServerTextBox.Text != _CurrentDbServer)
                ApplicationSettings.SetStringValue("SQLServer", _DbServerTextBox.Text);
            if (_DatabaseTextBox.Text != _CurrentDatabase)
                ApplicationSettings.SetStringValue("SQLDatabase", _DatabaseTextBox.Text);
            if (_DbUserTextBox.Text != _CurrentUser)
                ApplicationSettings.SetStringValue("SQLUsername", _DbUserTextBox.Text);
            if (_DbPasswordTextBox.Text != _CurrentPassword)
                ApplicationSettings.SetStringValue("SQLPassword", _DbPasswordTextBox.Text);

            ApplicationSettings.Flush();

            Close();
        }
    }
}

