using System.Reflection;

namespace FluentAspect.Weaver.Core.Model
{
   public class Interceptor
   {
      private readonly MethodInfo _methodInfo;

      public Interceptor(MethodInfo methodInfo_P)
      {
         _methodInfo = methodInfo_P;
      }

      public MethodInfo Method
      {
         get { return _methodInfo; }
      }
   }
}