using System;
using System.Reflection;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters.Before.Instance
{
    public class NetAspectAttribute
    {
        private Type type;

        public NetAspectAttribute(Assembly assembly, string name)
        {
            type = assembly.FindType(name);
        }

        public object BeforeInstance
        {
            get { return type.GetField("Beforeinstance", BindingFlags.Public | BindingFlags.Static).GetValue(null); }
        }
    }
}