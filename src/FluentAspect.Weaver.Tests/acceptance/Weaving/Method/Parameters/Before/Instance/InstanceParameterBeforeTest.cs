using System;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters.Before.Instance
{
   public class InstanceParameterBeforeTest : SimpleAcceptanceTest
   {
      protected override void ConfigureAssembly(AssemblyDefinitionDefiner assembly)
      {
         var aspect = CreateAspect(assembly);
         Weave(assembly, aspect);
      }

      private static void Weave(AssemblyDefinitionDefiner assembly, MethodWeavingAspectDefiner aspect)
      {
         var method = assembly.WithType("MyClassToWeave").WithMethod("MyMethodToWeave");
         method.AddAspect(aspect);
      }

      private static MethodWeavingAspectDefiner CreateAspect(AssemblyDefinitionDefiner assembly)
      {
         var aspect = assembly.WithMethodWeavingAspect("MyAspectAttribute");
         aspect.AddBefore()
               .WithParameter<object>("instance");
         return aspect;
      }

      protected override void EnsureAssembly(Assembly assembly_P)
      {
          var o = assembly_P.CreateObject("MyClassToWeave");
          o.CallMethod("MyMethodToWeave");

          var actual = assembly_P.FindType("MyAspectAttribute").GetField("Beforeinstance", BindingFlags.Public | BindingFlags.Static).GetValue(null);

          Assert.AreEqual(o, actual);
      }
   }

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