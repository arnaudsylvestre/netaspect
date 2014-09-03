using System;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.ATrier;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving
{
   public class AroundMethodWeaverFactory
   {
      private readonly IInterceptorAroundMethodBuilder builder;
      private readonly IWevingPreconditionInjector weavingPreconditionInjector;

      public AroundMethodWeaverFactory(IInterceptorAroundMethodBuilder builder, IWevingPreconditionInjector weavingPreconditionInjector)
      {
         this.builder = builder;
         this.weavingPreconditionInjector = weavingPreconditionInjector;
      }

      public IIlInjector Create(MethodDefinition method, MethodInfo interceptorMethod, Action<MethodWeavingInfo, InterceptorParameterConfigurations> fillSpecific)
      {
         if (interceptorMethod == null)
            return new NoIIlInjector();

         var weavingInfo_P = new MethodWeavingInfo
         {
            Interceptor = interceptorMethod,
            Method = method,
         };
         var interceptorParameterConfigurations = new InterceptorParameterConfigurations();
         builder.FillCommon(weavingInfo_P, interceptorParameterConfigurations);
         fillSpecific(weavingInfo_P, interceptorParameterConfigurations);

         return new Injector(method, interceptorMethod, interceptorParameterConfigurations, weavingPreconditionInjector);
      }

      public IIlInjector CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod)
      {
         return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => builder.FillBeforeSpecific(info, interceptorParameterConfigurations));
      }

      public IIlInjector CreateForOnFinally(MethodDefinition method,
         MethodInfo interceptorMethod)
      {
         return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => builder.FillOnFinallySpecific(info, interceptorParameterConfigurations));
      }

      public IIlInjector CreateForAfter(MethodDefinition method, MethodInfo interceptorMethod)
      {
         return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => builder.FillAfterSpecific(info, interceptorParameterConfigurations));
      }

      public IIlInjector CreateForExceptions(MethodDefinition method, MethodInfo interceptorMethod)
      {
         return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => builder.FillOnExceptionSpecific(info, interceptorParameterConfigurations));
      }
   }
}
