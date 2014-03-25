using System;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Core.Weaver.Checkers
{
    public class ExceptionInterceptorParametersChercker : IInterceptorParameterChecker
    {
        public void Check(ParameterInfo parameter, ErrorHandler errorListener)
        {
            Ensure.NotReferenced(parameter, errorListener);
            Ensure.OfType(parameter, errorListener, typeof (Exception).FullName);
            //}, (info, instructions) => { instructions.Add(Instruction.Create(OpCodes.Ldloc, exception)); }
        }
    }
}