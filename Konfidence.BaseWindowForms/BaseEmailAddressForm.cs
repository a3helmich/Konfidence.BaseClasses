// Enter your email address and you will recieve your settings file shortly.

using System.ComponentModel;
using System.Windows.Forms;

namespace Konfidence.BaseWindowForms
{
    /// <summary>
    /// Summary description for BaseEmailAddressForm.
    /// </summary>
    public class BaseEmailAddressForm : Form
    {
        private TextBox _EmailTextBox;
        private Button _CancelButton;
        private Button _ButtonOk;
        private Label _EmailLabel;
        private Label _LabelDescription;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly Container _Components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseEmailAddressForm));
            this._EmailTextBox = new System.Windows.Forms.TextBox();
            this._CancelButton = new System.Windows.Forms.Button();
            this._ButtonOk = new System.Windows.Forms.Button();
            this._EmailLabel = new System.Windows.Forms.Label();
            this._LabelDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _EmailTextBox
            // 
            resources.ApplyResources(this._EmailTextBox, "_EmailTextBox");
            this._EmailTextBox.Name = "_EmailTextBox";
            // 
            // _CancelButton
            // 
            this._CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this._CancelButton, "_CancelButton");
            this._CancelButton.Name = "_CancelButton";
            // 
            // _ButtonOk
            // 
            this._ButtonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this._ButtonOk, "_ButtonOk");
            this._ButtonOk.Name = "_ButtonOk";
            // 
            // _EmailLabel
            // 
            resources.ApplyResources(this._EmailLabel, "_EmailLabel");
            this._EmailLabel.Name = "_EmailLabel";
            // 
            // labelDescription
            // 
            resources.ApplyResources(this._LabelDescription, "_LabelDescription");
            this._LabelDescription.Name = "_LabelDescription";
            // 
            // BaseEmailAddressForm
            // 
            this.AcceptButton = this._ButtonOk;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this._LabelDescription);
            this.Controls.Add(this._EmailTextBox);
            this.Controls.Add(this._CancelButton);
            this.Controls.Add(this._ButtonOk);
            this.Controls.Add(this._EmailLabel);
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
                return _EmailTextBox.Text;
            }
        }
    }
}
