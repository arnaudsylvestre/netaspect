using System.Collections.Generic;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Core.Errors
{
    public interface IErrorListener
    {
        void OnError(ErrorCode code, FileLocation location, params object[] parameters);
    }
}