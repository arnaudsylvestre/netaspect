using System;

namespace FluentAspect.Weaver.Core.Weavers.Methods
{
    public interface IAroundWeaverConfiguration
    {
        string ToCallBefore(Type interceptorType);
        string ToCallAfter(Type interceptorType);
        string ToCallOnException(Type interceptorType);
    }
}