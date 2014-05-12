using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Core.Weaver;
using NetAspect.Weaver.Core.Weaver.Engine;

namespace NetAspect.Weaver.Tests.Helpers
{
    public class AppDomainIsolatedTestRunner : MarshalByRefObject
    {
        public string RunFromType(string dll_L, string typeName, List<ErrorReport.Error> checkErrors, string otherDll, string otherTypeName)
        {
            Assembly assembly = Assembly.LoadFrom(dll_L);
            Assembly otherAssembly = Assembly.LoadFrom(otherDll);
            Type type = assembly.GetTypes().First(t => t.FullName == typeName);
            Type otherType = otherAssembly.GetTypes().First(t => t.FullName == otherTypeName);
            WeaverEngine weaver = WeaverFactory.Create(t => TypeMustBeSaved(t, typeName));
            var errorHandler = weaver.Weave(ComputeTypes(type, otherType), ComputeTypes(type, otherType), (a) => a + ".Test");
            var builder = new StringBuilder();
            errorHandler.Dump(builder);
            File.WriteAllText(@"C:\temp.txt", builder.ToString());
            Assert.AreEqual(checkErrors.Select(e => e.Message), errorHandler.Errors.Select(e => e.Message));
            return builder.ToString();
        }

       private bool TypeMustBeSaved(TypeDefinition typeDefinition_P, string typeName_P)
       {
          //return true;
          typeName_P = typeName_P.Replace("+", "/");
          List<string> whiteList = new List<string>()
          {
             typeof(NetAspectTest<>).FullName,
             typeof(NetAspectTest<,>).FullName,
             typeof(RunWeavingTest).FullName, 
             typeof(AppDomainIsolatedTestRunner).FullName,  
             typeof(ErrorHandlerExtensions).FullName,             
             "<Module>",
             typeName_P,
          };
          if (whiteList.Contains(typeDefinition_P.FullName))
             return true;
          foreach (var typeDefinition_L in typeDefinition_P.NestedTypes)
          {
             if (typeDefinition_L.FullName == typeName_P)
                return true;
          }
          return false;
       }

       private static Type[] ComputeTypes(Type type, Type otherType)
        {
            var computeTypes = type.DeclaringType.GetNestedTypes().ToList();
            computeTypes.Add(otherType);
            return computeTypes.ToArray();
        }

        public void Ensure(string assemblyFile, string typeName)
        {
            var assemblyToTest = Assembly.LoadFrom(assemblyFile);

            var type_L = assemblyToTest.GetType(typeName);
            type_L.GetMethod("Check").Invoke(Activator.CreateInstance(type_L), new object[0]);
        }
    }
}