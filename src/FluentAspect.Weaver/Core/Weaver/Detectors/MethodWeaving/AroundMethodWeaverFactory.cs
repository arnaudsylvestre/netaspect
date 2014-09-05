using System;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving
{
   public class AroundMethodWeaverFactory
   {
      private readonly IInterceptorAroundMethodBuilder builder;
      private readonly IWevingPreconditionInjector<VariablesForMethod> weavingPreconditionInjector;

      public AroundMethodWeaverFactory(IInterceptorAroundMethodBuilder builder, IWevingPreconditionInjector<VariablesForMethod> weavingPreconditionInjector)
      {
         this.builder = builder;
         this.weavingPreconditionInjector = weavingPreconditionInjector;
      }

      public IIlInjector<VariablesForMethod> Create(MethodDefinition method, MethodInfo interceptorMethod, Action<MethodWeavingInfo, InterceptorParameterConfigurations<VariablesForMethod>> fillSpecific)
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

      public IIlInjector<VariablesForMethod> CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod)
      {
         return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => builder.FillBeforeSpecific(info, interceptorParameterConfigurations));
      }

      public IIlInjector<VariablesForMethod> CreateForOnFinally(MethodDefinition method,
         MethodInfo interceptorMethod)
      {
         return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => builder.FillOnFinallySpecific(info, interceptorParameterConfigurations));
      }

      public IIlInjector<VariablesForMethod> CreateForAfter(MethodDefinition method, MethodInfo interceptorMethod)
      {
         return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => builder.FillAfterSpecific(info, interceptorParameterConfigurations));
      }

      public IIlInjector<VariablesForMethod> CreateForExceptions(MethodDefinition method, MethodInfo interceptorMethod)
      {
         return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => builder.FillOnExceptionSpecific(info, interceptorParameterConfigurations));
      }
   }
}
