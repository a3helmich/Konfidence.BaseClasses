// Enter your email address and you will recieve your settings file shortly.

using System.ComponentModel;
using System.Windows.Forms;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.BaseWindowForms
{
    /// <summary>
    /// Summary description for BaseEmailAddressForm.
    /// </summary>
    public class BaseEmailAddressForm : Form
    {
        private TextBox _emailTextBox;
        private Button _cancelButton;
        private Button _buttonOk;
        private Label _emailLabel;
        private Label _labelDescription;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly Container _components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseEmailAddressForm));
            this._emailTextBox = new System.Windows.Forms.TextBox();
            this._cancelButton = new System.Windows.Forms.Button();
            this._buttonOk = new System.Windows.Forms.Button();
            this._emailLabel = new System.Windows.Forms.Label();
            this._labelDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _EmailTextBox
            // 
            resources.ApplyResources(this._emailTextBox, "_emailTextBox");
            this._emailTextBox.Name = "_emailTextBox";
            // 
            // _CancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this._cancelButton, "_cancelButton");
            this._cancelButton.Name = "_cancelButton";
            // 
            // _ButtonOk
            // 
            this._buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this._buttonOk, "_buttonOk");
            this._buttonOk.Name = "_buttonOk";
            // 
            // _EmailLabel
            // 
            resources.ApplyResources(this._emailLabel, "_emailLabel");
            this._emailLabel.Name = "_emailLabel";
            // 
            // labelDescription
            // 
            resources.ApplyResources(this._labelDescription, "_labelDescription");
            this._labelDescription.Name = "_labelDescription";
            // 
            // BaseEmailAddressForm
            // 
            this.AcceptButton = this._buttonOk;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this._labelDescription);
            this.Controls.Add(this._emailTextBox);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._buttonOk);
            this.Controls.Add(this._emailLabel);
            this.Name = "BaseEmailAddressForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        [UsedImplicitly]
        public string Email => _emailTextBox.Text;
    }
}
