using System.Collections.Generic;
using System.Reflection;

namespace NetAspect.Weaver.Core.Model.Aspect
{
   public class Interceptors
   {
       private readonly List<MethodInfo> _methodInfo;

       public Interceptors(List<MethodInfo> methodInfo_P)
      {
         _methodInfo = methodInfo_P;
      }

      public IEnumerable<MethodInfo> Methods
      {
         get { return _methodInfo; }
      }
   }
}
