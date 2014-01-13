using System;
using System.Reflection;

namespace FluentAspect.Weaver.Tests.Core
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

        public object AfterInstance
        {
            get { return type.GetField("Afterinstance", BindingFlags.Public | BindingFlags.Static).GetValue(null); }
        }

        public object OnExceptionInstance
        {
            get { return type.GetField("OnExceptioninstance", BindingFlags.Public | BindingFlags.Static).GetValue(null); }
        }

       public object AfterParameters
        {
           get { return type.GetField("Afterparameters", BindingFlags.Public | BindingFlags.Static).GetValue(null); }
       }

       public object GetInstance(string methodName)
        {
            return type.GetField(string.Format("{0}instance", methodName), BindingFlags.Public | BindingFlags.Static).GetValue(null);
        }
    }
}