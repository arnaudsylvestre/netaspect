using System;

namespace FluentAspect.Sample.AOP
{
    public class AssemblyNetAspectAttribute : Attribute
    {
        public bool IsNetAspectAttribute { get { return true; } }

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