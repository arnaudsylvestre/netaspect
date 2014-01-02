using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Weavers.Helpers;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MethodAttributes = Mono.Cecil.MethodAttributes;

namespace FluentAspect.Weaver.Core.Weavers.Methods
{
    public static class AroundWeaverConfigurationExtensions
    {
        public static bool HasCallOnException(this IAroundWeaverConfiguration configuration, List<Type> interceptorTypes)
        {
            foreach (Type interceptorType in interceptorTypes)
            {
                MethodInfo callOnException = interceptorType.GetMethod(configuration.ToCallOnException(interceptorType));
                if (callOnException != null)
                    return true;
            }
            return false;
        }

        public static bool Needs(this IAroundWeaverConfiguration configuration,
                                 List<Type> interceptorTypes, string variableName)
        {
            foreach (Type interceptorType in interceptorTypes)
            {
                var parameters = new List<ParameterInfo>();
                var callBefore = interceptorType.GetMethod(configuration.ToCallBefore(interceptorType));
                if (callBefore != null)
                    parameters.AddRange(callBefore.GetParameters().ToList());
                MethodInfo methodInfo = interceptorType.GetMethod(configuration.ToCallAfter(interceptorType));
                if (methodInfo != null)
                    parameters.AddRange(methodInfo.GetParameters().ToList());
                MethodInfo callOnException = interceptorType.GetMethod(configuration.ToCallOnException(interceptorType));
                if (callOnException != null)
                    parameters.AddRange(callOnException.GetParameters().ToList());

                IEnumerable<string> enumerable = from p in parameters where p.Name == variableName select p.Name;
                if (enumerable.Any())
                    return true;
            }
            return false;
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
            VariableDefinition methodInfo = null;

            if (configuration.Needs(interceptorType, Method)) 
                methodInfo = myMethod.CreateMethodInfo();
            VariableDefinition weavedResult = myMethod.CreateWeavedResult();

            if (configuration.HasCallOnException(interceptorType))

                myMethod.Add(new TryCatch(
                                 il =>
                                 {
                                     Weave(myMethod, interceptorType, wrappedMethod, configuration, interceptor, methodInfo, args, il, weavedResult);
                                 },
                                 il =>
                                 {
                                     VariableDefinition e = CreateException(myMethod.MethodDefinition);
                                     CallExceptionInterceptor(myMethod.MethodDefinition, interceptor, methodInfo, args,
                                                              e, interceptorType, myMethod.Il, configuration);
                                     myMethod.Il.AppendThrow();
                                 }));
            else
            {
                Weave(myMethod, interceptorType, wrappedMethod, configuration, interceptor, methodInfo, args, myMethod.Il, weavedResult);
            }


            myMethod.Return(weavedResult);
        }

        private void Weave(Method myMethod, List<Type> interceptorType, MethodDefinition wrappedMethod,
                           IAroundWeaverConfiguration configuration, List<VariableDefinition> interceptor, VariableDefinition methodInfo,
                           VariableDefinition args, ILProcessor il, VariableDefinition weavedResult)
        {
            CallBefore(myMethod.MethodDefinition, interceptor, methodInfo, args,
                       interceptorType, il, configuration);
            CallWeavedMethod(myMethod.MethodDefinition,
                             wrappedMethod, il, weavedResult);
            CallAfter(myMethod.MethodDefinition, interceptor, methodInfo, args, weavedResult,
                      interceptorType, il, configuration);
        }

        private void CallExceptionInterceptor(MethodDefinition method, List<VariableDefinition> interceptor, VariableDefinition methodInfo, VariableDefinition args, VariableDefinition ex, List<Type> interceptorType, ILProcessor il, IAroundWeaverConfiguration configuration)
        {
            il.Emit(OpCodes.Stloc, ex);
            var caller = new InterceptorCaller(il, method);

            Fill(method, methodInfo, args, caller);
            caller.AddVariable("exception", ex, false);

            for (int i = 0; i < interceptorType.Count; i++)
            {
                caller.Call(interceptor[i], configuration.ToCallOnException(interceptorType[i]), interceptorType[i]);
            }
        }

        private VariableDefinition CreateException(MethodDefinition method)
        {
            return method.CreateVariable(typeof (Exception));
        }

        private void CallAfter(MethodDefinition method, List<VariableDefinition> interceptor, VariableDefinition methodInfo, VariableDefinition args, VariableDefinition handleResult, List<Type> interceptorType, ILProcessor il, IAroundWeaverConfiguration configuration)
        {
            var caller = new InterceptorCaller(il, method);

            Fill(method, methodInfo, args, caller);
            caller.AddVariable("result", handleResult, true);

            for (int i = 0; i < interceptorType.Count; i++)
            {
                caller.Call(interceptor[i], configuration.ToCallAfter(interceptorType[i]), interceptorType[i]);
            }
        }

        private void CallWeavedMethod(MethodDefinition method, MethodDefinition wrappedMethod, ILProcessor il, VariableDefinition weavedResult)
        {
            if ((wrappedMethod.Attributes & MethodAttributes.Static) != MethodAttributes.Static)
                il.Emit(OpCodes.Ldarg_0);
            foreach (ParameterDefinition p in method.Parameters.ToArray())
            {
                il.Emit(OpCodes.Ldarg, p);
            }

            OpCode call = OpCodes.Callvirt;
            if ((method.Attributes & MethodAttributes.Static) == MethodAttributes.Static)
                call = OpCodes.Call;
            il.Emit(call, wrappedMethod.MakeGeneric(method.GenericParameters.ToArray()));

            if (method.ReturnType.MetadataType != MetadataType.Void)
                il.Emit(OpCodes.Stloc, weavedResult);
        }

        private void CallBefore(MethodDefinition method, List<VariableDefinition> interceptor, VariableDefinition methodInfo, VariableDefinition args, List<Type> interceptorType, ILProcessor il, IAroundWeaverConfiguration configuration)
        {
            var caller = new InterceptorCaller(il, method);

            Fill(method, methodInfo, args, caller);

            for (int i = 0; i < interceptorType.Count; i++)
            {
                caller.Call(interceptor[i], configuration.ToCallBefore(interceptorType[i]), interceptorType[i]);
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
        
    }
}