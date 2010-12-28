using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WebProjectValidator.FileListChecker;
using WebProjectValidator.HelperClasses;
using Konfidence.Base;
using WebProjectValidator.EnumTypes;

namespace WebProjectValidator
{
    public partial class MainForm : Form
    {
        MainFormPresenter _Presenter = new MainFormPresenter();

        public MainForm()
        {
            InitializeComponent();

            InitializeComponentEx();
        }

        private void InitializeComponentEx()
        {
            dgvDesignerFileMissing.AutoGenerateColumns = false;
            dgvProjectTypeValidation.AutoGenerateColumns = false;
            dgvUserControlMissing.AutoGenerateColumns = false;
        }

        private void PresenterToForm()
        {
            tbSolutionFolder.Text = _Presenter.SolutionFolder;
            tbProjectName.Text = _Presenter.ProjectName;
            lProjectFileNameDisplay.Text = _Presenter.ProjectFile;

            rbCS.Checked = _Presenter.IsCS;
            rbVB.Checked = _Presenter.IsVB;
            
            bConvertToWebApplication.Enabled = _Presenter.ConvertButtonsEnabled();
            bConvertToWebProject.Enabled = _Presenter.ConvertButtonsEnabled();

            #region ProjectTypeValidation
            if (_Presenter.ProjectTypeValidationItemVisible())
            {
                tsslTotal.Visible = _Presenter.ProjectTypeValidationItemVisible();
                tsslValid.Visible = _Presenter.ProjectTypeValidationItemVisible();
                tsslInValid.Visible = _Presenter.ProjectTypeValidationItemVisible();
                tsslRowCount.Visible = _Presenter.ProjectTypeValidationItemVisible();

                tsslTotal.Text = _Presenter.ProjectFileCountText;
                tsslValid.Text = _Presenter.ProjectFileValidCountText;
                tsslInValid.Text = _Presenter.ProjectFileInvalidCountText;
                tsslRowCount.Text = _Presenter.ProjectFileListCountText;
            }

            dgvProjectTypeValidation.DataSource = _Presenter.ProjectTypeValidationList;
            #endregion ProjectTypeValidation

            #region MissingUserControlValidation
            if (_Presenter.UserControlMissingValidationItemVisible())
            {
                tsslTotal.Visible = _Presenter.UserControlMissingValidationItemVisible();
                tsslValid.Visible = _Presenter.UserControlMissingValidationItemVisible();
                tsslInValid.Visible = _Presenter.UserControlMissingValidationItemVisible();
                tsslRowCount.Visible = _Presenter.UserControlMissingValidationItemVisible();

                tsslTotal.Text = _Presenter.UserControlMissingListCountText;
                tsslValid.Text = _Presenter.UserControlMissingValidCountText;
                tsslInValid.Text = _Presenter.UserControlMissingInvalidCountText;
                tsslRowCount.Text = _Presenter.UserControlMissingListCountText;
            }

            dgvUserControlMissing.DataSource = _Presenter.MissingUserControlList;
            #endregion MissingUserControlValidation
      
        }

        private void FormToPresenter()
        {
            _Presenter.SolutionFolder = tbSolutionFolder.Text;
            _Presenter.ProjectName = tbProjectName.Text;

            _Presenter.IsCS = rbCS.Checked;
            _Presenter.IsWebProjectCheck = rbCheckWebProject.Checked;

            _Presenter.IsUserControlValidCheck = rbUserControlValid.Checked;
            _Presenter.IsUserControlInvalidCheck = rbUserControlInvalid.Checked;
            _Presenter.IsUserControlMissingCheck = rbUserControlMissing.Checked;
            _Presenter.IsUserControlUnusedCheck = rbUserControlUnused.Checked;

            if (tabControl.SelectedTab.Equals(tpDesignerFileMissing))
            {
                _Presenter.TabPageType = TabPageType.DesignerFileMissing;
            }
            if (tabControl.SelectedTab.Equals(tpProjectTypeValidation))
            {
                _Presenter.TabPageType = TabPageType.ProjectTypeValidation;
            }
            if (tabControl.SelectedTab.Equals(tpUserControlMissing))
            {
                _Presenter.TabPageType = TabPageType.UserControlMissing;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            PresenterToForm();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormToPresenter();

            _Presenter.Close();
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            FormToPresenter();

            if (!Execute())
            {
                MessageBox.Show(_Presenter.ErrorMessage);
            }
        }

        private bool Execute()
        {
            if (_Presenter.Validate())
            {
                switch (_Presenter.TabPageType)
                {
                    case TabPageType.ProjectTypeValidation:
                        ProjectTypeValidation();
                        break;
                    case TabPageType.DesignerFileMissing:
                        DesignerFileMissing();
                        break;
                    case TabPageType.UserControlMissing:
                        UserControlMissing();
                        break;
                }
                return true;
            }
            return false;
        }

        private void DesignerFileMissing()
        {
            FormToPresenter();

            ListProcessor processor = new ListProcessor(_Presenter.ProjectName, _Presenter.ProjectFolder, _Presenter.LanguageType, _Presenter.ProjectFile);
            ListFilterType filter = GetDesignerFileMissingFilterType();

            dgvDesignerFileMissing.DataSource = processor.processDesignerFileMissing(_Presenter.SourceFileList, _Presenter.DesignerFileList, filter);

            tsslTotal.Visible = true;
            tsslTotal.Text = "Total: " + processor.Count;
            tsslValid.Visible = true;
            tsslValid.Text = "Existing: " + processor.ValidCount;
            tsslInValid.Visible = true;
            tsslInValid.Text = "Missing: " + processor.InvalidCount;
            tsslRowCount.Visible = true;
            tsslRowCount.Text = "RowCount: " + dgvDesignerFileMissing.RowCount;

            PresenterToForm();
        }

        private void ProjectTypeValidation()
        {
            FormToPresenter();

            _Presenter.ProjectTypeValidation();

            PresenterToForm();
        }

        private void UserControlMissing()
        {
            FormToPresenter();

            _Presenter.UserControlValidation();

            PresenterToForm();
        }

        private ListFilterType GetDesignerFileMissingFilterType()
        {
            if (rbDesignerFileExists.Checked)
            {
                return ListFilterType.DesignerFileExists;
            }

            if (rbDesignerFileMissing.Checked)
            {
                return ListFilterType.DesignerFileMissing;
            }

            return ListFilterType.All;
        }

        private void bConvertToWebApplication_Click(object sender, EventArgs e)
        {
            FormToPresenter();

            _Presenter.ConvertToWebApplication();

            PresenterToForm();
        }

        private void bConvertToWebProject_Click(object sender, EventArgs e)
        {
            FormToPresenter();

            _Presenter.ConvertToWebProject();

            PresenterToForm();
        }


        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormToPresenter();

            PresenterToForm();
        }

        private void bSelectSolutionFolder_Click(object sender, EventArgs e)
        {
            FormToPresenter();

            SelectSolutionFolder();

            PresenterToForm();
        }

        private void SelectSolutionFolder()
        {
            folderBrowserDialog.SelectedPath = _Presenter.SolutionFolder;

            if (folderBrowserDialog.ShowDialog().Equals(DialogResult.OK))
            {
                _Presenter.SolutionFolder = folderBrowserDialog.SelectedPath;
            }
        }
    }
}
