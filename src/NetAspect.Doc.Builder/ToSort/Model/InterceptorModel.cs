using System.Collections.Generic;

namespace NetAspect.Doc.Builder.Model
{
    public class InterceptorModel
    {
        public InterceptorModel()
        {
            Parameters = new List<Parameter>();
        }

        public string Name { get; set; }

        public List<Parameter> Parameters { get; set; }

        public Event Event
        {
            get { return InterceptorModelHelper.ExtractEvent(Name); }
        }

        public Kind Kind
        {
            get { return InterceptorModelHelper.ExtractKind(Name); }
        }

        public class Parameter
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}