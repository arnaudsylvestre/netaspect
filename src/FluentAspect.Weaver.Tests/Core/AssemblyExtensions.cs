using System;
using System.Linq;
using System.Reflection;
using FluentAspect.Sample;
using FluentAspect.Weaver.Helpers;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters.Before.Instance
{
    public static class AssemblyExtensions
   {
       public static object CreateObject(this Assembly assembly, string type, params object[] parameters)
      {
          var first = FindType(assembly, type);

          return first.GetConstructors()[0].Invoke(parameters);
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
   }
}