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

        public object BeforeCallMethodValueValue
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterCallMethodValueColumnNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterCallMethodValueFileName
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterCallMethodValueFilePath
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterCallMethodValueLineNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterCallMethodValueValue
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeCallMethodValueCaller
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeCallMethodValueColumnNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeCallMethodValueFileName
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeCallMethodValueFilePath
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeCallMethodValueLineNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeUpdatePropertyValueValue
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterGetPropertyValueCaller
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterGetPropertyValueColumnNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterGetPropertyValueFileName
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterGetPropertyValueFilePath
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterGetPropertyValueLineNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterGetPropertyValueValue
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeGetPropertyValueCaller
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeGetPropertyValueColumnNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeGetPropertyValueFileName
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeGetPropertyValueFilePath
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeGetPropertyValueLineNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeGetPropertyValueValue
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterUpdatePropertyValueCaller
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterUpdatePropertyValueColumnNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterUpdatePropertyValueFileName
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterUpdatePropertyValueFilePath
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterUpdatePropertyValueLineNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterUpdatePropertyValueValue
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeUpdatePropertyValueCaller
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeUpdatePropertyValueColumnNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeUpdatePropertyValueFileName
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeUpdatePropertyValueFilePath
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeUpdatePropertyValueLineNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeSubscribeEventValueValue
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterCallEventValueCaller
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterCallEventValueColumnNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterCallEventValueFileName
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterCallEventValueFilePath
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterCallEventValueLineNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterCallEventValueValue
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeCallEventValueCaller
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeCallEventValueColumnNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeCallEventValueFileName
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeCallEventValueFilePath
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeCallEventValueLineNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeCallEventValueValue
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterSubscribeEventValueCaller
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterSubscribeEventValueColumnNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterSubscribeEventValueFileName
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterSubscribeEventValueFilePath
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterSubscribeEventValueLineNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object AfterSubscribeEventValueValue
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeSubscribeEventValueCaller
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeSubscribeEventValueColumnNumber
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeSubscribeEventValueFileName
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeSubscribeEventValueFilePath
        {
            get { throw new NotImplementedException(); }
        }

        public object BeforeSubscribeEventValueLineNumber
        {
            get { throw new NotImplementedException(); }
        }
    }
}