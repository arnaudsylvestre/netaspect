using System;
using System.Collections.Generic;
using Mono.Cecil;
using NetAspect.Weaver.Apis.AssemblyChecker.Peverify;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Core.Weaver;
using NetAspect.Weaver.Core.Weaver.Checkers.Aspects;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving;
using NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving;
using NetAspect.Weaver.Core.Weaver.Detectors.ParameterWeaving;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Engine.AspectFinders;
using NetAspect.Weaver.Core.Weaver.Engine.AssemblyPoolFactories;
using NetAspect.Weaver.Core.Weaver.Engine.Lifecycle;
using NetAspect.Weaver.Factory.Configuration;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Factory
{
   public static class WeaverFactory
   {
      public static WeaverEngine Create()
      {
         return Create(null);
      }

      public static WeaverEngine Create(Func<TypeDefinition, bool> typesToSave_P)
      {
         var aspectBuilder = new AspectBuilder(
            new Dictionary<LifeCycle, ILifeCycleHandler>
            {
               {LifeCycle.Transient, new TransientLifeCycleHandler()},
               {LifeCycle.PerInstance, new PerInstanceLifeCycleHandler()},
               {LifeCycle.PerType, new PerInstanceLifeCycleHandler {Static = true}},
            });
         return new WeaverEngine(
            new WeavingModelComputer(
               new DefaultAspectFinder(),
               new DefaultAspectChecker(),
               new List<ICallWeavingDetector>
               {
                  BuildCallGetFieldDetector(aspectBuilder),
                  BuildCallUpdateFieldDetector(aspectBuilder),
                  BuildCallMethodDetector(aspectBuilder),
                  BuildCallConstructorDetector(aspectBuilder),
                  BuildCallGetPropertyDetector(aspectBuilder),
                  BuildCallUpdatePropertyDetector(aspectBuilder),
               },
               new List<IMethodWeavingDetector>
               {
                  BuildMethodDetector(aspectBuilder),
                  BuildPropertyGetterDetector(),
                  BuildPropertyUpdaterDetector(),
                  BuildConstructorDetector(),
                  BuildMethodParameterDetector(),
                  BuildConstructorParameterDetector(),
               },
               aspectBuilder),
            new DefaultAssemblyPoolFactory(new PeVerifyAssemblyChecker(), typesToSave_P),
            new ErrorInfoComputer(
               new Dictionary<ErrorCode, ErrorInfo>
               {
                  {ErrorCode.ImpossibleToOutTheParameter, new ErrorInfo("impossible to out the parameter '{0}' in the method {1} of the type '{2}'")},
                  {ErrorCode.ImpossibleToReferenceTheParameter, new ErrorInfo("impossible to ref/out the parameter '{0}' in the method {1} of the type '{2}'")},
                  {ErrorCode.ParameterWithBadType, new ErrorInfo("the {0} parameter in the method {1} of the type '{2}' is declared with the type '{3}' but it is expected to be {4}")},
                  {ErrorCode.SelectorMustBeStatic, new ErrorInfo("The selector {0} in the aspect {1} must be static")},
                  {ErrorCode.NoDebuggingInformationAvailable, new ErrorInfo("The parameter {0} in method {1} of type {2} will have the default value because there is no debugging information")},
                  {
                     ErrorCode.ParameterWithBadTypeBecauseReturnMethod,
                     new ErrorInfo("the {0} parameter in the method {1} of the type '{2}' is declared with the type '{3}' but it is expected to be {4} because the return type of the method {5} in the type {6}")
                  },
                  {ErrorCode.MustNotBeVoid, new ErrorInfo("Impossible to use the {0} parameter in the method {1} of the type '{2}' because the return type of the method {3} in the type {4} is void")},
                  {ErrorCode.ImpossibleToRefGenericParameter, new ErrorInfo("Impossible to ref a generic parameter")},
                  {ErrorCode.ParameterCanNotBeUsedInStaticMethod, new ErrorInfo("the {0} parameter can not be used for static method interceptors")},
                  {ErrorCode.UnknownParameter, new ErrorInfo("The parameter '{0}' is unknown. Expected one of : {1}")},
                  {ErrorCode.NotAvailableInStaticStruct, new ErrorInfo("the {0} parameter in the method {1} of the type '{2}' is not available for static member in struct")},
                  {ErrorCode.NotAvailableInStatic, new ErrorInfo(ErrorLevel.Warning, "the {0} parameter in the method {1} of the type '{2}' is not available for static member : default value will be passed")},
                  {ErrorCode.ParameterAlreadyDeclared, new ErrorInfo("The parameter {0} is already declared")},
                  {ErrorCode.SelectorMustReturnBooleanValue, new ErrorInfo("The selector {0} in the aspect {1} must return boolean value")},
                  {ErrorCode.SelectorBadParameterType, new ErrorInfo("The parameter {0} in the method {1} of the aspect {2} is expected to be {3}")},
                  {ErrorCode.SelectorBadParameterName, new ErrorInfo("The parameter {0} in the method {1} of the aspect {2} is unexpected. {3} can be used")},
                  {ErrorCode.InterceptorMustBeVoid, new ErrorInfo("The {0} interceptor in the {1} aspect must be void")},
                  {ErrorCode.TooManySelectorsWithSameName, new ErrorInfo("Only one {0} can be defined in the aspect {1}")},
                  {ErrorCode.SelectorMustHaveParameters, new ErrorInfo("The selector {0} of the aspect {1} must have the following parameters : {2}")},
                  {ErrorCode.AssemblyGeneratedIsNotCompliant, new ErrorInfo(ErrorLevel.Failure, "An internal error : {0}")}
               }),
               new MethodWeaver(aspectBuilder));
      }

      private static IMethodWeavingDetector BuildMethodDetector(AspectBuilder aspectBuilder)
      {
         return new MethodWeavingDetector<MethodDefinition>(
            aspect => aspect.After,
            new AroundMethodWeaverFactory(new MethodWeavingMethodInjectorFactory(), new NoWevingPreconditionInjector()),
            aspect => aspect.Before,
            MethodCompliance.IsMethod,
            m => m,
            aspect => aspect.OnException,
            aspect => aspect.OnFinally,
            aspect => aspect.MethodSelector
            );
      }

      private static IMethodWeavingDetector BuildMethodParameterDetector()
      {
         return new ParameterWeavingDetector(
            aspect => aspect.AfterMethodForParameter,
            new AroundMethodForParameterWeaverFactory(new MethodWeavingParameterInjectorFactory(), new NoWevingPreconditionInjector()),
            aspect => aspect.BeforeMethodForParameter,
            MethodCompliance.IsMethodParameter,
            aspect => aspect.OnExceptionMethodForParameter,
            aspect => aspect.OnFinallyMethodForParameter,
            aspect => aspect.ParameterSelector
            );
      }

      private static IMethodWeavingDetector BuildConstructorParameterDetector()
      {
         return new ParameterWeavingDetector(
            aspect => aspect.AfterConstructorForParameter,
            new AroundMethodForParameterWeaverFactory(new MethodWeavingParameterInjectorFactory(), new NoWevingPreconditionInjector()),
            aspect => aspect.BeforeConstructorForParameter,
            MethodCompliance.IsConstructorForParameter,
            aspect => aspect.OnExceptionConstructorForParameter,
            aspect => aspect.OnFinallyConstructorForParameter,
            aspect => aspect.ParameterSelector
            );
      }

      private static IMethodWeavingDetector BuildConstructorDetector()
      {
         return new MethodWeavingDetector<MethodDefinition>(
            aspect => aspect.AfterConstructor,
            new AroundMethodWeaverFactory(new ConstructorWeavingMethodInjectorFactory(), new NoWevingPreconditionInjector()),
            aspect => aspect.BeforeConstructor,
            MethodCompliance.IsConstructor,
            m => m,
            aspect => aspect.OnExceptionConstructor,
            aspect => aspect.OnFinallyConstructor,
            aspect => aspect.ConstructorSelector
            );
      }

      private static IMethodWeavingDetector BuildPropertyGetterDetector()
      {
         return new MethodWeavingDetector<PropertyDefinition>(
            aspect => aspect.AfterPropertyGetMethod,
            new AroundMethodWeaverFactory(new PropertyGetterWeavingMethodInjectorFactory(), new NoWevingPreconditionInjector()),
            aspect => aspect.BeforePropertyGetMethod,
            MethodCompliance.IsPropertyGetterMethod,
            m => m.GetPropertyForGetter(),
            aspect => aspect.OnExceptionPropertyGetMethod,
            aspect => aspect.OnFinallyPropertyGetMethod,
            aspect => aspect.PropertySelector
            );
      }

      private static IMethodWeavingDetector BuildPropertyUpdaterDetector()
      {
         return new MethodWeavingDetector<PropertyDefinition>(
            aspect => aspect.AfterPropertySetMethod,
            new AroundMethodWeaverFactory(new PropertySetterWeavingMethodInjectorFactory(), new NoWevingPreconditionInjector()),
            aspect => aspect.BeforePropertySetMethod,
            MethodCompliance.IsPropertySetterMethod,
            m => m.GetPropertyForSetter(),
            aspect => aspect.OnExceptionPropertySetMethod,
            aspect => aspect.OnFinallyPropertySetMethod,
            aspect => aspect.PropertySelector
            );
      }

      private static InstructionWeavingDetector<FieldDefinition> BuildCallGetFieldDetector(AspectBuilder aspectBuilder)
      {
         return new InstructionWeavingDetector<FieldDefinition>(
            InstructionCompliance.IsGetFieldInstruction,
            aspect => aspect.FieldSelector,
            new AroundInstructionWeaverFactory(new CallGetFieldInterceptorAroundInstructionBuilder(), new NoWevingPreconditionInjector()),
            instruction => (instruction.Operand as FieldReference).Resolve(),
            aspect => aspect.BeforeGetField,
            aspect => aspect.AfterGetField,
            aspectBuilder);
      }

      private static InstructionWeavingDetector<FieldDefinition> BuildCallUpdateFieldDetector(AspectBuilder aspectBuilder)
      {
         return new InstructionWeavingDetector<FieldDefinition>(
            InstructionCompliance.IsUpdateFieldInstruction,
            aspect => aspect.FieldSelector,
            new AroundInstructionWeaverFactory(new CallUpdateFieldInterceptorAroundInstructionBuilder(), new NoWevingPreconditionInjector()),
            instruction => (instruction.Operand as FieldReference).Resolve(),
            aspect => aspect.BeforeUpdateField,
            aspect => aspect.AfterUpdateField,
            aspectBuilder);
      }

      private static InstructionWeavingDetector<PropertyDefinition> BuildCallUpdatePropertyDetector(AspectBuilder aspectBuilder)
      {
         return new InstructionWeavingDetector<PropertyDefinition>(
            InstructionCompliance.IsSetPropertyCall,
            aspect => aspect.PropertySelector,
            new AroundInstructionWeaverFactory(new CallSetPropertyInterceptorAroundInstructionBuilder(), new NoWevingPreconditionInjector()),
            instruction => (instruction.Operand as MethodReference).Resolve().GetPropertyForSetter(),
            aspect => aspect.BeforeSetProperty,
            aspect => aspect.AfterSetProperty,
            aspectBuilder);
      }

      private static InstructionWeavingDetector<PropertyDefinition> BuildCallGetPropertyDetector(AspectBuilder aspectBuilder)
      {
         return new InstructionWeavingDetector<PropertyDefinition>(
            InstructionCompliance.IsGetPropertyCall,
            aspect => aspect.PropertySelector,
            new AroundInstructionWeaverFactory(new CallGetPropertyInterceptorAroundInstructionBuilder(), new NoWevingPreconditionInjector()),
            instruction => (instruction.Operand as MethodReference).Resolve().GetPropertyForGetter(),
            aspect => aspect.BeforeGetProperty,
            aspect => aspect.AfterGetProperty,
            aspectBuilder);
      }

      private static InstructionWeavingDetector<MethodDefinition> BuildCallMethodDetector(AspectBuilder aspectBuilder)
      {
         return new InstructionWeavingDetector<MethodDefinition>(
            InstructionCompliance.IsCallMethodInstruction,
            aspect => aspect.MethodSelector,
            new AroundInstructionWeaverFactory(new CallMethodInterceptorAroundInstructionBuilder(), new OverrideWevingPreconditionInjector()),
            instruction => (instruction.Operand as MethodReference).Resolve(),
            aspect => aspect.BeforeCallMethod,
            aspect => aspect.AfterCallMethod,
            aspectBuilder);
      }

      private static InstructionWeavingDetector<MethodDefinition> BuildCallConstructorDetector(AspectBuilder aspectBuilder)
      {
         return new InstructionWeavingDetector<MethodDefinition>(
            InstructionCompliance.IsCallConstructorInstruction,
            aspect => aspect.ConstructorSelector,
            new AroundInstructionWeaverFactory(new CallConstructorInterceptorAroundInstructionBuilder(), new OverrideWevingPreconditionInjector()),
            instruction => (instruction.Operand as MethodReference).Resolve(),
            aspect => aspect.BeforeCallConstructor,
            aspect => aspect.AfterCallConstructor,
            aspectBuilder);
      }
   }
}
