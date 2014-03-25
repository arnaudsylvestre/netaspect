using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Weaver.Checkers
{
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
            Ensure.NotStaticButDefaultValue(parameter, errorListener, fieldDefinition);
        }
    }
}