using System;
using System.Reflection;

namespace NetAspect.Sample.AOP
{
    public class CheckBeforeAspectAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void Before(object instance, MethodInfo method, object[] parameters)
        {
            ((BeforeParameter) parameters[0]).Value = "Value set in before";
        }
    }
}