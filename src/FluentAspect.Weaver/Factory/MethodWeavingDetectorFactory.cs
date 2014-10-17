using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver;
using NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving;
using NetAspect.Weaver.Core.Weaver.Detectors.ParameterWeaving;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Factory.Configuration;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Factory
{
    public static class MethodWeavingDetectorFactory
    {
        public static IMethodWeavingDetector BuildMethodDetector()
        {
            return new MethodWeavingDetector<MethodDefinition>(
               aspect => aspect.After,
               new AroundMethodWeaverFactory(new MethodWeavingMethodInjectorFactory(), new NoWeavingPreconditionInjector()),
               aspect => aspect.Before,
               MethodCompliance.IsMethod,
               m => m,
               aspect => aspect.OnException,
               aspect => aspect.OnFinally,
               aspect => aspect.MethodSelector
               );
        }

        public static IMethodWeavingDetector BuildMethodParameterDetector()
        {
            return new ParameterWeavingDetector(
               aspect => aspect.AfterMethodForParameter,
               new AroundMethodForParameterWeaverFactory(new MethodWeavingParameterInjectorFactory(), new NoWeavingPreconditionInjector()),
               aspect => aspect.BeforeMethodForParameter,
               MethodCompliance.IsMethodParameter,
               aspect => aspect.OnExceptionMethodForParameter,
               aspect => aspect.OnFinallyMethodForParameter,
               aspect => aspect.ParameterSelector
               );
        }

        public static IMethodWeavingDetector BuildConstructorParameterDetector()
        {
            return new ParameterWeavingDetector(
               aspect => aspect.AfterConstructorForParameter,
               new AroundMethodForParameterWeaverFactory(new MethodWeavingParameterInjectorFactory(), new NoWeavingPreconditionInjector()),
               aspect => aspect.BeforeConstructorForParameter,
               MethodCompliance.IsConstructorForParameter,
               aspect => aspect.OnExceptionConstructorForParameter,
               aspect => aspect.OnFinallyConstructorForParameter,
               aspect => aspect.ParameterSelector
               );
        }

        public static IMethodWeavingDetector BuildConstructorDetector()
        {
            return new MethodWeavingDetector<MethodDefinition>(
               aspect => aspect.AfterConstructor,
               new AroundMethodWeaverFactory(new ConstructorWeavingMethodInjectorFactory(), new NoWeavingPreconditionInjector()),
               aspect => aspect.BeforeConstructor,
               MethodCompliance.IsConstructor,
               m => m,
               aspect => aspect.OnExceptionConstructor,
               aspect => aspect.OnFinallyConstructor,
               aspect => aspect.ConstructorSelector
               );
        }

        public static IMethodWeavingDetector BuildPropertyGetterDetector()
        {
            return new MethodWeavingDetector<PropertyDefinition>(
               aspect => aspect.AfterPropertyGetMethod,
               new AroundMethodWeaverFactory(new PropertyGetterWeavingMethodInjectorFactory(), new NoWeavingPreconditionInjector()),
               aspect => aspect.BeforePropertyGetMethod,
               MethodCompliance.IsPropertyGetterMethod,
               m => m.GetPropertyForGetter(),
               aspect => aspect.OnExceptionPropertyGetMethod,
               aspect => aspect.OnFinallyPropertyGetMethod,
               aspect => aspect.PropertySelector
               );
        }

        public static IMethodWeavingDetector BuildPropertyUpdaterDetector()
        {
            return new MethodWeavingDetector<PropertyDefinition>(
               aspect => aspect.AfterPropertySetMethod,
               new AroundMethodWeaverFactory(new PropertySetterWeavingMethodInjectorFactory(), new NoWeavingPreconditionInjector()),
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