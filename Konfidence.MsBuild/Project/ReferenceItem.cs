using Konfidence.Base;

namespace Konfidence.MsBuild.Project
{
    public class ReferenceItem 
    {
        public ReferenceItem()
        {
            Name = string.Empty;
            SpecificVersionElement = string.Empty;
            HintPathElement = string.Empty;
            IncludeAttribute = string.Empty;

            IsProjectReference = false;
        }

        public string SpecificVersionElement { get; set; }

        public string HintPathElement { get; set; }

        public string IncludeAttribute { get; set; }

        public string Name { get; set; }

        public bool IsProjectReference { get; set; }

    }
}