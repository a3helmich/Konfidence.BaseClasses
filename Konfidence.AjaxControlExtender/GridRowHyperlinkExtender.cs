using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Konfidence.AjaxControlExtender
{
    [TargetControlType(typeof(Control))]
    public class GridRowHyperlinkExtender : ExtenderControl
    {
        private string _TargetUrl;
        private List<string> _IdList;

        #region simple properties
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        public string TargetUrl
        {
            get { return _TargetUrl; }
            set { _TargetUrl = value; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<string> IdList
        {
            get { return _IdList; }
            set { _IdList = value; }
        }
        #endregion simple properties

        public GridRowHyperlinkExtender()
        {
        }

        protected override IEnumerable<ScriptDescriptor> GetScriptDescriptors(Control targetControl)
        {
            ScriptBehaviorDescriptor descriptor;
            List<ScriptDescriptor> scriptDescriptorList = new List<ScriptDescriptor>();

            if (targetControl != null)
            {
                descriptor = new ScriptBehaviorDescriptor("Ace.GridRowHyperlinkExtender", targetControl.ClientID);

                descriptor.AddProperty("TargetUrl", this.TargetUrl);
                descriptor.AddProperty("IdList", this.IdList);

                scriptDescriptorList.Add(descriptor);
            }

            return scriptDescriptorList;
        }

        protected override IEnumerable<ScriptReference> GetScriptReferences()
        {
            ScriptReference reference = new ScriptReference("Konfidence.AjaxControlExtender.JavaScript.GridRowHyperlinkExtender.js", "Konfidence.AjaxControlExtender");

            List<ScriptReference> scriptReferenceList = new List<ScriptReference>();

            scriptReferenceList.Add(reference);

            return scriptReferenceList;
        }
    }
}
