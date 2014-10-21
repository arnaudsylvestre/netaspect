using System;

namespace NetAspect.Sample.AOP
{
    public class AssemblyAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void Before()
        {
            throw new NotSupportedException("Weaved through assembly");
        }

        public static bool WeaveMethod(string methodName)
        {
            return methodName == "WeavedThroughAssembly";
        }
    }
}