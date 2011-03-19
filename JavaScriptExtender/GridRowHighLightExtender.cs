using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;

namespace JavaScriptExtender
{
    [TargetControlType(typeof(Control))]
    public class GridRowHighLightExtender : ExtenderControl
    {
        private string _HighlightCssClass;
        private string _NormalCssClass;
        private string _TargetUrl;
        private List<string> _IdList;

        #region simple properties
        public string HighlightCssClass
        {
            get { return _HighlightCssClass; }
            set { _HighlightCssClass = value; }
        }

        public string NormalCssClass
        {
            get { return _NormalCssClass; }
            set { _NormalCssClass = value; }
        }

        public string TargetUrl
        {
            get { return _TargetUrl; }
            set { _TargetUrl = value; }
        }

        public List<string> IdList
        {
            get { return _IdList; }
            set { _IdList = value; }
        }
        #endregion simple properties

        public GridRowHighLightExtender()
        {
        }

        protected override IEnumerable<ScriptDescriptor> GetScriptDescriptors(Control targetControl)
        {
            ScriptBehaviorDescriptor descriptor;
            List<ScriptDescriptor> scriptDescriptorList = new List<ScriptDescriptor>();

            descriptor = new ScriptBehaviorDescriptor("JavaScriptControls.GridRowHighLightExtender", targetControl.ClientID);

            descriptor.AddProperty("HighlightCssClass", this.HighlightCssClass);
            descriptor.AddProperty("NormalCssClass", this.NormalCssClass);
            descriptor.AddProperty("TargetUrl", this.TargetUrl);
            descriptor.AddProperty("IdList", this.IdList);

            scriptDescriptorList.Add(descriptor);

            return scriptDescriptorList;
        }

        protected override IEnumerable<ScriptReference> GetScriptReferences()
        {
            ScriptReference reference = new ScriptReference("JavaScriptExtender.JavaScript.GridRowHighLightExtender.js", "JavaScriptExtender");

            List<ScriptReference> scriptReferenceList = new List<ScriptReference>();

            scriptReferenceList.Add(reference);

            return scriptReferenceList;
        }
    }
}
