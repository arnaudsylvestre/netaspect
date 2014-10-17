using System;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.ParameterWeaving
{
   public class IlInjectorsFactoryForParameter
   {
       private readonly IInterceptorParameterConfigurationForParameterFiller _filler;
      private readonly IWevingPreconditionInjector<VariablesForMethod> weavingPreconditionInjector;

      public IlInjectorsFactoryForParameter(IInterceptorParameterConfigurationForParameterFiller _filler, IWevingPreconditionInjector<VariablesForMethod> weavingPreconditionInjector)
      {
         this._filler = _filler;
         this.weavingPreconditionInjector = weavingPreconditionInjector;
      }

      public IIlInjector<VariablesForMethod> Create(MethodDefinition method, MethodInfo interceptorMethod, Action<ParameterWeavingInfo, InterceptorParameterConfigurations<VariablesForMethod>> fillSpecific, ParameterDefinition parameter)
      {
         if (interceptorMethod == null)
             return new NoIIlInjector<VariablesForMethod>();

         var weavingInfo_P = new ParameterWeavingInfo
         {
            Interceptor = interceptorMethod,
            Method = method,
            Parameter = parameter,
         };
         var interceptorParameterConfigurations = new InterceptorParameterConfigurations<VariablesForMethod>();
         _filler.FillCommon(weavingInfo_P, interceptorParameterConfigurations);
         fillSpecific(weavingInfo_P, interceptorParameterConfigurations);

         return new Injector<VariablesForMethod>(method, interceptorMethod, interceptorParameterConfigurations, weavingPreconditionInjector);
      }

      public IIlInjector<VariablesForMethod> CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod, ParameterDefinition parameter)
      {
         return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => {}, parameter);
      }

      public IIlInjector<VariablesForMethod> CreateForOnFinally(MethodDefinition method, MethodInfo interceptorMethod, ParameterDefinition parameter)
      {
          return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => { }, parameter);
      }

      public IIlInjector<VariablesForMethod> CreateForAfter(MethodDefinition method, MethodInfo interceptorMethod, ParameterDefinition parameter)
      {
          return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => { }, parameter);
      }

      public IIlInjector<VariablesForMethod> CreateForExceptions(MethodDefinition method, MethodInfo interceptorMethod, ParameterDefinition parameter)
      {
         return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => _filler.FillOnExceptionSpecific(info, interceptorParameterConfigurations), parameter);
      }
   }
}
