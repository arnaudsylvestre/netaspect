using System;
using System.Reflection;
using FluentAspect.Weaver.Tests.Core;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters.Before
{
   [Serializable]
   public class SimpleClassAndWeaverAcceptanceTestBuilder : IAcceptanceTestBuilder<SimpleClassAndWeaver, SimpleClassAndWeaverActual>
   {
      private const string _aspectName = "MyAspectAttribute";
      private const string _typeName = "MyClassToWeave";
      private const string _methodName = "MyMethodToWeave";

      public SimpleClassAndWeaver CreateSample(AssemblyDefinitionDefiner assembly)
      {
         var type = assembly.WithType(_typeName);
         var aspect = assembly.WithMethodWeavingAspect(_aspectName);
         var method = type.WithMethod(_methodName);
         method.AddAspect(aspect);
         return new SimpleClassAndWeaver()
            {
               ClassToWeave = type,
               Aspect = aspect,     
               MethodToWeave = method,
            };
      }

      public SimpleClassAndWeaverActual CreateActual(Assembly assemblyDllP_P, DoAcceptanceHelper helper)
      {
         return new SimpleClassAndWeaverActual(assemblyDllP_P, _typeName, _methodName)
            {
               Aspect = new NetAspectAttribute(assemblyDllP_P, _aspectName)
            };
      }
   }
}