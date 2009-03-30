using System.Xml;
using System.Collections.Generic;
using DataItemGeneratorClasses.HelperClasses;

namespace DataItemGeneratorClasses.ProjectGenerator
{
    class ProjectReferenceGenerator: ProjectBaseGenerator
    {
        private XmlNode _ReferenceItemGroup = null;
        private ProjectAssemblyItemDictionary _ProjectAssemblyItemDictionary = null;

        public ProjectReferenceGenerator(ProjectXmlDocument xmlDocument) : base(xmlDocument)
        {
            _ReferenceItemGroup = GetItemGroup("reference");

            Initialize();

            Execute();
        }

        private void Initialize()
        {
            _ProjectAssemblyItemDictionary = new ProjectAssemblyItemDictionary();

            CleanProjectAssemblyDictionary();

            if (_ProjectAssemblyItemDictionary.Count > 0)
            {
                XmlDocument.Changed = true;
            }
        }

        private void Execute()
        {
            foreach (ProjectAssemblyItem assemblyItem in _ProjectAssemblyItemDictionary.Values)
            {
                XmlElement referenceElement = CreateElement("Reference");

                _ReferenceItemGroup.AppendChild(referenceElement);

                if (!string.IsNullOrEmpty(assemblyItem.SpecificVersionElement))
                {
                    XmlElement specificVersionElement = CreateElement("SpecificVersion");

                    referenceElement.AppendChild(specificVersionElement);

                    specificVersionElement.InnerText = assemblyItem.SpecificVersionElement;
                }

                if (!string.IsNullOrEmpty(assemblyItem.HintPathElement))
                {
                    XmlElement hintPathElement = CreateElement("HintPath");

                    referenceElement.AppendChild(hintPathElement);

                    hintPathElement.InnerText = assemblyItem.HintPathElement;
                }

                if (!string.IsNullOrEmpty(assemblyItem.IncludeAttribute))
                {
                    XmlAttribute includeAttribute = XmlDocument.CreateAttribute("Include");

                    referenceElement.Attributes.Append(includeAttribute);

                    includeAttribute.InnerText = assemblyItem.IncludeAttribute;
                }
            }
        }

        private void CleanProjectAssemblyDictionary()
        {
            XmlNodeList referenceNodeList = _ReferenceItemGroup.SelectNodes("p:Reference", XmlNamespaceManager);

            foreach (XmlNode referenceNode in referenceNodeList)
            {
                string[] assemblyText = referenceNode.Attributes["Include"].InnerText.Split(',');

                string assemblyName = assemblyText[0].Trim();

                if (_ProjectAssemblyItemDictionary.FindAssembly(assemblyName))
                {
                    _ProjectAssemblyItemDictionary.Remove(assemblyName);
                }
            }
        }
    }
}
