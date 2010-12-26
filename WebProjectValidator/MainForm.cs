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
            dgvCodeFileCheck.AutoGenerateColumns = false;
            dgvUserControlMissing.AutoGenerateColumns = false;
        }

        private void PresenterToForm()
        {
            tbSolutionFolder.Text = _Presenter.SolutionFolder;
            tbProjectName.Text = _Presenter.ProjectName;
            lProjectFileNameDisplay.Text = _Presenter.ProjectFile;

            rbCS.Checked = _Presenter.IsCS;
            rbVB.Checked = _Presenter.IsVB;
            
            bConvertToWebApplication.Enabled = _Presenter.ConvertButtonsEnabled;
            bConvertToWebProject.Enabled = _Presenter.ConvertButtonsEnabled;
        }

        private void FormToPresenter()
        {
            _Presenter.SolutionFolder = tbSolutionFolder.Text;
            _Presenter.ProjectName = tbProjectName.Text;

            _Presenter.IsCS = rbCS.Checked;
            _Presenter.IsWebProjectCheck = rbCheckWebProject.Checked;

            if (tabControl.SelectedTab.Equals(tpDesignerFileMissing))
            {
                _Presenter.TabPageType = TabPageType.DesignerFileMissing;
            }
            if (tabControl.SelectedTab.Equals(tpCodeFileCheck))
            {
                _Presenter.TabPageType = TabPageType.CodeFileCheck;
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
                    case TabPageType.CodeFileCheck:
                        CodeFileCheck();
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

            ListProcessor processor = new ListProcessor(tbProjectName.Text, _Presenter.ProjectFolder, _Presenter.LanguageType, _Presenter.ProjectFile);
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

        private void CodeFileCheck()
        {
            FormToPresenter();

            ListProcessor processor = new ListProcessor(tbProjectName.Text, _Presenter.ProjectFolder, _Presenter.LanguageType, _Presenter.ProjectFile);

            dgvCodeFileCheck.DataSource = processor.processCodeFileCheck(_Presenter.WebFileList, _Presenter.FilterType);

            tsslTotal.Visible = true;
            tsslTotal.Text = "Total: " + processor.Count;
            tsslValid.Visible = true;
            tsslValid.Text = "Valid: " + processor.ValidCount;
            tsslInValid.Visible = true;
            tsslInValid.Text = "Invalid: " + processor.InvalidCount;
            tsslRowCount.Visible = true;
            tsslRowCount.Text = "RowCount: " + dgvCodeFileCheck.RowCount;

            PresenterToForm();
        }

        private void UserControlMissing()
        {
            FormToPresenter();

            ListProcessor processor = new ListProcessor(tbProjectName.Text, _Presenter.ProjectFolder, _Presenter.LanguageType, _Presenter.ProjectFile);
            ListFilterType filter = GetUserControlMissingFilterType();

            dgvUserControlMissing.DataSource = processor.processUserControlMissing(_Presenter.WebFileList, filter);

            lProjectFileNameDisplay.Text = processor.ProjectFileName;

            tsslTotal.Visible = true;
            tsslTotal.Text = "Total: " + processor.Count;
            tsslValid.Visible = true;
            tsslValid.Text = "Valid: " + processor.ValidCount;
            tsslInValid.Visible = true;
            tsslInValid.Text = "Invalid: " + processor.InvalidCount;
            tsslRowCount.Visible = true;
            tsslRowCount.Text = "RowCount: " + dgvUserControlMissing.RowCount;

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

        private ListFilterType GetUserControlMissingFilterType()
        {
            if (rbUserControlValid.Checked)
            {
                return ListFilterType.Valid;
            }

            if (rbUserControlInvalid.Checked)
            {
                return ListFilterType.Invalid;
            }

            if (rbUserControlMissing.Checked)
            {
                return ListFilterType.DesignerFileMissing;
            }

            if (rbUserControlUnused.Checked)
            {
                return ListFilterType.Unused;
            }

            return ListFilterType.All;
        }

        private void bConvertToWebApplication_Click(object sender, EventArgs e)
        {
            FormToPresenter();

            ListProcessor processor = new ListProcessor(tbProjectName.Text, _Presenter.ProjectFolder, _Presenter.LanguageType, _Presenter.ProjectFile);

            // web application uses no projectfile -> all files must be converted
            // the FileList WebFileList must be transfered to a DesignerFileItemList
            DesignerFileItemList FileItemList = new DesignerFileItemList(_Presenter.ProjectFolder,_Presenter.WebFileList);

            processor.ConvertToWebApplication(FileItemList);

            PresenterToForm();
        }

        private void bConvertToWebProject_Click(object sender, EventArgs e)
        {
            FormToPresenter();

            ListProcessor processor = new ListProcessor(tbProjectName.Text, _Presenter.ProjectFolder, _Presenter.LanguageType, _Presenter.ProjectFile);

            List<DesignerFileItem> repairList = processor.processCodeFileCheck(_Presenter.WebFileList, _Presenter.FilterType);

            // web project uses a projectfile -> only files included in the project file must be converted
            processor.ConvertToWebProject(repairList);

            PresenterToForm();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormToPresenter();

            PresenterToForm();
        }

        private void bFolderBrowse_Click(object sender, EventArgs e)
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
