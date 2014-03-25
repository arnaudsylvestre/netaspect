using System;

namespace FluentAspect.Sample
{
    public class CheckCallerAttribute : Attribute
    {
        public string NetAspectAttributeKind = "CallWeaving";

        public static void AfterCall(object caller)
        {
            throw new Exception(caller.GetType() == typeof (MyClassToWeave) ? "OK" : "KO");
        }
    }
}