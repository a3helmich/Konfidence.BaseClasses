using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Resources;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Konfidence.BaseWindowForms
{
    /// <summary>
    /// Summary description for WamAboutForm.
    /// </summary>
    public class BaseAboutForm : Form
    {
        protected TextBox CopyrightTextBox;
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

        private LinkLabel _KonfidenceLinkLabel;
        private Button _ButtonOk;
        protected TextBox TextBoxRegistrationCode;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly Container _Components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseAboutForm));
            this.CopyrightTextBox = new System.Windows.Forms.TextBox();
            this._ButtonOk = new System.Windows.Forms.Button();
            this._KonfidenceLinkLabel = new System.Windows.Forms.LinkLabel();
            this.TextBoxRegistrationCode = new System.Windows.Forms.TextBox();
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
            resources.ApplyResources(this._ButtonOk, "_ButtonOk");
            this._ButtonOk.Name = "_ButtonOk";
            this._ButtonOk.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // konfidenceLinkLabel
            // 
            resources.ApplyResources(this._KonfidenceLinkLabel, "_KonfidenceLinkLabel");
            this._KonfidenceLinkLabel.Name = "_KonfidenceLinkLabel";
            this._KonfidenceLinkLabel.TabStop = true;
            this._KonfidenceLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.konfidenceLinkLabel_LinkClicked);
            // 
            // textBoxRegistrationCode
            // 
            this.TextBoxRegistrationCode.AcceptsReturn = true;
            resources.ApplyResources(this.TextBoxRegistrationCode, "TextBoxRegistrationCode");
            this.TextBoxRegistrationCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBoxRegistrationCode.Name = "TextBoxRegistrationCode";
            this.TextBoxRegistrationCode.ReadOnly = true;
            // 
            // BaseAboutForm
            // 
            this.AcceptButton = this._ButtonOk;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.TextBoxRegistrationCode);
            this.Controls.Add(this._KonfidenceLinkLabel);
            this.Controls.Add(this.CopyrightTextBox);
            this.Controls.Add(this._ButtonOk);
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
                var resources = new ResourceManager(typeof(BaseAboutForm));
                var warningMessage = resources.GetString("BaseAboutFormException.WarningMessage");

                if (classType == null)
                {
                    throw new BaseAboutFormException(resources.GetString("BaseAboutFormException.NullWarningMessage") + warningMessage);
                }

                throw new BaseAboutFormException(classType + warningMessage);
            }

            return true;
        }

        private void konfidenceLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(_KonfidenceLinkLabel.Text);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

    [Serializable]
    public class BaseAboutFormException : Exception
    {
        public BaseAboutFormException() : base(new ResourceManager(typeof(BaseAboutForm)).GetString("BaseAboutFormException.DontKnowWarningMessage")) { }
        public BaseAboutFormException(string message) : base(message) { }
        public BaseAboutFormException(string message, Exception exception) : base(message, exception) { }
        protected BaseAboutFormException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }
    }
}
