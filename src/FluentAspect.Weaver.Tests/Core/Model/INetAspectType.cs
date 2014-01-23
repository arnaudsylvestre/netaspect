using Mono.Cecil;

namespace FluentAspect.Weaver.Tests.Core.Model
{
    public interface INetAspectType
    {
        TypeDefinition TypeDefinition { get; }
    }
}