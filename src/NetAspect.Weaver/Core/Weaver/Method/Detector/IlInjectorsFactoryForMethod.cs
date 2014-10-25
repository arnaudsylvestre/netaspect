using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
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
       
      public IIlInjector<VariablesForMethod> CreateForBefore(MethodDefinition method, Interceptors interceptorMethod)
      {
          return Create(method, interceptorMethod, NoSpecific);
      }

      public IIlInjector<VariablesForMethod> CreateForOnFinally(MethodDefinition method, Interceptors interceptorMethod)
      {
          return Create(method, interceptorMethod, NoSpecific);
      }

      public IIlInjector<VariablesForMethod> CreateForAfter(MethodDefinition method, Interceptors interceptorMethod)
      {
         return Create(method, interceptorMethod, builder.FillAfterSpecific);
      }

      public IIlInjector<VariablesForMethod> CreateForExceptions(MethodDefinition method, Interceptors interceptorMethod)
      {
         return Create(method, interceptorMethod, builder.FillOnExceptionSpecific);
      }

      private IIlInjector<VariablesForMethod> Create(MethodDefinition method, Interceptors interceptorMethod, Action<CommonWeavingInfo, InterceptorParameterPossibilities<VariablesForMethod>> fillSpecific)
      {
          if (!interceptorMethod.Methods.Any())
              return new NoIIlInjector<VariablesForMethod>();

          var weavingInfo_P = new CommonWeavingInfo
          {
              Interceptor = new List<MethodInfo>(interceptorMethod.Methods),
              Method = method,
          };
          var interceptorParameterConfigurations = new InterceptorParameterPossibilities<VariablesForMethod>();
          builder.FillCommon(weavingInfo_P, interceptorParameterConfigurations);
          fillSpecific(weavingInfo_P, interceptorParameterConfigurations);

          return new Injector<VariablesForMethod>(method, weavingInfo_P.Interceptor, interceptorParameterConfigurations, weavingPreconditionInjector);
      }
   }
}
