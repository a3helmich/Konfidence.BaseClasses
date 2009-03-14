using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Resources;

using Konfidence.UtilHelper;

namespace Konfidence.BaseWindowForms
{

    public class BaseRegistrationForm : System.Windows.Forms.Form
    {
        private string _EmptyString;

        private IApplicationSettings _ApplicationSettings = ApplicationSettingsFactory.ApplicationSettings(Application.ProductName);

        private System.Windows.Forms.Label EmailLabel;
        private System.Windows.Forms.Label CityLabel;
        private System.Windows.Forms.Label ZipcodeLabel;
        private System.Windows.Forms.Label Address1Label;
        private System.Windows.Forms.Label Address2Label;
        private System.Windows.Forms.Label StateLabel;
        private System.Windows.Forms.Label CountryLabel;
        private System.Windows.Forms.Label CompanyLabel;
        private System.Windows.Forms.Label PhoneLabel;
        private System.Windows.Forms.Label FirstNameLabel;
        private System.Windows.Forms.Label LastNameLabel;
        private System.Windows.Forms.Label ApplicationLabel;
        private System.Windows.Forms.TextBox LastNameTextBox;
        private System.Windows.Forms.TextBox CompanyTextBox;
        private System.Windows.Forms.TextBox EmailTextBox;
        private System.Windows.Forms.TextBox PhoneTextBox;
        private System.Windows.Forms.TextBox Address1TextBox;
        private System.Windows.Forms.TextBox Address2TextBox;
        private System.Windows.Forms.TextBox ZipTextBox;
        private System.Windows.Forms.TextBox CityTextBox;
        private System.Windows.Forms.TextBox StateTextBox;
        private System.Windows.Forms.TextBox CountryTextBox;
        private System.Windows.Forms.TextBox FirstNameTextBox;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.TextBox FirstErrorTextBox;
        private System.Windows.Forms.TextBox SerialTextBox;
        private System.Windows.Forms.Label SerialLabel;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonReRegister;
        private System.Windows.Forms.Label FirstNameRequiredLabel;
        private System.Windows.Forms.Label LastNameRequiredLabel;
        private System.Windows.Forms.Label EMailRequiredLabel;
        private System.Windows.Forms.Label Address1RequiredLabel;
        private System.Windows.Forms.Label ZipRequiredLabel;
        private System.Windows.Forms.Label CityRequiredLabel;
        private System.Windows.Forms.Label CountryRequiredLabel;
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
            ResourceManager resources = new System.Resources.ResourceManager(typeof(BaseRegistrationForm));

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
            this.EmailLabel = new System.Windows.Forms.Label();
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
            this.ApplicationLabel = new System.Windows.Forms.Label();
            this.LastNameTextBox = new System.Windows.Forms.TextBox();
            this.CompanyTextBox = new System.Windows.Forms.TextBox();
            this.EmailTextBox = new System.Windows.Forms.TextBox();
            this.PhoneTextBox = new System.Windows.Forms.TextBox();
            this.Address1TextBox = new System.Windows.Forms.TextBox();
            this.Address2TextBox = new System.Windows.Forms.TextBox();
            this.ZipTextBox = new System.Windows.Forms.TextBox();
            this.CityTextBox = new System.Windows.Forms.TextBox();
            this.StateTextBox = new System.Windows.Forms.TextBox();
            this.CountryTextBox = new System.Windows.Forms.TextBox();
            this.FirstNameTextBox = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.buttonCancel = new System.Windows.Forms.Button();
            this.FirstNameRequiredLabel = new System.Windows.Forms.Label();
            this.LastNameRequiredLabel = new System.Windows.Forms.Label();
            this.EMailRequiredLabel = new System.Windows.Forms.Label();
            this.Address1RequiredLabel = new System.Windows.Forms.Label();
            this.ZipRequiredLabel = new System.Windows.Forms.Label();
            this.CityRequiredLabel = new System.Windows.Forms.Label();
            this.CountryRequiredLabel = new System.Windows.Forms.Label();
            this.FirstErrorTextBox = new System.Windows.Forms.TextBox();
            this.SerialTextBox = new System.Windows.Forms.TextBox();
            this.SerialLabel = new System.Windows.Forms.Label();
            this.buttonReRegister = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // EmailLabel
            // 
            resources.ApplyResources(this.EmailLabel, "EmailLabel");
            this.EmailLabel.Name = "EmailLabel";
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
            resources.ApplyResources(this.ApplicationLabel, "ApplicationLabel");
            this.ApplicationLabel.Name = "ApplicationLabel";
            // 
            // LastNameTextBox
            // 
            resources.ApplyResources(this.LastNameTextBox, "LastNameTextBox");
            this.LastNameTextBox.Name = "LastNameTextBox";
            // 
            // CompanyTextBox
            // 
            resources.ApplyResources(this.CompanyTextBox, "CompanyTextBox");
            this.CompanyTextBox.Name = "CompanyTextBox";
            // 
            // EmailTextBox
            // 
            resources.ApplyResources(this.EmailTextBox, "EmailTextBox");
            this.EmailTextBox.Name = "EmailTextBox";
            // 
            // PhoneTextBox
            // 
            resources.ApplyResources(this.PhoneTextBox, "PhoneTextBox");
            this.PhoneTextBox.Name = "PhoneTextBox";
            // 
            // Address1TextBox
            // 
            resources.ApplyResources(this.Address1TextBox, "Address1TextBox");
            this.Address1TextBox.Name = "Address1TextBox";
            // 
            // Address2TextBox
            // 
            resources.ApplyResources(this.Address2TextBox, "Address2TextBox");
            this.Address2TextBox.Name = "Address2TextBox";
            // 
            // ZipTextBox
            // 
            resources.ApplyResources(this.ZipTextBox, "ZipTextBox");
            this.ZipTextBox.Name = "ZipTextBox";
            // 
            // CityTextBox
            // 
            resources.ApplyResources(this.CityTextBox, "CityTextBox");
            this.CityTextBox.Name = "CityTextBox";
            // 
            // StateTextBox
            // 
            resources.ApplyResources(this.StateTextBox, "StateTextBox");
            this.StateTextBox.Name = "StateTextBox";
            // 
            // CountryTextBox
            // 
            resources.ApplyResources(this.CountryTextBox, "CountryTextBox");
            this.CountryTextBox.Name = "CountryTextBox";
            // 
            // FirstNameTextBox
            // 
            resources.ApplyResources(this.FirstNameTextBox, "FirstNameTextBox");
            this.FirstNameTextBox.Name = "FirstNameTextBox";
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // FirstNameRequiredLabel
            // 
            resources.ApplyResources(this.FirstNameRequiredLabel, "FirstNameRequiredLabel");
            this.FirstNameRequiredLabel.Name = "FirstNameRequiredLabel";
            // 
            // LastNameRequiredLabel
            // 
            resources.ApplyResources(this.LastNameRequiredLabel, "LastNameRequiredLabel");
            this.LastNameRequiredLabel.Name = "LastNameRequiredLabel";
            // 
            // EMailRequiredLabel
            // 
            resources.ApplyResources(this.EMailRequiredLabel, "EMailRequiredLabel");
            this.EMailRequiredLabel.Name = "EMailRequiredLabel";
            // 
            // Address1RequiredLabel
            // 
            resources.ApplyResources(this.Address1RequiredLabel, "Address1RequiredLabel");
            this.Address1RequiredLabel.Name = "Address1RequiredLabel";
            // 
            // ZipRequiredLabel
            // 
            resources.ApplyResources(this.ZipRequiredLabel, "ZipRequiredLabel");
            this.ZipRequiredLabel.Name = "ZipRequiredLabel";
            // 
            // CityRequiredLabel
            // 
            resources.ApplyResources(this.CityRequiredLabel, "CityRequiredLabel");
            this.CityRequiredLabel.Name = "CityRequiredLabel";
            // 
            // CountryRequiredLabel
            // 
            resources.ApplyResources(this.CountryRequiredLabel, "CountryRequiredLabel");
            this.CountryRequiredLabel.Name = "CountryRequiredLabel";
            // 
            // FirstErrorTextBox
            // 
            this.FirstErrorTextBox.AcceptsReturn = true;
            resources.ApplyResources(this.FirstErrorTextBox, "FirstErrorTextBox");
            this.FirstErrorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FirstErrorTextBox.Name = "FirstErrorTextBox";
            this.FirstErrorTextBox.ReadOnly = true;
            // 
            // SerialTextBox
            // 
            this.SerialTextBox.AcceptsReturn = true;
            resources.ApplyResources(this.SerialTextBox, "SerialTextBox");
            this.SerialTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SerialTextBox.Name = "SerialTextBox";
            this.SerialTextBox.ReadOnly = true;
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
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.buttonReRegister);
            this.Controls.Add(this.SerialLabel);
            this.Controls.Add(this.SerialTextBox);
            this.Controls.Add(this.FirstErrorTextBox);
            this.Controls.Add(this.LastNameTextBox);
            this.Controls.Add(this.CompanyTextBox);
            this.Controls.Add(this.EmailTextBox);
            this.Controls.Add(this.Address1TextBox);
            this.Controls.Add(this.Address2TextBox);
            this.Controls.Add(this.ZipTextBox);
            this.Controls.Add(this.CityTextBox);
            this.Controls.Add(this.StateTextBox);
            this.Controls.Add(this.CountryTextBox);
            this.Controls.Add(this.FirstNameTextBox);
            this.Controls.Add(this.PhoneTextBox);
            this.Controls.Add(this.CountryRequiredLabel);
            this.Controls.Add(this.CityRequiredLabel);
            this.Controls.Add(this.ZipRequiredLabel);
            this.Controls.Add(this.Address1RequiredLabel);
            this.Controls.Add(this.EMailRequiredLabel);
            this.Controls.Add(this.LastNameRequiredLabel);
            this.Controls.Add(this.FirstNameRequiredLabel);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.ApplicationLabel);
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
            this.Controls.Add(this.EmailLabel);
            this.Name = "BaseRegistrationForm";
            this.Load += new System.EventHandler(this.BaseRegistrationForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private void SetFieldStatus(bool fieldStatus)
        {
            SerialLabel.Visible = fieldStatus;
            EmailTextBox.ReadOnly = fieldStatus;
            buttonOK.Enabled = !fieldStatus;
            if (buttonOK.Enabled)
            {
                AcceptButton = buttonOK;
            }
            else
            {
                AcceptButton = buttonReRegister;
            }
        }

        private void BaseRegistrationForm_Load(object sender, System.EventArgs e)
        {
            ApplicationLabel.Text = ApplicationLabel.Text.ToUpper(CultureInfo.CurrentCulture);

            FirstNameTextBox.Text = _ApplicationSettings.GetStringValue("FirstNameTextBox.Text");
            LastNameTextBox.Text = _ApplicationSettings.GetStringValue("LastNameTextBox.Text");
            CompanyTextBox.Text = _ApplicationSettings.GetStringValue("CompanyTextBox.Text");
            EmailTextBox.Text = _ApplicationSettings.GetStringValue("EmailTextBox.Text");
            PhoneTextBox.Text = _ApplicationSettings.GetStringValue("PhoneTextBox.Text");
            Address1TextBox.Text = _ApplicationSettings.GetStringValue("Address1TextBox.Text");
            Address2TextBox.Text = _ApplicationSettings.GetStringValue("Address2TextBox.Text");
            ZipTextBox.Text = _ApplicationSettings.GetStringValue("ZipTextBox.Text");
            CityTextBox.Text = _ApplicationSettings.GetStringValue("CityTextBox.Text");
            StateTextBox.Text = _ApplicationSettings.GetStringValue("StateTextBox.Text");
            CountryTextBox.Text = _ApplicationSettings.GetStringValue("CountryTextBox.Text");
            SerialTextBox.Text = _ApplicationSettings.GetStringValue("Serial.Text");

            if (SerialTextBox.Text.Length > 0)
                SetFieldStatus(true);
            else
                SetFieldStatus(false);
        }

        private bool ValidateTextBox(TextBox textBox, bool IsValid)
        {
            string RequiredInformation = "This is required information for registration";
            string NoError = string.Empty;

            if (textBox.Text.Length == 0)
            {
                errorProvider.SetError(textBox, RequiredInformation);
                if (IsValid)
                    IsValid = false;

                if (FirstErrorTextBox.Text.Length == 0)
                    FirstErrorTextBox.Text = textBox.AccessibleName + ": " + RequiredInformation;
            }
            else
            {
                errorProvider.SetError(textBox, NoError);
            }

            return IsValid;
        }

        private bool ValidateEmail(TextBox textBox, bool IsValid)
        {
            string RequiredEmail = "Email address does not have a valid format";

            if (textBox.Text.Length > 0)
            {
                if (!BaseUtilHelper.IsValidEmail(textBox.Text))
                {
                    errorProvider.SetError(textBox, RequiredEmail);
                    IsValid = false;

                    if (FirstErrorTextBox.Text.Length == 0)
                        FirstErrorTextBox.Text = textBox.AccessibleName + ": " + RequiredEmail;
                }
            }

            return IsValid;
        }

        private bool DoValidation()
        {
            bool Valid = true;

            FirstErrorTextBox.Text = _EmptyString;

            Valid = ValidateTextBox(FirstNameTextBox, Valid);
            Valid = ValidateTextBox(LastNameTextBox, Valid);
            Valid = ValidateTextBox(EmailTextBox, Valid);

            Valid = ValidateEmail(EmailTextBox, Valid);

            Valid = ValidateTextBox(Address1TextBox, Valid);
            Valid = ValidateTextBox(CountryTextBox, Valid);
            Valid = ValidateTextBox(CityTextBox, Valid);
            Valid = ValidateTextBox(ZipTextBox, Valid);

            return Valid;
        }

        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            if (DoValidation())
            {
                DialogResult = DialogResult.OK;

                _ApplicationSettings.SetStringValue("FirstNameTextBox.Text", FirstNameTextBox.Text);
                _ApplicationSettings.SetStringValue("LastNameTextBox.Text", LastNameTextBox.Text);
                _ApplicationSettings.SetStringValue("CompanyTextBox.Text", CompanyTextBox.Text);
                _ApplicationSettings.SetStringValue("EmailTextBox.Text", EmailTextBox.Text);
                _ApplicationSettings.SetStringValue("PhoneTextBox.Text", PhoneTextBox.Text);
                _ApplicationSettings.SetStringValue("Address1TextBox.Text", Address1TextBox.Text);
                _ApplicationSettings.SetStringValue("Address2TextBox.Text", Address2TextBox.Text);
                _ApplicationSettings.SetStringValue("ZipTextBox.Text", ZipTextBox.Text);
                _ApplicationSettings.SetStringValue("CityTextBox.Text", CityTextBox.Text);
                _ApplicationSettings.SetStringValue("StateTextBox.Text", StateTextBox.Text);
                _ApplicationSettings.SetStringValue("CountryTextBox.Text", CountryTextBox.Text);

                _ApplicationSettings.Flush();

                Close();
            }
        }

        private void CancelButton_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }

        private void ReRegisterbutton_Click(object sender, System.EventArgs e)
        {
            // MessageBox.Show("test");
        }

        public string Address1
        {
            get { return Address1TextBox.Text; }
        }
        public string Address2
        {
            get { return Address2TextBox.Text; }
        }
        public string LastName
        {
            get { return LastNameTextBox.Text; }
        }
        public string FirstName
        {
            get { return FirstNameTextBox.Text; }
        }
        public string Company
        {
            get { return CompanyTextBox.Text; }
        }
        public string Email
        {
            get { return EmailTextBox.Text; }
        }
        public string Phone
        {
            get { return PhoneTextBox.Text; }
        }
        public string Zip
        {
            get { return ZipTextBox.Text; }
        }
        public string City
        {
            get { return CityTextBox.Text; }
        }
        public string State
        {
            get { return StateTextBox.Text; }
        }
        public string Country
        {
            get { return CountryTextBox.Text; }
        }

    }
}
