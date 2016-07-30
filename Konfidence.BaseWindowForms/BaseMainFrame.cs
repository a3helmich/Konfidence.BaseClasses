using System.ComponentModel;
using System.Windows.Forms;
using Konfidence.Base;

namespace Konfidence.BaseWindowForms
{
    /// <summary>
    /// Summary description for BaseMainFrame.
    /// </summary>
    public class BaseMainFrame : UserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private readonly Container _components = null;

        public BaseMainFrame()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call

        }

        public virtual void AfterCreate()
        {
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_components.IsAssigned())
                {
                    _components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // BaseMainFrame
            // 
            this.Name = "BaseMainFrame";
            this.Size = new System.Drawing.Size(504, 224);

        }
        #endregion
    }
}
