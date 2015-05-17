using System;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Windows.Forms;
using Konfidence.UtilHelper;

namespace Konfidence.BaseWindowForms
{

    public class BaseRegistrationForm : Form
    {
        private readonly string _EmptyString;

        private readonly IApplicationSettings _ApplicationSettings = ApplicationSettingsFactory.ApplicationSettings(Application.ProductName);

        private Label _EmailLabel;
        private Label CityLabel;
        private Label ZipcodeLabel;
        private Label Address1Label;
        private Label Address2Label;
        private Label StateLabel;
        private Label CountryLabel;
        private Label CompanyLabel;
        private Label PhoneLabel;
        private Label FirstNameLabel;
        private Label LastNameLabel;
        private Label _ApplicationLabel;
        private TextBox _LastNameTextBox;
        private TextBox _CompanyTextBox;
        private TextBox _EmailTextBox;
        private TextBox _PhoneTextBox;
        private TextBox _Address1TextBox;
        private TextBox _Address2TextBox;
        private TextBox _ZipTextBox;
        private TextBox _CityTextBox;
        private TextBox _StateTextBox;
        private TextBox _CountryTextBox;
        private TextBox _FirstNameTextBox;
        private Button _ButtonOk;
        private ErrorProvider _ErrorProvider;
        private TextBox _FirstErrorTextBox;
        private TextBox _SerialTextBox;
        private Label SerialLabel;
        private Button _ButtonCancel;
        private Button buttonReRegister;
        private Label _FirstNameRequiredLabel;
        private Label _LastNameRequiredLabel;
        private Label _EMailRequiredLabel;
        private Label _Address1RequiredLabel;
        private Label _ZipRequiredLabel;
        private Label _CityRequiredLabel;
        private Label _CountryRequiredLabel;
        private IContainer components;

        public BaseRegistrationForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            var resources = new ResourceManager(typeof(BaseRegistrationForm));

            _EmptyString = resources.GetString("EmptyString.Text");
        }

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseRegistrationForm));
            this._EmailLabel = new System.Windows.Forms.Label();
            this.CityLabel = new System.Windows.Forms.Label();
            this.ZipcodeLabel = new System.Windows.Forms.Label();
            this.Address1Label = new System.Windows.Forms.Label();
            this.FirstNameLabel = new System.Windows.Forms.Label();
            this.StateLabel = new System.Windows.Forms.Label();
            this.CountryLabel = new System.Windows.Forms.Label();
            this.Address2Label = new System.Windows.Forms.Label();
            this.CompanyLabel = new System.Windows.Forms.Label();
            this.PhoneLabel = new System.Windows.Forms.Label();
            this.LastNameLabel = new System.Windows.Forms.Label();
            this._ApplicationLabel = new System.Windows.Forms.Label();
            this._LastNameTextBox = new System.Windows.Forms.TextBox();
            this._CompanyTextBox = new System.Windows.Forms.TextBox();
            this._EmailTextBox = new System.Windows.Forms.TextBox();
            this._PhoneTextBox = new System.Windows.Forms.TextBox();
            this._Address1TextBox = new System.Windows.Forms.TextBox();
            this._Address2TextBox = new System.Windows.Forms.TextBox();
            this._ZipTextBox = new System.Windows.Forms.TextBox();
            this._CityTextBox = new System.Windows.Forms.TextBox();
            this._StateTextBox = new System.Windows.Forms.TextBox();
            this._CountryTextBox = new System.Windows.Forms.TextBox();
            this._FirstNameTextBox = new System.Windows.Forms.TextBox();
            this._ButtonOk = new System.Windows.Forms.Button();
            this._ErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this._ButtonCancel = new System.Windows.Forms.Button();
            this._FirstNameRequiredLabel = new System.Windows.Forms.Label();
            this._LastNameRequiredLabel = new System.Windows.Forms.Label();
            this._EMailRequiredLabel = new System.Windows.Forms.Label();
            this._Address1RequiredLabel = new System.Windows.Forms.Label();
            this._ZipRequiredLabel = new System.Windows.Forms.Label();
            this._CityRequiredLabel = new System.Windows.Forms.Label();
            this._CountryRequiredLabel = new System.Windows.Forms.Label();
            this._FirstErrorTextBox = new System.Windows.Forms.TextBox();
            this._SerialTextBox = new System.Windows.Forms.TextBox();
            this.SerialLabel = new System.Windows.Forms.Label();
            this.buttonReRegister = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._ErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // EmailLabel
            // 
            resources.ApplyResources(this._EmailLabel, "_EmailLabel");
            this._EmailLabel.Name = "_EmailLabel";
            // 
            // CityLabel
            // 
            resources.ApplyResources(this.CityLabel, "CityLabel");
            this.CityLabel.Name = "CityLabel";
            // 
            // ZipcodeLabel
            // 
            resources.ApplyResources(this.ZipcodeLabel, "ZipcodeLabel");
            this.ZipcodeLabel.Name = "ZipcodeLabel";
            // 
            // Address1Label
            // 
            resources.ApplyResources(this.Address1Label, "Address1Label");
            this.Address1Label.Name = "Address1Label";
            // 
            // FirstNameLabel
            // 
            resources.ApplyResources(this.FirstNameLabel, "FirstNameLabel");
            this.FirstNameLabel.Name = "FirstNameLabel";
            // 
            // StateLabel
            // 
            resources.ApplyResources(this.StateLabel, "StateLabel");
            this.StateLabel.Name = "StateLabel";
            // 
            // CountryLabel
            // 
            resources.ApplyResources(this.CountryLabel, "CountryLabel");
            this.CountryLabel.Name = "CountryLabel";
            // 
            // Address2Label
            // 
            resources.ApplyResources(this.Address2Label, "Address2Label");
            this.Address2Label.Name = "Address2Label";
            // 
            // CompanyLabel
            // 
            resources.ApplyResources(this.CompanyLabel, "CompanyLabel");
            this.CompanyLabel.Name = "CompanyLabel";
            // 
            // PhoneLabel
            // 
            resources.ApplyResources(this.PhoneLabel, "PhoneLabel");
            this.PhoneLabel.Name = "PhoneLabel";
            // 
            // LastNameLabel
            // 
            resources.ApplyResources(this.LastNameLabel, "LastNameLabel");
            this.LastNameLabel.Name = "LastNameLabel";
            // 
            // ApplicationLabel
            // 
            resources.ApplyResources(this._ApplicationLabel, "_ApplicationLabel");
            this._ApplicationLabel.Name = "_ApplicationLabel";
            // 
            // LastNameTextBox
            // 
            resources.ApplyResources(this._LastNameTextBox, "_LastNameTextBox");
            this._LastNameTextBox.Name = "_LastNameTextBox";
            // 
            // CompanyTextBox
            // 
            resources.ApplyResources(this._CompanyTextBox, "_CompanyTextBox");
            this._CompanyTextBox.Name = "_CompanyTextBox";
            // 
            // EmailTextBox
            // 
            resources.ApplyResources(this._EmailTextBox, "_EmailTextBox");
            this._EmailTextBox.Name = "_EmailTextBox";
            // 
            // PhoneTextBox
            // 
            resources.ApplyResources(this._PhoneTextBox, "_PhoneTextBox");
            this._PhoneTextBox.Name = "_PhoneTextBox";
            // 
            // Address1TextBox
            // 
            resources.ApplyResources(this._Address1TextBox, "_Address1TextBox");
            this._Address1TextBox.Name = "_Address1TextBox";
            // 
            // Address2TextBox
            // 
            resources.ApplyResources(this._Address2TextBox, "_Address2TextBox");
            this._Address2TextBox.Name = "_Address2TextBox";
            // 
            // ZipTextBox
            // 
            resources.ApplyResources(this._ZipTextBox, "_ZipTextBox");
            this._ZipTextBox.Name = "_ZipTextBox";
            // 
            // CityTextBox
            // 
            resources.ApplyResources(this._CityTextBox, "_CityTextBox");
            this._CityTextBox.Name = "_CityTextBox";
            // 
            // StateTextBox
            // 
            resources.ApplyResources(this._StateTextBox, "_StateTextBox");
            this._StateTextBox.Name = "_StateTextBox";
            // 
            // CountryTextBox
            // 
            resources.ApplyResources(this._CountryTextBox, "_CountryTextBox");
            this._CountryTextBox.Name = "_CountryTextBox";
            // 
            // FirstNameTextBox
            // 
            resources.ApplyResources(this._FirstNameTextBox, "_FirstNameTextBox");
            this._FirstNameTextBox.Name = "_FirstNameTextBox";
            // 
            // buttonOK
            // 
            resources.ApplyResources(this._ButtonOk, "_ButtonOk");
            this._ButtonOk.Name = "_ButtonOk";
            this._ButtonOk.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // errorProvider
            // 
            this._ErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this._ErrorProvider.ContainerControl = this;
            resources.ApplyResources(this._ErrorProvider, "_ErrorProvider");
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this._ButtonCancel, "_ButtonCancel");
            this._ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._ButtonCancel.Name = "_ButtonCancel";
            this._ButtonCancel.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // FirstNameRequiredLabel
            // 
            resources.ApplyResources(this._FirstNameRequiredLabel, "_FirstNameRequiredLabel");
            this._FirstNameRequiredLabel.Name = "_FirstNameRequiredLabel";
            // 
            // LastNameRequiredLabel
            // 
            resources.ApplyResources(this._LastNameRequiredLabel, "_LastNameRequiredLabel");
            this._LastNameRequiredLabel.Name = "_LastNameRequiredLabel";
            // 
            // EMailRequiredLabel
            // 
            resources.ApplyResources(this._EMailRequiredLabel, "_EMailRequiredLabel");
            this._EMailRequiredLabel.Name = "_EMailRequiredLabel";
            // 
            // Address1RequiredLabel
            // 
            resources.ApplyResources(this._Address1RequiredLabel, "_Address1RequiredLabel");
            this._Address1RequiredLabel.Name = "_Address1RequiredLabel";
            // 
            // ZipRequiredLabel
            // 
            resources.ApplyResources(this._ZipRequiredLabel, "_ZipRequiredLabel");
            this._ZipRequiredLabel.Name = "_ZipRequiredLabel";
            // 
            // CityRequiredLabel
            // 
            resources.ApplyResources(this._CityRequiredLabel, "_CityRequiredLabel");
            this._CityRequiredLabel.Name = "_CityRequiredLabel";
            // 
            // CountryRequiredLabel
            // 
            resources.ApplyResources(this._CountryRequiredLabel, "_CountryRequiredLabel");
            this._CountryRequiredLabel.Name = "_CountryRequiredLabel";
            // 
            // FirstErrorTextBox
            // 
            this._FirstErrorTextBox.AcceptsReturn = true;
            resources.ApplyResources(this._FirstErrorTextBox, "_FirstErrorTextBox");
            this._FirstErrorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._FirstErrorTextBox.Name = "_FirstErrorTextBox";
            this._FirstErrorTextBox.ReadOnly = true;
            // 
            // SerialTextBox
            // 
            this._SerialTextBox.AcceptsReturn = true;
            resources.ApplyResources(this._SerialTextBox, "_SerialTextBox");
            this._SerialTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._SerialTextBox.Name = "_SerialTextBox";
            this._SerialTextBox.ReadOnly = true;
            // 
            // SerialLabel
            // 
            resources.ApplyResources(this.SerialLabel, "SerialLabel");
            this.SerialLabel.Name = "SerialLabel";
            // 
            // buttonReRegister
            // 
            resources.ApplyResources(this.buttonReRegister, "buttonReRegister");
            this.buttonReRegister.Name = "buttonReRegister";
            this.buttonReRegister.Click += new System.EventHandler(this.ReRegisterbutton_Click);
            // 
            // BaseRegistrationForm
            // 
            this.AcceptButton = this._ButtonOk;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this._ButtonCancel;
            this.Controls.Add(this.buttonReRegister);
            this.Controls.Add(this.SerialLabel);
            this.Controls.Add(this._SerialTextBox);
            this.Controls.Add(this._FirstErrorTextBox);
            this.Controls.Add(this._LastNameTextBox);
            this.Controls.Add(this._CompanyTextBox);
            this.Controls.Add(this._EmailTextBox);
            this.Controls.Add(this._Address1TextBox);
            this.Controls.Add(this._Address2TextBox);
            this.Controls.Add(this._ZipTextBox);
            this.Controls.Add(this._CityTextBox);
            this.Controls.Add(this._StateTextBox);
            this.Controls.Add(this._CountryTextBox);
            this.Controls.Add(this._FirstNameTextBox);
            this.Controls.Add(this._PhoneTextBox);
            this.Controls.Add(this._CountryRequiredLabel);
            this.Controls.Add(this._CityRequiredLabel);
            this.Controls.Add(this._ZipRequiredLabel);
            this.Controls.Add(this._Address1RequiredLabel);
            this.Controls.Add(this._EMailRequiredLabel);
            this.Controls.Add(this._LastNameRequiredLabel);
            this.Controls.Add(this._FirstNameRequiredLabel);
            this.Controls.Add(this._ButtonCancel);
            this.Controls.Add(this._ButtonOk);
            this.Controls.Add(this._ApplicationLabel);
            this.Controls.Add(this.LastNameLabel);
            this.Controls.Add(this.PhoneLabel);
            this.Controls.Add(this.CompanyLabel);
            this.Controls.Add(this.CountryLabel);
            this.Controls.Add(this.StateLabel);
            this.Controls.Add(this.ZipcodeLabel);
            this.Controls.Add(this.CityLabel);
            this.Controls.Add(this.Address2Label);
            this.Controls.Add(this.FirstNameLabel);
            this.Controls.Add(this.Address1Label);
            this.Controls.Add(this._EmailLabel);
            this.Name = "BaseRegistrationForm";
            this.Load += new System.EventHandler(this.BaseRegistrationForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this._ErrorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private void SetFieldStatus(bool fieldStatus)
        {
            SerialLabel.Visible = fieldStatus;
            _EmailTextBox.ReadOnly = fieldStatus;
            _ButtonOk.Enabled = !fieldStatus;
            if (_ButtonOk.Enabled)
            {
                AcceptButton = _ButtonOk;
            }
            else
            {
                AcceptButton = buttonReRegister;
            }
        }

        private void BaseRegistrationForm_Load(object sender, EventArgs e)
        {
            _ApplicationLabel.Text = _ApplicationLabel.Text.ToUpper(CultureInfo.CurrentCulture);

            _FirstNameTextBox.Text = _ApplicationSettings.GetStringValue("FirstNameTextBox.Text");
            _LastNameTextBox.Text = _ApplicationSettings.GetStringValue("LastNameTextBox.Text");
            _CompanyTextBox.Text = _ApplicationSettings.GetStringValue("CompanyTextBox.Text");
            _EmailTextBox.Text = _ApplicationSettings.GetStringValue("EmailTextBox.Text");
            _PhoneTextBox.Text = _ApplicationSettings.GetStringValue("PhoneTextBox.Text");
            _Address1TextBox.Text = _ApplicationSettings.GetStringValue("Address1TextBox.Text");
            _Address2TextBox.Text = _ApplicationSettings.GetStringValue("Address2TextBox.Text");
            _ZipTextBox.Text = _ApplicationSettings.GetStringValue("ZipTextBox.Text");
            _CityTextBox.Text = _ApplicationSettings.GetStringValue("CityTextBox.Text");
            _StateTextBox.Text = _ApplicationSettings.GetStringValue("StateTextBox.Text");
            _CountryTextBox.Text = _ApplicationSettings.GetStringValue("CountryTextBox.Text");
            _SerialTextBox.Text = _ApplicationSettings.GetStringValue("Serial.Text");

            if (_SerialTextBox.Text.Length > 0)
                SetFieldStatus(true);
            else
                SetFieldStatus(false);
        }

        private bool ValidateTextBox(TextBox textBox, bool isValid)
        {
            const string requiredInformation = "This is required information for registration";
            string noError = string.Empty;

            if (textBox.Text.Length == 0)
            {
                _ErrorProvider.SetError(textBox, requiredInformation);
                if (isValid)
                    isValid = false;

                if (_FirstErrorTextBox.Text.Length == 0)
                    _FirstErrorTextBox.Text = textBox.AccessibleName + ": " + requiredInformation;
            }
            else
            {
                _ErrorProvider.SetError(textBox, noError);
            }

            return isValid;
        }

        private bool ValidateEmail(TextBox textBox, bool isValid)
        {
            const string requiredEmail = "Email address does not have a valid format";

            if (textBox.Text.Length > 0)
            {
                if (!BaseUtilHelper.IsValidEmail(textBox.Text))
                {
                    _ErrorProvider.SetError(textBox, requiredEmail);
                    isValid = false;

                    if (_FirstErrorTextBox.Text.Length == 0)
                        _FirstErrorTextBox.Text = textBox.AccessibleName + ": " + requiredEmail;
                }
            }

            return isValid;
        }

        private bool DoValidation()
        {
            _FirstErrorTextBox.Text = _EmptyString;

            bool valid = ValidateTextBox(_FirstNameTextBox, true);
            valid = ValidateTextBox(_LastNameTextBox, valid);
            valid = ValidateTextBox(_EmailTextBox, valid);

            valid = ValidateEmail(_EmailTextBox, valid);

            valid = ValidateTextBox(_Address1TextBox, valid);
            valid = ValidateTextBox(_CountryTextBox, valid);
            valid = ValidateTextBox(_CityTextBox, valid);
            valid = ValidateTextBox(_ZipTextBox, valid);

            return valid;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (DoValidation())
            {
                DialogResult = DialogResult.OK;

                _ApplicationSettings.SetStringValue("FirstNameTextBox.Text", _FirstNameTextBox.Text);
                _ApplicationSettings.SetStringValue("LastNameTextBox.Text", _LastNameTextBox.Text);
                _ApplicationSettings.SetStringValue("CompanyTextBox.Text", _CompanyTextBox.Text);
                _ApplicationSettings.SetStringValue("EmailTextBox.Text", _EmailTextBox.Text);
                _ApplicationSettings.SetStringValue("PhoneTextBox.Text", _PhoneTextBox.Text);
                _ApplicationSettings.SetStringValue("Address1TextBox.Text", _Address1TextBox.Text);
                _ApplicationSettings.SetStringValue("Address2TextBox.Text", _Address2TextBox.Text);
                _ApplicationSettings.SetStringValue("ZipTextBox.Text", _ZipTextBox.Text);
                _ApplicationSettings.SetStringValue("CityTextBox.Text", _CityTextBox.Text);
                _ApplicationSettings.SetStringValue("StateTextBox.Text", _StateTextBox.Text);
                _ApplicationSettings.SetStringValue("CountryTextBox.Text", _CountryTextBox.Text);

                _ApplicationSettings.Flush();

                Close();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }

        private void ReRegisterbutton_Click(object sender, EventArgs e)
        {
            // MessageBox.Show("test");
        }

        public string Address1
        {
            get { return _Address1TextBox.Text; }
        }
        public string Address2
        {
            get { return _Address2TextBox.Text; }
        }
        public string LastName
        {
            get { return _LastNameTextBox.Text; }
        }
        public string FirstName
        {
            get { return _FirstNameTextBox.Text; }
        }
        public string Company
        {
            get { return _CompanyTextBox.Text; }
        }
        public string Email
        {
            get { return _EmailTextBox.Text; }
        }
        public string Phone
        {
            get { return _PhoneTextBox.Text; }
        }
        public string Zip
        {
            get { return _ZipTextBox.Text; }
        }
        public string City
        {
            get { return _CityTextBox.Text; }
        }
        public string State
        {
            get { return _StateTextBox.Text; }
        }
        public string Country
        {
            get { return _CountryTextBox.Text; }
        }

    }
}
