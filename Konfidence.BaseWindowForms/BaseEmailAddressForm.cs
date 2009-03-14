using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;


// Enter your email address and you will recieve your settings file shortly.

namespace Konfidence.BaseWindowForms
{
    /// <summary>
    /// Summary description for BaseEmailAddressForm.
    /// </summary>
    public class BaseEmailAddressForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.TextBox EmailTextBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label EmailLabel;
        private System.Windows.Forms.Label label1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public BaseEmailAddressForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseEmailAddressForm));
            this.EmailTextBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.EmailLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // EmailTextBox
            // 
            resources.ApplyResources(this.EmailTextBox, "EmailTextBox");
            this.EmailTextBox.Name = "EmailTextBox";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            // 
            // EmailLabel
            // 
            resources.ApplyResources(this.EmailLabel, "EmailLabel");
            this.EmailLabel.Name = "EmailLabel";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // BaseEmailAddressForm
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label1);
            this.Controls.Add(this.EmailTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.EmailLabel);
            this.Name = "BaseEmailAddressForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        public string Email
        {
            get
            {
                return EmailTextBox.Text;
            }
        }
    }
}
