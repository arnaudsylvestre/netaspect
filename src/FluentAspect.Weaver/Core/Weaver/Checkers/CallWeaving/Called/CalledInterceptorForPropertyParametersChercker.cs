using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Weaver.Checkers.CallWeaving.Called
{
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
}