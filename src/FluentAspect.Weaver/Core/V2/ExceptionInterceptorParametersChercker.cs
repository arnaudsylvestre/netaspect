using System;
using System.Reflection;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Checkers;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters
{
    public class ExceptionInterceptorParametersChercker : IInterceptorParameterChecker
    {
        public void Check(ParameterInfo parameter, IErrorListener errorListener)
        {
            Ensure.NotReferenced(parameter, errorListener);
            Ensure.OfType(parameter, errorListener, typeof (Exception).FullName);
            //}, (info, instructions) => { instructions.Add(Instruction.Create(OpCodes.Ldloc, exception)); }
        }
    }
}