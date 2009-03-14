using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace BaseWindowComponents
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	public class SplitterPanel : System.Windows.Forms.UserControl
	{
    public System.Windows.Forms.Splitter splitter;
    public System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Panel _panel1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

    public Panel panel1
    {
      get { return _panel1; }
      set { _panel1 = value; }
    }


		public SplitterPanel()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitComponent call

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
      this._panel1 = new System.Windows.Forms.Panel();
      this.splitter = new System.Windows.Forms.Splitter();
      this.panel2 = new System.Windows.Forms.Panel();
      this.SuspendLayout();
      // 
      // _panel1
      // 
      this._panel1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
      this._panel1.Dock = System.Windows.Forms.DockStyle.Left;
      this._panel1.Location = new System.Drawing.Point(0, 0);
      this._panel1.Name = "_panel1";
      this._panel1.Size = new System.Drawing.Size(128, 240);
      this._panel1.TabIndex = 0;
      // 
      // splitter
      // 
      this.splitter.Location = new System.Drawing.Point(128, 0);
      this.splitter.Name = "splitter";
      this.splitter.Size = new System.Drawing.Size(16, 240);
      this.splitter.TabIndex = 1;
      this.splitter.TabStop = false;
      // 
      // panel2
      // 
      this.panel2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(255)), ((System.Byte)(192)));
      this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel2.Location = new System.Drawing.Point(144, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(136, 240);
      this.panel2.TabIndex = 2;
      // 
      // SplitterPanel
      // 
      this.Controls.Add(this.panel2);
      this.Controls.Add(this.splitter);
      this.Controls.Add(this._panel1);
      this.Name = "SplitterPanel";
      this.Size = new System.Drawing.Size(280, 240);
      this.ResumeLayout(false);

    }
		#endregion
	}
}
