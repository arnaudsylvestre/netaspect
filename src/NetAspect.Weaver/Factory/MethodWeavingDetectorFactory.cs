﻿using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver;
using NetAspect.Weaver.Core.Weaver.Method.Detector;
using NetAspect.Weaver.Core.Weaver.Parameters.Detector;
using NetAspect.Weaver.Core.Weaver.Session;
using NetAspect.Weaver.Core.Weaver.ToSort.ILInjector;
using NetAspect.Weaver.Factory.Configuration;
using NetAspect.Weaver.Factory.Configuration.Method;
using NetAspect.Weaver.Factory.Configuration.Parameters;
using NetAspect.Weaver.Factory.Tools;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Factory
{
    public static class MethodWeavingDetectorFactory
    {
        public static IMethodAspectInstanceDetector BuildMethodDetector()
        {
            return new MethodAspectInstanceDetector<MethodDefinition>(
               aspect => aspect.AfterMethod,
               new IlInjectorsFactoryForMethod(new MethodInterceptorParameterConfigurationForMethodFiller(), new NoWeavingPreconditionInjector()),
               aspect => aspect.BeforeMethod,
               MethodCompliance.IsMethod,
               m => m,
               aspect => aspect.OnExceptionMethod,
               aspect => aspect.OnFinallyMethod,
               aspect => aspect.MethodSelector
               );
        }

        public static IMethodAspectInstanceDetector BuildMethodParameterDetector()
        {
            return new ParameterAspectInstanceDetector(
               aspect => aspect.AfterMethodForParameter,
               new IlInjectorsFactoryForParameter(new MethodParameterInterceptorParameterConfigurationForMethodFiller(), new NoWeavingPreconditionInjector()),
               aspect => aspect.BeforeMethodForParameter,
               MethodCompliance.IsMethodParameter,
               aspect => aspect.OnExceptionMethodForParameter,
               aspect => aspect.OnFinallyMethodForParameter,
               aspect => aspect.ParameterSelector
               );
        }

        public static IMethodAspectInstanceDetector BuildConstructorParameterDetector()
        {
            return new ParameterAspectInstanceDetector(
               aspect => aspect.AfterConstructorForParameter,
               new IlInjectorsFactoryForParameter(new MethodParameterInterceptorParameterConfigurationForConstructorFiller(), new NoWeavingPreconditionInjector()),
               aspect => aspect.BeforeConstructorForParameter,
               MethodCompliance.IsConstructorForParameter,
               aspect => aspect.OnExceptionConstructorForParameter,
               aspect => aspect.OnFinallyConstructorForParameter,
               aspect => aspect.ParameterSelector
               );
        }

        public static IMethodAspectInstanceDetector BuildConstructorDetector()
        {
            return new MethodAspectInstanceDetector<MethodDefinition>(
               aspect => aspect.AfterConstructor,
               new IlInjectorsFactoryForMethod(new ConstructorInterceptorParameterConfigurationForMethodFiller(), new NoWeavingPreconditionInjector()),
               aspect => aspect.BeforeConstructor,
               MethodCompliance.IsConstructor,
               m => m,
               aspect => aspect.OnExceptionConstructor,
               aspect => aspect.OnFinallyConstructor,
               aspect => aspect.ConstructorSelector
               );
        }

        public static IMethodAspectInstanceDetector BuildPropertyGetterDetector()
        {
            return new MethodAspectInstanceDetector<PropertyDefinition>(
               aspect => aspect.AfterPropertyGetMethod,
               new IlInjectorsFactoryForMethod(new PropertyGetterInterceptorParameterConfigurationForMethodFiller(), new NoWeavingPreconditionInjector()),
               aspect => aspect.BeforePropertyGetMethod,
               MethodCompliance.IsPropertyGetterMethod,
               m => m.GetPropertyForGetter(),
               aspect => aspect.OnExceptionPropertyGetMethod,
               aspect => aspect.OnFinallyPropertyGetMethod,
               aspect => aspect.PropertySelector
               );
        }

        public static IMethodAspectInstanceDetector BuildPropertyUpdaterDetector()
        {
            return new MethodAspectInstanceDetector<PropertyDefinition>(
               aspect => aspect.AfterPropertySetMethod,
               new IlInjectorsFactoryForMethod(new PropertySetterInterceptorParameterConfigurationForMethodFiller(), new NoWeavingPreconditionInjector()),
               aspect => aspect.BeforePropertySetMethod,
               MethodCompliance.IsPropertySetterMethod,
               m => m.GetPropertyForSetter(),
               aspect => aspect.OnExceptionPropertySetMethod,
               aspect => aspect.OnFinallyPropertySetMethod,
               aspect => aspect.PropertySelector
               );
        }
    }
}