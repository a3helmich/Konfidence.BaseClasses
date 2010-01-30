using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Konfidence.Base;

namespace Konfidence.TeamFoundation
{
    public class BaseTfsXmlElement : BaseItem
    {
        private XmlElement _XmlElement = null;

        public BaseTfsXmlElement(XmlElement xmlElement)
        {
            _XmlElement = xmlElement;
        }

        protected virtual void Generate()
        {
            throw (new NotImplementedException("De generate method is required for: " + this.ToString()));
        }
    }
}
