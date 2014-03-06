using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2
{
    public static class MethodWeavingModelExtensions
    {
        public static void AddMethodCallWeavingModel(this WeavingModel weavingModel, MethodDefinition method,
                                                     Instruction beforeInstruction, NetAspectDefinition aspect,
                                                     Interceptor beforeCallMethod, Interceptor afterCallMethod)
        {
            MethodInfo beforCallInterceptorMethod = beforeCallMethod.Method;
            if (beforCallInterceptorMethod != null)
            {
                weavingModel.BeforeInstructions.Add(beforeInstruction,
                                                    CallMethodWeavingMethodInjectorFactory.CreateForBefore(method,
                                                                                                           beforCallInterceptorMethod,
                                                                                                           aspect.Type));
            }
            MethodInfo afterCallInterceptorMethod = afterCallMethod.Method;
            if (afterCallInterceptorMethod != null)
            {
                weavingModel.AfterInstructions.Add(beforeInstruction,
                                                   CallMethodWeavingMethodInjectorFactory.CreateForAfter(method,
                                                                                                         afterCallInterceptorMethod,
                                                                                                         aspect.Type));
            }
        }

        public static void AddMethodWeavingModel(this WeavingModel weavingModel, MethodDefinition method,
                                                 NetAspectDefinition aspect,
                                                 Interceptor before, Interceptor after, Interceptor onException,
                                                 Interceptor onFinally)
        {
            if (before.Method != null)
            {
                weavingModel.Method.Befores.Add(MethodWeavingMethodInjectorFactory.CreateForBefore(method, before.Method,
                                                                                                   aspect.Type));
            }
            if (after.Method != null)
            {
                weavingModel.Method.Afters.Add(MethodWeavingMethodInjectorFactory.CreateForAfter(method, after.Method,
                                                                                                 aspect.Type));
            }
            if (onException.Method != null)
            {
                weavingModel.Method.OnExceptions.Add(MethodWeavingMethodInjectorFactory.CreateForOnException(method,
                                                                                                             onException
                                                                                                                 .Method,
                                                                                                             aspect.Type));
            }
            if (onFinally.Method != null)
            {
                weavingModel.Method.OnFinallys.Add(MethodWeavingMethodInjectorFactory.CreateForOnFinally(method,
                                                                                                         onFinally
                                                                                                             .Method,
                                                                                                         aspect.Type));
            }
        }

        public static void AddPropertyGetWeavingModel(this WeavingModel weavingModel, MethodDefinition method,
                                                      NetAspectDefinition aspect,
                                                      Interceptor before, Interceptor after, Interceptor onException,
                                                      Interceptor onFinally)
        {
            if (before.Method != null)
            {
                weavingModel.Method.Befores.Add(MethodWeavingPropertyGetInjectorFactory.CreateForBefore(method,
                                                                                                        before.Method,
                                                                                                        aspect.Type));
            }
            if (after.Method != null)
            {
                weavingModel.Method.Afters.Add(MethodWeavingPropertyGetInjectorFactory.CreateForAfter(method,
                                                                                                      after.Method,
                                                                                                      aspect.Type));
            }
            if (onException.Method != null)
            {
                weavingModel.Method.OnExceptions.Add(MethodWeavingPropertyGetInjectorFactory.CreateForOnException(
                    method,
                    onException.Method,
                    aspect.Type));
            }
            if (onFinally.Method != null)
            {
                weavingModel.Method.OnFinallys.Add(MethodWeavingPropertyGetInjectorFactory.CreateForOnFinally(method,
                                                                                                              onFinally
                                                                                                                  .Method,
                                                                                                              aspect
                                                                                                                  .Type));
            }
        }

        public static void AddPropertySetWeavingModel(this WeavingModel weavingModel, MethodDefinition method,
                                                      NetAspectDefinition aspect,
                                                      Interceptor before, Interceptor after, Interceptor onException,
                                                      Interceptor onFinally)
        {
            if (before.Method != null)
            {
                weavingModel.Method.Befores.Add(MethodWeavingPropertySetInjectorFactory.CreateForBefore(method,
                                                                                                        before.Method,
                                                                                                        aspect.Type));
            }
            if (after.Method != null)
            {
                weavingModel.Method.Afters.Add(MethodWeavingPropertySetInjectorFactory.CreateForAfter(method,
                                                                                                      after.Method,
                                                                                                      aspect.Type));
            }
            if (onException.Method != null)
            {
                weavingModel.Method.OnExceptions.Add(MethodWeavingPropertySetInjectorFactory.CreateForOnException(
                    method,
                    onException.Method,
                    aspect.Type));
            }
            if (onFinally.Method != null)
            {
                weavingModel.Method.OnFinallys.Add(MethodWeavingPropertySetInjectorFactory.CreateForOnFinally(method,
                                                                                                              onFinally
                                                                                                                  .Method,
                                                                                                              aspect
                                                                                                                  .Type));
            }
        }
    }

    public class MetchodCallInjector : IInstructionIlInjector
    {
        private readonly Type _aspectType;
        private readonly MethodDefinition _method;
        private readonly ParametersIlGenerator<IlInstructionInjectorAvailableVariables> ilGenerator;
        private readonly MethodInfo interceptorMethod;
        private ParametersChecker _parametersChecker;

        public MetchodCallInjector(MethodDefinition method, Type aspectType, ParametersChecker parametersChecker,
                                   ParametersIlGenerator<IlInstructionInjectorAvailableVariables> ilGenerator,
                                   MethodInfo interceptorMethod)
        {
            _method = method;
            _aspectType = aspectType;
            _parametersChecker = parametersChecker;
            this.ilGenerator = ilGenerator;
            this.interceptorMethod = interceptorMethod;
        }

        public void Check(ErrorHandler errorHandler, IlInstructionInjectorAvailableVariables availableInformations)
        {
        }

        public void Inject(List<Instruction> instructions, IlInstructionInjectorAvailableVariables availableInformations)
        {
            VariableDefinition interceptor = _method.CreateVariable(_aspectType);
            instructions.AppendCreateNewObject(interceptor, _aspectType, _method.Module);
            instructions.Add(Instruction.Create(OpCodes.Ldloc, interceptor));
            ilGenerator.Generate(interceptorMethod.GetParameters(), instructions, availableInformations);
            instructions.Add(Instruction.Create(OpCodes.Call, _method.Module.Import(interceptorMethod)));
        }
    }
}