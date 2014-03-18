﻿using System.Reflection;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.V2.Model;
using FluentAspect.Weaver.Core.V2.Weaver.Call;
using FluentAspect.Weaver.Core.V2.Weaver.Checkers;
using FluentAspect.Weaver.Core.V2.Weaver.Generators;
using FluentAspect.Weaver.Core.V2.Weaver.Method;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2.Weaver.Engine
{
    public static class MethodWeavingModelExtensions
    {
        public static void AddMethodCallWeavingModel(this WeavingModel weavingModel, MethodDefinition method,
                                                     Instruction beforeInstruction, NetAspectDefinition aspect,
                                                     Interceptor beforeCallMethod, Interceptor afterCallMethod)
        {
            var beforCallInterceptorMethod = beforeCallMethod.Method;
            if (beforCallInterceptorMethod != null)
            {
                weavingModel.AfterInstructions.Add(beforeInstruction,
                                                   CallWeavingMethodInjectorFactory.CreateForBefore(method, beforCallInterceptorMethod, aspect.Type));
            }
            var afterCallInterceptorMethod = afterCallMethod.Method;
            if (afterCallInterceptorMethod != null)
            {
                weavingModel.AfterInstructions.Add(beforeInstruction,
                                                   CallWeavingMethodInjectorFactory.CreateForAfter(method, afterCallInterceptorMethod, aspect.Type));
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
}