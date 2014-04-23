using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Weaver.Checkers.CallWeaving.Called
{
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