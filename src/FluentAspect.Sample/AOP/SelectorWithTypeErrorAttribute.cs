using System;

namespace NetAspect.Sample.AOP
{
    public class SelectorWithTypeErrorAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void Before(string s)
        {
            if (s == null)
                throw new ArgumentNullException("s");
        }


        public static bool WeaveMethod(int methodName)
        {
            return false;
        }
    }
}