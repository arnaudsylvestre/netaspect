using System;

namespace FluentAspect.Weaver.Core.Model
{
    public class MethodWeavingConfiguration
    {
        public Type Type { get; set; }

        public Interceptor Before { get; set; }

        public Interceptor After { get; set; }

        public Interceptor OnException { get; set; }

        public Interceptor OnFinally { get; set; }
    }
}