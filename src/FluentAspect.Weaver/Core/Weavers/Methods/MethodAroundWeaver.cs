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
        public void CreateWeaver(Method myMethod, IEnumerable<MethodWeavingConfiguration> methodWeavingConfigurations, MethodDefinition wrappedMethod)
        {
            var variables = VariablesBuilder.CreateVariables(myMethod, methodWeavingConfigurations);
            OnExceptionInterceptorILGenerator onExceptionInterceptorIlGenerator = new OnExceptionInterceptorILGenerator(methodWeavingConfigurations);

            if (methodWeavingConfigurations.HasCallOnException())

                myMethod.Add(new TryCatch(
                                 () =>
                                 {
                                    Weave(myMethod, methodWeavingConfigurations, wrappedMethod, variables);
                                 },
                                 () =>
                                     {
                                         onExceptionInterceptorIlGenerator.GenerateOnExceptionInterceptor(myMethod, variables);
                                     
                                 }));
            else
            {
                Weave(myMethod, methodWeavingConfigurations, wrappedMethod, variables);
            }


            myMethod.Return(variables.handleResult);
        }

        

      private void Weave(Method myMethod, IEnumerable<MethodWeavingConfiguration> interceptorType, MethodDefinition wrappedMethod,
                           Variables variables)
        {

           AfterInterceptorILGenerator afterInterceptor_L = new AfterInterceptorILGenerator(interceptorType);
          BeforeInterceptorILGenerator beforeInterceptorIl = new BeforeInterceptorILGenerator(interceptorType);
          beforeInterceptorIl.CallBefore(myMethod, variables);
            CallWeavedMethod(myMethod, wrappedMethod, variables.handleResult);
            afterInterceptor_L.CallAfter(myMethod, variables);
        }

        


      private void CallWeavedMethod(Method method, MethodDefinition wrappedMethod, VariableDefinition weavedResult)
        {
            if (!wrappedMethod.IsStatic)
                method.Il.Emit(OpCodes.Ldarg_0);
            foreach (ParameterDefinition p in method.MethodDefinition.Parameters.ToArray())
            {
                method.Il.Emit(OpCodes.Ldarg, p);
            }

            OpCode call = OpCodes.Callvirt;
            if ((method.MethodDefinition.Attributes & MethodAttributes.Static) == MethodAttributes.Static)
                call = OpCodes.Call;
            method.Il.Emit(call, wrappedMethod.MakeGeneric(method.MethodDefinition.GenericParameters.ToArray()));

            if (method.MethodDefinition.ReturnType.MetadataType != MetadataType.Void)
                method.Il.Emit(OpCodes.Stloc, weavedResult);
        }
    }
}