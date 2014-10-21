using System.Collections.Generic;

namespace NetAspect.Doc.Builder.Model
{
    public class ParameterModel
    {
        public string Name { get; set; }
        public List<ParameterSample> Samples { get; set; }

        public class ParameterSample
        {
            public string ClassToWeaveCode { get; set; }
            public string AspectCode { get; set; }
            public string CallCode { get; set; }

            public string Description { get; set; }
        }
    }
}