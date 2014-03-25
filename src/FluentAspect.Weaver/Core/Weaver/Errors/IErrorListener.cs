namespace FluentAspect.Weaver.Core.Weaver.Errors
{
    public interface IErrorListener
    {
        void OnError(string message, params object[] args);
        void OnWarning(string message, params object[] args);
    }
}