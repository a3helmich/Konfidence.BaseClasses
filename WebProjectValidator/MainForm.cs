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

            tbFolder.Text = _BaseFolder;
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            FileList fileList = new FileList(_ProjectFolder, FileType.cs);
            ListProcessor processor = new ListProcessor(tbProjectName.Text, _ProjectFolder, LanguageType.cs);
            ListFilterType filter = ListFilterType.All;

            filter = GetFilterType();

            dgvDesignerFile.DataSource = processor.processDesignerFile(fileList, filter);

            tsslTotal.Visible = true;
            tsslTotal.Text = "Total: " + processor.Count;
            tsslValid.Visible = true;
            tsslValid.Text = "Existing: " + processor.ValidCount;
            tsslInValid.Visible = true;
            tsslInValid.Text = "Missing: " + processor.InvalidCount;

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
