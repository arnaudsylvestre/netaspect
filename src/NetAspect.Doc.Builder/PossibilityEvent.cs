using System.Collections.Generic;

namespace NetAspect.Doc.Builder
{
    public class PossibilityEvent
    {
        public PossibilityEvent()
        {
            Parameters = new List<Parameter>();
        }

        public string Kind { get; set; }
        public string Description { get; set; }
        public string MethodName { get; set; }
        public string Called { get; set; }

        public List<Parameter> Parameters { get; set; }

        public Sample Sample { get; set; }
    }
}