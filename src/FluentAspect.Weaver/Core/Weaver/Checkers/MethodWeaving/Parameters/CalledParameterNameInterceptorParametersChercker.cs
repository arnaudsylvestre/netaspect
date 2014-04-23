using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Weaver.Checkers.MethodWeaving.Parameters
{
   public class CalledParameterNameInterceptorParametersChercker : IInterceptorParameterChecker
   {
      private readonly ParameterDefinition _parameter;

      public CalledParameterNameInterceptorParametersChercker(ParameterDefinition parameter)
      {
         _parameter = parameter;
      }

      public void Check(ParameterInfo parameter, ErrorHandler errorListener)
      {
         Ensure.NotReferenced(parameter, errorListener);
         Ensure.OfType(parameter, errorListener, _parameter);
      }
   }
}