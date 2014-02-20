﻿using System.Reflection;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Checkers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters
{
    public class InstanceInterceptorParametersChercker : IInterceptorParameterChecker
    {
        private MethodDefinition methodDefinition;

        public InstanceInterceptorParametersChercker(MethodDefinition methodDefinition)
        {
            this.methodDefinition = methodDefinition;
        }

        public void Check(ParameterInfo parameter, IErrorListener errorListener)
        {
            Ensure.NotReferenced(parameter, errorListener);
            Ensure.OfType(parameter, errorListener, typeof(object).FullName, methodDefinition.DeclaringType.FullName);
            Ensure.NotStatic(parameter, errorListener, methodDefinition);
        }
    }
}