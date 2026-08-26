using System;
using System.Xml;
using Konfidence.MsBuild.ProjectBase;

namespace Konfidence.MsBuild.Project
{
    class PropertyConfigurationItemNode : ProjectItemNode
    {
        public PropertyConfigurationItemNode(XmlNode xmlNode)
            : base(xmlNode)
        {
        }

        internal XmlNode GetElement(string p)
        {
            throw new NotImplementedException();
        }
    }
}
