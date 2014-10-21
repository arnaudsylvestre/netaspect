using System;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.ToSort.ILInjector;

namespace NetAspect.Weaver.Core.Weaver.Method.Detector
{
   public class IlInjectorsFactoryForMethod
   {
      private readonly IInterceptorParameterConfigurationForMethodFiller builder;
      private readonly IWevingPreconditionInjector<VariablesForMethod> weavingPreconditionInjector;
       private readonly Action<CommonWeavingInfo, InterceptorParameterPossibilities<VariablesForMethod>> NoSpecific = (info, interceptorParameterConfigurations) => { };

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

      private IIlInjector<VariablesForMethod> Create(MethodDefinition method, MethodInfo interceptorMethod, Action<CommonWeavingInfo, InterceptorParameterPossibilities<VariablesForMethod>> fillSpecific)
      {
          if (interceptorMethod == null)
              return new NoIIlInjector<VariablesForMethod>();

          var weavingInfo_P = new CommonWeavingInfo
          {
              Interceptor = interceptorMethod,
              Method = method,
          };
          var interceptorParameterConfigurations = new InterceptorParameterPossibilities<VariablesForMethod>();
          builder.FillCommon(weavingInfo_P, interceptorParameterConfigurations);
          fillSpecific(weavingInfo_P, interceptorParameterConfigurations);

          return new Injector<VariablesForMethod>(method, interceptorMethod, interceptorParameterConfigurations, weavingPreconditionInjector);
      }
   }
}
