using System;

namespace FluentAspect.Sample.AOP
{
   class CheckWithReturnAttribute : Attribute
    {
      string NetAspectAttributeKind = "MethodWeaving";
      public void After(ref string result)
      {
          result = "Weaved";
      }

    }
}
