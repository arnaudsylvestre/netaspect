using System.Collections.Generic;

namespace NetAspect.Doc.Builder
{
    public class Possibility
    {
        public Possibility()
        {
            Events = new List<PossibilityEvent>();
            AvailableParameters = new List<Parameter>();
        }

        public string Kind { get; set; }
        public string Description { get; set; }
        public List<PossibilityEvent> Events { get; set; }
        public List<Parameter> AvailableParameters { get; set; }

        public string Title { get; set; }

        public string Member { get; set; }

        public string Group { get; set; }
    }
}