using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Sample.Dep;

namespace FluentAspect.Sample.AOP
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

    public class SelectorWithNoDefaultConstructorAttribute : Attribute
    {
        public IEnumerable<Assembly> AssembliesToWeave = new List<Assembly> { typeof(DepClassToWeave).Assembly };
        public string NetAspectAttributeKind = "MethodWeaving";

        public SelectorWithNoDefaultConstructorAttribute(string toto)
        {
            
        }

        public void Before(string s)
        {
            if (s == null)
                throw new ArgumentNullException("s");
        }


        public static bool WeaveMethod(string methodName)
        {
            return methodName == "EnsureNotNull";
        }
    }
}