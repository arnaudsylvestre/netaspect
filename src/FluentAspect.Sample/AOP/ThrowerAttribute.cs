using System;
using System.Reflection;

namespace FluentAspect.Sample.AOP
{

    public class ThrowerAttribute : Attribute
    {
       string NetAspectAttributeKind = "MethodWeaving";

        public void Before(object instance, MethodInfo method, object[] parameters)
        {
            if ((bool)parameters[0])
                throw new NotSupportedException();
        }
    }
}