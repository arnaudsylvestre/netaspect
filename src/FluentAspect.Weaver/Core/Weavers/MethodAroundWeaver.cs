using System;
using System.Reflection;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MethodAttributes = Mono.Cecil.MethodAttributes;

namespace FluentAspect.Weaver.Weavers
{
   public class MethodAroundWeaver
   {
      public void CreateWeaver(MethodDefinition method, Type interceptorType, MethodDefinition wrappedMethod)
      {
         ILProcessor il = method.Body.GetILProcessor();
         VariableDefinition interceptor = CreateInterceptor(method, interceptorType, il);
         VariableDefinition args = CreateArgsArray(method, il);
         VariableDefinition methodInfo = CreateMethodInfo(method, il);

         VariableDefinition weavedResult = CreateWeavedResult(method);

         Instruction beforeCatch = CreateNopForCatch(il);
         CallBefore(method, interceptor, methodInfo, args, interceptorType, il);
         VariableDefinition result = CallWeavedMethod(method, wrappedMethod, il);
         VariableDefinition handleResult = CreateHandleResult(method, result, il);
         CallAfter(method, interceptor, methodInfo, args, handleResult, interceptorType, il);
         Instruction instruction_L = il.Create(OpCodes.Nop);
         SetReturnValue(method, handleResult, weavedResult, il);
         Leave(il, instruction_L);

         Instruction onCatch = CreateNopForCatch(il);
         VariableDefinition e = CreateException(method);
         CallExceptionInterceptor(method, interceptor, methodInfo, args, e, interceptorType, il);
         Throw(il);
         Instruction endCatch = Leave(il, instruction_L);
         endCatch = il.Create(OpCodes.Nop);
         il.Append(endCatch);
         Return(method, weavedResult, il, instruction_L);

         CreateExceptionHandler(method, onCatch, endCatch, beforeCatch);
      }

      private VariableDefinition CreateHandleResult(MethodDefinition method_P,
                                                    VariableDefinition result_P,
                                                    ILProcessor il)
      {
         VariableDefinition handleResult_P = CreateVariable(method_P, typeof (object), "handleResult");
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

      private Instruction Leave(ILProcessor il, Instruction instructionP_P)
      {
         Instruction instruction_L = il.Create(OpCodes.Leave, instructionP_P);
         il.Append(instruction_L);
         return instruction_L;
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
         return CreateVariable(method, typeof (Exception), "e");
      }

      private VariableDefinition CreateWeavedResult(MethodDefinition method)
      {
         if (method.ReturnType.MetadataType != MetadataType.Void)
         {
            return CreateVariable(method, method.ReturnType, "weavedResult");
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
         il.Emit(OpCodes.Ldloc, interceptor);
         il.Emit(OpCodes.Ldarg_0);
         il.Emit(OpCodes.Ldloc, methodInfo);
         il.Emit(OpCodes.Ldloc, args);
         il.Emit(OpCodes.Callvirt, method.Module.Import(interceptorType.GetMethod("Before")));
      }

      private VariableDefinition CreateMethodInfo(MethodDefinition method, ILProcessor il)
      {
         VariableDefinition methodInfo = CreateVariable(method, typeof (MethodInfo), "method");
         il.Emit(OpCodes.Ldarg_0);
         il.Emit(OpCodes.Call, method.Module.Import(typeof (object).GetMethod("GetType", new Type[0])));
         il.Emit(OpCodes.Ldstr, method.Name);
         il.Emit(
            OpCodes.Callvirt,
            method.Module.Import(typeof (Type).GetMethod("GetMethod", new[] {typeof (string)})));
         il.Emit(OpCodes.Stloc, methodInfo);
         return methodInfo;
      }

      private VariableDefinition CreateArgsArray(MethodDefinition method, ILProcessor il)
      {
         VariableDefinition args = CreateVariable(method, typeof (object[]), "args");

         il.Emit(OpCodes.Ldc_I4, method.Parameters.Count);
         il.Emit(OpCodes.Newarr, method.Module.Import(typeof (object)));
         il.Emit(OpCodes.Stloc, args);

         foreach (ParameterDefinition p in method.Parameters.ToArray())
         {
            il.Emit(OpCodes.Ldloc, args);
            il.Emit(OpCodes.Ldc_I4, p.Index);
            il.Emit(OpCodes.Ldarg, p);
            if (p.ParameterType.IsValueType || p.ParameterType is GenericParameter)
               il.Emit(OpCodes.Box, p.ParameterType);
            il.Emit(OpCodes.Stelem_Ref);
         }

         return args;
      }

      private VariableDefinition CreateInterceptor(MethodDefinition method, Type interceptorType, ILProcessor il)
      {
         VariableDefinition interceptor = CreateVariable(method, interceptorType, "interceptor");
         CreateNewObject(method, interceptor, interceptorType, il);
         return interceptor;
      }

      private void CreateNewObject(MethodDefinition method,
                                   VariableDefinition interceptor,
                                   Type interceptorType,
                                   ILProcessor il)
      {
         il.Emit(OpCodes.Newobj, method.Module.Import(interceptorType.GetConstructors()[0]));
         il.Emit(OpCodes.Stloc, interceptor);
      }

      private VariableDefinition CreateVariable(MethodDefinition method, Type interceptorType, string name)
      {
         TypeReference typeReference = method.Module.Import(interceptorType);
         return CreateVariable(method, typeReference, name);
      }

      private static VariableDefinition CreateVariable(MethodDefinition method,
                                                       TypeReference typeReference,
                                                       string name)
      {
         VariableDefinition variableDefinition = new VariableDefinition(name, typeReference);
         method.Body.Variables.Add(variableDefinition);
         return variableDefinition;
      }
   }
}
