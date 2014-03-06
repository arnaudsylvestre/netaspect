using System;

namespace FluentAspect.Weaver.Core.Model
{
    public class CallWeavingConfiguration
    {
        public Type Type { get; set; }

        public Interceptor Before { get; set; }

        public Interceptor After { get; set; }
    }
}