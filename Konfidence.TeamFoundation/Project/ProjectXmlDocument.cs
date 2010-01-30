using System.Xml;

namespace Konfidence.TeamFoundation.Project
{
    public class ProjectXmlDocument : BaseTfsXmlDocument
    {
        private const string DLL_REFERENCE_ITEMGROUP_NAME = "Reference";

        public XmlNodeList DllReferenceItemGroupList
        {
            get
            {
                return GetItemGroupList(DLL_REFERENCE_ITEMGROUP_NAME);
            }
        }
    }
}
