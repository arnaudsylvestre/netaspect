using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Core.Core;
using FluentAspect.Core.Expressions;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Weavers
{
    public class MethodAroundWeaver
    {
         public void CreateWeaver(MethodDefinition method, Type interceptorType, MethodDefinition wrappedMethod)
         {
             var il = method.Body.GetILProcessor();
             var interceptor = CreateInterceptor(method, interceptorType, il);
             var args = CreateArgsArray(method, il);
             var methodInfo = CreateMethodInfo(method, il);
             var methodCall = CreateMethodCall(method, methodInfo, args, il);

             var weavedResult = CreateWeavedResult(method);

             var beforeCatch = CreateNopForCatch(il);
             CallBefore(method, interceptor, methodCall, interceptorType, il);
             var result = CallWeavedMethod(method, wrappedMethod, il);
             var methodCallResult = CreateMethodCallResult(method, result, il);
             CallAfter(method, interceptor, methodCall, methodCallResult, interceptorType, il);
             var instruction_L = il.Create(OpCodes.Nop);
             SetReturnValue(method, methodCallResult, weavedResult, il);
            Leave(il, instruction_L);

             var onCatch = CreateNopForCatch(il);
             var e = CreateException(method);
             var ex = CreateExceptionResult(method, e, il);
             CallExceptionInterceptor(method, interceptor, methodCall, ex, interceptorType, il);
             var cancelExceptionAndReturn = CreateCancelExceptionAndReturn(method, ex, il);
            //ThrowIfNecessary(method, cancelExceptionAndReturn);
             SetReturnValueOnException(method, cancelExceptionAndReturn, weavedResult, il);
            var endCatch = Leave(il, instruction_L);
            Return(method, weavedResult, il, instruction_L);

            CreateExceptionHandler(method, onCatch, endCatch, beforeCatch);
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
           il.Emit(OpCodes.Leave, instructionP_P);
           var nop = il.Create(OpCodes.Nop);
           il.Append(nop);
           return nop;
        }
        private Instruction CreateNopForCatch(ILProcessor il)
        {
           var nop = il.Create(OpCodes.Nop);
           il.Append(nop);
           return nop;
        }

        private void CreateExceptionHandler(MethodDefinition method, Instruction onCatch, Instruction endCatch, Instruction beforeCatchP_P)
        {
            var handler = new ExceptionHandler(ExceptionHandlerType.Catch)
            {
               TryStart = beforeCatchP_P,
                TryEnd = onCatch,
                HandlerStart = onCatch,
                HandlerEnd = endCatch,
                CatchType = method.Module.Import(typeof(Exception)),
            };

            method.Body.ExceptionHandlers.Add(handler);
        }

        private void SetReturnValueOnException(MethodDefinition method, VariableDefinition cancelExceptionAndReturn, VariableDefinition weavedResult, ILProcessor il)
         {
             if (method.ReturnType.MetadataType != MetadataType.Void)
             {
                 il.Emit(OpCodes.Ldloc, cancelExceptionAndReturn);
                 il.Emit(OpCodes.Castclass, method.ReturnType);
                 il.Emit(OpCodes.Stloc, weavedResult);
             }
        }

        private void ThrowIfNecessary(MethodDefinition method, VariableDefinition cancelExceptionAndReturn, ILProcessor il)
         {
            var reThrow = il.Create(OpCodes.Rethrow);
            il.Emit(OpCodes.Brtrue_S, reThrow);
        }

        private VariableDefinition CreateCancelExceptionAndReturn(MethodDefinition method, VariableDefinition ex, ILProcessor il)
         {
             var cancelExceptionAndReturn = CreateVariable(method, typeof(object));
             il.Emit(OpCodes.Ldloc, ex);
             il.Emit(OpCodes.Callvirt, method.Module.Import(typeof(ExceptionResult).GetMethod("get_CancelExceptionAndReturn")));
             il.Emit(OpCodes.Stloc, cancelExceptionAndReturn);
            return cancelExceptionAndReturn;
         }

        private void CallExceptionInterceptor(MethodDefinition method, VariableDefinition interceptor, VariableDefinition methodCall, VariableDefinition ex, Type interceptorType, ILProcessor il)
        {
            il.Emit(OpCodes.Ldloc, interceptor);
            il.Emit(OpCodes.Ldloc, methodCall);
            il.Emit(OpCodes.Ldloc, ex);
            il.Emit(OpCodes.Callvirt, method.Module.Import(interceptorType.GetMethod("OnException")));
        }

        private VariableDefinition CreateExceptionResult(MethodDefinition method, VariableDefinition e, ILProcessor il)
         {
            var ex = CreateVariable(method, typeof (ExceptionResult));

            il.Emit(OpCodes.Ldloc, e);
            il.Emit(OpCodes.Newobj, method.Module.Import(typeof(ExceptionResult).GetConstructors()[0]));
            il.Emit(OpCodes.Stloc, ex);

            return ex;

         }

        private VariableDefinition CreateException(MethodDefinition method)
        {
            return CreateVariable(method, typeof (Exception));
        }

        private VariableDefinition CreateWeavedResult(MethodDefinition method)
        {
            if (method.ReturnType.MetadataType != MetadataType.Void)
            {
                return CreateVariable(method, method.ReturnType);
            }
            return null;

        }

        private void SetReturnValue(MethodDefinition method, VariableDefinition methodCallResult, VariableDefinition weavedResult, ILProcessor il)
         {
            if (method.ReturnType.MetadataType != MetadataType.Void)
            {
                il.Emit(OpCodes.Ldloc, methodCallResult);
                il.Emit(OpCodes.Callvirt, method.Module.Import(typeof(MethodCallResult).GetMethod("get_Result")));
                il.Emit(OpCodes.Castclass, method.ReturnType);
                il.Emit(OpCodes.Stloc, weavedResult);
            }
        }

        private void CallAfter(MethodDefinition method, VariableDefinition interceptor, VariableDefinition methodCall, VariableDefinition methodCallResult, Type interceptorType, ILProcessor il)
         {
             il.Emit(OpCodes.Ldloc, interceptor);
             il.Emit(OpCodes.Ldloc, methodCall);
             il.Emit(OpCodes.Ldloc, methodCallResult);
             il.Emit(OpCodes.Callvirt, method.Module.Import(interceptorType.GetMethod("After")));
        }

        private VariableDefinition CreateMethodCallResult(MethodDefinition method, VariableDefinition result, ILProcessor il)
         {
             var methodCallResult = CreateVariable(method, typeof(MethodCallResult));
            if (result == null)
                il.Emit(OpCodes.Ldnull);
            else
             il.Emit(OpCodes.Ldloc, result);
            il.Emit(OpCodes.Newobj, method.Module.Import(typeof(MethodCallResult).GetConstructors()[0]));
             il.Emit(OpCodes.Stloc, methodCallResult);
            return methodCallResult;
         }

        private VariableDefinition CallWeavedMethod(MethodDefinition method, MethodDefinition wrappedMethod, ILProcessor il)
         {
            VariableDefinition result = null;
            if (method.ReturnType.MetadataType != MetadataType.Void)
            {
               result = new VariableDefinition("result", method.ReturnType);
               method.Body.Variables.Add(result);
            }
            il.Emit(OpCodes.Ldarg_0);
            foreach (var p in method.Parameters.ToArray())
            {
               il.Emit(OpCodes.Ldarg, p);
            }
           List<TypeReference> generics = new List<TypeReference>();
           MethodReference reference = new MethodReference(wrappedMethod.Name, method.ReturnType, method.DeclaringType);
           foreach (var typeReference_L in generics)
           {
              reference.GenericParameters.Add(new GenericParameter(typeReference_L.Name, wrappedMethod));
           }
           il.Emit(OpCodes.Callvirt, reference);
            //il.Emit(OpCodes.Pop);
            if (result != null)
               il.Emit(OpCodes.Stloc, result);
               
            return result;
         }

        private void CallBefore(MethodDefinition method, VariableDefinition interceptor, VariableDefinition methodCall, Type interceptorType, ILProcessor il)
         {
             il.Emit(OpCodes.Ldloc, interceptor);
             il.Emit(OpCodes.Ldloc, methodCall);
             il.Emit(OpCodes.Callvirt, method.Module.Import(interceptorType.GetMethod("Before")));
        }

        private VariableDefinition CreateMethodInfo(MethodDefinition method, ILProcessor il)
         {
             var methodInfo = CreateVariable(method, typeof(MethodInfo));
             il.Emit(OpCodes.Ldarg_0);
             il.Emit(OpCodes.Call, method.Module.Import(typeof(Type).GetMethod("GetType", new Type[0])));
             il.Emit(OpCodes.Ldstr, method.Name);
             il.Emit(OpCodes.Callvirt, method.Module.Import(typeof(Type).GetMethod("GetMethod", new[] {typeof(string)})));
             il.Emit(OpCodes.Stloc, methodInfo);
            return methodInfo;
         }

        private VariableDefinition CreateMethodCall(MethodDefinition method, VariableDefinition methodInfo, VariableDefinition args, ILProcessor il)
         {
             var methodCall = CreateVariable(method, typeof(MethodCall));
             il.Emit(OpCodes.Ldarg_0);
             il.Emit(OpCodes.Ldloc, methodInfo);
             il.Emit(OpCodes.Ldloc, args);
             il.Emit(OpCodes.Newobj, method.Module.Import(typeof(MethodCall).GetConstructors()[0]));
             il.Emit(OpCodes.Stloc, methodCall);
            return methodCall;
         }

        private VariableDefinition CreateArgsArray(MethodDefinition method, ILProcessor il)
         {
           var args = CreateVariable(method, typeof(object[]));

           il.Emit(OpCodes.Ldc_I4, method.Parameters.Count);
           il.Emit(OpCodes.Newarr, method.Module.Import(typeof(object)));
           il.Emit(OpCodes.Stloc, args);

            foreach (var p in method.Parameters.ToArray())
           {
               il.Emit(OpCodes.Ldloc, args);
               il.Emit(OpCodes.Ldc_I4, p.Index);
               il.Emit(OpCodes.Ldarg, p);
               if (p.ParameterType.IsValueType)
                   il.Emit(OpCodes.Box, p.ParameterType);
               il.Emit(OpCodes.Stelem_Ref);
           }

             return args;
         }

        private VariableDefinition CreateInterceptor(MethodDefinition method, Type interceptorType, ILProcessor il)
        {
             var interceptor = CreateVariable(method, interceptorType);
             CreateNewObject(method, interceptor, interceptorType, il);
             return interceptor;
        }

        private void CreateNewObject(MethodDefinition method, VariableDefinition interceptor, Type interceptorType, ILProcessor il)
        {
            il.Emit(OpCodes.Newobj, method.Module.Import(interceptorType.GetConstructors()[0]));
            il.Emit(OpCodes.Stloc, interceptor);
        }

        private VariableDefinition CreateVariable(MethodDefinition method, Type interceptorType)
        {
            var typeReference = method.Module.Import(interceptorType);
            return CreateVariable(method, typeReference);
        }

        private static VariableDefinition CreateVariable(MethodDefinition method, TypeReference typeReference)
        {
            var variableDefinition = new VariableDefinition(typeReference);
            method.Body.Variables.Add(variableDefinition);
            return variableDefinition;
        }
    }
}