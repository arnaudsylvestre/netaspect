using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Weaver.Checkers.CallWeaving.Called
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


    public class CalledInterceptorForPropertyParametersChercker : IInterceptorParameterChecker
    {
        private readonly PropertyDefinition property;

        public CalledInterceptorForPropertyParametersChercker(PropertyDefinition property)
        {
            this.property = property;
        }

        public void Check(ParameterInfo parameter, ErrorHandler errorListener)
        {
            Ensure.NotReferenced(parameter, errorListener);
            Ensure.OfType(parameter, errorListener, typeof(object).FullName, property.DeclaringType.FullName);
            Ensure.NotStaticButDefaultValue(parameter, errorListener, property);
        }
    }
    public class CalledInterceptorForMethodParametersChercker : IInterceptorParameterChecker
    {
        private readonly MethodDefinition methodDefinition;

        public CalledInterceptorForMethodParametersChercker(MethodDefinition methodDefinition)
        {
            this.methodDefinition = methodDefinition;
        }

        public void Check(ParameterInfo parameter, ErrorHandler errorListener)
        {
            Ensure.NotReferenced(parameter, errorListener);
            Ensure.OfType(parameter, errorListener, typeof(object).FullName, methodDefinition.DeclaringType.FullName);
            Ensure.NotStaticButDefaultValue(parameter, errorListener, methodDefinition);
        }
    }
}