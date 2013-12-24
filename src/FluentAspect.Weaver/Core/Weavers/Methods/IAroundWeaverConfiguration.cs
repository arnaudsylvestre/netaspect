using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentAspect.Weaver.Weavers
{
    public interface IAroundWeaverConfiguration
    {
        string ToCallBefore(Type interceptorType);
        string ToCallAfter(Type interceptorType);
        string ToCallOnException(Type interceptorType);
    }
}