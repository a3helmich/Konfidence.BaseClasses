using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;

namespace JavaScriptExtender
{
    [TargetControlType(typeof(Control))]
    public class AsyncFileUploadExtender : ExtenderControl
    {
        private string _AsyncFileUploaderId = string.Empty;
        private string _AsyncCVUploaderId = string.Empty;
        private string _AsyncVideoUploaderId = string.Empty;

        private string _AccountId = string.Empty;
        private List<string> _ButtonIdList = new List<string>();

        public string AsyncFileUploaderId
        {
            get { return _AsyncFileUploaderId; }
            set { _AsyncFileUploaderId = value; }
        }

        public string AsyncCVUploaderId
        {
            get { return _AsyncCVUploaderId; }
            set { _AsyncCVUploaderId = value; }
        }

        public string AsyncVideoUploaderId
        {
            get { return _AsyncVideoUploaderId; }
            set { _AsyncVideoUploaderId = value; }
        }

        public string AccountId
        {
            get { return _AccountId; }
            set { _AccountId = value; }
        }

        public List<string> ButtonIdList
        {
            get { return _ButtonIdList; }
            set { _ButtonIdList = value; }
        }

        public AsyncFileUploadExtender()
        {
        }

        protected override IEnumerable<ScriptDescriptor> GetScriptDescriptors(Control targetControl)
        {
            ScriptBehaviorDescriptor descriptor;
            List<ScriptDescriptor> scriptDescriptorList = new List<ScriptDescriptor>();

            descriptor = new ScriptBehaviorDescriptor("JavaScriptControls.AsyncFileUploadExtender", targetControl.ClientID);

            descriptor.AddProperty("AsyncFileUploaderId", this.AsyncFileUploaderId);
            descriptor.AddProperty("AsyncCVUploaderId", this.AsyncCVUploaderId);
            descriptor.AddProperty("AsyncVideoUploaderId", this.AsyncVideoUploaderId);
            descriptor.AddProperty("AccountId", this.AccountId);
            descriptor.AddProperty("ButtonIdList", this.ButtonIdList);

            scriptDescriptorList.Add(descriptor);

            return scriptDescriptorList;
        }

        protected override IEnumerable<ScriptReference> GetScriptReferences()
        {
            ScriptReference reference = new ScriptReference("JavaScriptExtender.JavaScript.AsyncFileUploadExtender.js", "JavaScriptExtender");

            List<ScriptReference> scriptReferenceList = new List<ScriptReference>();

            scriptReferenceList.Add(reference);

            return scriptReferenceList;
        }
    }
}
