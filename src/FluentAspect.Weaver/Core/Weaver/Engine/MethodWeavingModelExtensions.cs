using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weaver.Call;
using FluentAspect.Weaver.Core.Weaver.Method;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weaver.Engine
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

        public static void AddGetFieldCallWeavingModel(this WeavingModel weavingModel, MethodDefinition method,
                                                     Instruction beforeInstruction, NetAspectDefinition aspect,
                                                     Interceptor beforeCallMethod, Interceptor afterCallMethod)
        {
            var beforCallInterceptorMethod = beforeCallMethod.Method;
            if (beforCallInterceptorMethod != null)
            {
                weavingModel.AfterInstructions.Add(beforeInstruction,
                                                   CallWeavingFieldInjectorFactory.CreateForBefore(method, beforCallInterceptorMethod, aspect.Type, beforeInstruction));
            }
            var afterCallInterceptorMethod = afterCallMethod.Method;
            if (afterCallInterceptorMethod != null)
            {
                weavingModel.AfterInstructions.Add(beforeInstruction,
                                                   CallWeavingFieldInjectorFactory.CreateForAfter(method, afterCallInterceptorMethod, aspect.Type, beforeInstruction));
            }
        }

        public static void AddUpdateFieldCallWeavingModel(this WeavingModel weavingModel, MethodDefinition method,
                                                     Instruction beforeInstruction, NetAspectDefinition aspect,
                                                     Interceptor beforeCallMethod, Interceptor afterCallMethod)
        {
            var beforCallInterceptorMethod = beforeCallMethod.Method;
            if (beforCallInterceptorMethod != null)
            {
                weavingModel.AfterInstructions.Add(beforeInstruction,
                                                   CallWeavingUpdateFieldInjectorFactory.CreateForBefore(method, beforCallInterceptorMethod, aspect.Type, beforeInstruction));
            }
            var afterCallInterceptorMethod = afterCallMethod.Method;
            if (afterCallInterceptorMethod != null)
            {
                weavingModel.AfterInstructions.Add(beforeInstruction,
                                                   CallWeavingUpdateFieldInjectorFactory.CreateForAfter(method, afterCallInterceptorMethod, aspect.Type, beforeInstruction));
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