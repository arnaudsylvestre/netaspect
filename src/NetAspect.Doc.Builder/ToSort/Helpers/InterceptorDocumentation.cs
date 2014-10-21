using System.Collections.Generic;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder.Helpers
{
    public class InterceptorDocumentation
    {
        public InterceptorDocumentation()
        {
            Parameters = new List<string>();
        }

        public string Name { get; set; }

        public string Kind
        {
            get { return InterceptorModelHelper.ExtractKind(Name).ToString(); }
        }

        public string Event
        {
            get { return InterceptorModelHelper.ExtractEvent(Name).ToString(); }
        }

        public List<string> Parameters { get; set; }
        public string Member { get; set; }

        public string CallCode { get; set; }

        public string AspectCode { get; set; }

        public string ClassToWeaveCode { get; set; }

        public string When { get; set; }
    }
}