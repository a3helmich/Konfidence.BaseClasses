using System.Collections.Generic;
using System.Web.UI;

namespace JavaScriptExtender
{
    [TargetControlType(typeof(Control))]
    public class AsyncFileUploadExtender : ExtenderControl
    {
        private string _AsyncFileUploaderId = string.Empty;
        private string _AsyncCvUploaderId = string.Empty;
        private string _AsyncVideoUploaderId = string.Empty;

        private string _AccountId = string.Empty;
        private List<string> _ButtonIdList = new List<string>();

        public string AsyncFileUploaderId
        {
            get { return _AsyncFileUploaderId; }
            set { _AsyncFileUploaderId = value; }
        }

        public string AsyncCvUploaderId
        {
            get { return _AsyncCvUploaderId; }
            set { _AsyncCvUploaderId = value; }
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

        protected override IEnumerable<ScriptDescriptor> GetScriptDescriptors(Control targetControl)
        {
            var scriptDescriptorList = new List<ScriptDescriptor>();

            var descriptor = new ScriptBehaviorDescriptor("JavaScriptControls.AsyncFileUploadExtender", targetControl.ClientID);

            descriptor.AddProperty("AsyncFileUploaderId", AsyncFileUploaderId);
            descriptor.AddProperty("AsyncCVUploaderId", AsyncCvUploaderId);
            descriptor.AddProperty("AsyncVideoUploaderId", AsyncVideoUploaderId);
            descriptor.AddProperty("AccountId", AccountId);
            descriptor.AddProperty("ButtonIdList", ButtonIdList);

            scriptDescriptorList.Add(descriptor);

            return scriptDescriptorList;
        }

        protected override IEnumerable<ScriptReference> GetScriptReferences()
        {
            var reference = new ScriptReference("JavaScriptExtender.JavaScript.AsyncFileUploadExtender.js", "JavaScriptExtender");

            var scriptReferenceList = new List<ScriptReference> {reference};

            return scriptReferenceList;
        }
    }
}
