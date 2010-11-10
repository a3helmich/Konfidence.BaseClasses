﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WebProjectValidator.FileListChecker;
using WebProjectValidator.HelperClasses;

namespace WebProjectValidator
{
    public partial class MainForm : Form
    {
        private string _BaseFolder = @"C:\Projects\Konfidence\KonfidenceWebSite\";

        private string _ProjectFolder = string.Empty;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _ProjectFolder = _BaseFolder + tbProjectName.Text;

            tbFolder.Text = _ProjectFolder;

            dgvDesignerFile.AutoGenerateColumns = false;
            dgvCodeFile.AutoGenerateColumns = false;
            dgvUserControl.AutoGenerateColumns = false;
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab.Equals(tpDesignerFileMissing))
            {
                DesignerFileMissing();
            }

            if (tabControl.SelectedTab.Equals(tpCodeFileCheck))
            {
                CodeFileCheck();
            }

            if (tabControl.SelectedTab.Equals(tpUserControlMissing))
            {
                UserControlMissing();
            }
        }

        private void DesignerFileMissing()
        {
            FileList fileList = new FileList(_ProjectFolder, FileType.cs, ListType.Included);
            FileList searchList = new FileList(_ProjectFolder, FileType.cs, ListType.Excluded);
            ListProcessor processor = new ListProcessor(tbProjectName.Text, _ProjectFolder, GetLanguageType());
            ListFilterType filter = ListFilterType.All;

            filter = GetDesignerFileMissingFilterType();

            dgvDesignerFile.DataSource = processor.processDesignerFile(fileList, searchList, filter);

            tsslTotal.Visible = true;
            tsslTotal.Text = "Total: " + processor.Count;
            tsslValid.Visible = true;
            tsslValid.Text = "Existing: " + processor.ValidCount;
            tsslInValid.Visible = true;
            tsslInValid.Text = "Missing: " + processor.InvalidCount;
            tsslRowCount.Visible = true;
            tsslRowCount.Text = "RowCount: " + dgvDesignerFile.RowCount;
        }

        private void CodeFileCheck()
        {
            FileList designerFileList = new FileList(_ProjectFolder, FileType.web, ListType.Included);
            ListProcessor processor = new ListProcessor(tbProjectName.Text, _ProjectFolder, GetLanguageType());
            ListFilterType filter = ListFilterType.All;

            filter = GetCodeFileCheckFilterType();

            dgvCodeFile.DataSource = processor.processCodeFileCheck(designerFileList, filter);

            tsslTotal.Visible = true;
            tsslTotal.Text = "Total: " + processor.Count;
            tsslValid.Visible = true;
            tsslValid.Text = "Valid: " + processor.ValidCount;
            tsslInValid.Visible = true;
            tsslInValid.Text = "Invalid: " + processor.InvalidCount;
            tsslRowCount.Visible = true;
            tsslRowCount.Text = "RowCount: " + dgvCodeFile.RowCount;
        }

        private void UserControlMissing()
        {
            FileList designerFileList = new FileList(_ProjectFolder, FileType.web, ListType.Included);
            ListProcessor processor = new ListProcessor(tbProjectName.Text, _ProjectFolder, GetLanguageType());
            ListFilterType filter = ListFilterType.All;

            filter = GetUserControlMissingFilterType();

            dgvUserControl.DataSource = processor.processUserControlMissing(designerFileList, filter);

            tsslTotal.Visible = true;
            tsslTotal.Text = "Total: " + processor.Count;
            tsslValid.Visible = true;
            tsslValid.Text = "Valid: " + processor.ValidCount;
            tsslInValid.Visible = true;
            tsslInValid.Text = "Invalid: " + processor.InvalidCount;
            tsslRowCount.Visible = true;
        }

        private LanguageType GetLanguageType()
        {
            if (rbCS.Checked)
            {
                return LanguageType.cs;
            }

            return LanguageType.vb;
        }

        private ListFilterType GetDesignerFileMissingFilterType()
        {
            if (rbExists.Checked)
            {
                return ListFilterType.Valid;
            }

            if (rbMissing.Checked)
            {
                return ListFilterType.Invalid;
            }

            return ListFilterType.All;
        }

        private ListFilterType GetCodeFileCheckFilterType()
        {
            if (rbValid.Checked)
            {
                return ListFilterType.Valid;
            }

            if (rbInvalid.Checked)
            {
                return ListFilterType.Invalid;
            }

            return ListFilterType.All;
        }

        private ListFilterType GetUserControlMissingFilterType()
        {
            return ListFilterType.All;
        }
    }
}
