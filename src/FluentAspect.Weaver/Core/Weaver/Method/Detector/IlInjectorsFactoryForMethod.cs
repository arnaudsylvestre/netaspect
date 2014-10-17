using System;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving
{
   public class IlInjectorsFactoryForMethod
   {
      private readonly IInterceptorParameterConfigurationForMethodFiller builder;
      private readonly IWevingPreconditionInjector<VariablesForMethod> weavingPreconditionInjector;
       private readonly Action<MethodWeavingInfo, InterceptorParameterConfigurations<VariablesForMethod>> NoSpecific = (info, interceptorParameterConfigurations) => { };

      public IlInjectorsFactoryForMethod(IInterceptorParameterConfigurationForMethodFiller builder, IWevingPreconditionInjector<VariablesForMethod> weavingPreconditionInjector)
      {
         this.builder = builder;
         this.weavingPreconditionInjector = weavingPreconditionInjector;
      }
       
      public IIlInjector<VariablesForMethod> CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod)
      {
          return Create(method, interceptorMethod, NoSpecific);
      }

      public IIlInjector<VariablesForMethod> CreateForOnFinally(MethodDefinition method, MethodInfo interceptorMethod)
      {
          return Create(method, interceptorMethod, NoSpecific);
      }

       public IIlInjector<VariablesForMethod> CreateForAfter(MethodDefinition method, MethodInfo interceptorMethod)
      {
         return Create(method, interceptorMethod, builder.FillAfterSpecific);
      }

      public IIlInjector<VariablesForMethod> CreateForExceptions(MethodDefinition method, MethodInfo interceptorMethod)
      {
         return Create(method, interceptorMethod, builder.FillOnExceptionSpecific);
      }

      private IIlInjector<VariablesForMethod> Create(MethodDefinition method, MethodInfo interceptorMethod, Action<MethodWeavingInfo, InterceptorParameterConfigurations<VariablesForMethod>> fillSpecific)
      {
          if (interceptorMethod == null)
              return new NoIIlInjector<VariablesForMethod>();

          var weavingInfo_P = new MethodWeavingInfo
          {
              Interceptor = interceptorMethod,
              Method = method,
          };
          var interceptorParameterConfigurations = new InterceptorParameterConfigurations<VariablesForMethod>();
          builder.FillCommon(weavingInfo_P, interceptorParameterConfigurations);
          fillSpecific(weavingInfo_P, interceptorParameterConfigurations);

          return new Injector<VariablesForMethod>(method, interceptorMethod, interceptorParameterConfigurations, weavingPreconditionInjector);
      }
   }
}
