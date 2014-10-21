using System.Collections.Generic;

namespace NetAspect.Doc.Builder
{
    public class DocumentationFromTest
    {
        public List<InterceptorDescription> Interceptors = new List<InterceptorDescription>();
        public List<ParameterDescription> Parameters = new List<ParameterDescription>();
        public List<PossibilityDescription> Possibilities = new List<PossibilityDescription>();
        public List<TestDescription> Tests = new List<TestDescription>();
    }
}