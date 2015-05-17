using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI;

namespace Konfidence.AjaxControlExtender
{
    [TargetControlType(typeof(Control))]
    public class GridRowHyperlinkExtender : ExtenderControl
    {
        #region simple properties

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        public string TargetUrl { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly"), SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<string> IdList { get; set; }

        #endregion simple properties

        protected override IEnumerable<ScriptDescriptor> GetScriptDescriptors(Control targetControl)
        {
            var scriptDescriptorList = new List<ScriptDescriptor>();

            if (targetControl != null)
            {
                var descriptor = new ScriptBehaviorDescriptor("Ace.GridRowHyperlinkExtender", targetControl.ClientID);

                descriptor.AddProperty("TargetUrl", TargetUrl);
                descriptor.AddProperty("IdList", IdList);

                scriptDescriptorList.Add(descriptor);
            }

            return scriptDescriptorList;
        }

        protected override IEnumerable<ScriptReference> GetScriptReferences()
        {
            var reference = new ScriptReference("Konfidence.AjaxControlExtender.JavaScript.GridRowHyperlinkExtender.js", "Konfidence.AjaxControlExtender");

            var scriptReferenceList = new List<ScriptReference> {reference};

            return scriptReferenceList;
        }
    }
}
