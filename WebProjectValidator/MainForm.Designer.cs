namespace WebProjectValidator
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.lProjectName = new System.Windows.Forms.Label();
            this.tbProjectName = new System.Windows.Forms.TextBox();
            this.rbCS = new System.Windows.Forms.RadioButton();
            this.rbVB = new System.Windows.Forms.RadioButton();
            this.bStart = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpDesignerFileMissing = new System.Windows.Forms.TabPage();
            this.panel5 = new System.Windows.Forms.Panel();
            this.dgvDesignerFile = new System.Windows.Forms.DataGridView();
            this.dgvFileMissingProject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvFileMissingFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvFileMissingExists = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvFileMissingControlFolder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rbExists = new System.Windows.Forms.RadioButton();
            this.rbMissing = new System.Windows.Forms.RadioButton();
            this.tpCodeFileCheck = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvCodeFile = new System.Windows.Forms.DataGridView();
            this.dgvCodeFileProject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCodeFileFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCodeFileValid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCodeFileControlFolder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbCodeFileCheckAll = new System.Windows.Forms.RadioButton();
            this.rbValid = new System.Windows.Forms.RadioButton();
            this.rbInvalid = new System.Windows.Forms.RadioButton();
            this.tpUserControlMissing = new System.Windows.Forms.TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.dgvUserControl = new System.Windows.Forms.DataGridView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tbFolder = new System.Windows.Forms.TextBox();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.tsslInValid = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslValid = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslTotal = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslRowCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl.SuspendLayout();
            this.tpDesignerFileMissing.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDesignerFile)).BeginInit();
            this.panel3.SuspendLayout();
            this.tpCodeFileCheck.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCodeFile)).BeginInit();
            this.panel1.SuspendLayout();
            this.tpUserControlMissing.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserControl)).BeginInit();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // lProjectName
            // 
            this.lProjectName.AutoSize = true;
            this.lProjectName.Location = new System.Drawing.Point(19, 48);
            this.lProjectName.Name = "lProjectName";
            this.lProjectName.Size = new System.Drawing.Size(40, 13);
            this.lProjectName.TabIndex = 0;
            this.lProjectName.Text = "Project";
            // 
            // tbProjectName
            // 
            this.tbProjectName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbProjectName.Location = new System.Drawing.Point(67, 46);
            this.tbProjectName.Name = "tbProjectName";
            this.tbProjectName.Size = new System.Drawing.Size(218, 20);
            this.tbProjectName.TabIndex = 1;
            this.tbProjectName.Text = "Konfidence";
            // 
            // rbCS
            // 
            this.rbCS.AutoSize = true;
            this.rbCS.Checked = true;
            this.rbCS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbCS.Location = new System.Drawing.Point(167, 72);
            this.rbCS.Name = "rbCS";
            this.rbCS.Size = new System.Drawing.Size(38, 17);
            this.rbCS.TabIndex = 1;
            this.rbCS.TabStop = true;
            this.rbCS.Text = "C#";
            this.rbCS.UseVisualStyleBackColor = true;
            // 
            // rbVB
            // 
            this.rbVB.AutoSize = true;
            this.rbVB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbVB.Location = new System.Drawing.Point(67, 72);
            this.rbVB.Name = "rbVB";
            this.rbVB.Size = new System.Drawing.Size(38, 17);
            this.rbVB.TabIndex = 0;
            this.rbVB.Text = "VB";
            this.rbVB.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.rbVB.UseVisualStyleBackColor = true;
            // 
            // bStart
            // 
            this.bStart.ForeColor = System.Drawing.SystemColors.ControlText;
            this.bStart.Location = new System.Drawing.Point(22, 12);
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(75, 23);
            this.bStart.TabIndex = 3;
            this.bStart.Text = "Start";
            this.bStart.UseVisualStyleBackColor = true;
            this.bStart.Click += new System.EventHandler(this.bStart_Click);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tpDesignerFileMissing);
            this.tabControl.Controls.Add(this.tpCodeFileCheck);
            this.tabControl.Controls.Add(this.tpUserControlMissing);
            this.tabControl.Location = new System.Drawing.Point(16, 102);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(956, 322);
            this.tabControl.TabIndex = 4;
            // 
            // tpDesignerFileMissing
            // 
            this.tpDesignerFileMissing.Controls.Add(this.panel5);
            this.tpDesignerFileMissing.Controls.Add(this.panel3);
            this.tpDesignerFileMissing.Location = new System.Drawing.Point(4, 22);
            this.tpDesignerFileMissing.Name = "tpDesignerFileMissing";
            this.tpDesignerFileMissing.Padding = new System.Windows.Forms.Padding(3);
            this.tpDesignerFileMissing.Size = new System.Drawing.Size(948, 296);
            this.tpDesignerFileMissing.TabIndex = 1;
            this.tpDesignerFileMissing.Text = "Designer File Missing";
            this.tpDesignerFileMissing.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.dgvDesignerFile);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 33);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(942, 260);
            this.panel5.TabIndex = 1;
            // 
            // dgvDesignerFile
            // 
            this.dgvDesignerFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDesignerFile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvFileMissingProject,
            this.dgvFileMissingFileName,
            this.dgvFileMissingExists,
            this.dgvFileMissingControlFolder});
            this.dgvDesignerFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDesignerFile.Location = new System.Drawing.Point(0, 0);
            this.dgvDesignerFile.Name = "dgvDesignerFile";
            this.dgvDesignerFile.RowHeadersVisible = false;
            this.dgvDesignerFile.Size = new System.Drawing.Size(942, 260);
            this.dgvDesignerFile.TabIndex = 0;
            // 
            // dgvFileMissingProject
            // 
            this.dgvFileMissingProject.DataPropertyName = "Project";
            this.dgvFileMissingProject.HeaderText = "Project";
            this.dgvFileMissingProject.Name = "dgvFileMissingProject";
            this.dgvFileMissingProject.ReadOnly = true;
            // 
            // dgvFileMissingFileName
            // 
            this.dgvFileMissingFileName.DataPropertyName = "FileName";
            this.dgvFileMissingFileName.HeaderText = "Filename";
            this.dgvFileMissingFileName.Name = "dgvFileMissingFileName";
            this.dgvFileMissingFileName.ReadOnly = true;
            this.dgvFileMissingFileName.Width = 200;
            // 
            // dgvFileMissingExists
            // 
            this.dgvFileMissingExists.DataPropertyName = "Valid";
            this.dgvFileMissingExists.HeaderText = "Exists";
            this.dgvFileMissingExists.Name = "dgvFileMissingExists";
            this.dgvFileMissingExists.ReadOnly = true;
            this.dgvFileMissingExists.Width = 55;
            // 
            // dgvFileMissingControlFolder
            // 
            this.dgvFileMissingControlFolder.DataPropertyName = "ControlFolder";
            this.dgvFileMissingControlFolder.HeaderText = "Folder";
            this.dgvFileMissingControlFolder.Name = "dgvFileMissingControlFolder";
            this.dgvFileMissingControlFolder.ReadOnly = true;
            this.dgvFileMissingControlFolder.Width = 565;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rbAll);
            this.panel3.Controls.Add(this.rbExists);
            this.panel3.Controls.Add(this.rbMissing);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(942, 30);
            this.panel3.TabIndex = 0;
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbAll.Location = new System.Drawing.Point(146, 7);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(35, 17);
            this.rbAll.TabIndex = 4;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "All";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // rbExists
            // 
            this.rbExists.AutoSize = true;
            this.rbExists.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbExists.Location = new System.Drawing.Point(78, 7);
            this.rbExists.Name = "rbExists";
            this.rbExists.Size = new System.Drawing.Size(60, 17);
            this.rbExists.TabIndex = 3;
            this.rbExists.Text = "Existing";
            this.rbExists.UseVisualStyleBackColor = true;
            // 
            // rbMissing
            // 
            this.rbMissing.AutoSize = true;
            this.rbMissing.Checked = true;
            this.rbMissing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbMissing.Location = new System.Drawing.Point(12, 7);
            this.rbMissing.Name = "rbMissing";
            this.rbMissing.Size = new System.Drawing.Size(59, 17);
            this.rbMissing.TabIndex = 2;
            this.rbMissing.TabStop = true;
            this.rbMissing.Text = "Missing";
            this.rbMissing.UseVisualStyleBackColor = true;
            // 
            // tpCodeFileCheck
            // 
            this.tpCodeFileCheck.Controls.Add(this.panel2);
            this.tpCodeFileCheck.Controls.Add(this.panel1);
            this.tpCodeFileCheck.Location = new System.Drawing.Point(4, 22);
            this.tpCodeFileCheck.Name = "tpCodeFileCheck";
            this.tpCodeFileCheck.Padding = new System.Windows.Forms.Padding(3);
            this.tpCodeFileCheck.Size = new System.Drawing.Size(948, 296);
            this.tpCodeFileCheck.TabIndex = 0;
            this.tpCodeFileCheck.Text = "CodeFile Check";
            this.tpCodeFileCheck.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvCodeFile);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 33);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(942, 260);
            this.panel2.TabIndex = 1;
            // 
            // dgvCodeFile
            // 
            this.dgvCodeFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCodeFile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvCodeFileProject,
            this.dgvCodeFileFileName,
            this.dgvCodeFileValid,
            this.dgvCodeFileControlFolder});
            this.dgvCodeFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCodeFile.Location = new System.Drawing.Point(0, 0);
            this.dgvCodeFile.Name = "dgvCodeFile";
            this.dgvCodeFile.RowHeadersVisible = false;
            this.dgvCodeFile.Size = new System.Drawing.Size(942, 260);
            this.dgvCodeFile.TabIndex = 0;
            // 
            // dgvCodeFileProject
            // 
            this.dgvCodeFileProject.DataPropertyName = "Project";
            this.dgvCodeFileProject.HeaderText = "Project";
            this.dgvCodeFileProject.Name = "dgvCodeFileProject";
            this.dgvCodeFileProject.ReadOnly = true;
            // 
            // dgvCodeFileFileName
            // 
            this.dgvCodeFileFileName.DataPropertyName = "FileName";
            this.dgvCodeFileFileName.HeaderText = "File";
            this.dgvCodeFileFileName.Name = "dgvCodeFileFileName";
            this.dgvCodeFileFileName.ReadOnly = true;
            this.dgvCodeFileFileName.Width = 200;
            // 
            // dgvCodeFileValid
            // 
            this.dgvCodeFileValid.DataPropertyName = "Valid";
            this.dgvCodeFileValid.HeaderText = "Valid";
            this.dgvCodeFileValid.Name = "dgvCodeFileValid";
            this.dgvCodeFileValid.ReadOnly = true;
            this.dgvCodeFileValid.Width = 55;
            // 
            // dgvCodeFileControlFolder
            // 
            this.dgvCodeFileControlFolder.DataPropertyName = "ControlFolder";
            this.dgvCodeFileControlFolder.HeaderText = "Folder";
            this.dgvCodeFileControlFolder.Name = "dgvCodeFileControlFolder";
            this.dgvCodeFileControlFolder.ReadOnly = true;
            this.dgvCodeFileControlFolder.Width = 565;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbCodeFileCheckAll);
            this.panel1.Controls.Add(this.rbValid);
            this.panel1.Controls.Add(this.rbInvalid);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(942, 30);
            this.panel1.TabIndex = 0;
            // 
            // rbCodeFileCheckAll
            // 
            this.rbCodeFileCheckAll.AutoSize = true;
            this.rbCodeFileCheckAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbCodeFileCheckAll.Location = new System.Drawing.Point(146, 7);
            this.rbCodeFileCheckAll.Name = "rbCodeFileCheckAll";
            this.rbCodeFileCheckAll.Size = new System.Drawing.Size(35, 17);
            this.rbCodeFileCheckAll.TabIndex = 7;
            this.rbCodeFileCheckAll.TabStop = true;
            this.rbCodeFileCheckAll.Text = "All";
            this.rbCodeFileCheckAll.UseVisualStyleBackColor = true;
            // 
            // rbValid
            // 
            this.rbValid.AutoSize = true;
            this.rbValid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbValid.Location = new System.Drawing.Point(78, 7);
            this.rbValid.Name = "rbValid";
            this.rbValid.Size = new System.Drawing.Size(47, 17);
            this.rbValid.TabIndex = 6;
            this.rbValid.Text = "Valid";
            this.rbValid.UseVisualStyleBackColor = true;
            // 
            // rbInvalid
            // 
            this.rbInvalid.AutoSize = true;
            this.rbInvalid.Checked = true;
            this.rbInvalid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbInvalid.Location = new System.Drawing.Point(12, 7);
            this.rbInvalid.Name = "rbInvalid";
            this.rbInvalid.Size = new System.Drawing.Size(55, 17);
            this.rbInvalid.TabIndex = 5;
            this.rbInvalid.TabStop = true;
            this.rbInvalid.Text = "Invalid";
            this.rbInvalid.UseVisualStyleBackColor = true;
            // 
            // tpUserControlMissing
            // 
            this.tpUserControlMissing.Controls.Add(this.panel6);
            this.tpUserControlMissing.Controls.Add(this.panel4);
            this.tpUserControlMissing.Location = new System.Drawing.Point(4, 22);
            this.tpUserControlMissing.Name = "tpUserControlMissing";
            this.tpUserControlMissing.Padding = new System.Windows.Forms.Padding(3);
            this.tpUserControlMissing.Size = new System.Drawing.Size(948, 296);
            this.tpUserControlMissing.TabIndex = 2;
            this.tpUserControlMissing.Text = "User Control Missing";
            this.tpUserControlMissing.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.dgvUserControl);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(3, 33);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(942, 260);
            this.panel6.TabIndex = 1;
            // 
            // dgvUserControl
            // 
            this.dgvUserControl.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUserControl.Location = new System.Drawing.Point(0, 0);
            this.dgvUserControl.Name = "dgvUserControl";
            this.dgvUserControl.Size = new System.Drawing.Size(942, 260);
            this.dgvUserControl.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(942, 30);
            this.panel4.TabIndex = 0;
            // 
            // tbFolder
            // 
            this.tbFolder.Location = new System.Drawing.Point(291, 46);
            this.tbFolder.Name = "tbFolder";
            this.tbFolder.ReadOnly = true;
            this.tbFolder.Size = new System.Drawing.Size(393, 20);
            this.tbFolder.TabIndex = 2;
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslInValid,
            this.tsslValid,
            this.tsslTotal,
            this.tsslRowCount});
            this.statusBar.Location = new System.Drawing.Point(0, 427);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(984, 22);
            this.statusBar.TabIndex = 5;
            this.statusBar.Text = "statusStrip1";
            // 
            // tsslInValid
            // 
            this.tsslInValid.Name = "tsslInValid";
            this.tsslInValid.Size = new System.Drawing.Size(48, 17);
            this.tsslInValid.Text = "Missing";
            this.tsslInValid.Visible = false;
            // 
            // tsslValid
            // 
            this.tsslValid.Name = "tsslValid";
            this.tsslValid.Size = new System.Drawing.Size(35, 17);
            this.tsslValid.Text = "Exists";
            this.tsslValid.Visible = false;
            // 
            // tsslTotal
            // 
            this.tsslTotal.Name = "tsslTotal";
            this.tsslTotal.Size = new System.Drawing.Size(34, 17);
            this.tsslTotal.Text = "Total";
            this.tsslTotal.Visible = false;
            // 
            // tsslRowCount
            // 
            this.tsslRowCount.Name = "tsslRowCount";
            this.tsslRowCount.Size = new System.Drawing.Size(63, 17);
            this.tsslRowCount.Text = "RowCount";
            this.tsslRowCount.Visible = false;
            // 
            // MainForm
            // 
            this.AcceptButton = this.bStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 449);
            this.Controls.Add(this.rbVB);
            this.Controls.Add(this.rbCS);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.bStart);
            this.Controls.Add(this.tbFolder);
            this.Controls.Add(this.tbProjectName);
            this.Controls.Add(this.lProjectName);
            this.Name = "MainForm";
            this.Text = "Web Project Validator";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tpDesignerFileMissing.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDesignerFile)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tpCodeFileCheck.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCodeFile)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tpUserControlMissing.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserControl)).EndInit();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lProjectName;
        private System.Windows.Forms.TextBox tbProjectName;
        private System.Windows.Forms.RadioButton rbVB;
        private System.Windows.Forms.RadioButton rbCS;
        private System.Windows.Forms.Button bStart;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpCodeFileCheck;
        private System.Windows.Forms.TabPage tpDesignerFileMissing;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TabPage tpUserControlMissing;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridView dgvCodeFile;
        private System.Windows.Forms.DataGridView dgvDesignerFile;
        private System.Windows.Forms.DataGridView dgvUserControl;
        private System.Windows.Forms.TextBox tbFolder;
        private System.Windows.Forms.RadioButton rbExists;
        private System.Windows.Forms.RadioButton rbMissing;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel tsslValid;
        private System.Windows.Forms.ToolStripStatusLabel tsslInValid;
        private System.Windows.Forms.ToolStripStatusLabel tsslTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvFileMissingProject;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvFileMissingFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvFileMissingExists;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvFileMissingControlFolder;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvCodeFileProject;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvCodeFileFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvCodeFileValid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvCodeFileControlFolder;
        private System.Windows.Forms.RadioButton rbCodeFileCheckAll;
        private System.Windows.Forms.RadioButton rbValid;
        private System.Windows.Forms.RadioButton rbInvalid;
        private System.Windows.Forms.ToolStripStatusLabel tsslRowCount;
    }
}

