using System.Collections.Generic;
using System.Xml;
using Konfidence.Base;
using Konfidence.MsBuild.DocumentBase;

namespace Konfidence.MsBuild.ProjectBase
{
    public abstract class ProjectItemNodeList<T, TV>: List<T> where T: ProjectItemNode where TV: ProjectNode
    {
        private TV _groupNode;
        private readonly BaseProjectXmlDocument _projectXmlDocument;

        #region abstractmethods
        /// <summary>
        /// create and return a xxxDataItem derived from ProjectItemNode
        /// </summary>
        /// <returns></returns>
        protected internal abstract T GetItemNode(XmlNode projectItemNode, XmlNamespaceManager xmlNamespaceManager);
        protected internal abstract TV GetGroupNode(BaseProjectXmlDocument projectXmlDocument);
        protected internal abstract TV CreateGroupNode(BaseProjectXmlDocument projectXmlDocument);

        #endregion abstractmethods

        protected ProjectItemNodeList(BaseProjectXmlDocument projectXmlDocument)
        {
            _projectXmlDocument = projectXmlDocument;

            // ReSharper disable once VirtualMemberCallInConstructor
            _groupNode = GetGroupNode(_projectXmlDocument);

            var itemNodeList = _groupNode.GetNodeList();

            if (itemNodeList.IsAssigned())
            {
                foreach (XmlNode projectItemNode in itemNodeList)
                {
                    // ReSharper disable once VirtualMemberCallInConstructor
                    var baseItemNode = GetItemNode(projectItemNode, _groupNode.XmlNamespaceManager);

                    Add(baseItemNode);
                }
            }
        }

        protected internal XmlElement AppendChild()
        {
            if (Count == 0)
            {
                _groupNode = CreateGroupNode(_projectXmlDocument);
            }

            var newElement = _groupNode.AppendChild();

            var baseItemNode = GetItemNode(newElement, _groupNode.XmlNamespaceManager);

            Add(baseItemNode);

            return newElement;
        }
    }
}
