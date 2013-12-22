using System;
using System.Collections.Generic;
using FluentAspect.Weaver.Core;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Weavers.Methods
{
   public class AroundMethodWeaver : IWeaveable
   {
       private List<Type> interceptorType;
      private MethodDefinition definition;

      public AroundMethodWeaver(List<Type> interceptorType, MethodDefinition definition_P)
      {
         this.interceptorType = interceptorType;
         definition = definition_P;
      }

      public void Weave()
      {
          Check();
         var newMethod = CreateNewMethodBasedOnMethodToWeave(definition, interceptorType);
         definition.DeclaringType.Methods.Add(newMethod);
      }

       private void Check()
       {
           if (!definition.HasBody)
           {
               if (definition.DeclaringType.IsInterface)
                   throw new Exception(string.Format("A method declared in interface can not be weaved : {0}.{1}", definition.DeclaringType.Name, definition.Name));
               if ((definition.Attributes & MethodAttributes.Abstract) == MethodAttributes.Abstract)
                   throw new Exception(string.Format("An abstract method can not be weaved : {0}.{1}", definition.DeclaringType.Name, definition.Name));
           }
       }

       private MethodDefinition CreateNewMethodBasedOnMethodToWeave(MethodDefinition methodDefinition, List<Type> interceptor)
       {
         var wrappedMethod = methodDefinition.Clone(ComputeNewName(methodDefinition));

         methodDefinition.Body.Instructions.Clear();
         methodDefinition.Body.Variables.Clear();
         
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