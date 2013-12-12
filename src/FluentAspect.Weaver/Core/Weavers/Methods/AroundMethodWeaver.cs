using System;
using FluentAspect.Core.Core;
using FluentAspect.Weaver.Core;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Weavers.Methods
{
   public class AroundMethodWeaver : IWeaveable
   {
      private Type interceptorType;
      private MethodDefinition definition;

      public AroundMethodWeaver(Type interceptorType, MethodDefinition definition_P)
      {
         this.interceptorType = interceptorType;
         definition = definition_P;
      }

      public void Weave()
      {
         var newMethod = CreateNewMethodBasedOnMethodToWeave(definition, interceptorType);
         definition.DeclaringType.Methods.Add(newMethod);
      }

       private MethodDefinition CreateNewMethodBasedOnMethodToWeave(MethodDefinition methodDefinition, Type interceptor)
       {
         var wrappedMethod = methodDefinition.Clone(ComputeNewName(methodDefinition));

         
         MethodAroundWeaver weaver = new MethodAroundWeaver();
         weaver.CreateWeaver(methodDefinition, interceptor, wrappedMethod);
          methodDefinition.Body.InitLocals = true;
         return wrappedMethod;
      }

      private string ComputeNewName(MethodDefinition methodDefinition)
      {
         return "-Weaved-" + methodDefinition.Name;
      }
   }
}