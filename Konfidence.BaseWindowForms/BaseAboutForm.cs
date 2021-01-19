using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Resources;
using System.Runtime.Serialization;
using System.Windows.Forms;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.BaseWindowForms
{
    /// <summary>
    /// Summary description for WamAboutForm.
    /// </summary>
    public class BaseAboutForm : Form
    {
        protected TextBox CopyrightTextBox;

        [UsedImplicitly]
        public string CopyrightText
        {
            get => CopyrightTextBox.Text;
            set => CopyrightTextBox.Text = value;
        }

        private LinkLabel _konfidenceLinkLabel;
        private Button _buttonOk;
        protected TextBox TextBoxRegistrationCode;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly Container _components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseAboutForm));
            this.CopyrightTextBox = new System.Windows.Forms.TextBox();
            this._buttonOk = new System.Windows.Forms.Button();
            this._konfidenceLinkLabel = new System.Windows.Forms.LinkLabel();
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
            // _buttonOk
            // 
            resources.ApplyResources(this._buttonOk, "_buttonOk");
            this._buttonOk.Name = "_buttonOk";
            this._buttonOk.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // _konfidenceLinkLabel
            // 
            resources.ApplyResources(this._konfidenceLinkLabel, "_konfidenceLinkLabel");
            this._konfidenceLinkLabel.Name = "_konfidenceLinkLabel";
            this._konfidenceLinkLabel.TabStop = true;
            this._konfidenceLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.konfidenceLinkLabel_LinkClicked);
            // 
            // TextBoxRegistrationCode
            // 
            this.TextBoxRegistrationCode.AcceptsReturn = true;
            resources.ApplyResources(this.TextBoxRegistrationCode, "TextBoxRegistrationCode");
            this.TextBoxRegistrationCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBoxRegistrationCode.Name = "TextBoxRegistrationCode";
            this.TextBoxRegistrationCode.ReadOnly = true;
            // 
            // BaseAboutForm
            // 
            this.AcceptButton = this._buttonOk;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.TextBoxRegistrationCode);
            this.Controls.Add(this._konfidenceLinkLabel);
            this.Controls.Add(this.CopyrightTextBox);
            this.Controls.Add(this._buttonOk);
            this.MinimizeBox = false;
            this.Name = "BaseAboutForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        public static bool IsDerivedClass([NotNull] Type classType)
        {
            if (!classType.IsAssigned() || !classType.IsSubclassOf(typeof(BaseAboutForm)))
            {
                var resources = new ResourceManager(typeof(BaseAboutForm));
                var warningMessage = resources.GetString("BaseAboutFormException.WarningMessage");

                if (!classType.IsAssigned())
                {
                    throw new BaseAboutFormException(resources.GetString("BaseAboutFormException.NullWarningMessage") + warningMessage);
                }

                throw new BaseAboutFormException(classType + warningMessage);
            }

            return true;
        }

        private void konfidenceLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(_konfidenceLinkLabel.Text);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

    [Serializable]
    public class BaseAboutFormException : Exception
    {
        [UsedImplicitly]
        public BaseAboutFormException() : base(new ResourceManager(typeof(BaseAboutForm)).GetString("BaseAboutFormException.DontKnowWarningMessage")) { }
        public BaseAboutFormException(string message) : base(message) { }
        public BaseAboutFormException(string message, Exception exception) : base(message, exception) { }
        protected BaseAboutFormException([NotNull] SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }
    }
}
