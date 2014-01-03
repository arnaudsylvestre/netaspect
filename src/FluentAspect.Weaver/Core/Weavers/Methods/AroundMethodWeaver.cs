using System;
using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Weavers.Helpers;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Weavers.Methods
{
   public class AroundMethodWeaver : IWeaveable
   {
      private List<NetAspectAttribute> interceptorType;
      private MethodDefinition definition;

      public AroundMethodWeaver(List<NetAspectAttribute> interceptorType, MethodDefinition definition_P)
      {
         this.interceptorType = interceptorType;
         definition = definition_P;
      }

      public void Check(ErrorHandler errorHandler)
      {
         
      }

      public void Weave(ErrorHandler errorP_P)
      {
          Check();
          WeaveMethod(definition, interceptorType);
      }



       public static void WeaveMethod(MethodDefinition methodDefinition, List<NetAspectAttribute> interceptorTypes)
       {
           var newMethod = CreateNewMethodBasedOnMethodToWeave(methodDefinition, interceptorTypes);
           methodDefinition.DeclaringType.Methods.Add(newMethod);
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

       private static MethodDefinition CreateNewMethodBasedOnMethodToWeave(MethodDefinition methodDefinition, List<NetAspectAttribute> interceptor)
       {
         var wrappedMethod = methodDefinition.Clone(ComputeNewName(methodDefinition));

         methodDefinition.Body.Instructions.Clear();
         methodDefinition.Body.Variables.Clear();
         
         MethodAroundWeaver weaver = new MethodAroundWeaver();
         weaver.CreateWeaver(new Method(methodDefinition), from i in interceptor select i.MethodWeavingConfiguration, wrappedMethod);
          methodDefinition.Body.InitLocals = true;
         return wrappedMethod;
      }

      private static string ComputeNewName(MethodDefinition methodDefinition)
      {
         return "-Weaved-" + methodDefinition.Name;
      }
   }
}