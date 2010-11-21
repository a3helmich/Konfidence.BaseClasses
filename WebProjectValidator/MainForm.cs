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

            dgvDesignerFileMissing.AutoGenerateColumns = false;
            dgvCodeFileCheck.AutoGenerateColumns = false;
            dgvUserControlMissing.AutoGenerateColumns = false;

            LoadDefaults();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveDefaults();
        }

        private void SaveDefaults()
        {
            ConfigurationStore configurationStore = new ConfigurationStore();

            configurationStore.SetProperty("ProjectName", tbProjectName.Text);
            configurationStore.SetProperty("ProjectFolder", tbFolder.Text);

            string rbCSText = "0";
            if (rbCS.Checked)
            {
                rbCSText = "1";
            }

            configurationStore.SetProperty("rbCSChecked", rbCSText);

            configurationStore.Save();
        }

        private void LoadDefaults()
        {
            ConfigurationStore configurationStore = new ConfigurationStore();

            string getText = string.Empty;

            configurationStore.GetProperty("ProjectName", out getText);

            tbProjectName.Text = getText;

            configurationStore.GetProperty("ProjectFolder", out getText);

            tbFolder.Text = getText;

            configurationStore.GetProperty("rbCSChecked", out getText);

            rbCS.Checked = false;
            rbVB.Checked = false;
            if (getText.Equals("1"))
            {
                rbCS.Checked = true;
            }
            else
            {
                rbVB.Checked = true;
            }
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

        private FileType GetLanguageFileType()
        {
            LanguageType languageType = GetLanguageType();

            switch (languageType)
            {
                case LanguageType.cs:
                    return FileType.cs;
                case LanguageType.vb:
                    return FileType.vb;
            }

            return FileType.web;
        }

        private void DesignerFileMissing()
        {

            FileList fileList = new FileList(_ProjectFolder, GetLanguageFileType(), ListType.Included);
            FileList searchList = new FileList(_ProjectFolder, GetLanguageFileType(), ListType.Excluded);
            ListProcessor processor = new ListProcessor(tbProjectName.Text, _ProjectFolder, GetLanguageType());
            ListFilterType filter = ListFilterType.All;

            filter = GetDesignerFileMissingFilterType();

            dgvDesignerFileMissing.DataSource = processor.processDesignerFileMissing(fileList, searchList, filter);

            tsslTotal.Visible = true;
            tsslTotal.Text = "Total: " + processor.Count;
            tsslValid.Visible = true;
            tsslValid.Text = "Existing: " + processor.ValidCount;
            tsslInValid.Visible = true;
            tsslInValid.Text = "Missing: " + processor.InvalidCount;
            tsslRowCount.Visible = true;
            tsslRowCount.Text = "RowCount: " + dgvDesignerFileMissing.RowCount;
        }

        private void CodeFileCheck()
        {
            FileList designerFileList = new FileList(_ProjectFolder, FileType.web, ListType.Included);
            ListProcessor processor = new ListProcessor(tbProjectName.Text, _ProjectFolder, GetLanguageType());
            ListFilterType filter = ListFilterType.All;

            filter = GetCodeFileCheckFilterType();

            dgvCodeFileCheck.DataSource = processor.processCodeFileCheck(designerFileList, filter);

            tsslTotal.Visible = true;
            tsslTotal.Text = "Total: " + processor.Count;
            tsslValid.Visible = true;
            tsslValid.Text = "Valid: " + processor.ValidCount;
            tsslInValid.Visible = true;
            tsslInValid.Text = "Invalid: " + processor.InvalidCount;
            tsslRowCount.Visible = true;
            tsslRowCount.Text = "RowCount: " + dgvCodeFileCheck.RowCount;
        }

        private void UserControlMissing()
        {
            FileList designerFileList = new FileList(_ProjectFolder, FileType.web, ListType.Included);
            ListProcessor processor = new ListProcessor(tbProjectName.Text, _ProjectFolder, GetLanguageType());
            ListFilterType filter = ListFilterType.All;

            filter = GetUserControlMissingFilterType();

            dgvUserControlMissing.DataSource = processor.processUserControlMissing(designerFileList, filter);

            tsslTotal.Visible = true;
            tsslTotal.Text = "Total: " + processor.Count;
            tsslValid.Visible = true;
            tsslValid.Text = "Valid: " + processor.ValidCount;
            tsslInValid.Visible = true;
            tsslInValid.Text = "Invalid: " + processor.InvalidCount;
            tsslRowCount.Visible = true;
            tsslRowCount.Text = "RowCount: " + dgvUserControlMissing.RowCount;
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
            if (rbDesignerFileExists.Checked)
            {
                return ListFilterType.Exists;
            }

            if (rbDesignerFileMissing.Checked)
            {
                return ListFilterType.Missing;
            }

            return ListFilterType.All;
        }

        private ListFilterType GetCodeFileCheckFilterType()
        {
            if (rbCodeFileCheckValid.Checked)
            {
                return ListFilterType.Valid;
            }

            if (rbCodeFileCheckInvalid.Checked)
            {
                return ListFilterType.Invalid;
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
                return ListFilterType.Missing;
            }

            if (rbUserControlUnused.Checked)
            {
                return ListFilterType.Unused;
            }

            return ListFilterType.All;
        }

        private void bFixAll_Click(object sender, EventArgs e)
        {
            if (dgvCodeFileCheck.RowCount > 0)
            {
                FileList designerFileList = new FileList(_ProjectFolder, FileType.web, ListType.Included);
                ListProcessor processor = new ListProcessor(tbProjectName.Text, _ProjectFolder, GetLanguageType());
                ListFilterType filter = ListFilterType.All;

                filter = GetCodeFileCheckFilterType();

                List<DesignerFileItem> repairList = processor.processCodeFileCheck(designerFileList, filter);

                processor.repairCodeFile(repairList);
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tpCodeFileCheck)
            {
                bFixAll.Enabled = true;
            }
            else
            {
                bFixAll.Enabled = false;
            }
        }

        private void bFolderBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = tbFolder.Text;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tbFolder.Text = folderBrowserDialog.SelectedPath;
            }
        }
    }
}
