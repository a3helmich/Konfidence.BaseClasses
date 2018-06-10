using System;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Windows.Forms;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.UtilHelper;

namespace Konfidence.BaseWindowForms
{

    public class BaseRegistrationForm : Form
    {
        private readonly string _emptyString;

        private readonly IApplicationSettings _applicationSettings = ApplicationSettingsFactory.ApplicationSettings(Application.ProductName);

        private Label _emailLabel;
        private Label _cityLabel;
        private Label _zipcodeLabel;
        private Label _address1Label;
        private Label _address2Label;
        private Label _stateLabel;
        private Label _countryLabel;
        private Label _companyLabel;
        private Label _phoneLabel;
        private Label _firstNameLabel;
        private Label _lastNameLabel;
        private Label _applicationLabel;
        private TextBox _lastNameTextBox;
        private TextBox _companyTextBox;
        private TextBox _emailTextBox;
        private TextBox _phoneTextBox;
        private TextBox _address1TextBox;
        private TextBox _address2TextBox;
        private TextBox _zipTextBox;
        private TextBox _cityTextBox;
        private TextBox _stateTextBox;
        private TextBox _countryTextBox;
        private TextBox _firstNameTextBox;
        private Button _buttonOk;
        private ErrorProvider _errorProvider;
        private TextBox _firstErrorTextBox;
        private TextBox _serialTextBox;
        private Label _serialLabel;
        private Button _buttonCancel;
        private Button _buttonReRegister;
        private Label _firstNameRequiredLabel;
        private Label _lastNameRequiredLabel;
        private Label _eMailRequiredLabel;
        private Label _address1RequiredLabel;
        private Label _zipRequiredLabel;
        private Label _cityRequiredLabel;
        private Label _countryRequiredLabel;
        private IContainer _components;

        public BaseRegistrationForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            var resources = new ResourceManager(typeof(BaseRegistrationForm));

            _emptyString = resources.GetString("EmptyString.Text");
        }

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
            this._components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseRegistrationForm));
            this._emailLabel = new System.Windows.Forms.Label();
            this._cityLabel = new System.Windows.Forms.Label();
            this._zipcodeLabel = new System.Windows.Forms.Label();
            this._address1Label = new System.Windows.Forms.Label();
            this._firstNameLabel = new System.Windows.Forms.Label();
            this._stateLabel = new System.Windows.Forms.Label();
            this._countryLabel = new System.Windows.Forms.Label();
            this._address2Label = new System.Windows.Forms.Label();
            this._companyLabel = new System.Windows.Forms.Label();
            this._phoneLabel = new System.Windows.Forms.Label();
            this._lastNameLabel = new System.Windows.Forms.Label();
            this._applicationLabel = new System.Windows.Forms.Label();
            this._lastNameTextBox = new System.Windows.Forms.TextBox();
            this._companyTextBox = new System.Windows.Forms.TextBox();
            this._emailTextBox = new System.Windows.Forms.TextBox();
            this._phoneTextBox = new System.Windows.Forms.TextBox();
            this._address1TextBox = new System.Windows.Forms.TextBox();
            this._address2TextBox = new System.Windows.Forms.TextBox();
            this._zipTextBox = new System.Windows.Forms.TextBox();
            this._cityTextBox = new System.Windows.Forms.TextBox();
            this._stateTextBox = new System.Windows.Forms.TextBox();
            this._countryTextBox = new System.Windows.Forms.TextBox();
            this._firstNameTextBox = new System.Windows.Forms.TextBox();
            this._buttonOk = new System.Windows.Forms.Button();
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this._components);
            this._buttonCancel = new System.Windows.Forms.Button();
            this._firstNameRequiredLabel = new System.Windows.Forms.Label();
            this._lastNameRequiredLabel = new System.Windows.Forms.Label();
            this._eMailRequiredLabel = new System.Windows.Forms.Label();
            this._address1RequiredLabel = new System.Windows.Forms.Label();
            this._zipRequiredLabel = new System.Windows.Forms.Label();
            this._cityRequiredLabel = new System.Windows.Forms.Label();
            this._countryRequiredLabel = new System.Windows.Forms.Label();
            this._firstErrorTextBox = new System.Windows.Forms.TextBox();
            this._serialTextBox = new System.Windows.Forms.TextBox();
            this._serialLabel = new System.Windows.Forms.Label();
            this._buttonReRegister = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // EmailLabel
            // 
            resources.ApplyResources(this._emailLabel, "_emailLabel");
            this._emailLabel.Name = "_emailLabel";
            // 
            // CityLabel
            // 
            resources.ApplyResources(this._cityLabel, "_cityLabel");
            this._cityLabel.Name = "_cityLabel";
            // 
            // ZipcodeLabel
            // 
            resources.ApplyResources(this._zipcodeLabel, "_zipcodeLabel");
            this._zipcodeLabel.Name = "_zipcodeLabel";
            // 
            // Address1Label
            // 
            resources.ApplyResources(this._address1Label, "_address1Label");
            this._address1Label.Name = "_address1Label";
            // 
            // FirstNameLabel
            // 
            resources.ApplyResources(this._firstNameLabel, "_firstNameLabel");
            this._firstNameLabel.Name = "_firstNameLabel";
            // 
            // StateLabel
            // 
            resources.ApplyResources(this._stateLabel, "_stateLabel");
            this._stateLabel.Name = "_stateLabel";
            // 
            // CountryLabel
            // 
            resources.ApplyResources(this._countryLabel, "_countryLabel");
            this._countryLabel.Name = "_countryLabel";
            // 
            // Address2Label
            // 
            resources.ApplyResources(this._address2Label, "_address2Label");
            this._address2Label.Name = "_address2Label";
            // 
            // CompanyLabel
            // 
            resources.ApplyResources(this._companyLabel, "_companyLabel");
            this._companyLabel.Name = "_companyLabel";
            // 
            // PhoneLabel
            // 
            resources.ApplyResources(this._phoneLabel, "_phoneLabel");
            this._phoneLabel.Name = "_phoneLabel";
            // 
            // LastNameLabel
            // 
            resources.ApplyResources(this._lastNameLabel, "_lastNameLabel");
            this._lastNameLabel.Name = "_lastNameLabel";
            // 
            // ApplicationLabel
            // 
            resources.ApplyResources(this._applicationLabel, "_applicationLabel");
            this._applicationLabel.Name = "_applicationLabel";
            // 
            // LastNameTextBox
            // 
            resources.ApplyResources(this._lastNameTextBox, "_lastNameTextBox");
            this._lastNameTextBox.Name = "_lastNameTextBox";
            // 
            // CompanyTextBox
            // 
            resources.ApplyResources(this._companyTextBox, "_companyTextBox");
            this._companyTextBox.Name = "_companyTextBox";
            // 
            // EmailTextBox
            // 
            resources.ApplyResources(this._emailTextBox, "_emailTextBox");
            this._emailTextBox.Name = "_emailTextBox";
            // 
            // PhoneTextBox
            // 
            resources.ApplyResources(this._phoneTextBox, "_phoneTextBox");
            this._phoneTextBox.Name = "_phoneTextBox";
            // 
            // Address1TextBox
            // 
            resources.ApplyResources(this._address1TextBox, "_address1TextBox");
            this._address1TextBox.Name = "_address1TextBox";
            // 
            // Address2TextBox
            // 
            resources.ApplyResources(this._address2TextBox, "_address2TextBox");
            this._address2TextBox.Name = "_address2TextBox";
            // 
            // ZipTextBox
            // 
            resources.ApplyResources(this._zipTextBox, "_zipTextBox");
            this._zipTextBox.Name = "_zipTextBox";
            // 
            // CityTextBox
            // 
            resources.ApplyResources(this._cityTextBox, "_cityTextBox");
            this._cityTextBox.Name = "_cityTextBox";
            // 
            // StateTextBox
            // 
            resources.ApplyResources(this._stateTextBox, "_stateTextBox");
            this._stateTextBox.Name = "_stateTextBox";
            // 
            // CountryTextBox
            // 
            resources.ApplyResources(this._countryTextBox, "_countryTextBox");
            this._countryTextBox.Name = "_countryTextBox";
            // 
            // FirstNameTextBox
            // 
            resources.ApplyResources(this._firstNameTextBox, "_firstNameTextBox");
            this._firstNameTextBox.Name = "_firstNameTextBox";
            // 
            // buttonOK
            // 
            resources.ApplyResources(this._buttonOk, "_buttonOk");
            this._buttonOk.Name = "_buttonOk";
            this._buttonOk.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // errorProvider
            // 
            this._errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this._errorProvider.ContainerControl = this;
            resources.ApplyResources(this._errorProvider, "_errorProvider");
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this._buttonCancel, "_buttonCancel");
            this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._buttonCancel.Name = "_buttonCancel";
            this._buttonCancel.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // FirstNameRequiredLabel
            // 
            resources.ApplyResources(this._firstNameRequiredLabel, "_firstNameRequiredLabel");
            this._firstNameRequiredLabel.Name = "_firstNameRequiredLabel";
            // 
            // LastNameRequiredLabel
            // 
            resources.ApplyResources(this._lastNameRequiredLabel, "_lastNameRequiredLabel");
            this._lastNameRequiredLabel.Name = "_lastNameRequiredLabel";
            // 
            // EMailRequiredLabel
            // 
            resources.ApplyResources(this._eMailRequiredLabel, "_eMailRequiredLabel");
            this._eMailRequiredLabel.Name = "_eMailRequiredLabel";
            // 
            // Address1RequiredLabel
            // 
            resources.ApplyResources(this._address1RequiredLabel, "_address1RequiredLabel");
            this._address1RequiredLabel.Name = "_address1RequiredLabel";
            // 
            // ZipRequiredLabel
            // 
            resources.ApplyResources(this._zipRequiredLabel, "_zipRequiredLabel");
            this._zipRequiredLabel.Name = "_zipRequiredLabel";
            // 
            // CityRequiredLabel
            // 
            resources.ApplyResources(this._cityRequiredLabel, "_cityRequiredLabel");
            this._cityRequiredLabel.Name = "_cityRequiredLabel";
            // 
            // CountryRequiredLabel
            // 
            resources.ApplyResources(this._countryRequiredLabel, "_countryRequiredLabel");
            this._countryRequiredLabel.Name = "_countryRequiredLabel";
            // 
            // FirstErrorTextBox
            // 
            this._firstErrorTextBox.AcceptsReturn = true;
            resources.ApplyResources(this._firstErrorTextBox, "_firstErrorTextBox");
            this._firstErrorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._firstErrorTextBox.Name = "_firstErrorTextBox";
            this._firstErrorTextBox.ReadOnly = true;
            // 
            // SerialTextBox
            // 
            this._serialTextBox.AcceptsReturn = true;
            resources.ApplyResources(this._serialTextBox, "_serialTextBox");
            this._serialTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._serialTextBox.Name = "_serialTextBox";
            this._serialTextBox.ReadOnly = true;
            // 
            // SerialLabel
            // 
            resources.ApplyResources(this._serialLabel, "_serialLabel");
            this._serialLabel.Name = "_serialLabel";
            // 
            // buttonReRegister
            // 
            resources.ApplyResources(this._buttonReRegister, "_buttonReRegister");
            this._buttonReRegister.Name = "_buttonReRegister";
            this._buttonReRegister.Click += new System.EventHandler(this.ReRegisterbutton_Click);
            // 
            // BaseRegistrationForm
            // 
            this.AcceptButton = this._buttonOk;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this._buttonCancel;
            this.Controls.Add(this._buttonReRegister);
            this.Controls.Add(this._serialLabel);
            this.Controls.Add(this._serialTextBox);
            this.Controls.Add(this._firstErrorTextBox);
            this.Controls.Add(this._lastNameTextBox);
            this.Controls.Add(this._companyTextBox);
            this.Controls.Add(this._emailTextBox);
            this.Controls.Add(this._address1TextBox);
            this.Controls.Add(this._address2TextBox);
            this.Controls.Add(this._zipTextBox);
            this.Controls.Add(this._cityTextBox);
            this.Controls.Add(this._stateTextBox);
            this.Controls.Add(this._countryTextBox);
            this.Controls.Add(this._firstNameTextBox);
            this.Controls.Add(this._phoneTextBox);
            this.Controls.Add(this._countryRequiredLabel);
            this.Controls.Add(this._cityRequiredLabel);
            this.Controls.Add(this._zipRequiredLabel);
            this.Controls.Add(this._address1RequiredLabel);
            this.Controls.Add(this._eMailRequiredLabel);
            this.Controls.Add(this._lastNameRequiredLabel);
            this.Controls.Add(this._firstNameRequiredLabel);
            this.Controls.Add(this._buttonCancel);
            this.Controls.Add(this._buttonOk);
            this.Controls.Add(this._applicationLabel);
            this.Controls.Add(this._lastNameLabel);
            this.Controls.Add(this._phoneLabel);
            this.Controls.Add(this._companyLabel);
            this.Controls.Add(this._countryLabel);
            this.Controls.Add(this._stateLabel);
            this.Controls.Add(this._zipcodeLabel);
            this.Controls.Add(this._cityLabel);
            this.Controls.Add(this._address2Label);
            this.Controls.Add(this._firstNameLabel);
            this.Controls.Add(this._address1Label);
            this.Controls.Add(this._emailLabel);
            this.Name = "BaseRegistrationForm";
            this.Load += new System.EventHandler(this.BaseRegistrationForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private void SetFieldStatus(bool fieldStatus)
        {
            _serialLabel.Visible = fieldStatus;
            _emailTextBox.ReadOnly = fieldStatus;
            _buttonOk.Enabled = !fieldStatus;

            AcceptButton = _buttonOk.Enabled ? _buttonOk : _buttonReRegister;
        }

        private void BaseRegistrationForm_Load(object sender, EventArgs e)
        {
            _applicationLabel.Text = _applicationLabel.Text.ToUpper(CultureInfo.CurrentCulture);

            _firstNameTextBox.Text = _applicationSettings.GetStringValue("FirstNameTextBox.Text");
            _lastNameTextBox.Text = _applicationSettings.GetStringValue("LastNameTextBox.Text");
            _companyTextBox.Text = _applicationSettings.GetStringValue("CompanyTextBox.Text");
            _emailTextBox.Text = _applicationSettings.GetStringValue("EmailTextBox.Text");
            _phoneTextBox.Text = _applicationSettings.GetStringValue("PhoneTextBox.Text");
            _address1TextBox.Text = _applicationSettings.GetStringValue("Address1TextBox.Text");
            _address2TextBox.Text = _applicationSettings.GetStringValue("Address2TextBox.Text");
            _zipTextBox.Text = _applicationSettings.GetStringValue("ZipTextBox.Text");
            _cityTextBox.Text = _applicationSettings.GetStringValue("CityTextBox.Text");
            _stateTextBox.Text = _applicationSettings.GetStringValue("StateTextBox.Text");
            _countryTextBox.Text = _applicationSettings.GetStringValue("CountryTextBox.Text");
            _serialTextBox.Text = _applicationSettings.GetStringValue("Serial.Text");

            SetFieldStatus(_serialTextBox.Text.Length > 0);
        }

        private bool ValidateTextBox(TextBox textBox, bool isValid)
        {
            const string requiredInformation = "This is required information for registration";
            var noError = string.Empty;

            if (textBox.Text.Length == 0)
            {
                _errorProvider.SetError(textBox, requiredInformation);
                if (isValid)
                    isValid = false;

                if (_firstErrorTextBox.Text.Length == 0)
                    _firstErrorTextBox.Text = textBox.AccessibleName + ": " + requiredInformation;
            }
            else
            {
                _errorProvider.SetError(textBox, noError);
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
                    _errorProvider.SetError(textBox, requiredEmail);
                    isValid = false;

                    if (_firstErrorTextBox.Text.Length == 0)
                        _firstErrorTextBox.Text = textBox.AccessibleName + ": " + requiredEmail;
                }
            }

            return isValid;
        }

        private bool DoValidation()
        {
            _firstErrorTextBox.Text = _emptyString;

            var valid = ValidateTextBox(_firstNameTextBox, true);
            valid = ValidateTextBox(_lastNameTextBox, valid);
            valid = ValidateTextBox(_emailTextBox, valid);

            valid = ValidateEmail(_emailTextBox, valid);

            valid = ValidateTextBox(_address1TextBox, valid);
            valid = ValidateTextBox(_countryTextBox, valid);
            valid = ValidateTextBox(_cityTextBox, valid);
            valid = ValidateTextBox(_zipTextBox, valid);

            return valid;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (DoValidation())
            {
                DialogResult = DialogResult.OK;

                _applicationSettings.SetStringValue("FirstNameTextBox.Text", _firstNameTextBox.Text);
                _applicationSettings.SetStringValue("LastNameTextBox.Text", _lastNameTextBox.Text);
                _applicationSettings.SetStringValue("CompanyTextBox.Text", _companyTextBox.Text);
                _applicationSettings.SetStringValue("EmailTextBox.Text", _emailTextBox.Text);
                _applicationSettings.SetStringValue("PhoneTextBox.Text", _phoneTextBox.Text);
                _applicationSettings.SetStringValue("Address1TextBox.Text", _address1TextBox.Text);
                _applicationSettings.SetStringValue("Address2TextBox.Text", _address2TextBox.Text);
                _applicationSettings.SetStringValue("ZipTextBox.Text", _zipTextBox.Text);
                _applicationSettings.SetStringValue("CityTextBox.Text", _cityTextBox.Text);
                _applicationSettings.SetStringValue("StateTextBox.Text", _stateTextBox.Text);
                _applicationSettings.SetStringValue("CountryTextBox.Text", _countryTextBox.Text);

                _applicationSettings.Flush();

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

        [UsedImplicitly]
        public string Address1 => _address1TextBox.Text;
        [UsedImplicitly]
        public string Address2 => _address2TextBox.Text;
        [UsedImplicitly]
        public string LastName => _lastNameTextBox.Text;
        [UsedImplicitly]
        public string FirstName => _firstNameTextBox.Text;
        [UsedImplicitly]
        public string Company => _companyTextBox.Text;
        [UsedImplicitly]
        public string Email => _emailTextBox.Text;
        [UsedImplicitly]
        public string Phone => _phoneTextBox.Text;
        public string Zip => _zipTextBox.Text;
        [UsedImplicitly]
        public string City => _cityTextBox.Text;
        public string State => _stateTextBox.Text;
        [UsedImplicitly]
        public string Country => _countryTextBox.Text;
    }
}
