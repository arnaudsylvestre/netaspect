using System;

namespace FluentAspect.Sample.AOP
{
   class CheckWithReturnInterceptorAttribute : Attribute
    {
      string NetAspectAttributeKind = "MethodWeaving";
      public void After(ref string result)
      {
          result = "Weaved";
      }

    }
}
