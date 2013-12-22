using System;

namespace FluentAspect.Core.Methods
{
    public interface IMethod
    {
        string Name { get; }
        IType DeclaringType { get; }
    }
    public interface IConstructor
    {
        string Name { get; }
        IType DeclaringType { get; }
    }
}