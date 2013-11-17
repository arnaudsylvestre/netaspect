using System;

namespace FluentAspect.Core.Methods
{
    public interface IMethod
    {
        string Name { get; }
        Type DeclaringType { get; }
    }
}