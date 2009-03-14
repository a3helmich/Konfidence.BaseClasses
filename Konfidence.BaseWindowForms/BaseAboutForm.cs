using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;



namespace Konfidence.BaseWindowForms
{
    /// <summary>
    /// Summary description for WamAboutForm.
    /// </summary>
    public class BaseAboutForm : System.Windows.Forms.Form
    {
        protected System.Windows.Forms.TextBox CopyrightTextBox;
        #region CopyrightText
        public string CopyrightText
        {
            get
            {
                return CopyrightTextBox.Text;
            }
            set
            {
                CopyrightTextBox.Text = value;
            }
        }
        #endregion

        private System.Windows.Forms.LinkLabel konfidenceLinkLabel;
        private System.Windows.Forms.Button buttonOK;
        protected TextBox textBoxRegistrationCode;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public BaseAboutForm()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseAboutForm));
            this.CopyrightTextBox = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.konfidenceLinkLabel = new System.Windows.Forms.LinkLabel();
            this.textBoxRegistrationCode = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // CopyrightTextBox
            // 
            this.CopyrightTextBox.AcceptsReturn = true;
            resources.ApplyResources(this.CopyrightTextBox, "CopyrightTextBox");
            this.CopyrightTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CopyrightTextBox.Name = "CopyrightTextBox";
            this.CopyrightTextBox.ReadOnly = true;
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // konfidenceLinkLabel
            // 
            resources.ApplyResources(this.konfidenceLinkLabel, "konfidenceLinkLabel");
            this.konfidenceLinkLabel.Name = "konfidenceLinkLabel";
            this.konfidenceLinkLabel.TabStop = true;
            this.konfidenceLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.konfidenceLinkLabel_LinkClicked);
            // 
            // textBoxRegistrationCode
            // 
            this.textBoxRegistrationCode.AcceptsReturn = true;
            resources.ApplyResources(this.textBoxRegistrationCode, "textBoxRegistrationCode");
            this.textBoxRegistrationCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxRegistrationCode.Name = "textBoxRegistrationCode";
            this.textBoxRegistrationCode.ReadOnly = true;
            // 
            // BaseAboutForm
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.textBoxRegistrationCode);
            this.Controls.Add(this.konfidenceLinkLabel);
            this.Controls.Add(this.CopyrightTextBox);
            this.Controls.Add(this.buttonOK);
            this.MinimizeBox = false;
            this.Name = "BaseAboutForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        public static bool IsDerivedClass(Type classType)
        {
            if (classType == null || !classType.IsSubclassOf(typeof(BaseAboutForm)))
            {
                ResourceManager resources = new ResourceManager(typeof(BaseAboutForm));
                string WarningMessage = resources.GetString("BaseAboutFormException.WarningMessage");

                if (classType == null)
                    throw new BaseAboutFormException(resources.GetString("BaseAboutFormException.NullWarningMessage") + WarningMessage);
                else
                    throw new BaseAboutFormException(classType.ToString() + WarningMessage);
            }

            return true;
        }

        private void konfidenceLinkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(konfidenceLinkLabel.Text);
        }

        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }

    [SerializableAttribute]
    public class BaseAboutFormException : Exception
    {
        public BaseAboutFormException() : base(new ResourceManager(typeof(BaseAboutForm)).GetString("BaseAboutFormException.DontKnowWarningMessage")) { }
        public BaseAboutFormException(string message) : base(message) { }
        public BaseAboutFormException(string message, Exception exception) : base(message, exception) { }
        protected BaseAboutFormException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }
    }
}
