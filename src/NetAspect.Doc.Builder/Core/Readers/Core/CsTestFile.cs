using System.Collections.Generic;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder.Helpers
{
    public class CsTestFile
    {
        public CsTestFile()
        {
            Parameters = new List<string>();
            UserCode = "";
            ClassToWeaveCode = "";
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

        public bool CanBeRun { get; set; }

        public string TestName { get; set; }

        public string UserCode { get; set; }
    }
}