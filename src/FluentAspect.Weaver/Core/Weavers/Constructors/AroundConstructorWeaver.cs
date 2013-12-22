using System;
using System.Collections.Generic;
using FluentAspect.Weaver.Core;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Weavers.Methods
{
   public class AroundConstructorWeaver : IWeaveable
   {
       private List<Type> interceptorType;
      private MethodDefinition definition;

      public AroundConstructorWeaver(List<Type> interceptorType, MethodDefinition definition_P)
      {
         this.interceptorType = interceptorType;
         definition = definition_P;
      }

      public void Weave()
      {
         var newMethod = CreateNewMethodBasedOnMethodToWeave(definition, interceptorType);
         definition.DeclaringType.Methods.Add(newMethod);
      }

       private MethodDefinition CreateNewMethodBasedOnMethodToWeave(MethodDefinition methodDefinition, List<Type> interceptor)
       {
         var wrappedMethod = methodDefinition.Clone("-Weaved-Constructor");


         ConstructorAroundWeaver weaver = new ConstructorAroundWeaver();
         weaver.CreateWeaver(methodDefinition, interceptor, wrappedMethod);
         methodDefinition.Body.InitLocals = true;
         return wrappedMethod;
      }
   }
}