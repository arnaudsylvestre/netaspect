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

        public object BeforeUpdateFieldValueFilePath
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeUpdateFieldValueColumnNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeUpdateFieldValueFileName
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeUpdateFieldValueCaller
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterUpdateFieldValueValue
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterUpdateFieldValueCaller
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterUpdateFieldValueColumnNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterUpdateFieldValueFileName
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterUpdateFieldValueFilePath
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterUpdateFieldValueLineNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeGetFieldValueValue
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterGetFieldValueCaller
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterGetFieldValueColumnNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterGetFieldValueFileName
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterGetFieldValueLineNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterGetFieldValueFilePath
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterGetFieldValueValue
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeGetFieldValueCaller
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeGetFieldValueColumnNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeGetFieldValueFileName
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeGetFieldValueFilePath
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeGetFieldValueLineNumber
        {
            get { throw new NotImplementedException(); }
        }
    }
}