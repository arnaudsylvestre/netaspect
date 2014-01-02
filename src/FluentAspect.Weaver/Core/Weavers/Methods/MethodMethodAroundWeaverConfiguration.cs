using System;

namespace FluentAspect.Weaver.Core.Weavers.Methods
{
    public class MethodMethodAroundWeaverConfiguration : IAroundWeaverConfiguration
    {
        public string ToCallBefore(Type interceptorType)
        {
            return "Before";
        }

        public string ToCallAfter(Type interceptorType)
        {
            return "After";
        }

        public string ToCallOnException(Type interceptorType)
        {
            return "OnException";
        }
    }
}