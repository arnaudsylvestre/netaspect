using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentAspect.Weaver.Weavers
{
    public class MethodMethodAroundWeaverConfiguration : IAroundWeaverConfiguration
    {
        public IEnumerable<string> ToCallBefore(Type interceptorType)
        {
            return new List<string>{"Before"};
        }

        public IEnumerable<string> ToCallAfter(Type interceptorType)
        {
            return new List<string> { "After" };
        }

        public IEnumerable<string> ToCallOnException(Type interceptorType)
        {
            return new List<string> { "OnException" };
        }
    }
}