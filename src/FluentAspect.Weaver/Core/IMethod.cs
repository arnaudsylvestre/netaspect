using System;

namespace FluentAspect.Core.Methods
{
    public interface IMethod
    {
        string Name { get; }
        IType DeclaringType { get; }
    }
    public interface IProperty
    {
        string Name { get; }
        IType DeclaringType { get; }
    }
}