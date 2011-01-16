﻿using System;
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
            dgvProjectFileValidation.AutoGenerateColumns = false;
            dgvProjectTypeValidation.AutoGenerateColumns = false;
            dgvUserControlValidation.AutoGenerateColumns = false;

            tpProjectFileValidation.Tag = TabPageType.ProjectFileValidation;
            tpProjectTypeValidation.Tag = TabPageType.ProjectTypeValidation;
            tpUserControlValidation.Tag = TabPageType.UserControlValidation;
        }

        private void PresenterToForm()
        {
            tbSolutionFolder.Text = _Presenter.SolutionFolder;
            tbProjectName.Text = _Presenter.ProjectName;
            tbProjectFile.Text = _Presenter.ProjectFile;

            rbCS.Checked = _Presenter.IsCS;
            rbVB.Checked = _Presenter.IsVB;
            
            bConvertToWebsite.Enabled = _Presenter.ConvertButtonsEnabled();
            bConvertToWebProject.Enabled = _Presenter.ConvertButtonsEnabled();

            switch (_Presenter.TabPageType)
            {
                case TabPageType.ProjectFileValidation:
                    {
                        GetProjectFileValidationCounts();
                        break;
                    }
                case TabPageType.ProjectTypeValidation:
                    {
                        GetProjectTypeValidationCounts();
                        break;
                    }
                case TabPageType.UserControlValidation:
                    {
                        GetMissingUserControlValidationCounts();
                        break;
                    }
            }
        }

        private void GetProjectFileValidationCounts()
        {
            tsslTotal.Text = _Presenter.DesignerFileCountText;
            tsslStatus1.Text = _Presenter.DesignerFileExistsCountText;
            tsslStatus2.Text = _Presenter.DesignerFileMissingCountText;
            tsslListCount.Text = _Presenter.DesignerFileListCountText;

            dgvProjectFileValidation.DataSource = _Presenter.ProjectFileValidationList;
        }

        private void GetProjectTypeValidationCounts()
        {
            tsslTotal.Text = _Presenter.ProjectFileCountText;
            tsslStatus1.Text = _Presenter.ProjectFileValidCountText;
            tsslStatus2.Text = _Presenter.ProjectFileInvalidCountText;
            tsslListCount.Text = _Presenter.ProjectFileListCountText;

            dgvProjectTypeValidation.DataSource = _Presenter.ProjectTypeValidationList;
        }

        private void GetMissingUserControlValidationCounts()
        {
            tsslTotal.Text = _Presenter.UserControlCountText;
            tsslStatus1.Text = _Presenter.UserControlValidCountText;
            tsslStatus2.Text = _Presenter.UserControlInvalidCountText;
            tsslListCount.Text = _Presenter.UserControlListCountText;

            dgvUserControlValidation.DataSource = _Presenter.UserControlValidationList;
        }

        private void FormToPresenter()
        {
            _Presenter.SolutionFolder = tbSolutionFolder.Text;
            _Presenter.ProjectName = tbProjectName.Text;

            _Presenter.IsCS = rbCS.Checked;

            _Presenter.IsDesignerFileExistsChecked = rbDesignerFileExists.Checked;
            _Presenter.IsInProjectFileChecked = rbInProjectFile.Checked;

            _Presenter.IsWebProjectCheck = rbCheckWebProject.Checked;

            _Presenter.IsUserControlValidCheck = rbUserControlValid.Checked;
            _Presenter.IsUserControlInvalidCheck = rbUserControlInvalid.Checked;
            _Presenter.IsUserControlMissingCheck = rbUserControlMissing.Checked;
            _Presenter.IsUserControlUnusedCheck = rbUserControlUnused.Checked;

            if (_Presenter.IsValidTag(tabControl.SelectedTab.Tag))
            {
                _Presenter.TabPageType = (TabPageType)tabControl.SelectedTab.Tag;
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

        private void bRefresh_Click(object sender, EventArgs e)
        {
            FormToPresenter();

            ExecuteEvent(ExecuteEventType.Refresh);

            PresenterToForm();
        }

        private void bConvertToWebsite_Click(object sender, EventArgs e)
        {
            FormToPresenter();

            ExecuteEvent(ExecuteEventType.ConvertToWebsite);

            PresenterToForm();
        }

        private void bConvertToWebProject_Click(object sender, EventArgs e)
        {
            FormToPresenter();

            ExecuteEvent(ExecuteEventType.ConvertToWebProject);

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

        private void ExecuteEvent(ExecuteEventType executeEventType)
        {
            if (!_Presenter.ExecuteEvent(executeEventType))
            {
                MessageBox.Show(_Presenter.ErrorMessage);
            }
        }
    }
}
