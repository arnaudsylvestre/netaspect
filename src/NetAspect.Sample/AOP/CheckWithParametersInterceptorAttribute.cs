using System;

namespace NetAspect.Sample.AOP
{
    public class CheckWithParametersInterceptorAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void After(object[] parameters, ref string result)
        {
            result = parameters[0].ToString();
        }
    }
}