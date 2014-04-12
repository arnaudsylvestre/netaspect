using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Call;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors
{
    public static class MethodWeavingModelExtensions
    {
        public static void AddMethodCallWeavingModel(this WeavingModel weavingModel, MethodDefinition method,
                                                     Instruction beforeInstruction, NetAspectDefinition aspect,
                                                     Interceptor beforeCallMethod, Interceptor afterCallMethod)
        {
            IIlInjector<IlInjectorAvailableVariablesForInstruction> before = new NoIIlInjector();
            IIlInjector<IlInjectorAvailableVariablesForInstruction> after = new NoIIlInjector();

            var beforCallInterceptorMethod = beforeCallMethod.Method;
            if (beforCallInterceptorMethod != null)
            {
               before = CallWeavingMethodInjectorFactory.CreateForBefore(method, beforCallInterceptorMethod, aspect.Type, beforeInstruction);
            }
            var afterCallInterceptorMethod = afterCallMethod.Method;
            if (afterCallInterceptorMethod != null)
            {
               after = CallWeavingMethodInjectorFactory.CreateForAfter(method, afterCallInterceptorMethod, aspect.Type, beforeInstruction);
            }
            weavingModel.AddAroundInstructionWeaver(beforeInstruction, new AroundInstructionWeaver(new CallGetFieldInitializerWeaver(), before, after));
        }

        public static void AddGetFieldCallWeavingModel(this WeavingModel weavingModel, MethodDefinition method,
                                                     Instruction beforeInstruction, NetAspectDefinition aspect,
                                                     Interceptor beforeCallMethod, Interceptor afterCallMethod)
        {
            IIlInjector<IlInjectorAvailableVariablesForInstruction> before = new NoIIlInjector();
            IIlInjector<IlInjectorAvailableVariablesForInstruction> after = new NoIIlInjector();

            var beforCallInterceptorMethod = beforeCallMethod.Method;
            if (beforCallInterceptorMethod != null)
            {
                before = CallWeavingFieldInjectorFactory.CreateForBefore(method, beforCallInterceptorMethod, aspect.Type, beforeInstruction);
            }
            var afterCallInterceptorMethod = afterCallMethod.Method;
            if (afterCallInterceptorMethod != null)
            {
                after = CallWeavingFieldInjectorFactory.CreateForAfter(method, afterCallInterceptorMethod, aspect.Type, beforeInstruction);
            }
            weavingModel.AddAroundInstructionWeaver(beforeInstruction, new AroundInstructionWeaver(new CallGetFieldInitializerWeaver(), before, after));
        }

        public static void AddGetPropertyCallWeavingModel(this WeavingModel weavingModel, MethodDefinition method, Instruction beforeInstruction, NetAspectDefinition aspect, Interceptor beforeCallMethod, Interceptor afterCallMethod, PropertyDefinition property)
        {
            IIlInjector<IlInjectorAvailableVariablesForInstruction> before = new NoIIlInjector();
            IIlInjector<IlInjectorAvailableVariablesForInstruction> after = new NoIIlInjector();

            var beforCallInterceptorMethod = beforeCallMethod.Method;
            if (beforCallInterceptorMethod != null)
            {
                before = CallWeavingGetPropertyInjectorFactory.CreateForBefore(method, beforCallInterceptorMethod, aspect.Type, beforeInstruction, property);
            }
            var afterCallInterceptorMethod = afterCallMethod.Method;
            if (afterCallInterceptorMethod != null)
            {
                after = CallWeavingGetPropertyInjectorFactory.CreateForAfter(method, afterCallInterceptorMethod, aspect.Type, beforeInstruction, property);
            }
            weavingModel.AddAroundInstructionWeaver(beforeInstruction, new AroundInstructionWeaver(new CallGetFieldInitializerWeaver(), before, after));
        }

        public static void AddUpdateFieldCallWeavingModel(this WeavingModel weavingModel, MethodDefinition method,
                                                     Instruction beforeInstruction, NetAspectDefinition aspect,
                                                     Interceptor beforeCallMethod, Interceptor afterCallMethod)
        {
            IIlInjector<IlInjectorAvailableVariablesForInstruction> before = new NoIIlInjector();
            IIlInjector<IlInjectorAvailableVariablesForInstruction> after = new NoIIlInjector();

            var beforCallInterceptorMethod = beforeCallMethod.Method;
            if (beforCallInterceptorMethod != null)
            {
                before = CallWeavingUpdateFieldInjectorFactory.CreateForBefore(method, beforCallInterceptorMethod, aspect.Type, beforeInstruction);
            }
            var afterCallInterceptorMethod = afterCallMethod.Method;
            if (afterCallInterceptorMethod != null)
            {
                after = CallWeavingUpdateFieldInjectorFactory.CreateForAfter(method, afterCallInterceptorMethod, aspect.Type, beforeInstruction);
            }
            weavingModel.AddAroundInstructionWeaver(beforeInstruction, new AroundInstructionWeaver(new CallGetFieldInitializerWeaver(), before, after));
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

        public static void AddPropertyGetMethodWeavingModel(this WeavingModel weavingModel, MethodDefinition method,
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

        public static void AddPropertySetMethodWeavingModel(this WeavingModel weavingModel, MethodDefinition method,
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