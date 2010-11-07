using System;
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

        private void UserControlMissing()
        {
            throw new NotImplementedException();
        }

        private void CodeFileCheck()
        {
            throw new NotImplementedException();
        }

        private void DesignerFileMissing()
        {
            FileList fileList = new FileList(_ProjectFolder, FileType.cs, ListType.Included);
            FileList searchList = new FileList(_ProjectFolder, FileType.cs, ListType.Excluded);
            ListProcessor processor = new ListProcessor(tbProjectName.Text, _ProjectFolder, LanguageType.cs);
            ListFilterType filter = ListFilterType.All;

            filter = GetFilterType();

            dgvDesignerFile.DataSource = processor.processDesignerFile(fileList, searchList, filter);

            tsslTotal.Visible = true;
            tsslTotal.Text = "Total: " + processor.Count;
            tsslValid.Visible = true;
            tsslValid.Text = "Existing: " + processor.ValidCount;
            tsslInValid.Visible = true;
            tsslInValid.Text = "Missing: " + processor.InvalidCount;

            lRowCount.Text = dgvDesignerFile.RowCount.ToString();
        }

        private ListFilterType GetFilterType()
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
    }
}
