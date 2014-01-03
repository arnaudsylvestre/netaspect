using System;
using System.Collections.Generic;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Weavers.Constructors
{
   public class AroundConstructorWeaver : IWeaveable
   {
       private List<NetAspectAttribute> interceptorType;
      private MethodDefinition definition;

      public AroundConstructorWeaver(List<NetAspectAttribute> interceptorType, MethodDefinition definition_P)
      {
         this.interceptorType = interceptorType;
         definition = definition_P;
      }

      public void Check(ErrorHandler errorHandler)
      {
         
      }

      public void Weave(ErrorHandler errorP_P)
      {
         var newMethod = CreateNewMethodBasedOnMethodToWeave(definition, interceptorType);
         definition.DeclaringType.Methods.Add(newMethod);
      }

       private MethodDefinition CreateNewMethodBasedOnMethodToWeave(MethodDefinition methodDefinition, List<NetAspectAttribute> interceptor)
       {
         var wrappedMethod = methodDefinition.Clone("-Weaved-Constructor");


         ConstructorAroundWeaver weaver = new ConstructorAroundWeaver();
         weaver.CreateWeaver(methodDefinition, interceptor, wrappedMethod);
         methodDefinition.Body.InitLocals = true;
         return wrappedMethod;
      }
   }
}