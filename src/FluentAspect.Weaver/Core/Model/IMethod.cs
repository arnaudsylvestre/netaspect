using System.Collections.Generic;

namespace FluentAspect.Weaver.Core.Model
{
    public interface IMethod
    {
        string Name { get; }
        IType DeclaringType { get; }
        IEnumerable<IParameter> Parameters { get; }
    }
    public interface IProperty
    {
        string Name { get; }
        IType DeclaringType { get; }
        IEnumerable<IParameter> Indices { get; }
    }

    public interface IParameter
    {
        IType Type { get; }
        string Name { get; }
    }
}