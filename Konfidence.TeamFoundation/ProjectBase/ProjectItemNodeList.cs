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
        private BaseTfsXmlDocument _TfsXmlDocument = null;

        #region abstractmethods
        /// <summary>
        /// create and return a xxxDataItem derived from ProjectItemNode
        /// </summary>
        /// <returns></returns>
        internal protected abstract T GetItemNode(XmlNode projectItemNode, XmlNamespaceManager xmlNamespaceManager);
        internal protected abstract V GetGroupNode(BaseTfsXmlDocument tfsXmlDocument);
        internal protected abstract V CreateGroupNode(BaseTfsXmlDocument tfsXmlDocument);

        #endregion abstractmethods

        public ProjectItemNodeList(BaseTfsXmlDocument tfsXmlDocument)
        {
            _TfsXmlDocument = tfsXmlDocument;

            _GroupNode = GetGroupNode(_TfsXmlDocument);

            foreach (XmlNode projectItemNode in _GroupNode.GetItemNodeList())
            {
                T baseItemNode = GetItemNode(projectItemNode, _GroupNode.XmlNamespaceManager);

                Add(baseItemNode);
            }
        }

        // TODO : terugzetten naar internal protected
//        internal protected XmlElement AppendChild()
        internal protected XmlElement AppendChild()
        {
            if (Count == 0)
            {
                _GroupNode = CreateGroupNode(_TfsXmlDocument);
            }

            XmlElement newElement = _GroupNode.AppendChild();

            T baseItemNode = GetItemNode(newElement, _GroupNode.XmlNamespaceManager);

            Add(baseItemNode);

            return newElement;
        }
    }
}
