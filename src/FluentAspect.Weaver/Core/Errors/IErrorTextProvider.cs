namespace FluentAspect.Weaver.Core.Errors
{
    public interface IErrorTextProvider
    {
        string GetMessage(ErrorKind kind);
    }
}