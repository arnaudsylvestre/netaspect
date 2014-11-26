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

namespace NetAspect.Weaver.Core.Weaver.Parameters.Detector
{
   public class IlInjectorsFactoryForParameter
   {
       private readonly IInterceptorParameterConfigurationForParameterFiller _filler;
      private readonly IWeavingPreconditionInjector<VariablesForMethod> weavingPreconditionInjector;

      public IlInjectorsFactoryForParameter(IInterceptorParameterConfigurationForParameterFiller _filler, IWeavingPreconditionInjector<VariablesForMethod> weavingPreconditionInjector)
      {
         this._filler = _filler;
         this.weavingPreconditionInjector = weavingPreconditionInjector;
      }

      public IIlInjector<VariablesForMethod> Create(MethodDefinition method, Interceptors interceptorMethod, Action<ParameterWeavingInfo, InterceptorParameterPossibilities<VariablesForMethod>> fillSpecific, ParameterDefinition parameter)
      {
         if (!interceptorMethod.Methods.Any())
             return new NoIIlInjector<VariablesForMethod>();

         var weavingInfo_P = new ParameterWeavingInfo
         {
            Interceptor = interceptorMethod.Methods,
            Method = method,
            Parameter = parameter,
         };
         var interceptorParameterConfigurations = new InterceptorParameterPossibilities<VariablesForMethod>();
         _filler.FillCommon(weavingInfo_P, interceptorParameterConfigurations);
         fillSpecific(weavingInfo_P, interceptorParameterConfigurations);

         return new Injector<VariablesForMethod>(method, weavingInfo_P.Interceptor, interceptorParameterConfigurations, weavingPreconditionInjector);
      }

      public IIlInjector<VariablesForMethod> CreateForBefore(MethodDefinition method, Interceptors interceptorMethod, ParameterDefinition parameter)
      {
         return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => {}, parameter);
      }

      public IIlInjector<VariablesForMethod> CreateForOnFinally(MethodDefinition method, Interceptors interceptorMethod, ParameterDefinition parameter)
      {
          return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => { }, parameter);
      }

      public IIlInjector<VariablesForMethod> CreateForAfter(MethodDefinition method, Interceptors interceptorMethod, ParameterDefinition parameter)
      {
          return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => { }, parameter);
      }

      public IIlInjector<VariablesForMethod> CreateForExceptions(MethodDefinition method, Interceptors interceptorMethod, ParameterDefinition parameter)
      {
          return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => _filler.FillOnExceptionSpecific(info, interceptorParameterConfigurations), parameter);
      }
   }
}
