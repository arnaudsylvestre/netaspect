using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Weavers.Helpers;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MethodAttributes = Mono.Cecil.MethodAttributes;

namespace FluentAspect.Weaver.Weavers
{
    public class MethodMethodAroundWeaverConfiguration : MethodAroundWeaver.IAroundWeaverConfiguration
    {
        public MethodInfo ToCallBefore(Type interceptorType)
        {
            return interceptorType.GetMethod("Before");
        }

        public MethodInfo ToCallAfter(Type interceptorType)
        {
            return interceptorType.GetMethod("After");
        }

        public MethodInfo ToCallOnException(Type interceptorType)
        {
            return interceptorType.GetMethod("OnException");
        }
    }

    public static class AroundWeaverConfigurationExtensions
    {
        public static bool Needs(this MethodAroundWeaver.IAroundWeaverConfiguration configuration,
                                 List<Type> interceptorTypes, string variableName)
        {
            foreach (Type interceptorType in interceptorTypes)
            {
                var parameters = new List<ParameterInfo>();
                MethodInfo callBefore = configuration.ToCallBefore(interceptorType);
                if (callBefore != null)
                    parameters.AddRange(callBefore.GetParameters().ToList());
                MethodInfo methodInfo = configuration.ToCallAfter(interceptorType);
                if (methodInfo != null)
                    parameters.AddRange(methodInfo.GetParameters().ToList());
                MethodInfo callOnException = configuration.ToCallOnException(interceptorType);
                if (callOnException != null)
                    parameters.AddRange(callOnException.GetParameters().ToList());

                IEnumerable<string> enumerable = from p in parameters where p.Name == variableName select p.Name;
                if (enumerable.Any())
                    return true;
            }
            return false;
        }

        public static bool NeedsCallBefore(this MethodAroundWeaver.IAroundWeaverConfiguration configuration,
                                           List<Type> interceptorTypes)
        {
            return interceptorTypes.Any(interceptorType => configuration.ToCallBefore(interceptorType) != null);
        }

        public static bool NeedsCallAfter(this MethodAroundWeaver.IAroundWeaverConfiguration configuration,
                                          List<Type> interceptorTypes)
        {
            return interceptorTypes.Any(interceptorType => configuration.ToCallAfter(interceptorType) != null);
        }

        public static bool NeedsCallOnException(this MethodAroundWeaver.IAroundWeaverConfiguration configuration,
                                                List<Type> interceptorTypes)
        {
            return interceptorTypes.Any(interceptorType => configuration.ToCallOnException(interceptorType) != null);
        }
    }

    public class MethodAroundWeaver
    {
        private const string ParameterParameters = "parameters";
        private const string Instance = "instance";
        private const string Method = "method";

        public void CreateWeaver(MethodDefinition method, List<Type> interceptorType, MethodDefinition wrappedMethod,
                                 IAroundWeaverConfiguration configuration)
        {
            CreateWeaver(new Method(method), interceptorType, wrappedMethod, configuration);
        }

        public void CreateWeaver(Method myMethod, List<Type> interceptorType, MethodDefinition wrappedMethod,
                                 IAroundWeaverConfiguration configuration)
        {
            List<VariableDefinition> interceptor = myMethod.CreateAndInitializeVariable(interceptorType);
            VariableDefinition args = null;
            if (configuration.Needs(interceptorType, ParameterParameters))
                args = myMethod.CreateArgsArrayFromParameters();
            VariableDefinition methodInfo = myMethod.CreateMethodInfo();
            VariableDefinition weavedResult = myMethod.CreateWeavedResult();

            myMethod.Add(new TryCatch(
                             il =>
                                 {
                                     CallBefore(myMethod.MethodDefinition, interceptor, methodInfo, args,
                                                interceptorType, il);
                                     VariableDefinition result = CallWeavedMethod(myMethod.MethodDefinition,
                                                                                  wrappedMethod, il);
                                     VariableDefinition handleResult = myMethod.CreateHandleResult(result);
                                     CallAfter(myMethod.MethodDefinition, interceptor, methodInfo, args, handleResult,
                                               interceptorType, il);
                                     myMethod.SetReturnValue(handleResult, weavedResult);
                                 },
                             il =>
                                 {
                                     VariableDefinition e = CreateException(myMethod.MethodDefinition);
                                     CallExceptionInterceptor(myMethod.MethodDefinition, interceptor, methodInfo, args,
                                                              e, interceptorType, myMethod.Il);
                                     myMethod.Il.AppendThrow();
                                 }));


            myMethod.Return(weavedResult);
        }

        private void CallExceptionInterceptor(MethodDefinition method,
                                              List<VariableDefinition> interceptor,
                                              VariableDefinition methodInfo,
                                              VariableDefinition args,
                                              VariableDefinition ex,
                                              List<Type> interceptorType,
                                              ILProcessor il)
        {
            il.Emit(OpCodes.Stloc, ex);
            var caller = new InterceptorCaller(il, method);

            Fill(method, methodInfo, args, caller);
            caller.AddVariable("exception", ex, false);

            for (int i = 0; i < interceptorType.Count; i++)
            {
                caller.Call(interceptor[i], "OnException", interceptorType[i]);
            }
        }

        private VariableDefinition CreateException(MethodDefinition method)
        {
            return method.CreateVariable(typeof (Exception));
        }

        private void CallAfter(MethodDefinition method,
                               List<VariableDefinition> interceptor,
                               VariableDefinition methodInfo,
                               VariableDefinition args,
                               VariableDefinition handleResult,
                               List<Type> interceptorType,
                               ILProcessor il)
        {
            var caller = new InterceptorCaller(il, method);

            Fill(method, methodInfo, args, caller);
            caller.AddVariable("result", handleResult, true);

            for (int i = 0; i < interceptorType.Count; i++)
            {
                caller.Call(interceptor[i], "After", interceptorType[i]);
            }
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
                                List<VariableDefinition> interceptor,
                                VariableDefinition methodInfo,
                                VariableDefinition args,
                                List<Type> interceptorType,
                                ILProcessor il)
        {
            var caller = new InterceptorCaller(il, method);

            Fill(method, methodInfo, args, caller);

            for (int i = 0; i < interceptorType.Count; i++)
            {
                caller.Call(interceptor[i], "Before", interceptorType[i]);
            }
        }

        private static void Fill(MethodDefinition method, VariableDefinition methodInfo, VariableDefinition args,
                                 InterceptorCaller caller)
        {
            caller.AddThis(Instance);
            caller.AddVariable(Method, methodInfo, false);
            caller.AddVariable(ParameterParameters, args, false);
            caller.AddParameters(method.Parameters);
        }

        public interface IAroundWeaverConfiguration
        {
            MethodInfo ToCallBefore(Type interceptorType);
            MethodInfo ToCallAfter(Type interceptorType);
            MethodInfo ToCallOnException(Type interceptorType);
        }

        
    }
}