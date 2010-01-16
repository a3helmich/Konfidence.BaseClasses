namespace Konfidence.ReferenceReBaserApp
{
    partial class ChangesForm
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
            this.overviewLabel = new System.Windows.Forms.Label();
            this.overviewListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // overviewLabel
            // 
            this.overviewLabel.AutoSize = true;
            this.overviewLabel.Location = new System.Drawing.Point(13, 13);
            this.overviewLabel.Name = "overviewLabel";
            this.overviewLabel.Size = new System.Drawing.Size(194, 13);
            this.overviewLabel.TabIndex = 0;
            this.overviewLabel.Text = "Overzicht van de aangepaste projecten";
            // 
            // overviewListBox
            // 
            this.overviewListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.overviewListBox.FormattingEnabled = true;
            this.overviewListBox.Location = new System.Drawing.Point(16, 45);
            this.overviewListBox.Name = "overviewListBox";
            this.overviewListBox.Size = new System.Drawing.Size(434, 420);
            this.overviewListBox.TabIndex = 1;
            // 
            // ChangesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 480);
            this.Controls.Add(this.overviewListBox);
            this.Controls.Add(this.overviewLabel);
            this.Name = "ChangesForm";
            this.Text = "ChangesForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label overviewLabel;
        private System.Windows.Forms.ListBox overviewListBox;
    }
}