using System.Collections.Generic;
using System.Web.UI;

namespace JavaScriptExtender
{
    [TargetControlType(typeof(Control))]
    public class GridRowHighLightExtender : ExtenderControl
    {
        #region simple properties

        public string HighlightCssClass { get; set; }

        public string NormalCssClass { get; set; }

        public string TargetUrl { get; set; }

        public List<string> IdList { get; set; }

        #endregion simple properties

        protected override IEnumerable<ScriptDescriptor> GetScriptDescriptors(Control targetControl)
        {
            var scriptDescriptorList = new List<ScriptDescriptor>();

            var descriptor = new ScriptBehaviorDescriptor("JavaScriptControls.GridRowHighLightExtender", targetControl.ClientID);

            descriptor.AddProperty("HighlightCssClass", HighlightCssClass);
            descriptor.AddProperty("NormalCssClass", NormalCssClass);
            descriptor.AddProperty("TargetUrl", TargetUrl);
            descriptor.AddProperty("IdList", IdList);

            scriptDescriptorList.Add(descriptor);

            return scriptDescriptorList;
        }

        protected override IEnumerable<ScriptReference> GetScriptReferences()
        {
            var reference = new ScriptReference("JavaScriptExtender.JavaScript.GridRowHighLightExtender.js", "JavaScriptExtender");

            var scriptReferenceList = new List<ScriptReference> {reference};

            return scriptReferenceList;
        }
    }
}
