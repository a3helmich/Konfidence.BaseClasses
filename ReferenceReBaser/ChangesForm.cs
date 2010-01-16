using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Konfidence.ReferenceReBaserApp
{
    public partial class ChangesForm : Form
    {
        public ChangesForm()
        {
            InitializeComponent();
        }

        public void ShowList(List<string> overviewList)
        {
            overviewListBox.DataSource = overviewList;

            Show();
        }
    }
}
