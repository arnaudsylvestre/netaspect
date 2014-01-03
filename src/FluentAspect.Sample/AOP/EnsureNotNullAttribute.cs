using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Sample.Dep;

namespace FluentAspect.Sample.AOP
{
    public class EnsureNotNullAttribute : Attribute
    {
        IEnumerable<Assembly> AssembliesToWeave = new List<Assembly>() {typeof (DepClassToWeave).Assembly};
        string NetAspectAttributeKind = "MethodWeaving";

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