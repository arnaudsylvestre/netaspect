using System.Reflection;

namespace FluentAspect.Core.Expressions
{
   public class MethodCall
   {
      public MethodCall(object this_P, MethodInfo method, object[] parameters_P)
      {
         This = this_P;
         Method = method;
         Parameters = parameters_P;
      }

      public object This { get; private set; }
      public MethodInfo Method { get; private set; }
      public object[] Parameters { get; private set; }
   }
}