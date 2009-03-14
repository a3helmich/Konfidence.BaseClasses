using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Konfidence.BaseWindowForms
{
    public class BaseDatabaseConfigForm : BaseWindowForms.BaseDialogForm
    {
        private System.Windows.Forms.TextBox DBServerTextBox;
        private System.Windows.Forms.Label DBServerLabel;
        private System.Windows.Forms.TextBox DBUserTextBox;
        private System.Windows.Forms.Label DBUserLabel;
        private System.ComponentModel.IContainer components = null;

        private string CurrentDBServer;
        private string CurrentDatabase;
        private string CurrentUser;
        private System.Windows.Forms.Label DatabaseLabel;
        private System.Windows.Forms.TextBox DatabaseTextBox;
        private System.Windows.Forms.TextBox DBPasswordTextBox;
        private System.Windows.Forms.Label DBPasswordLabel;
        private string CurrentPassword;


        public BaseDatabaseConfigForm()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
            CurrentDBServer = ApplicationSettings.GetStringValue("SQLServer");
            CurrentDatabase = ApplicationSettings.GetStringValue("SQLDatabase");
            CurrentUser = ApplicationSettings.GetStringValue("SQLUsername");
            CurrentPassword = ApplicationSettings.GetStringValue("SQLPassword");

            DBServerTextBox.Text = CurrentDBServer;
            DatabaseTextBox.Text = CurrentDatabase;
            DBUserTextBox.Text = CurrentUser;
            DBPasswordTextBox.Text = CurrentPassword;
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
            this.DBServerTextBox = new System.Windows.Forms.TextBox();
            this.DBServerLabel = new System.Windows.Forms.Label();
            this.DBUserTextBox = new System.Windows.Forms.TextBox();
            this.DBUserLabel = new System.Windows.Forms.Label();
            this.DatabaseTextBox = new System.Windows.Forms.TextBox();
            this.DatabaseLabel = new System.Windows.Forms.Label();
            this.DBPasswordTextBox = new System.Windows.Forms.TextBox();
            this.DBPasswordLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(204, 144);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(124, 144);
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // DBServerTextBox
            // 
            this.DBServerTextBox.AccessibleName = "First Name";
            this.DBServerTextBox.Location = new System.Drawing.Point(151, 16);
            this.DBServerTextBox.Name = "DBServerTextBox";
            this.DBServerTextBox.Size = new System.Drawing.Size(128, 20);
            this.DBServerTextBox.TabIndex = 0;
            // 
            // DBServerLabel
            // 
            this.DBServerLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.DBServerLabel.Location = new System.Drawing.Point(17, 17);
            this.DBServerLabel.Name = "DBServerLabel";
            this.DBServerLabel.Size = new System.Drawing.Size(96, 18);
            this.DBServerLabel.TabIndex = 18;
            this.DBServerLabel.Text = "Database server";
            this.DBServerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DBUserTextBox
            // 
            this.DBUserTextBox.AccessibleName = "First Name";
            this.DBUserTextBox.Location = new System.Drawing.Point(151, 72);
            this.DBUserTextBox.Name = "DBUserTextBox";
            this.DBUserTextBox.Size = new System.Drawing.Size(128, 20);
            this.DBUserTextBox.TabIndex = 2;
            this.DBUserTextBox.TextChanged += new System.EventHandler(this.DBUserTextBox_TextChanged);
            // 
            // DBUserLabel
            // 
            this.DBUserLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.DBUserLabel.Location = new System.Drawing.Point(17, 72);
            this.DBUserLabel.Name = "DBUserLabel";
            this.DBUserLabel.Size = new System.Drawing.Size(96, 18);
            this.DBUserLabel.TabIndex = 20;
            this.DBUserLabel.Text = "Username";
            this.DBUserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DatabaseTextBox
            // 
            this.DatabaseTextBox.AccessibleName = "First Name";
            this.DatabaseTextBox.Location = new System.Drawing.Point(151, 44);
            this.DatabaseTextBox.Name = "DatabaseTextBox";
            this.DatabaseTextBox.Size = new System.Drawing.Size(128, 20);
            this.DatabaseTextBox.TabIndex = 1;
            // 
            // DatabaseLabel
            // 
            this.DatabaseLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.DatabaseLabel.Location = new System.Drawing.Point(17, 44);
            this.DatabaseLabel.Name = "DatabaseLabel";
            this.DatabaseLabel.Size = new System.Drawing.Size(96, 18);
            this.DatabaseLabel.TabIndex = 22;
            this.DatabaseLabel.Text = "Database";
            this.DatabaseLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DBPasswordTextBox
            // 
            this.DBPasswordTextBox.AccessibleName = "First Name";
            this.DBPasswordTextBox.Location = new System.Drawing.Point(151, 100);
            this.DBPasswordTextBox.Name = "DBPasswordTextBox";
            this.DBPasswordTextBox.Size = new System.Drawing.Size(128, 20);
            this.DBPasswordTextBox.TabIndex = 3;
            // 
            // DBPasswordLabel
            // 
            this.DBPasswordLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.DBPasswordLabel.Location = new System.Drawing.Point(17, 100);
            this.DBPasswordLabel.Name = "DBPasswordLabel";
            this.DBPasswordLabel.Size = new System.Drawing.Size(96, 18);
            this.DBPasswordLabel.TabIndex = 24;
            this.DBPasswordLabel.Text = "Password";
            this.DBPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BaseDatabaseConfigForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(296, 174);
            this.Controls.Add(this.DBPasswordTextBox);
            this.Controls.Add(this.DBPasswordLabel);
            this.Controls.Add(this.DatabaseTextBox);
            this.Controls.Add(this.DatabaseLabel);
            this.Controls.Add(this.DBUserTextBox);
            this.Controls.Add(this.DBUserLabel);
            this.Controls.Add(this.DBServerTextBox);
            this.Controls.Add(this.DBServerLabel);
            this.Name = "BaseDatabaseConfigForm";
            this.Controls.SetChildIndex(this.buttonOK, 0);
            this.Controls.SetChildIndex(this.buttonCancel, 0);
            this.Controls.SetChildIndex(this.DBServerLabel, 0);
            this.Controls.SetChildIndex(this.DBServerTextBox, 0);
            this.Controls.SetChildIndex(this.DBUserLabel, 0);
            this.Controls.SetChildIndex(this.DBUserTextBox, 0);
            this.Controls.SetChildIndex(this.DatabaseLabel, 0);
            this.Controls.SetChildIndex(this.DatabaseTextBox, 0);
            this.Controls.SetChildIndex(this.DBPasswordLabel, 0);
            this.Controls.SetChildIndex(this.DBPasswordTextBox, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void DBUserTextBox_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            if (DBServerTextBox.Text != CurrentDBServer)
                ApplicationSettings.SetStringValue("SQLServer", DBServerTextBox.Text);
            if (DatabaseTextBox.Text != CurrentDatabase)
                ApplicationSettings.SetStringValue("SQLDatabase", DatabaseTextBox.Text);
            if (DBUserTextBox.Text != CurrentUser)
                ApplicationSettings.SetStringValue("SQLUsername", DBUserTextBox.Text);
            if (DBPasswordTextBox.Text != CurrentPassword)
                ApplicationSettings.SetStringValue("SQLPassword", DBPasswordTextBox.Text);

            ApplicationSettings.Flush();

            Close();
        }
    }
}

