using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using AjaxControlToolkit;
using Konfidence.Base;

namespace Konfidence.AjaxControlExtender
{
    [TargetControlType(typeof(Control))]
    [ClientScriptResource("AjaxControlToolkit", "Common.Common.js")]  // lijkt niet te werken
    [RequiredScript(typeof(CommonToolkitScripts))]
    public class FocusBehavior : ExtenderControl
    {
        private string _HighlightCssClass;
        private string _NormalCssClass;

        #region simple properties
        public string NormalCssClass
        {
            get { return _NormalCssClass; }
            set { _NormalCssClass = value; }
        }

        public string HighlightCssClass
        {
            get { return _HighlightCssClass; }
            set { _HighlightCssClass = value; }
        }
        #endregion simple properties

        public FocusBehavior()
        {
        }

        // class in js-file registration
        protected override IEnumerable<ScriptDescriptor> GetScriptDescriptors(Control targetControl)
        {
            ScriptBehaviorDescriptor descriptor;
            List<ScriptDescriptor> scriptDescriptorList = new List<ScriptDescriptor>();

            if (targetControl != null)
            {
                descriptor = new ScriptBehaviorDescriptor("AjaxExtender.FocusBehavior", targetControl.ClientID);

                // properties op de js-class koppelen aan de properties op de cs-class
                descriptor.AddProperty("HighlightCssClass", this.HighlightCssClass);
                descriptor.AddProperty("NormalCssClass", this.NormalCssClass);

                scriptDescriptorList.Add(descriptor);
            }

            return scriptDescriptorList;
        }

        // file registration in dll
        protected override IEnumerable<ScriptReference> GetScriptReferences()
        {
            // the control searches for the .js file in a resource file, the resource file is in this case the controls own .dll file 
            ScriptReference reference = new ScriptReference("Konfidence.AjaxControlExtender.JavaScript.FocusBehavior.js", "Konfidence.AjaxControlExtender");
            List<ScriptReference> scriptReferenceList = new List<ScriptReference>();

            scriptReferenceList.Add(reference);

            return scriptReferenceList;
        }
    }
}
