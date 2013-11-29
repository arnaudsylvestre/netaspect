using System;
using FluentAspect.Core.Core;
using FluentAspect.Weaver.Core;
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
         var newMethod = Around(definition, interceptorType);
         definition.DeclaringType.Methods.Add(newMethod);
      }

      private MethodDefinition Around(MethodDefinition methodDefinition, Type interceptor)
      {
         var weavedMethodName = ComputeNewName(methodDefinition);
         var definition = CreateNewMethodBasedOnMethodToWeave(methodDefinition, weavedMethodName, interceptor);
         return definition;
      }

      private MethodDefinition CreateNewMethodBasedOnMethodToWeave(MethodDefinition methodDefinition, string weavedMethodName, Type interceptor)
      {
         MethodDefinition wrappedMethod = new MethodDefinition(ComputeNewName(methodDefinition), methodDefinition.Attributes, methodDefinition.ReturnType);

         foreach (var parameterDefinition in methodDefinition.Parameters)
         {
            wrappedMethod.Parameters.Add(parameterDefinition);
         }
         foreach (var instruction in methodDefinition.Body.Instructions)
         {
            wrappedMethod.Body.Instructions.Add(instruction);
         }
         methodDefinition.Body.Instructions.Clear();
         foreach (var variable in methodDefinition.Body.Variables)
         {
            wrappedMethod.Body.Variables.Add(variable);
         }
         wrappedMethod.Body.InitLocals = methodDefinition.Body.InitLocals;
         methodDefinition.Body.Variables.Clear();
         foreach (var exceptionHandler in methodDefinition.Body.ExceptionHandlers)
         {
            wrappedMethod.Body.ExceptionHandlers.Add(exceptionHandler);
         }
         foreach (var genericParameter in methodDefinition.GenericParameters)
         {
            wrappedMethod.GenericParameters.Add(new GenericParameter(genericParameter.Name, wrappedMethod));
         }
         MethodAroundWeaver weaver = new MethodAroundWeaver();
         weaver.CreateWeaver(methodDefinition, interceptor, wrappedMethod);
         return wrappedMethod;
      }

      private string ComputeNewName(MethodDefinition methodDefinition)
      {
         return methodDefinition.Name + "______________________________Weaved";
      }
   }
}