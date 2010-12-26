﻿namespace WebProjectValidator
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lProjectName = new System.Windows.Forms.Label();
            this.tbProjectName = new System.Windows.Forms.TextBox();
            this.rbCS = new System.Windows.Forms.RadioButton();
            this.rbVB = new System.Windows.Forms.RadioButton();
            this.bStart = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpDesignerFileMissing = new System.Windows.Forms.TabPage();
            this.panel5 = new System.Windows.Forms.Panel();
            this.dgvDesignerFileMissing = new System.Windows.Forms.DataGridView();
            this.dgvFileMissingProject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvFileMissingFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvFileMissingExists = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvFileMissingControlFolder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rbDesignerFileAll = new System.Windows.Forms.RadioButton();
            this.rbDesignerFileExists = new System.Windows.Forms.RadioButton();
            this.rbDesignerFileMissing = new System.Windows.Forms.RadioButton();
            this.tpCodeFileCheck = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvCodeFileCheck = new System.Windows.Forms.DataGridView();
            this.dgvCodeFileProject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCodeFileFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCodeFileValid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCodeFileControlFolder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbCheckWebProject = new System.Windows.Forms.RadioButton();
            this.rbCheckWebApplication = new System.Windows.Forms.RadioButton();
            this.tpUserControlMissing = new System.Windows.Forms.TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.dgvUserControlMissing = new System.Windows.Forms.DataGridView();
            this.dgvUserControlMissingProject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvUserControlMissingFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvUserControlMissingValid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvUserControlMissingReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel4 = new System.Windows.Forms.Panel();
            this.rbUserControlUnused = new System.Windows.Forms.RadioButton();
            this.rbUserControlMissing = new System.Windows.Forms.RadioButton();
            this.rbUserControlAll = new System.Windows.Forms.RadioButton();
            this.rbUserControlValid = new System.Windows.Forms.RadioButton();
            this.rbUserControlInvalid = new System.Windows.Forms.RadioButton();
            this.tbSolutionFolder = new System.Windows.Forms.TextBox();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.tsslInValid = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslValid = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslTotal = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslRowCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.bConvertToWebApplication = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.bFolderBrowse = new System.Windows.Forms.Button();
            this.lProjectFileNameDisplay = new System.Windows.Forms.Label();
            this.lProjectFileName = new System.Windows.Forms.Label();
            this.bConvertToWebProject = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tpDesignerFileMissing.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDesignerFileMissing)).BeginInit();
            this.panel3.SuspendLayout();
            this.tpCodeFileCheck.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCodeFileCheck)).BeginInit();
            this.panel1.SuspendLayout();
            this.tpUserControlMissing.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserControlMissing)).BeginInit();
            this.panel4.SuspendLayout();
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
            this.rbCS.Location = new System.Drawing.Point(123, 72);
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
            this.bStart.Size = new System.Drawing.Size(90, 23);
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
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
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
            this.tpDesignerFileMissing.Text = "Designer file";
            this.tpDesignerFileMissing.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.dgvDesignerFileMissing);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 33);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(942, 260);
            this.panel5.TabIndex = 1;
            // 
            // dgvDesignerFileMissing
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDesignerFileMissing.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDesignerFileMissing.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDesignerFileMissing.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvFileMissingProject,
            this.dgvFileMissingFileName,
            this.dgvFileMissingExists,
            this.dgvFileMissingControlFolder});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDesignerFileMissing.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDesignerFileMissing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDesignerFileMissing.Location = new System.Drawing.Point(0, 0);
            this.dgvDesignerFileMissing.Name = "dgvDesignerFileMissing";
            this.dgvDesignerFileMissing.RowHeadersVisible = false;
            this.dgvDesignerFileMissing.Size = new System.Drawing.Size(942, 260);
            this.dgvDesignerFileMissing.TabIndex = 0;
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
            this.dgvFileMissingExists.DataPropertyName = "Exists";
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
            this.panel3.Controls.Add(this.rbDesignerFileAll);
            this.panel3.Controls.Add(this.rbDesignerFileExists);
            this.panel3.Controls.Add(this.rbDesignerFileMissing);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(942, 30);
            this.panel3.TabIndex = 0;
            // 
            // rbDesignerFileAll
            // 
            this.rbDesignerFileAll.AutoSize = true;
            this.rbDesignerFileAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbDesignerFileAll.Location = new System.Drawing.Point(145, 7);
            this.rbDesignerFileAll.Name = "rbDesignerFileAll";
            this.rbDesignerFileAll.Size = new System.Drawing.Size(35, 17);
            this.rbDesignerFileAll.TabIndex = 4;
            this.rbDesignerFileAll.TabStop = true;
            this.rbDesignerFileAll.Text = "All";
            this.rbDesignerFileAll.UseVisualStyleBackColor = true;
            // 
            // rbDesignerFileExists
            // 
            this.rbDesignerFileExists.AutoSize = true;
            this.rbDesignerFileExists.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbDesignerFileExists.Location = new System.Drawing.Point(78, 7);
            this.rbDesignerFileExists.Name = "rbDesignerFileExists";
            this.rbDesignerFileExists.Size = new System.Drawing.Size(60, 17);
            this.rbDesignerFileExists.TabIndex = 3;
            this.rbDesignerFileExists.Text = "Existing";
            this.rbDesignerFileExists.UseVisualStyleBackColor = true;
            // 
            // rbDesignerFileMissing
            // 
            this.rbDesignerFileMissing.AutoSize = true;
            this.rbDesignerFileMissing.Checked = true;
            this.rbDesignerFileMissing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbDesignerFileMissing.Location = new System.Drawing.Point(12, 7);
            this.rbDesignerFileMissing.Name = "rbDesignerFileMissing";
            this.rbDesignerFileMissing.Size = new System.Drawing.Size(59, 17);
            this.rbDesignerFileMissing.TabIndex = 2;
            this.rbDesignerFileMissing.TabStop = true;
            this.rbDesignerFileMissing.Text = "Missing";
            this.rbDesignerFileMissing.UseVisualStyleBackColor = true;
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
            this.tpCodeFileCheck.Text = "Project type validation";
            this.tpCodeFileCheck.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvCodeFileCheck);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 33);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(942, 260);
            this.panel2.TabIndex = 1;
            // 
            // dgvCodeFileCheck
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCodeFileCheck.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvCodeFileCheck.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCodeFileCheck.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvCodeFileProject,
            this.dgvCodeFileFileName,
            this.dgvCodeFileValid,
            this.dgvCodeFileControlFolder});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCodeFileCheck.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvCodeFileCheck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCodeFileCheck.Location = new System.Drawing.Point(0, 0);
            this.dgvCodeFileCheck.Name = "dgvCodeFileCheck";
            this.dgvCodeFileCheck.RowHeadersVisible = false;
            this.dgvCodeFileCheck.Size = new System.Drawing.Size(942, 260);
            this.dgvCodeFileCheck.TabIndex = 0;
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
            this.panel1.Controls.Add(this.rbCheckWebProject);
            this.panel1.Controls.Add(this.rbCheckWebApplication);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(942, 30);
            this.panel1.TabIndex = 0;
            // 
            // rbCheckWebProject
            // 
            this.rbCheckWebProject.AutoSize = true;
            this.rbCheckWebProject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbCheckWebProject.Location = new System.Drawing.Point(145, 7);
            this.rbCheckWebProject.Name = "rbCheckWebProject";
            this.rbCheckWebProject.Size = new System.Drawing.Size(82, 17);
            this.rbCheckWebProject.TabIndex = 6;
            this.rbCheckWebProject.Text = "Web project";
            this.rbCheckWebProject.UseVisualStyleBackColor = true;
            // 
            // rbCheckWebApplication
            // 
            this.rbCheckWebApplication.AutoSize = true;
            this.rbCheckWebApplication.Checked = true;
            this.rbCheckWebApplication.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbCheckWebApplication.Location = new System.Drawing.Point(12, 7);
            this.rbCheckWebApplication.Name = "rbCheckWebApplication";
            this.rbCheckWebApplication.Size = new System.Drawing.Size(101, 17);
            this.rbCheckWebApplication.TabIndex = 5;
            this.rbCheckWebApplication.TabStop = true;
            this.rbCheckWebApplication.Text = "Web application";
            this.rbCheckWebApplication.UseVisualStyleBackColor = true;
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
            this.tpUserControlMissing.Text = "User control missing";
            this.tpUserControlMissing.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.dgvUserControlMissing);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(3, 33);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(942, 260);
            this.panel6.TabIndex = 1;
            // 
            // dgvUserControlMissing
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUserControlMissing.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvUserControlMissing.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUserControlMissing.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvUserControlMissingProject,
            this.dgvUserControlMissingFileName,
            this.dgvUserControlMissingValid,
            this.dgvUserControlMissingReason});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvUserControlMissing.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvUserControlMissing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUserControlMissing.Location = new System.Drawing.Point(0, 0);
            this.dgvUserControlMissing.Name = "dgvUserControlMissing";
            this.dgvUserControlMissing.RowHeadersVisible = false;
            this.dgvUserControlMissing.Size = new System.Drawing.Size(942, 260);
            this.dgvUserControlMissing.TabIndex = 0;
            // 
            // dgvUserControlMissingProject
            // 
            this.dgvUserControlMissingProject.DataPropertyName = "Project";
            this.dgvUserControlMissingProject.HeaderText = "Project";
            this.dgvUserControlMissingProject.Name = "dgvUserControlMissingProject";
            this.dgvUserControlMissingProject.ReadOnly = true;
            // 
            // dgvUserControlMissingFileName
            // 
            this.dgvUserControlMissingFileName.DataPropertyName = "Reference";
            this.dgvUserControlMissingFileName.HeaderText = "Reference";
            this.dgvUserControlMissingFileName.Name = "dgvUserControlMissingFileName";
            this.dgvUserControlMissingFileName.ReadOnly = true;
            this.dgvUserControlMissingFileName.Width = 400;
            // 
            // dgvUserControlMissingValid
            // 
            this.dgvUserControlMissingValid.DataPropertyName = "Valid";
            this.dgvUserControlMissingValid.HeaderText = "Valid";
            this.dgvUserControlMissingValid.Name = "dgvUserControlMissingValid";
            this.dgvUserControlMissingValid.ReadOnly = true;
            this.dgvUserControlMissingValid.Width = 55;
            // 
            // dgvUserControlMissingReason
            // 
            this.dgvUserControlMissingReason.DataPropertyName = "ErrorMessage";
            this.dgvUserControlMissingReason.HeaderText = "Reason";
            this.dgvUserControlMissingReason.Name = "dgvUserControlMissingReason";
            this.dgvUserControlMissingReason.ReadOnly = true;
            this.dgvUserControlMissingReason.Width = 365;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.rbUserControlUnused);
            this.panel4.Controls.Add(this.rbUserControlMissing);
            this.panel4.Controls.Add(this.rbUserControlAll);
            this.panel4.Controls.Add(this.rbUserControlValid);
            this.panel4.Controls.Add(this.rbUserControlInvalid);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(942, 30);
            this.panel4.TabIndex = 0;
            // 
            // rbUserControlUnused
            // 
            this.rbUserControlUnused.AutoSize = true;
            this.rbUserControlUnused.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbUserControlUnused.Location = new System.Drawing.Point(225, 7);
            this.rbUserControlUnused.Name = "rbUserControlUnused";
            this.rbUserControlUnused.Size = new System.Drawing.Size(61, 17);
            this.rbUserControlUnused.TabIndex = 12;
            this.rbUserControlUnused.TabStop = true;
            this.rbUserControlUnused.Text = "Unused";
            this.rbUserControlUnused.UseVisualStyleBackColor = true;
            // 
            // rbUserControlMissing
            // 
            this.rbUserControlMissing.AutoSize = true;
            this.rbUserControlMissing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbUserControlMissing.Location = new System.Drawing.Point(145, 7);
            this.rbUserControlMissing.Name = "rbUserControlMissing";
            this.rbUserControlMissing.Size = new System.Drawing.Size(59, 17);
            this.rbUserControlMissing.TabIndex = 11;
            this.rbUserControlMissing.TabStop = true;
            this.rbUserControlMissing.Text = "Missing";
            this.rbUserControlMissing.UseVisualStyleBackColor = true;
            // 
            // rbUserControlAll
            // 
            this.rbUserControlAll.AutoSize = true;
            this.rbUserControlAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbUserControlAll.Location = new System.Drawing.Point(304, 7);
            this.rbUserControlAll.Name = "rbUserControlAll";
            this.rbUserControlAll.Size = new System.Drawing.Size(35, 17);
            this.rbUserControlAll.TabIndex = 10;
            this.rbUserControlAll.TabStop = true;
            this.rbUserControlAll.Text = "All";
            this.rbUserControlAll.UseVisualStyleBackColor = true;
            // 
            // rbUserControlValid
            // 
            this.rbUserControlValid.AutoSize = true;
            this.rbUserControlValid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbUserControlValid.Location = new System.Drawing.Point(78, 7);
            this.rbUserControlValid.Name = "rbUserControlValid";
            this.rbUserControlValid.Size = new System.Drawing.Size(47, 17);
            this.rbUserControlValid.TabIndex = 9;
            this.rbUserControlValid.Text = "Valid";
            this.rbUserControlValid.UseVisualStyleBackColor = true;
            // 
            // rbUserControlInvalid
            // 
            this.rbUserControlInvalid.AutoSize = true;
            this.rbUserControlInvalid.Checked = true;
            this.rbUserControlInvalid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbUserControlInvalid.Location = new System.Drawing.Point(12, 7);
            this.rbUserControlInvalid.Name = "rbUserControlInvalid";
            this.rbUserControlInvalid.Size = new System.Drawing.Size(55, 17);
            this.rbUserControlInvalid.TabIndex = 8;
            this.rbUserControlInvalid.TabStop = true;
            this.rbUserControlInvalid.Text = "Invalid";
            this.rbUserControlInvalid.UseVisualStyleBackColor = true;
            // 
            // tbSolutionFolder
            // 
            this.tbSolutionFolder.Location = new System.Drawing.Point(291, 46);
            this.tbSolutionFolder.Name = "tbSolutionFolder";
            this.tbSolutionFolder.ReadOnly = true;
            this.tbSolutionFolder.Size = new System.Drawing.Size(393, 20);
            this.tbSolutionFolder.TabIndex = 2;
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
            // bConvertToWebApplication
            // 
            this.bConvertToWebApplication.Enabled = false;
            this.bConvertToWebApplication.Location = new System.Drawing.Point(123, 12);
            this.bConvertToWebApplication.Name = "bConvertToWebApplication";
            this.bConvertToWebApplication.Size = new System.Drawing.Size(90, 23);
            this.bConvertToWebApplication.TabIndex = 6;
            this.bConvertToWebApplication.Text = "Convert to App";
            this.bConvertToWebApplication.UseVisualStyleBackColor = true;
            this.bConvertToWebApplication.Click += new System.EventHandler(this.bConvertToWebApplication_Click);
            // 
            // bFolderBrowse
            // 
            this.bFolderBrowse.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bFolderBrowse.Location = new System.Drawing.Point(690, 43);
            this.bFolderBrowse.Margin = new System.Windows.Forms.Padding(0);
            this.bFolderBrowse.Name = "bFolderBrowse";
            this.bFolderBrowse.Size = new System.Drawing.Size(24, 23);
            this.bFolderBrowse.TabIndex = 7;
            this.bFolderBrowse.Text = "...";
            this.bFolderBrowse.UseVisualStyleBackColor = true;
            this.bFolderBrowse.Click += new System.EventHandler(this.bFolderBrowse_Click);
            // 
            // lProjectFileNameDisplay
            // 
            this.lProjectFileNameDisplay.AutoSize = true;
            this.lProjectFileNameDisplay.Location = new System.Drawing.Point(291, 72);
            this.lProjectFileNameDisplay.Name = "lProjectFileNameDisplay";
            this.lProjectFileNameDisplay.Size = new System.Drawing.Size(140, 13);
            this.lProjectFileNameDisplay.TabIndex = 8;
            this.lProjectFileNameDisplay.Text = "Not required for this function";
            // 
            // lProjectFileName
            // 
            this.lProjectFileName.AutoSize = true;
            this.lProjectFileName.Location = new System.Drawing.Point(229, 72);
            this.lProjectFileName.Name = "lProjectFileName";
            this.lProjectFileName.Size = new System.Drawing.Size(56, 13);
            this.lProjectFileName.TabIndex = 9;
            this.lProjectFileName.Text = "Project file";
            // 
            // bConvertToWebProject
            // 
            this.bConvertToWebProject.Enabled = false;
            this.bConvertToWebProject.Location = new System.Drawing.Point(221, 12);
            this.bConvertToWebProject.Name = "bConvertToWebProject";
            this.bConvertToWebProject.Size = new System.Drawing.Size(90, 23);
            this.bConvertToWebProject.TabIndex = 10;
            this.bConvertToWebProject.Text = "Convert to Proj";
            this.bConvertToWebProject.UseVisualStyleBackColor = true;
            this.bConvertToWebProject.Click += new System.EventHandler(this.bConvertToWebProject_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.bStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 449);
            this.Controls.Add(this.bConvertToWebProject);
            this.Controls.Add(this.lProjectFileName);
            this.Controls.Add(this.lProjectFileNameDisplay);
            this.Controls.Add(this.bFolderBrowse);
            this.Controls.Add(this.bConvertToWebApplication);
            this.Controls.Add(this.rbVB);
            this.Controls.Add(this.rbCS);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.bStart);
            this.Controls.Add(this.tbSolutionFolder);
            this.Controls.Add(this.tbProjectName);
            this.Controls.Add(this.lProjectName);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainForm";
            this.Text = "Web Project Validator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tpDesignerFileMissing.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDesignerFileMissing)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tpCodeFileCheck.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCodeFileCheck)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tpUserControlMissing.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserControlMissing)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
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
        private System.Windows.Forms.DataGridView dgvCodeFileCheck;
        private System.Windows.Forms.DataGridView dgvDesignerFileMissing;
        private System.Windows.Forms.DataGridView dgvUserControlMissing;
        private System.Windows.Forms.TextBox tbSolutionFolder;
        private System.Windows.Forms.RadioButton rbDesignerFileExists;
        private System.Windows.Forms.RadioButton rbDesignerFileMissing;
        private System.Windows.Forms.RadioButton rbDesignerFileAll;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel tsslValid;
        private System.Windows.Forms.ToolStripStatusLabel tsslInValid;
        private System.Windows.Forms.ToolStripStatusLabel tsslTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvCodeFileProject;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvCodeFileFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvCodeFileValid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvCodeFileControlFolder;
        private System.Windows.Forms.RadioButton rbCheckWebProject;
        private System.Windows.Forms.RadioButton rbCheckWebApplication;
        private System.Windows.Forms.ToolStripStatusLabel tsslRowCount;
        private System.Windows.Forms.RadioButton rbUserControlAll;
        private System.Windows.Forms.RadioButton rbUserControlValid;
        private System.Windows.Forms.RadioButton rbUserControlInvalid;
        private System.Windows.Forms.RadioButton rbUserControlUnused;
        private System.Windows.Forms.RadioButton rbUserControlMissing;
        private System.Windows.Forms.Button bConvertToWebApplication;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvFileMissingProject;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvFileMissingFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvFileMissingExists;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvFileMissingControlFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button bFolderBrowse;
        private System.Windows.Forms.Label lProjectFileNameDisplay;
        private System.Windows.Forms.Label lProjectFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvUserControlMissingProject;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvUserControlMissingFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvUserControlMissingValid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvUserControlMissingReason;
        private System.Windows.Forms.Button bConvertToWebProject;
    }
}

