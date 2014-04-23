using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Engine.Instructions;
using NetAspect.Weaver.Core.Weaver.Generators;
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
            IIlInjector before = new NoIIlInjector();
            IIlInjector after = new NoIIlInjector();

            var beforCallInterceptorMethod = beforeCallMethod.Method;
            if (beforCallInterceptorMethod != null)
            {
               before = CallWeavingMethodInjectorFactory.CreateForBefore(method, beforCallInterceptorMethod, aspect, beforeInstruction);
            }
            var afterCallInterceptorMethod = afterCallMethod.Method;
            if (afterCallInterceptorMethod != null)
            {
               after = CallWeavingMethodInjectorFactory.CreateForAfter(method, afterCallInterceptorMethod, aspect, beforeInstruction);
            }
            weavingModel.AddAroundInstructionWeaver(beforeInstruction, new AroundInstructionWeaver(before, after));
        }

        public static void AddGetFieldCallWeavingModel(this WeavingModel weavingModel, MethodDefinition method,
                                                     Instruction beforeInstruction, NetAspectDefinition aspect,
                                                     Interceptor beforeCallMethod, Interceptor afterCallMethod)
        {
            IIlInjector before = new NoIIlInjector();
            IIlInjector after = new NoIIlInjector();

            var beforCallInterceptorMethod = beforeCallMethod.Method;
            if (beforCallInterceptorMethod != null)
            {
                before = CallWeavingFieldInjectorFactory.CreateForBefore(method, beforCallInterceptorMethod, aspect, beforeInstruction);
            }
            var afterCallInterceptorMethod = afterCallMethod.Method;
            if (afterCallInterceptorMethod != null)
            {
                after = CallWeavingFieldInjectorFactory.CreateForAfter(method, afterCallInterceptorMethod, aspect, beforeInstruction);
            }
            weavingModel.AddAroundInstructionWeaver(beforeInstruction, new AroundInstructionWeaver(before, after));
        }

        public static void AddGetPropertyCallWeavingModel(this WeavingModel weavingModel, MethodDefinition method, Instruction beforeInstruction, NetAspectDefinition aspect, Interceptor beforeCallMethod, Interceptor afterCallMethod, PropertyDefinition property)
        {
            IIlInjector before = new NoIIlInjector();
            IIlInjector after = new NoIIlInjector();

            var beforCallInterceptorMethod = beforeCallMethod.Method;
            if (beforCallInterceptorMethod != null)
            {
                before = CallWeavingGetPropertyInjectorFactory.CreateForBefore(method, beforCallInterceptorMethod, aspect, beforeInstruction, property);
            }
            var afterCallInterceptorMethod = afterCallMethod.Method;
            if (afterCallInterceptorMethod != null)
            {
                after = CallWeavingGetPropertyInjectorFactory.CreateForAfter(method, afterCallInterceptorMethod, aspect, beforeInstruction, property);
            }
            weavingModel.AddAroundInstructionWeaver(beforeInstruction, new AroundInstructionWeaver(before, after));
        }



        public static void AddSetPropertyCallWeavingModel(this WeavingModel weavingModel, MethodDefinition method, Instruction beforeInstruction, NetAspectDefinition aspect, Interceptor beforeCallMethod, Interceptor afterCallMethod, PropertyDefinition property)
        {
           IIlInjector before = new NoIIlInjector();
           IIlInjector after = new NoIIlInjector();

           var beforCallInterceptorMethod = beforeCallMethod.Method;
           if (beforCallInterceptorMethod != null)
           {
              before = CallWeavingSetPropertyInjectorFactory.CreateForBefore(method, beforCallInterceptorMethod, aspect, beforeInstruction, property);
           }
           var afterCallInterceptorMethod = afterCallMethod.Method;
           if (afterCallInterceptorMethod != null)
           {
              after = CallWeavingSetPropertyInjectorFactory.CreateForAfter(method, afterCallInterceptorMethod, aspect, beforeInstruction, property);
           }
           weavingModel.AddAroundInstructionWeaver(beforeInstruction, new AroundInstructionWeaver(before, after));
        }

        public static void AddUpdateFieldCallWeavingModel(this WeavingModel weavingModel, MethodDefinition method,
                                                     Instruction beforeInstruction, NetAspectDefinition aspect,
                                                     Interceptor beforeCallMethod, Interceptor afterCallMethod)
        {
            IIlInjector before = new NoIIlInjector();
            IIlInjector after = new NoIIlInjector();

            var beforCallInterceptorMethod = beforeCallMethod.Method;
            if (beforCallInterceptorMethod != null)
            {
                before = CallWeavingUpdateFieldInjectorFactory.CreateForBefore(method, beforCallInterceptorMethod, aspect, beforeInstruction);
            }
            var afterCallInterceptorMethod = afterCallMethod.Method;
            if (afterCallInterceptorMethod != null)
            {
                after = CallWeavingUpdateFieldInjectorFactory.CreateForAfter(method, afterCallInterceptorMethod, aspect, beforeInstruction);
            }
            weavingModel.AddAroundInstructionWeaver(beforeInstruction, new AroundInstructionWeaver(before, after));
        }
        

        public static void AddMethodWeavingModel(this WeavingModel weavingModel, MethodDefinition method,
                                                 NetAspectDefinition aspect,
                                                 Interceptor before, Interceptor after, Interceptor onException,
                                                 Interceptor onFinally)
        {
            if (before.Method != null)
            {
                weavingModel.Method.Befores.Add(MethodWeavingMethodInjectorFactory.CreateForBefore(method, before.Method,
                                                                                                   aspect));
            }
            if (after.Method != null)
            {
                weavingModel.Method.Afters.Add(MethodWeavingMethodInjectorFactory.CreateForAfter(method, after.Method,
                                                                                                 aspect));
            }
            if (onException.Method != null)
            {
                weavingModel.Method.OnExceptions.Add(MethodWeavingMethodInjectorFactory.CreateForOnException(method,
                                                                                                             onException
                                                                                                                 .Method,
                                                                                                             aspect));
            }
            if (onFinally.Method != null)
            {
                weavingModel.Method.OnFinallys.Add(MethodWeavingMethodInjectorFactory.CreateForOnFinally(method,
                                                                                                         onFinally
                                                                                                             .Method,
                                                                                                         aspect));
            }
        }

        public static void AddConstructorWeavingModel(this WeavingModel weavingModel, MethodDefinition method,
                                                  NetAspectDefinition aspect,
                                                  Interceptor before, Interceptor after, Interceptor onException,
                                                  Interceptor onFinally)
        {
           if (before.Method != null)
           {
              weavingModel.Method.Befores.Add(ConstructorWeavingMethodInjectorFactory.CreateForBefore(method, before.Method,
                                                                                                 aspect));
           }
           if (after.Method != null)
           {
              weavingModel.Method.Afters.Add(ConstructorWeavingMethodInjectorFactory.CreateForAfter(method, after.Method,
                                                                                               aspect));
           }
           if (onException.Method != null)
           {
              weavingModel.Method.OnExceptions.Add(ConstructorWeavingMethodInjectorFactory.CreateForOnException(method,
                                                                                                           onException
                                                                                                               .Method,
                                                                                                           aspect));
           }
           if (onFinally.Method != null)
           {
              weavingModel.Method.OnFinallys.Add(ConstructorWeavingMethodInjectorFactory.CreateForOnFinally(method,
                                                                                                       onFinally
                                                                                                           .Method,
                                                                                                       aspect));
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
                                                                                                        aspect));
            }
            if (after.Method != null)
            {
                weavingModel.Method.Afters.Add(MethodWeavingPropertyGetInjectorFactory.CreateForAfter(method,
                                                                                                      after.Method,
                                                                                                      aspect));
            }
            if (onException.Method != null)
            {
                weavingModel.Method.OnExceptions.Add(MethodWeavingPropertyGetInjectorFactory.CreateForOnException(
                    method,
                    onException.Method,
                    aspect));
            }
            if (onFinally.Method != null)
            {
                weavingModel.Method.OnFinallys.Add(MethodWeavingPropertyGetInjectorFactory.CreateForOnFinally(method,
                                                                                                              onFinally
                                                                                                                  .Method,
                                                                                                              aspect
                                                                                                                  ));
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
                                                                                                        aspect));
            }
            if (after.Method != null)
            {
                weavingModel.Method.Afters.Add(MethodWeavingPropertySetInjectorFactory.CreateForAfter(method,
                                                                                                      after.Method,
                                                                                                      aspect));
            }
            if (onException.Method != null)
            {
                weavingModel.Method.OnExceptions.Add(MethodWeavingPropertySetInjectorFactory.CreateForOnException(
                    method,
                    onException.Method,
                    aspect));
            }
            if (onFinally.Method != null)
            {
                weavingModel.Method.OnFinallys.Add(MethodWeavingPropertySetInjectorFactory.CreateForOnFinally(method,
                                                                                                              onFinally
                                                                                                                  .Method,
                                                                                                              aspect
                                                                                                                  ));
            }
        }
    }
}