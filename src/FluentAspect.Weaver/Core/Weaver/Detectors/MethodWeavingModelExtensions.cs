using System.Reflection;
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

    public interface ICallWeavingModelFactory
    {
        IIlInjector<IlInjectorAvailableVariablesForInstruction> CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod, NetAspectDefinition aspect, Instruction instruction);
        IIlInjector<IlInjectorAvailableVariablesForInstruction> CreateForAfter(MethodDefinition method, MethodInfo interceptorMethod, NetAspectDefinition aspect, Instruction instruction);
    }

    public static class MethodWeavingModelExtensions
    {
        public static void AddMethodCallWeavingModel(this MethodWeavingModel methodWeavingModel, MethodDefinition method,
                                                     Instruction beforeInstruction, NetAspectDefinition aspect,
                                                     Interceptor beforeCallMethod, Interceptor afterCallMethod)
        {
            IIlInjector<IlInjectorAvailableVariablesForInstruction> before = new NoIIlInjector<IlInjectorAvailableVariablesForInstruction>();
            IIlInjector<IlInjectorAvailableVariablesForInstruction> after = new NoIIlInjector<IlInjectorAvailableVariablesForInstruction>();

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
            methodWeavingModel.AddAroundInstructionWeaver(beforeInstruction, new AroundInstructionWeaver(before, after));
        }

        public static void AddGetFieldCallWeavingModel(this MethodWeavingModel methodWeavingModel, MethodDefinition method,
                                                     Instruction beforeInstruction, NetAspectDefinition aspect,
                                                     Interceptor beforeCallMethod, Interceptor afterCallMethod)
        {
            IIlInjector<IlInjectorAvailableVariablesForInstruction> before = new NoIIlInjector<IlInjectorAvailableVariablesForInstruction>();
            IIlInjector<IlInjectorAvailableVariablesForInstruction> after = new NoIIlInjector<IlInjectorAvailableVariablesForInstruction>();

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
            methodWeavingModel.AddAroundInstructionWeaver(beforeInstruction, new AroundInstructionWeaver(before, after));
        }

        public static void AddGetPropertyCallWeavingModel(this MethodWeavingModel methodWeavingModel, MethodDefinition method, Instruction beforeInstruction, NetAspectDefinition aspect, Interceptor beforeCallMethod, Interceptor afterCallMethod, PropertyDefinition property)
        {
            IIlInjector<IlInjectorAvailableVariablesForInstruction> before = new NoIIlInjector<IlInjectorAvailableVariablesForInstruction>();
            IIlInjector<IlInjectorAvailableVariablesForInstruction> after = new NoIIlInjector<IlInjectorAvailableVariablesForInstruction>();

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
            methodWeavingModel.AddAroundInstructionWeaver(beforeInstruction, new AroundInstructionWeaver(before, after));
        }



        public static void AddSetPropertyCallWeavingModel(this MethodWeavingModel methodWeavingModel, MethodDefinition method, Instruction beforeInstruction, NetAspectDefinition aspect, Interceptor beforeCallMethod, Interceptor afterCallMethod, PropertyDefinition property)
        {
            IIlInjector<IlInjectorAvailableVariablesForInstruction> before = new NoIIlInjector<IlInjectorAvailableVariablesForInstruction>();
            IIlInjector<IlInjectorAvailableVariablesForInstruction> after = new NoIIlInjector<IlInjectorAvailableVariablesForInstruction>();

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
           methodWeavingModel.AddAroundInstructionWeaver(beforeInstruction, new AroundInstructionWeaver(before, after));
        }

        public static void AddUpdateFieldCallWeavingModel(this MethodWeavingModel methodWeavingModel, MethodDefinition method,
                                                     Instruction beforeInstruction, NetAspectDefinition aspect,
                                                     Interceptor beforeCallMethod, Interceptor afterCallMethod)
        {
            IIlInjector<IlInjectorAvailableVariablesForInstruction> before = new NoIIlInjector<IlInjectorAvailableVariablesForInstruction>();
            IIlInjector<IlInjectorAvailableVariablesForInstruction> after = new NoIIlInjector<IlInjectorAvailableVariablesForInstruction>();

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
            methodWeavingModel.AddAroundInstructionWeaver(beforeInstruction, new AroundInstructionWeaver(before, after));
        }
        

        public static void AddMethodWeavingModel(this MethodWeavingModel methodWeavingModel, MethodDefinition method,
                                                 NetAspectDefinition aspect,
                                                 Interceptor before, Interceptor after, Interceptor onException,
                                                 Interceptor onFinally)
        {
            if (before.Method != null)
            {
                methodWeavingModel.Method.Befores.Add(MethodWeavingMethodInjectorFactory.CreateForBefore(method, before.Method,
                                                                                                   aspect));
            }
            if (after.Method != null)
            {
                methodWeavingModel.Method.Afters.Add(MethodWeavingMethodInjectorFactory.CreateForAfter(method, after.Method,
                                                                                                 aspect));
            }
            if (onException.Method != null)
            {
                methodWeavingModel.Method.OnExceptions.Add(MethodWeavingMethodInjectorFactory.CreateForOnException(method,
                                                                                                             onException
                                                                                                                 .Method,
                                                                                                             aspect));
            }
            if (onFinally.Method != null)
            {
                methodWeavingModel.Method.OnFinallys.Add(MethodWeavingMethodInjectorFactory.CreateForOnFinally(method,
                                                                                                         onFinally
                                                                                                             .Method,
                                                                                                         aspect));
            }
        }

        public static void AddConstructorWeavingModel(this MethodWeavingModel methodWeavingModel, MethodDefinition method,
                                                  NetAspectDefinition aspect,
                                                  Interceptor before, Interceptor after, Interceptor onException,
                                                  Interceptor onFinally)
        {
           if (before.Method != null)
           {
              methodWeavingModel.Method.Befores.Add(ConstructorWeavingMethodInjectorFactory.CreateForBefore(method, before.Method,
                                                                                                 aspect));
           }
           if (after.Method != null)
           {
              methodWeavingModel.Method.Afters.Add(ConstructorWeavingMethodInjectorFactory.CreateForAfter(method, after.Method,
                                                                                               aspect));
           }
           if (onException.Method != null)
           {
              methodWeavingModel.Method.OnExceptions.Add(ConstructorWeavingMethodInjectorFactory.CreateForOnException(method,
                                                                                                           onException
                                                                                                               .Method,
                                                                                                           aspect));
           }
           if (onFinally.Method != null)
           {
              methodWeavingModel.Method.OnFinallys.Add(ConstructorWeavingMethodInjectorFactory.CreateForOnFinally(method,
                                                                                                       onFinally
                                                                                                           .Method,
                                                                                                       aspect));
           }
        }

        public static void AddPropertyGetMethodWeavingModel(this MethodWeavingModel methodWeavingModel, MethodDefinition method,
                                                      NetAspectDefinition aspect,
                                                      Interceptor before, Interceptor after, Interceptor onException,
                                                      Interceptor onFinally)
        {
            if (before.Method != null)
            {
                methodWeavingModel.Method.Befores.Add(MethodWeavingPropertyGetInjectorFactory.CreateForBefore(method,
                                                                                                        before.Method,
                                                                                                        aspect));
            }
            if (after.Method != null)
            {
                methodWeavingModel.Method.Afters.Add(MethodWeavingPropertyGetInjectorFactory.CreateForAfter(method,
                                                                                                      after.Method,
                                                                                                      aspect));
            }
            if (onException.Method != null)
            {
                methodWeavingModel.Method.OnExceptions.Add(MethodWeavingPropertyGetInjectorFactory.CreateForOnException(
                    method,
                    onException.Method,
                    aspect));
            }
            if (onFinally.Method != null)
            {
                methodWeavingModel.Method.OnFinallys.Add(MethodWeavingPropertyGetInjectorFactory.CreateForOnFinally(method,
                                                                                                              onFinally
                                                                                                                  .Method,
                                                                                                              aspect
                                                                                                                  ));
            }
        }

        public static void AddPropertySetMethodWeavingModel(this MethodWeavingModel methodWeavingModel, MethodDefinition method,
                                                      NetAspectDefinition aspect,
                                                      Interceptor before, Interceptor after, Interceptor onException,
                                                      Interceptor onFinally)
        {
            if (before.Method != null)
            {
                methodWeavingModel.Method.Befores.Add(MethodWeavingPropertySetInjectorFactory.CreateForBefore(method,
                                                                                                        before.Method,
                                                                                                        aspect));
            }
            if (after.Method != null)
            {
                methodWeavingModel.Method.Afters.Add(MethodWeavingPropertySetInjectorFactory.CreateForAfter(method,
                                                                                                      after.Method,
                                                                                                      aspect));
            }
            if (onException.Method != null)
            {
                methodWeavingModel.Method.OnExceptions.Add(MethodWeavingPropertySetInjectorFactory.CreateForOnException(
                    method,
                    onException.Method,
                    aspect));
            }
            if (onFinally.Method != null)
            {
                methodWeavingModel.Method.OnFinallys.Add(MethodWeavingPropertySetInjectorFactory.CreateForOnFinally(method,
                                                                                                              onFinally
                                                                                                                  .Method,
                                                                                                              aspect
                                                                                                                  ));
            }
        }
    }
}