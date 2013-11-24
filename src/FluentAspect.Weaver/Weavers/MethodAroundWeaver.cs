using System;
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
             var interceptor = CreateInterceptor(method, interceptorType);
             var args = CreateArgsArray(method);
             var methodInfo = CreateMethodInfo(method);
             var methodCall = CreateMethodCall(method, methodInfo, args);
             var weavedResult = CreateWeavedResult(method);

             CallBefore(method, interceptor, methodCall, interceptorType);
             var result = CallWeavedMethod(method, wrappedMethod);
             var methodCallResult = CreateMethodCallResult(method, result);
             CallAfter(method, interceptor, methodCall, methodCallResult, interceptorType);
             SetReturnValue(method, methodCallResult, weavedResult);

             var onCatch = CreateNopForCatch(method);
             var e = CreateException(method);
             var ex = CreateExceptionResult(method, e);
             CallExceptionInterceptor(method, interceptor, methodCall, ex, interceptorType);
             var cancelExceptionAndReturn = CreateCancelExceptionAndReturn(method, ex);
             ThrowIfNecessary(method, cancelExceptionAndReturn);
             SetReturnValueOnException(method, cancelExceptionAndReturn, weavedResult);

             var endCatch = CreateNopForCatch(method);
             Return(method, weavedResult);

             CreateExceptionHandler(method, onCatch, endCatch);
         }

        private void Return(MethodDefinition method, VariableDefinition weavedResult)
         {
             var il = method.Body.GetILProcessor();
            if (method.ReturnType.MetadataType == MetadataType.Void)
                return;
            il.Emit(OpCodes.Ldloc, weavedResult);
            il.Emit(OpCodes.Ret);
        }

        private Instruction CreateNopForCatch(MethodDefinition method)
        {
            var il = method.Body.GetILProcessor();
            var nop = il.Create(OpCodes.Nop);
            il.Append(nop);
            return nop;
        }

        private void CreateExceptionHandler(MethodDefinition method, Instruction onCatch, Instruction endCatch)
        {
            var handler = new ExceptionHandler(ExceptionHandlerType.Catch)
            {
                TryStart = method.Body.Instructions.First(),
                TryEnd = onCatch,
                HandlerStart = onCatch,
                HandlerEnd = endCatch,
                CatchType = method.Module.Import(typeof(Exception)),
            };

            method.Body.ExceptionHandlers.Add(handler);
        }

        private void SetReturnValueOnException(MethodDefinition method, VariableDefinition cancelExceptionAndReturn, VariableDefinition weavedResult)
         {
             var il = method.Body.GetILProcessor();
             if (method.ReturnType.MetadataType != MetadataType.Void)
             {
                 il.Emit(OpCodes.Ldloc, cancelExceptionAndReturn);
                 il.Emit(OpCodes.Castclass, method.ReturnType);
                 il.Emit(OpCodes.Stloc, weavedResult);
             }
        }

        private void ThrowIfNecessary(MethodDefinition method, VariableDefinition cancelExceptionAndReturn)
         {
             var il = method.Body.GetILProcessor();
            var reThrow = il.Create(OpCodes.Rethrow);
            il.Emit(OpCodes.Brtrue_S, reThrow);
        }

        private VariableDefinition CreateCancelExceptionAndReturn(MethodDefinition method, VariableDefinition ex)
         {
             var il = method.Body.GetILProcessor();
             var cancelExceptionAndReturn = CreateVariable(method, typeof(object));
             il.Emit(OpCodes.Ldloc, ex);
             il.Emit(OpCodes.Callvirt, method.Module.Import(typeof(ExceptionResult).GetMethod("get_CancelExceptionAndReturn")));
             il.Emit(OpCodes.Stloc, cancelExceptionAndReturn);
            return cancelExceptionAndReturn;
         }

        private void CallExceptionInterceptor(MethodDefinition method, VariableDefinition interceptor, VariableDefinition methodCall, VariableDefinition ex, Type interceptorType)
        {
            var il = method.Body.GetILProcessor();
            il.Emit(OpCodes.Ldloc, interceptor);
            il.Emit(OpCodes.Ldloc, methodCall);
            il.Emit(OpCodes.Ldloc, ex);
            il.Emit(OpCodes.Callvirt, method.Module.Import(interceptorType.GetMethod("OnException")));
        }

        private VariableDefinition CreateExceptionResult(MethodDefinition method, VariableDefinition e)
         {
             var il = method.Body.GetILProcessor();
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
            return CreateVariable(method, method.ReturnType);
        }

        private void SetReturnValue(MethodDefinition method, VariableDefinition methodCallResult, VariableDefinition weavedResult)
         {
             var il = method.Body.GetILProcessor();
            if (method.ReturnType.MetadataType != MetadataType.Void)
            {
                il.Emit(OpCodes.Ldloc, methodCallResult);
                il.Emit(OpCodes.Callvirt, method.Module.Import(typeof(MethodCallResult).GetMethod("get_Result")));
                il.Emit(OpCodes.Castclass, method.ReturnType);
                il.Emit(OpCodes.Stloc, weavedResult);
            }
        }

        private void CallAfter(MethodDefinition method, VariableDefinition interceptor, VariableDefinition methodCall, VariableDefinition methodCallResult, Type interceptorType)
         {
             var il = method.Body.GetILProcessor();
             il.Emit(OpCodes.Ldloc, interceptor);
             il.Emit(OpCodes.Ldloc, methodCall);
             il.Emit(OpCodes.Ldloc, methodCallResult);
             il.Emit(OpCodes.Callvirt, method.Module.Import(interceptorType.GetMethod("After")));
        }

        private VariableDefinition CreateMethodCallResult(MethodDefinition method, VariableDefinition result)
         {
             var il = method.Body.GetILProcessor();
             var methodCallResult = CreateVariable(method, typeof(MethodCallResult));
             il.Emit(OpCodes.Ldloc, result);
             il.Emit(OpCodes.Newobj, method.Module.Import(typeof(MethodCall).GetConstructors()[0]));
             il.Emit(OpCodes.Stloc, methodCallResult);
            return methodCallResult;
         }

        private VariableDefinition CallWeavedMethod(MethodDefinition method, MethodDefinition wrappedMethod)
         {
             var il = method.Body.GetILProcessor();
            VariableDefinition result = null;
            if (method.ReturnType.MetadataType != MetadataType.Void)
            {
                result = new VariableDefinition(method.ReturnType);
            }
            il.Emit(OpCodes.Ldarg_0);
            foreach (var p in method.Parameters.ToArray())
            {
                il.Emit(OpCodes.Ldarg, p);
            }
            il.Emit(OpCodes.Call, wrappedMethod);
            if (result != null)
                il.Emit(OpCodes.Stloc, result);
            return result;
        }

        private void CallBefore(MethodDefinition method, VariableDefinition interceptor, VariableDefinition methodCall, Type interceptorType)
         {
             var il = method.Body.GetILProcessor();
             il.Emit(OpCodes.Ldloc, interceptor);
             il.Emit(OpCodes.Ldloc, methodCall);
             il.Emit(OpCodes.Callvirt, method.Module.Import(interceptorType.GetMethod("Before")));
        }

        private VariableDefinition CreateMethodInfo(MethodDefinition method)
         {
             var il = method.Body.GetILProcessor();
             var methodInfo = CreateVariable(method, typeof(MethodInfo));
             il.Emit(OpCodes.Ldarg_0);
             il.Emit(OpCodes.Call, method.Module.Import(typeof(Type).GetMethod("GetType", new Type[0])));
             il.Emit(OpCodes.Ldstr, method.Name);
             il.Emit(OpCodes.Callvirt, method.Module.Import(typeof(Type).GetMethod("GetMethod", new[] {typeof(string)})));
             il.Emit(OpCodes.Stloc, methodInfo);
            return methodInfo;
         }

        private VariableDefinition CreateMethodCall(MethodDefinition method, VariableDefinition methodInfo, VariableDefinition args)
         {
             var il = method.Body.GetILProcessor();
             var methodCall = CreateVariable(method, typeof(MethodCall));
             il.Emit(OpCodes.Ldarg_0);
             il.Emit(OpCodes.Ldloc, methodInfo);
             il.Emit(OpCodes.Ldloc, args);
             il.Emit(OpCodes.Newobj, method.Module.Import(typeof(MethodCall).GetConstructors()[0]));
             il.Emit(OpCodes.Stloc, methodCall);
            return methodCall;
         }

        private VariableDefinition CreateArgsArray(MethodDefinition method)
         {
           var il = method.Body.GetILProcessor();
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

        private VariableDefinition CreateInterceptor(MethodDefinition method, Type interceptorType)
        {
             var interceptor = CreateVariable(method, interceptorType);
             CreateNewObject(method, interceptor, interceptorType);
             return interceptor;
        }

        private void CreateNewObject(MethodDefinition method, VariableDefinition interceptor, Type interceptorType)
        {
            var il = method.Body.GetILProcessor();
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