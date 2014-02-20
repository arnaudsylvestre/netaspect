namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters
{
    public interface IErrorListener
    {
        void OnError(string message, params object[] args);
        void OnWarning(string message, params object[] args);
    }
}