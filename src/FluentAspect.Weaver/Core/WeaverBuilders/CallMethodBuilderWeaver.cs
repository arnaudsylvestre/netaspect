using System;
using System.Collections.Generic;
using FluentAspect.Weaver.Core.Configuration;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Weavers.Calls;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.WeaverBuilders
{
   public class CallMethodBuilderWeaver : IWeaverBuilder
   {
       public IEnumerable<IWeaveable> BuildWeavers(AssemblyDefinition assemblyDefinition, WeavingConfiguration configuration, ErrorHandler errorHandler)
      {
         List<IWeaveable> weavers = new List<IWeaveable>();
         var methods = assemblyDefinition.GetAllMethods();

           foreach (var method in methods)
           {
               if (!method.HasBody)
                   continue;
               foreach (var instruction in method.Body.Instructions)
               {
                   if (instruction.OpCode == OpCodes.Call && instruction.Operand is MethodReference)
                   {
                       foreach (var methodMatch in configuration.Methods)
                       {
                           if (methodMatch.Matcher(new MethodDefinitionAdapter(instruction.Operand as MethodReference)))
                           {
                               var actualInterceptors = new List<Type>();

                               foreach (var interceptorType in methodMatch.InterceptorTypes)
                               {
                                   if (interceptorType.GetMethod("BeforeCall") != null || interceptorType.GetMethod("AfterCall") != null)
                                       actualInterceptors.Add(interceptorType);
                               }
                               if (actualInterceptors.Count != 0)
                                weavers.Add(new CallMethodWeaver(method, instruction, methodMatch.InterceptorTypes));
                           }
                       }
                   }
               }
           }

         return weavers;
      }
   }
}