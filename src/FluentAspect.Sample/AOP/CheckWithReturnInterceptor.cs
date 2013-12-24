using System;
using System.Reflection;

namespace FluentAspect.Sample
{
   class CheckWithReturnInterceptorNetAspectAttribute : Attribute
    {

      public void After(ref string result)
      {
          result = "Weaved";
      }

    }
}
