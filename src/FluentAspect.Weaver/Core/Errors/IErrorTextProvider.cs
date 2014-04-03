namespace NetAspect.Weaver.Core.Errors
{
    public interface IErrorTextProvider
    {
        string GetMessage(ErrorKind kind);
    }
}