namespace FluentAspect.Weaver.Core.Model
{
    public interface IParameter
    {
        IType Type { get; }
        string Name { get; }
    }
}