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

       public object AfterMethod
       {
          get { return type.GetField("Aftermethod", BindingFlags.Public | BindingFlags.Static).GetValue(null); }
       }

       public object BeforeMethod
       {
          get { return type.GetField("Beforemethod", BindingFlags.Public | BindingFlags.Static).GetValue(null); }
       }

       public object OnExceptionMethod
       {
          get { return type.GetField("OnExceptionmethod", BindingFlags.Public | BindingFlags.Static).GetValue(null); }
       }

        public object BeforeCaller
       {
           get { return type.GetField("Beforecaller", BindingFlags.Public | BindingFlags.Static).GetValue(null); }
        }

        public object BeforeUpdateFieldValueLineNumber
        {
            get { return type.GetField("BeforeUpdateFieldValuelineNumber", BindingFlags.Public | BindingFlags.Static).GetValue(null); }
        }

        public object BeforeUpdateFieldValueValue
        {
            get { return type.GetField("BeforeUpdateFieldValuevalue", BindingFlags.Public | BindingFlags.Static).GetValue(null); }
        }

        public object GetInstance(string methodName)
        {
            return type.GetField(string.Format("{0}instance", methodName), BindingFlags.Public | BindingFlags.Static).GetValue(null);
        }
    }
}