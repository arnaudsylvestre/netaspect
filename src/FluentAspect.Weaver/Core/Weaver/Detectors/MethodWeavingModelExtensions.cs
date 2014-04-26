using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors
{
    public static class MethodWeavingModelExtensions
    {
        

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