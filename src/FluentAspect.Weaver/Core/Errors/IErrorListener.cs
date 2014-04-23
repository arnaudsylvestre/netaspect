using System.Collections.Generic;

namespace NetAspect.Weaver.Core.Errors
{
    public interface IErrorListener
    {
        void OnError(string message, params object[] args);
        void OnWarning(string message, params object[] args);
        void OnError(ErrorCode code, FileLocation location, List<object> parameters);
    }
}