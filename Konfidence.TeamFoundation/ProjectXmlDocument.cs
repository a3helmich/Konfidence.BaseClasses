using System.Xml;
using System.Collections.Generic;
using Konfidence.TeamFoundation.Project;
using Konfidence.TeamFoundation.ProjectBase;

namespace Konfidence.TeamFoundation
{
    public class ProjectXmlDocument : BaseTfsXmlDocument
    {
        private const string DLL_REFERENCE_ITEMGROUP_NAME = "Reference";
        private const string PROJECT_REFERENCE_ITEMGROUP_NAME = "ProjectReference";
        private const string PROJECT_COMPILE_ITEMGROUP_NAME = "Compile";

        private DllReferenceItemNodeList _DllReferenceItemNodeList = null;

        public ProjectXmlDocument()
        {
            //_DllReferenceItemGroup = new DllReferenceGroupNode(this);
        }

        // TODO : when a DllReferenceNode is added to the Xml it must also be added to the list
        public DllReferenceItemNodeList DllReferenceItemGroupList
        {
            get
            {
                if (!IsAssigned(_DllReferenceItemNodeList))
                {
                    _DllReferenceItemNodeList = new DllReferenceItemNodeList(this);

                    //foreach (XmlNode dllReference in GetItemGroupList(DLL_REFERENCE_ITEMGROUP_NAME))
                    //{
                    //    _DllReferenceItemNodeList.Add(new DllReferenceItemNode(dllReference, this.XmlNamespaceManager));
                    //}

                }
                return _DllReferenceItemNodeList;
            }
        }

        // TODO : when a CompileProjectNode is added to the Xml it must also be added to the list
        // TODO : XmlNodeList omzetten naar List<CompileProjectNode>
        public XmlNodeList CompileProjectItemGroupList
        {
            get
            {
                return GetItemGroupList(PROJECT_COMPILE_ITEMGROUP_NAME);
            }
        }

        // TODO : when a ProjectReferenceNode is added to the Xml it must also be added to the list
        // TODO : XmlNodeList omzetten naar List<ProjectReferenceNode>
        public XmlNodeList ProjectReferenceItemGroupList
        {
            get
            {
                return GetItemGroupList(PROJECT_REFERENCE_ITEMGROUP_NAME);
            }
        }
    }
}
