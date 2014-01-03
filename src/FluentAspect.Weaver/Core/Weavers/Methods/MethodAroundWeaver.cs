using System;
using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Weavers.Helpers;
using FluentAspect.Weaver.Core.Weavers.Methods.Interceptors;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MethodAttributes = Mono.Cecil.MethodAttributes;

namespace FluentAspect.Weaver.Core.Weavers.Methods
{
   public class MethodAroundWeaver
    {
      private readonly InterceptorHelpers _interceptorHelpers = new InterceptorHelpers();
      public const string ParameterParameters = "parameters";
      public const string Instance = "instance";
      public const string Method = "method";

        public void CreateWeaver(MethodDefinition method, IEnumerable<MethodWeavingConfiguration> interceptorType, MethodDefinition wrappedMethod)
        {
            CreateWeaver(new Method(method), interceptorType, wrappedMethod);
        }

        public void CreateWeaver(Method myMethod, IEnumerable<MethodWeavingConfiguration> interceptorType, MethodDefinition wrappedMethod)
        {



           Variables variables = CreateVariables(myMethod, interceptorType);

            if (AroundWeaverConfigurationExtensions.HasCallOnException(interceptorType))

                myMethod.Add(new TryCatch(
                                 il =>
                                 {
                                    Weave(myMethod, interceptorType, wrappedMethod, variables, il);
                                 },
                                 il =>
                                 {
                                     VariableDefinition e = myMethod.MethodDefinition.CreateVariable(typeof (Exception));
                                     CallExceptionInterceptor(myMethod.MethodDefinition, variables,
                                                              e, interceptorType, myMethod.Il);
                                     myMethod.Il.AppendThrow();
                                 }));
            else
            {
                Weave(myMethod, interceptorType, wrappedMethod, variables, myMethod.Il);
            }


            myMethod.Return(variables.handleResult);
        }

        private Variables CreateVariables(Method myMethod, IEnumerable<MethodWeavingConfiguration> interceptorType)
      {
         List<VariableDefinition> interceptor = myMethod.CreateAndInitializeVariable(interceptorType);
         VariableDefinition args = null;
         if (AroundWeaverConfigurationExtensions.Needs(interceptorType, ParameterParameters))
            args = myMethod.CreateArgsArrayFromParameters();
         VariableDefinition methodInfo = null;

         if (AroundWeaverConfigurationExtensions.Needs(interceptorType, Method))
            methodInfo = myMethod.CreateMethodInfo();
         VariableDefinition weavedResult = myMethod.CreateWeavedResult();
           return new Variables()
              {
                 Interceptors = interceptor,
                 args                 = args,
                 handleResult = weavedResult,
                 methodInfo                 = methodInfo,
              };
      }

      private void Weave(Method myMethod, IEnumerable<MethodWeavingConfiguration> interceptorType, MethodDefinition wrappedMethod,
                           Variables variables, ILProcessor il)
        {

           AfterInterceptorILGenerator afterInterceptor_L = new AfterInterceptorILGenerator(interceptorType);
            CallBefore(myMethod.MethodDefinition, variables,
                       interceptorType, il);
            CallWeavedMethod(myMethod.MethodDefinition,
                             wrappedMethod, il, variables.handleResult);
            afterInterceptor_L.CallAfter(myMethod.MethodDefinition, variables, il);
        }

        private void CallExceptionInterceptor(MethodDefinition method, Variables variables, VariableDefinition ex, IEnumerable<MethodWeavingConfiguration> interceptorType, ILProcessor il)
        {
            il.Emit(OpCodes.Stloc, ex);
            var caller = new InterceptorCaller(il, method);

            InterceptorHelpers.FillForBefore(method, variables.methodInfo, variables.args, caller);
            caller.AddVariable("exception", ex, false);

            for (int i = 0; i < interceptorType.Count(); i++)
            {
               caller.Call(variables.Interceptors[i], interceptorType.ToList()[i].OnException.Method, interceptorType.ToList()[i]);
            }
        }


      private void CallWeavedMethod(MethodDefinition method, MethodDefinition wrappedMethod, ILProcessor il, VariableDefinition weavedResult)
        {
            if (!wrappedMethod.IsStatic)
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

        private void CallBefore(MethodDefinition method, Variables variables, IEnumerable<MethodWeavingConfiguration> interceptorType, ILProcessor il)
        {
            var caller = new InterceptorCaller(il, method);

            InterceptorHelpers.FillForBefore(method, variables.methodInfo, variables.args, caller);

            for (int i = 0; i < interceptorType.Count(); i++)
            {
               caller.Call(variables.Interceptors[i], interceptorType.ToList()[i].Before.Method, interceptorType.ToList()[i]);
            }
        }
    }
}