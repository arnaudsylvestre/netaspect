using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Weavers.Helpers;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MethodAttributes = Mono.Cecil.MethodAttributes;

namespace FluentAspect.Weaver.Weavers
{
   public class MethodAroundWeaver
   {
      public void CreateWeaver(MethodDefinition method, Type interceptorType, MethodDefinition wrappedMethod)
      {
         CreateWeaver(new Method(method), interceptorType, wrappedMethod);
      }

      public void CreateWeaver(Method myMethod, Type interceptorType, MethodDefinition wrappedMethod)
      {
         var interceptor = myMethod.CreateAndInitializeVariable(interceptorType);
         var args = myMethod.CreateArgsArrayFromParameters();
         var methodInfo = myMethod.CreateMethodInfo();
         var weavedResult = myMethod.CreateWeavedResult();

          myMethod.Add(new TryCatch(
                           il => {
                                     CallBefore(myMethod.MethodDefinition, interceptor, methodInfo, args, interceptorType, il);
                                     var result = CallWeavedMethod(myMethod.MethodDefinition, wrappedMethod, il);
                                     var handleResult = myMethod.CreateHandleResult(result);
                                     CallAfter(myMethod.MethodDefinition, interceptor, methodInfo, args, handleResult, interceptorType, il);
                                     SetReturnValue(myMethod.MethodDefinition, handleResult, weavedResult, il);
                           },
                           il => {
                                     var e = CreateException(myMethod.MethodDefinition);
                                     CallExceptionInterceptor(myMethod.MethodDefinition, interceptor, methodInfo, args, e, interceptorType, myMethod.Il);
                                     myMethod.Il.AppendThrow();
                           }));


          myMethod.Return(weavedResult);

      }


      

       

      private void CallExceptionInterceptor(MethodDefinition method,
                                            VariableDefinition interceptor,
                                            VariableDefinition methodInfo,
                                            VariableDefinition args,
                                            VariableDefinition ex,
                                            Type interceptorType,
                                            ILProcessor il)
      {
         il.Emit(OpCodes.Stloc, ex);
         il.Emit(OpCodes.Ldloc, interceptor);
         il.Emit(OpCodes.Ldarg_0);
         il.Emit(OpCodes.Ldloc, methodInfo);
         il.Emit(OpCodes.Ldloc, args);
         il.Emit(OpCodes.Ldloc, ex);
         il.Emit(OpCodes.Callvirt, method.Module.Import(interceptorType.GetMethod("OnException")));
      }

      private VariableDefinition CreateException(MethodDefinition method)
      {
          return method.CreateVariable(typeof(Exception));
      }

      private void SetReturnValue(MethodDefinition method,
                                  VariableDefinition handleResult,
                                  VariableDefinition weavedResult,
                                  ILProcessor il)
      {
         if (method.ReturnType.MetadataType != MetadataType.Void)
         {
            il.Emit(OpCodes.Ldloc, handleResult);
            il.Emit(OpCodes.Castclass, method.ReturnType);
            il.Emit(OpCodes.Stloc, weavedResult);
         }
      }

      private void CallAfter(MethodDefinition method,
                             VariableDefinition interceptor,
                             VariableDefinition methodInfo,
                             VariableDefinition args,
                             VariableDefinition handleResult,
                             Type interceptorType,
                             ILProcessor il)
      {
         il.Emit(OpCodes.Ldloc, interceptor);
         il.Emit(OpCodes.Ldarg_0);
         il.Emit(OpCodes.Ldloc, methodInfo);
         il.Emit(OpCodes.Ldloc, args);
         il.Emit(OpCodes.Ldloca, handleResult);
         il.Emit(OpCodes.Callvirt, method.Module.Import(interceptorType.GetMethod("After")));
      }

      private VariableDefinition CallWeavedMethod(MethodDefinition method,
                                                  MethodDefinition wrappedMethod,
                                                  ILProcessor il)
      {
         VariableDefinition result = null;
         if (method.ReturnType.MetadataType != MetadataType.Void)
         {
            result = new VariableDefinition("result", method.ReturnType);
            method.Body.Variables.Add(result);
         }
         il.Emit(OpCodes.Ldarg_0);
         foreach (ParameterDefinition p in method.Parameters.ToArray())
         {
            il.Emit(OpCodes.Ldarg, p);
         }

         OpCode call = OpCodes.Callvirt;
         if ((method.Attributes & MethodAttributes.Static) == MethodAttributes.Static)
            call = OpCodes.Call;
         il.Emit(call, wrappedMethod.MakeGeneric(method.GenericParameters.ToArray()));
         if (result != null)
            il.Emit(OpCodes.Stloc, result);

         return result;
      }

      private void CallBefore(MethodDefinition method,
                              VariableDefinition interceptor,
                              VariableDefinition methodInfo,
                              VariableDefinition args,
                              Type interceptorType,
                              ILProcessor il)
      {
          il.AppendCallTo(interceptorType.GetMethod("Before"), method.Module, interceptor, ILProcessorExtensions.This, methodInfo, args);
      }
   }
}
