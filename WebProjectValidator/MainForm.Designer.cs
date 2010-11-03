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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lLanguage = new System.Windows.Forms.Label();
            this.rbCS = new System.Windows.Forms.RadioButton();
            this.rbVB = new System.Windows.Forms.RadioButton();
            this.bStart = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dgvCodeFile = new System.Windows.Forms.DataGridView();
            this.dgvDesignerFile = new System.Windows.Forms.DataGridView();
            this.dgvUserControl = new System.Windows.Forms.DataGridView();
            this.dgvFileMissingProject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvFileMissingFolder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvFileMissingFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvFileMissingInValid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCodeFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDesignerFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserControl)).BeginInit();
            this.SuspendLayout();
            // 
            // lProjectName
            // 
            this.lProjectName.AutoSize = true;
            this.lProjectName.Location = new System.Drawing.Point(13, 13);
            this.lProjectName.Name = "lProjectName";
            this.lProjectName.Size = new System.Drawing.Size(66, 13);
            this.lProjectName.TabIndex = 0;
            this.lProjectName.Text = "Projectnaam";
            // 
            // tbProjectName
            // 
            this.tbProjectName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbProjectName.Location = new System.Drawing.Point(100, 11);
            this.tbProjectName.Name = "tbProjectName";
            this.tbProjectName.Size = new System.Drawing.Size(218, 20);
            this.tbProjectName.TabIndex = 1;
            this.tbProjectName.Text = "Konfidence";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lLanguage);
            this.groupBox1.Controls.Add(this.rbCS);
            this.groupBox1.Controls.Add(this.rbVB);
            this.groupBox1.Location = new System.Drawing.Point(16, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(302, 31);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // lLanguage
            // 
            this.lLanguage.AutoSize = true;
            this.lLanguage.Location = new System.Drawing.Point(7, 13);
            this.lLanguage.Name = "lLanguage";
            this.lLanguage.Size = new System.Drawing.Size(28, 13);
            this.lLanguage.TabIndex = 2;
            this.lLanguage.Text = "Taal";
            // 
            // rbCS
            // 
            this.rbCS.AutoSize = true;
            this.rbCS.Checked = true;
            this.rbCS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbCS.Location = new System.Drawing.Point(192, 11);
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
            this.rbVB.Location = new System.Drawing.Point(84, 11);
            this.rbVB.Name = "rbVB";
            this.rbVB.Size = new System.Drawing.Size(38, 17);
            this.rbVB.TabIndex = 0;
            this.rbVB.Text = "VB";
            this.rbVB.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.rbVB.UseVisualStyleBackColor = true;
            // 
            // bStart
            // 
            this.bStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bStart.Location = new System.Drawing.Point(562, 45);
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(75, 23);
            this.bStart.TabIndex = 3;
            this.bStart.Text = "Start";
            this.bStart.UseVisualStyleBackColor = true;
            this.bStart.Click += new System.EventHandler(this.bStart_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(16, 84);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(621, 319);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(613, 293);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "CodeFile Check";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvCodeFile);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 33);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(607, 257);
            this.panel2.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(607, 30);
            this.panel1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel5);
            this.tabPage2.Controls.Add(this.panel3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(613, 293);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Designer File Missing";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.dgvDesignerFile);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 33);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(607, 257);
            this.panel5.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(607, 30);
            this.panel3.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panel6);
            this.tabPage3.Controls.Add(this.panel4);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(613, 293);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "User Control Missing";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.dgvUserControl);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(3, 33);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(607, 257);
            this.panel6.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(607, 30);
            this.panel4.TabIndex = 0;
            // 
            // dgvCodeFile
            // 
            this.dgvCodeFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCodeFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCodeFile.Location = new System.Drawing.Point(0, 0);
            this.dgvCodeFile.Name = "dgvCodeFile";
            this.dgvCodeFile.Size = new System.Drawing.Size(607, 257);
            this.dgvCodeFile.TabIndex = 0;
            // 
            // dgvDesignerFile
            // 
            this.dgvDesignerFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDesignerFile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvFileMissingProject,
            this.dgvFileMissingFolder,
            this.dgvFileMissingFileName,
            this.dgvFileMissingInValid});
            this.dgvDesignerFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDesignerFile.Location = new System.Drawing.Point(0, 0);
            this.dgvDesignerFile.Name = "dgvDesignerFile";
            this.dgvDesignerFile.Size = new System.Drawing.Size(607, 257);
            this.dgvDesignerFile.TabIndex = 0;
            // 
            // dgvUserControl
            // 
            this.dgvUserControl.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUserControl.Location = new System.Drawing.Point(0, 0);
            this.dgvUserControl.Name = "dgvUserControl";
            this.dgvUserControl.Size = new System.Drawing.Size(607, 257);
            this.dgvUserControl.TabIndex = 0;
            // 
            // dgvFileMissingProject
            // 
            this.dgvFileMissingProject.DataPropertyName = "Project";
            this.dgvFileMissingProject.HeaderText = "Project";
            this.dgvFileMissingProject.Name = "dgvFileMissingProject";
            this.dgvFileMissingProject.ReadOnly = true;
            // 
            // dgvFileMissingFolder
            // 
            this.dgvFileMissingFolder.DataPropertyName = "Folder";
            this.dgvFileMissingFolder.HeaderText = "Folder";
            this.dgvFileMissingFolder.Name = "dgvFileMissingFolder";
            this.dgvFileMissingFolder.ReadOnly = true;
            // 
            // dgvFileMissingFileName
            // 
            this.dgvFileMissingFileName.DataPropertyName = "FileName";
            this.dgvFileMissingFileName.HeaderText = "Filename";
            this.dgvFileMissingFileName.Name = "dgvFileMissingFileName";
            this.dgvFileMissingFileName.ReadOnly = true;
            // 
            // dgvFileMissingInValid
            // 
            this.dgvFileMissingInValid.DataPropertyName = "InValid";
            this.dgvFileMissingInValid.HeaderText = "Missing";
            this.dgvFileMissingInValid.Name = "dgvFileMissingInValid";
            this.dgvFileMissingInValid.ReadOnly = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 415);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.bStart);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbProjectName);
            this.Controls.Add(this.lProjectName);
            this.Name = "MainForm";
            this.Text = "Web Project Validator";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCodeFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDesignerFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lProjectName;
        private System.Windows.Forms.TextBox tbProjectName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbVB;
        private System.Windows.Forms.RadioButton rbCS;
        private System.Windows.Forms.Label lLanguage;
        private System.Windows.Forms.Button bStart;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridView dgvCodeFile;
        private System.Windows.Forms.DataGridView dgvDesignerFile;
        private System.Windows.Forms.DataGridView dgvUserControl;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvFileMissingProject;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvFileMissingFolder;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvFileMissingFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvFileMissingInValid;
    }
}

