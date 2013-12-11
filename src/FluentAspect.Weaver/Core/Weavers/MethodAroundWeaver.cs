using System;
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
          Method myMethod = new Method(method);
         var il = method.Body.GetILProcessor();
         var interceptor = myMethod.CreateAndInitializeVariable(interceptorType);
         var args = myMethod.CreateArgsArrayFromParameters();
         var methodInfo = myMethod.CreateMethodInfo();

         var weavedResult = CreateWeavedResult(method);

         var beforeCatch = CreateNopForCatch(il);
         CallBefore(method, interceptor, methodInfo, args, interceptorType, il);
         var result = CallWeavedMethod(method, wrappedMethod, il);
         var handleResult = CreateHandleResult(method, result, il);
         CallAfter(method, interceptor, methodInfo, args, handleResult, interceptorType, il);
         var instruction_L = il.Create(OpCodes.Nop);
         SetReturnValue(method, handleResult, weavedResult, il);
         il.AppendLeave(instruction_L);

         var onCatch = CreateNopForCatch(il);
         var e = CreateException(method);
         CallExceptionInterceptor(method, interceptor, methodInfo, args, e, interceptorType, il);
         Throw(il);
         Instruction endCatch = il.AppendLeave(instruction_L);
         endCatch = il.Create(OpCodes.Nop);
         il.Append(endCatch);
         Return(method, weavedResult, il, instruction_L);

         CreateExceptionHandler(method, onCatch, endCatch, beforeCatch);
      }

      private VariableDefinition CreateHandleResult(MethodDefinition method_P,
                                                    VariableDefinition result_P,
                                                    ILProcessor il)
      {
         VariableDefinition handleResult_P = method_P.CreateVariable(typeof (object));
         if (result_P == null)
            il.Emit(OpCodes.Ldnull);
         else
            il.Emit(OpCodes.Ldloc, result_P);
         il.Emit(OpCodes.Stloc, handleResult_P);
         return handleResult_P;
      }

      private void Return(MethodDefinition method, VariableDefinition weavedResult, ILProcessor il, Instruction ret)
      {
         il.Append(ret);
         if (method.ReturnType.MetadataType != MetadataType.Void)
            il.Emit(OpCodes.Ldloc, weavedResult);
         il.Emit(OpCodes.Ret);
      }

      private Instruction CreateNopForCatch(ILProcessor il)
      {
         Instruction nop = il.Create(OpCodes.Nop);
         il.Append(nop);
         return nop;
      }

      private void CreateExceptionHandler(MethodDefinition method,
                                          Instruction onCatch,
                                          Instruction endCatch,
                                          Instruction beforeCatchP_P)
      {
         ExceptionHandler handler = new ExceptionHandler(ExceptionHandlerType.Catch)
            {
               TryStart = beforeCatchP_P,
               TryEnd = onCatch,
               HandlerStart = onCatch,
               HandlerEnd = endCatch,
               CatchType = method.Module.Import(typeof (Exception)),
            };

         method.Body.ExceptionHandlers.Add(handler);
      }

       private void Throw(ILProcessor il)
      {
         il.Emit(OpCodes.Rethrow);
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

      private VariableDefinition CreateWeavedResult(MethodDefinition method)
      {
         if (method.ReturnType.MetadataType != MetadataType.Void)
         {
             return method.CreateVariable(method.ReturnType);
         }
         return null;
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
