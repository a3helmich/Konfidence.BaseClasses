using System.ComponentModel;
using System.Windows.Forms;
using Konfidence.UtilHelper;

namespace Konfidence.BaseWindowForms
{
    /// <summary>
    /// Summary description for BaseDialogForm.
    /// </summary>
    public class BaseDialogForm : Form
    {
        protected Button ButtonCancel;
        protected Button ButtonOk;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly Container _Components = null;

        private IApplicationSettings _ApplicationSettings;
        private string _ConfigurationName = string.Empty;

        public IApplicationSettings ApplicationSettings
        {
            get
            {
                if (_ApplicationSettings == null)
                    _ApplicationSettings = ApplicationSettingsFactory.ApplicationSettings(Application.ProductName);
                return _ApplicationSettings;
            }
        }

        public string ConfigurationName
        {
            get { return _ConfigurationName; }
            set { _ConfigurationName = value; }
        }

        public BaseDialogForm() : this(Application.ProductName)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public BaseDialogForm(string configurationName)
        {
            ConfigurationName = configurationName;
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
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ButtonOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ButtonCancel.Location = new System.Drawing.Point(153, 118);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 16;
            this.ButtonCancel.Text = "Cancel";
            // 
            // buttonOK
            // 
            this.ButtonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonOk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ButtonOk.Location = new System.Drawing.Point(73, 118);
            this.ButtonOk.Name = "ButtonOk";
            this.ButtonOk.Size = new System.Drawing.Size(75, 23);
            this.ButtonOk.TabIndex = 15;
            this.ButtonOk.Text = "OK";
            // 
            // BaseDialogForm
            // 
            this.AcceptButton = this.ButtonOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(296, 262);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOk);
            this.Name = "BaseDialogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BaseDialogForm";
            this.ResumeLayout(false);

        }
        #endregion
    }
}
