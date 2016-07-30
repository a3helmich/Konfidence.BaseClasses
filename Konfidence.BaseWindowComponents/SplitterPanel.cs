using System.ComponentModel;
using System.Windows.Forms;
using Konfidence.Base;

namespace Konfidence.BaseWindowComponents
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	public class SplitterPanel : UserControl
	{
    public Splitter Splitter;
    public Panel Panel2;
    private Panel _panel1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private readonly Container _components = null;

    public Panel Panel1
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
				if( _components.IsAssigned())
					_components.Dispose();
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
      this.Splitter = new System.Windows.Forms.Splitter();
      this.Panel2 = new System.Windows.Forms.Panel();
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
      this.Splitter.Location = new System.Drawing.Point(128, 0);
      this.Splitter.Name = "Splitter";
      this.Splitter.Size = new System.Drawing.Size(16, 240);
      this.Splitter.TabIndex = 1;
      this.Splitter.TabStop = false;
      // 
      // panel2
      // 
      this.Panel2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(255)), ((System.Byte)(192)));
      this.Panel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.Panel2.Location = new System.Drawing.Point(144, 0);
      this.Panel2.Name = "Panel2";
      this.Panel2.Size = new System.Drawing.Size(136, 240);
      this.Panel2.TabIndex = 2;
      // 
      // SplitterPanel
      // 
      this.Controls.Add(this.Panel2);
      this.Controls.Add(this.Splitter);
      this.Controls.Add(this._panel1);
      this.Name = "SplitterPanel";
      this.Size = new System.Drawing.Size(280, 240);
      this.ResumeLayout(false);

    }
		#endregion
	}
}
