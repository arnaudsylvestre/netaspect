﻿using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Weaver.Checkers.MethodWeaving.Parameters
{
    public class ParameterNameInterceptorParametersChercker : IInterceptorParameterChecker
    {
        private readonly ParameterDefinition _parameterName;

        public ParameterNameInterceptorParametersChercker(ParameterDefinition parameterName)
        {
            _parameterName = parameterName;
        }

        public void Check(ParameterInfo parameter, ErrorHandler errorListener)
        {
            Ensure.OfType(parameter, errorListener, _parameterName);
            Ensure.NotOut(parameter, errorListener);
        }
    }
}