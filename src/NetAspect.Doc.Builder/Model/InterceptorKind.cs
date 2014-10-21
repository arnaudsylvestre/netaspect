using System.Collections.Generic;

namespace NetAspect.Doc.Builder.Model
{
    public class InterceptorKind
    {
        public string Name { get; set; }
        public List<InterceptorKindConfiguration> Configurations { get; set; }
    }
}