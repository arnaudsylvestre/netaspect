using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Core.Weaver;
using NetAspect.Weaver.Factory;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.Helpers
{
   public class AppDomainIsolatedTestRunner : MarshalByRefObject
   {
      public string RunFromType(string dll_L, string typeName, List<ErrorReport.Error> checkErrors, string otherDll, string otherTypeName, string tempDirectory)
      {
         Copy(tempDirectory, "NetAspect.Weaver.dll");
         Copy(tempDirectory, "NetAspect.Sample.Dep.dll");
         Copy(tempDirectory, "Mono.Cecil.dll");
         Copy(tempDirectory, "nunit.framework.dll");
         Assembly assembly = Assembly.LoadFrom(dll_L);
         Assembly otherAssembly = Assembly.LoadFrom(otherDll);
         Type type = assembly.GetTypes().First(t => t.FullName == typeName);
         Type otherType = otherAssembly.GetTypes().First(t => t.FullName == otherTypeName);
         WeaverEngine weaver = WeaverFactory.Create(null, t => TypeMustBeSaved(t, typeName, otherTypeName));
         ErrorReport errorHandler = weaver.Weave(ComputeTypes(type, otherType), a => ChangeName(dll_L, a, tempDirectory), ComputeTypes(type, otherType));
         var builder = new StringBuilder();
         errorHandler.Dump(builder);
         File.WriteAllText(@"C:\temp.txt", builder.ToString());
         Assert.AreEqual(checkErrors.Select(e => e.Message), errorHandler.Errors.Select(e => e.Message));
         return builder.ToString();
      }

      private static void Copy(string tempDirectory, string filename)
      {
         File.Copy(filename, Path.Combine(tempDirectory, filename));
      }

      private static string ChangeName(string dll_L, string a, string tempDirectory)
      {
         return Path.Combine(tempDirectory, Path.GetFileName(a));
      }

      private bool TypeMustBeSaved(TypeDefinition typeDefinition_P, string typeName_P, string otherTypeName_P)
      {
         if (typeDefinition_P.Module.Assembly.FullName.Contains("NetAspect.Sample.Dep"))
            return true;
         //return true;
         typeName_P = typeName_P.Replace("+", "/");
         otherTypeName_P = otherTypeName_P.Replace("+", "/");
         var whiteList = new List<string>
         {
            typeof (NetAspectTest<>).FullName,
            typeof (NetAspectTest<,>).FullName,
            typeof (RunWeavingTest).FullName,
            typeof (AppDomainIsolatedTestRunner).FullName,
            typeof (ErrorHandlerExtensions).FullName,
            "<Module>",
            typeName_P,
            otherTypeName_P
         };
         if (whiteList.Contains(typeDefinition_P.FullName))
            return true;
         foreach (TypeDefinition typeDefinition_L in typeDefinition_P.NestedTypes)
         {
            if (typeDefinition_L.FullName == typeName_P || typeDefinition_L.FullName == otherTypeName_P)
               return true;
         }
         return false;
      }

      private static Type[] ComputeTypes(Type type, Type otherType)
      {
         List<Type> computeTypes = type.DeclaringType.GetNestedTypes().ToList();
         computeTypes.Add(otherType);
         return computeTypes.ToArray();
      }

      public void Ensure(string assemblyFile, string typeName)
      {
         Assembly.LoadFrom(Path.Combine(Path.GetDirectoryName(assemblyFile), "NetAspect.Sample.Dep.dll"));
         Assembly assemblyToTest = Assembly.LoadFrom(assemblyFile);

         Type type_L = assemblyToTest.GetType(typeName);
         type_L.GetMethod("Check").Invoke(Activator.CreateInstance(type_L), new object[0]);
      }
   }
}
