using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            FileList fileList = new FileList(_ProjectFolder, FileType.web);

        }
    }
}
