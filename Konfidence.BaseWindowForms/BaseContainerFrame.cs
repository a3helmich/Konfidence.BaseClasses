using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Konfidence.Base;
using Konfidence.BaseHelper;

namespace Konfidence.BaseWindowForms
{

    /// <summary>
    /// Summary description for BaseMainFrame.
    /// </summary>
    //  [DesignerAttribute(typeof(BaseMainframeControlDesigner)) ]
    public class BaseContainerFrame : UserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private readonly Container _Components = null;
        public Panel BaseFrameLeftContainerPanel;
        public Panel BaseFrameRightContainerPanel;
        public Splitter FrameSplitter;
        private bool _ShowDesignHelper;

        public bool ShowDesignHelper
        {
            get
            {
                Refresh();
                return _ShowDesignHelper;
            }
            set
            {
                _ShowDesignHelper = value;
                Refresh();
            }
        }

        public BaseContainerFrame()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            // TODO: Add any initialization after the InitializeComponent call
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (DesignMode && _ShowDesignHelper)
            {
                //Design time painting here
                BaseFrameLeftContainerPanel.BackColor = Color.FromArgb(255, 255, 128);
                BaseFrameRightContainerPanel.BackColor = Color.FromArgb(255, 192, 128);
            }
        }

        #region Cleanup
        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_Components.IsAssigned())
                {
                    _Components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        #endregion Cleanup

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BaseFrameLeftContainerPanel = new System.Windows.Forms.Panel();
            this.FrameSplitter = new System.Windows.Forms.Splitter();
            this.BaseFrameRightContainerPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // BaseFrameLeftContainerPanel
            // 
            this.BaseFrameLeftContainerPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.BaseFrameLeftContainerPanel.Location = new System.Drawing.Point(0, 0);
            this.BaseFrameLeftContainerPanel.Name = "BaseFrameLeftContainerPanel";
            this.BaseFrameLeftContainerPanel.Size = new System.Drawing.Size(200, 328);
            this.BaseFrameLeftContainerPanel.TabIndex = 0;
            // 
            // FrameSplitter
            // 
            this.FrameSplitter.Location = new System.Drawing.Point(200, 0);
            this.FrameSplitter.Name = "FrameSplitter";
            this.FrameSplitter.Size = new System.Drawing.Size(16, 328);
            this.FrameSplitter.TabIndex = 1;
            this.FrameSplitter.TabStop = false;
            // 
            // BaseFrameRightContainerPanel
            // 
            this.BaseFrameRightContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BaseFrameRightContainerPanel.Location = new System.Drawing.Point(216, 0);
            this.BaseFrameRightContainerPanel.Name = "BaseFrameRightContainerPanel";
            this.BaseFrameRightContainerPanel.Size = new System.Drawing.Size(224, 328);
            this.BaseFrameRightContainerPanel.TabIndex = 2;
            // 
            // BaseMainframe
            // 
            this.Controls.Add(this.BaseFrameRightContainerPanel);
            this.Controls.Add(this.FrameSplitter);
            this.Controls.Add(this.BaseFrameLeftContainerPanel);
            this.Name = "BaseMainframe";
            this.Size = new System.Drawing.Size(440, 328);
            this.ResumeLayout(false);

        }
        #endregion

        public BaseConfigClass Config { get; set; }

        public virtual void AfterCreate()
        {
            Application.DoEvents();
        }
    }

    //  public class BaseMainframeControlDesigner : ControlDesigner
    //  {
    //
    //    protected override void OnPaintAdornments(PaintEventArgs e)
    //    {
    //
    //      BaseMainframe baseMainframe = this.Control as BaseMainframe;
    //      //Design time painting here
    //      baseMainframe.BaseFrameRightContainerPanel.BackColor = Color.FromArgb(255, 192, 128);
    //      baseMainframe.BaseFrameLeftContainerPanel.BackColor = Color.FromArgb(255, 255, 128);
    //    }
    //    protected override void PreFilterProperties(System.Collections.IDictionary properties)
    //    {
    //      properties.Add("OutlineColor", TypeDescriptor.CreateProperty(typeof(BaseMainframeControlDesigner), "OutlineColor", typeof(System.Drawing.Color), null));
    //    }
    //  }
}

