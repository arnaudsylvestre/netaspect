using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Checkers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.V2.Weaver.Checkers
{
    public class InstanceInterceptorParametersChercker : IInterceptorParameterChecker
    {
        private readonly MethodDefinition methodDefinition;

        public InstanceInterceptorParametersChercker(MethodDefinition methodDefinition)
        {
            this.methodDefinition = methodDefinition;
        }

        public void Check(ParameterInfo parameter, ErrorHandler errorListener)
        {
            Ensure.NotReferenced(parameter, errorListener);
            Ensure.OfType(parameter, errorListener, typeof (object).FullName, methodDefinition.DeclaringType.FullName);
            Ensure.NotStatic(parameter, errorListener, methodDefinition);
        }
    }


    public class CalledInterceptorParametersChercker : IInterceptorParameterChecker
    {
        private readonly FieldDefinition fieldDefinition;

        public CalledInterceptorParametersChercker(FieldDefinition fieldDefinition)
        {
            this.fieldDefinition = fieldDefinition;
        }

        public void Check(ParameterInfo parameter, ErrorHandler errorListener)
        {
            Ensure.NotReferenced(parameter, errorListener);
            Ensure.OfType(parameter, errorListener, typeof(object).FullName, fieldDefinition.DeclaringType.FullName);
            Ensure.NotStatic(parameter, errorListener, fieldDefinition);
        }
    }
}