using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentAspect.Weaver.Weavers
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