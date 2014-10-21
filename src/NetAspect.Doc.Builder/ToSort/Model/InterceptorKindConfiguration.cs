using System.Collections.Generic;

namespace NetAspect.Doc.Builder.Model
{
    public class InterceptorKindConfiguration
    {
        public List<ParameterConfiguration> Parameters { get; set; }
        public List<InterceptorConfiguration> Interceptors { get; set; }

        public string Title { get; set; }
    }
}