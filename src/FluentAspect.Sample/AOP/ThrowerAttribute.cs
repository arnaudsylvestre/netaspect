using System;
using System.Reflection;

namespace FluentAspect.Sample.Attributes
{

    public class ThrowerNetAspectAttribute : Attribute
    {
        public void Before(object instance, MethodInfo method, object[] parameters)
        {
            if ((bool)parameters[0])
                throw new NotSupportedException();
        }
    }
}