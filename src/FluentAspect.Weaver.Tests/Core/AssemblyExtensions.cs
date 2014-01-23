using System;
using System.Linq;
using System.Reflection;

namespace FluentAspect.Weaver.Tests.Core
{
    public static class AssemblyExtensions
   {
       public static object CreateObject(this Assembly assembly, string type, params object[] parameters)
      {
          var first = FindType(assembly, type);

          return Activator.CreateInstance(first, parameters);
      }

       public static Type FindType(this Assembly assembly, string type)
       {
           return (from t in assembly.GetTypes() where t.Name == type select t).First();
       }

       public static object CallMethod(this object o, string methodName, params object[] parameters)
       {
           return o.GetType().GetMethod(methodName).Invoke(o, parameters);
       }
       public static object GetFieldValue(this object o, string fieldName)
       {
           return o.GetType().GetField(fieldName).GetValue(o);
       }

        public static object GetStaticFieldValue(this Assembly assembly, string typeName, string fieldName)
       {
           return assembly.FindType(typeName).GetField(fieldName).GetValue(null);
            
        }
   }
}