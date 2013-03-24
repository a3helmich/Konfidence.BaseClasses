using System.Collections.Generic;
using System.Web.UI;
using AjaxControlToolkit;

namespace Konfidence.AjaxControlExtender
{
    [TargetControlType(typeof(Control))]
    [ClientScriptResource("AjaxControlToolkit", "Common.Common.js")]  // lijkt niet te werken
    [RequiredScript(typeof(CommonToolkitScripts))]
    public class FocusBehavior : ExtenderControl
    {
        #region simple properties

        public string NormalCssClass { get; set; }

        public string HighlightCssClass { get; set; }

        #endregion simple properties

        // class in js-file registration
        protected override IEnumerable<ScriptDescriptor> GetScriptDescriptors(Control targetControl)
        {
            var scriptDescriptorList = new List<ScriptDescriptor>();

            if (targetControl != null)
            {
                var descriptor = new ScriptBehaviorDescriptor("Ace.FocusBehavior", targetControl.ClientID);

                // properties op de js-class koppelen aan de properties op de cs-class
                descriptor.AddProperty("HighlightCssClass", HighlightCssClass);
                descriptor.AddProperty("NormalCssClass", NormalCssClass);

                scriptDescriptorList.Add(descriptor);
            }

            return scriptDescriptorList;
        }

        // file registration in dll
        protected override IEnumerable<ScriptReference> GetScriptReferences()
        {
            // the control searches for the .js file in a resource file, the resource file is in this case the controls own .dll file 
            var reference = new ScriptReference("Konfidence.AjaxControlExtender.JavaScript.FocusBehavior.js", "Konfidence.AjaxControlExtender");

            var scriptReferenceList = new List<ScriptReference> {reference};

            return scriptReferenceList;
        }
    }
}
