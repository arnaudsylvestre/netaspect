using System;
using System.Reflection;

namespace FluentAspect.Sample.Attributes
{
    
    public class CheckBeforeNetAspectAttribute : Attribute
    {
        public void Before(object instance, MethodInfo method, object[] parameters)
        {
            ((BeforeParameter)parameters[0]).Value = "Value set in before";
        }
    }
}