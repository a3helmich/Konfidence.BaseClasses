using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Konfidence.TeamFoundation.ProjectBase
{
    public abstract class ProjectItemNodeList<T, V>: List<T> where T: ProjectItemNode where V: ProjectGroupNode
    {
        private V _GroupNode = null;

        //public V GroupNode
        //{
        //    get { return _GroupNode; }
        //}

        public ProjectItemNodeList(BaseTfsXmlDocument tfsXmlDocument)
        {
            _GroupNode = GetGroupNode(tfsXmlDocument);

            foreach (XmlNode projectItemNode in _GroupNode.GetItemNodeList())
            {
                T baseItemNode = GetNewItemNode(projectItemNode, _GroupNode.XmlNamespaceManager);

                Add(baseItemNode);
            }
        }

        /// <summary>
        /// create and return a xxxDataItem derived from ProjectItemNode
        /// </summary>
        /// <returns></returns>
        protected abstract T GetNewItemNode(XmlNode projectItemNode, XmlNamespaceManager xmlNamespaceManager);

        protected abstract V GetGroupNode(BaseTfsXmlDocument tfsXmlDocument);

        //public void GetItemNode(XmlNode projectItemNode)
        //{
        //    T baseItemNode = GetNewItemNode(projectItemNode, _GroupNode.XmlNamespaceManager);

        //    Add(baseItemNode);

        //    //return baseItemNode;
        //}
    }
}
