using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Konfidence.TeamFoundation.ProjectBase
{
    public abstract class ProjectItemNodeList<T, V>: List<T> where T: ProjectItemNode where V: ProjectNode
    {
        private V _GroupNode = null;

        #region abstractmethods
        /// <summary>
        /// create and return a xxxDataItem derived from ProjectItemNode
        /// </summary>
        /// <returns></returns>
        internal protected abstract T GetItemNode(XmlNode projectItemNode, XmlNamespaceManager xmlNamespaceManager);

        internal protected abstract V GetGroupNode(BaseTfsXmlDocument tfsXmlDocument);

        #endregion abstractmethods

        public ProjectItemNodeList(BaseTfsXmlDocument tfsXmlDocument)
        {
            _GroupNode = GetGroupNode(tfsXmlDocument);

            foreach (XmlNode projectItemNode in _GroupNode.GetItemNodeList())
            {
                T baseItemNode = GetItemNode(projectItemNode, _GroupNode.XmlNamespaceManager);

                Add(baseItemNode);
            }
        }

        // TODO : terugzetten naar internal protected
//        internal protected XmlElement AppendChild()
        public XmlElement AppendChild()
        {
            XmlElement newElement = _GroupNode.AppendChild();

            T baseItemNode = GetItemNode(newElement, _GroupNode.XmlNamespaceManager);

            Add(baseItemNode);

            return newElement;
        }
    }
}
