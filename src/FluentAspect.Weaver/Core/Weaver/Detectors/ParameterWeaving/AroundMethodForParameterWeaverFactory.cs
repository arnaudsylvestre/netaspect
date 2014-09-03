using System;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.ParameterWeaving
{
   public class AroundMethodForParameterWeaverFactory
   {
      private readonly IInterceptorAroundMethodForParameterBuilder builder;
      private readonly IWevingPreconditionInjector weavingPreconditionInjector;

      public AroundMethodForParameterWeaverFactory(IInterceptorAroundMethodForParameterBuilder builder, IWevingPreconditionInjector weavingPreconditionInjector)
      {
         this.builder = builder;
         this.weavingPreconditionInjector = weavingPreconditionInjector;
      }

      public IIlInjector Create(MethodDefinition method, MethodInfo interceptorMethod, Action<ParameterWeavingInfo, InterceptorParameterConfigurations> fillSpecific, ParameterDefinition parameter)
      {
         if (interceptorMethod == null)
            return new NoIIlInjector();

         var weavingInfo_P = new ParameterWeavingInfo
         {
            Interceptor = interceptorMethod,
            Method = method,
            Parameter = parameter,
         };
         var interceptorParameterConfigurations = new InterceptorParameterConfigurations();
         builder.FillCommon(weavingInfo_P, interceptorParameterConfigurations);
         fillSpecific(weavingInfo_P, interceptorParameterConfigurations);

         return new Injector(method, interceptorMethod, interceptorParameterConfigurations, weavingPreconditionInjector);
      }

      public IIlInjector CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod, ParameterDefinition parameter)
      {
         return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => builder.FillBeforeSpecific(info, interceptorParameterConfigurations), parameter);
      }

      public IIlInjector CreateForOnFinally(MethodDefinition method, MethodInfo interceptorMethod, ParameterDefinition parameter)
      {
         return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => builder.FillOnFinallySpecific(info, interceptorParameterConfigurations), parameter);
      }

      public IIlInjector CreateForAfter(MethodDefinition method, MethodInfo interceptorMethod, ParameterDefinition parameter)
      {
         return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => builder.FillAfterSpecific(info, interceptorParameterConfigurations), parameter);
      }

      public IIlInjector CreateForExceptions(MethodDefinition method, MethodInfo interceptorMethod, ParameterDefinition TODO)
      {
         return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => builder.FillOnExceptionSpecific(info, interceptorParameterConfigurations), TODO);
      }
   }
}
