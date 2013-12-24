using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentAspect.Weaver.Weavers
{
    public interface IAroundWeaverConfiguration
    {
        IEnumerable<string> ToCallBefore(Type interceptorType);
        IEnumerable<string> ToCallAfter(Type interceptorType);
        IEnumerable<string> ToCallOnException(Type interceptorType);
    }
}