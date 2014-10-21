using System;

namespace NetAspect.Sample.AOP
{
    public class CheckWithParametersReferencedAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void After(ref object[] parameters, ref string result)
        {
            result = parameters[0].ToString();
        }
    }
}