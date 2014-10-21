using System.Collections.Generic;

namespace NetAspect.Doc.Builder
{
    public class TestDescription
    {
        public TestDescription()
        {
            AspectParameters = new List<string>();
        }

        public string CallCode { get; set; }
        public string AspectCode { get; set; }
        public string ClassToWeaveCode { get; set; }
        public string Possibility { get; set; }
        public string Called { get; set; }
        public string Description { get; set; }
        public string MethodName { get; set; }
        public List<string> AspectParameters { get; set; }
        public string Kind { get; set; }
        public string Member { get; set; }
    }
}