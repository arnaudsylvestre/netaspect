using System.Reflection;

namespace FluentAspect.Weaver.Core
{
   public class InterceptorInformation
   {
      private readonly MethodInfo _methodInfo;

      public InterceptorInformation(MethodInfo methodInfo_P)
      {
         _methodInfo = methodInfo_P;
      }

      public MethodInfo Method
      {
         get { return _methodInfo; }
      }
   }
}