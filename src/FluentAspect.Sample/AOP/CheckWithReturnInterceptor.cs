using System;

namespace FluentAspect.Sample.AOP
{
   class CheckWithReturnInterceptorNetAspectAttribute : Attribute
    {

      public void After(ref string result)
      {
          result = "Weaved";
      }

    }
}
