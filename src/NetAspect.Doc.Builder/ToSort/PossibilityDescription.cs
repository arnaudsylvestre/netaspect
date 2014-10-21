using System.Collections.Generic;

namespace NetAspect.Doc.Builder
{
    public class PossibilityDescription
    {
        public string Kind { get; set; }
        public string Description { get; set; }
        public List<PossibilityEvent> Events { get; set; }

        public string Title { get; set; }

        public string Member { get; set; }
        public string Group { get; set; }
    }
}