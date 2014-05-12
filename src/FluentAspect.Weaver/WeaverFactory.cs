﻿using System;
using System.Collections.Generic;
using Mono.Cecil;
using NetAspect.Weaver.Apis.AssemblyChecker.Peverify;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Core.Weaver;
using NetAspect.Weaver.Core.Weaver.Detectors;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Kinds;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Engine.AspectCheckers;
using NetAspect.Weaver.Core.Weaver.Engine.AspectFinders;
using NetAspect.Weaver.Core.Weaver.Engine.AssemblyPoolFactories;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver
{
    public static class WeaverFactory
   {
       public static WeaverEngine Create()
       {
          return Create(null);
       }
       public static WeaverEngine Create(Func<TypeDefinition, bool> typesToSave_P)
       {

       AspectBuilder aspectBuilder = new AspectBuilder(new Dictionary<LifeCycle, ILifeCycleHandler>()
         {
            {LifeCycle.Transient, new TransientLifeCycleHandler()}
         });
         return new WeaverEngine(
            new WeavingModelComputer(new DefaultAspectFinder(),
                     new DefaultAspectChecker(),
                     new List<ICallWeavingDetector>
                     {
                          BuildCallGetFieldDetector(aspectBuilder),
                          BuildCallUpdateFieldDetector(aspectBuilder),
                          BuildCallMethodDetector(aspectBuilder),
                          BuildCallGetPropertyDetector(aspectBuilder),
                          BuildCallUpdatePropertyDetector(aspectBuilder),
                     },
                     new List<IMethodWeavingDetector>
                     {
                             
                     }),
            new DefaultAssemblyPoolFactory(new PeVerifyAssemblyChecker(), typesToSave_P),
            new ErrorInfoComputer(new Dictionary<ErrorCode, ErrorInfo>()
                {
                    {ErrorCode.ImpossibleToOutTheParameter, new ErrorInfo("impossible to out the parameter '{0}' in the method {1} of the type '{2}'")},
                    {ErrorCode.ImpossibleToReferenceTheParameter, new ErrorInfo("impossible to ref/out the parameter '{0}' in the method {1} of the type '{2}'")},
                    {ErrorCode.ParameterWithBadType, new ErrorInfo("the {0} parameter in the method {1} of the type '{2}' is declared with the type '{3}' but it is expected to be {4} because of the type of this parameter in the method {5} of the type {6}")},
                    {ErrorCode.SelectorMustBeStatic, new ErrorInfo("The selector {0} in the aspect {1} must be static")},
                    {ErrorCode.NoDebuggingInformationAvailable, new ErrorInfo("The parameter {0} in method {1} of type {2} will have the default value because there is no debugging information")},
                    {ErrorCode.ParameterWithBadTypeBecauseReturnMethod, new ErrorInfo("the {0} parameter in the method {1} of the type '{2}' is declared with the type '{3}' but it is expected to be {4} because the return type of the method {5} in the type {6}")},
                    {ErrorCode.MustNotBeVoid, new ErrorInfo("Impossible to use the {0} parameter in the method {1} of the type '{2}' because the return type of the method {3} in the type {4} is void")},
                    {ErrorCode.ImpossibleToRefGenericParameter, new ErrorInfo("Impossible to ref a generic parameter")},
                    {ErrorCode.ParameterCanNotBeUsedInStaticMethod, new ErrorInfo("the {0} parameter can not be used for static method interceptors")},
                    {ErrorCode.UnknownParameter, new ErrorInfo("The parameter '{0}' is unknown")},
                    {ErrorCode.NotAvailableInStaticStruct, new ErrorInfo("the {0} parameter in the method {1} of the type '{2}' is not available for static field in struct")},
                    {ErrorCode.NotAvailableInStatic, new ErrorInfo(ErrorLevel.Warning, "the {0} parameter in the method {1} of the type '{2}' is not available for static field : default value will be passed")},
                    {ErrorCode.ParameterAlreadyDeclared, new ErrorInfo("The parameter {0} is already declared")},
                    {ErrorCode.SelectorMustReturnBooleanValue, new ErrorInfo("The selector {0} in the aspect {1} must return boolean value")},
                    {ErrorCode.SelectorBadParameterType, new ErrorInfo("The parameter {0} in the method {1} of the aspect {2} is expected to be {3}")},
                }));
      }

       private static InstructionWeavingDetector<FieldDefinition> BuildCallGetFieldDetector(AspectBuilder aspectBuilder)
      {
          return new InstructionWeavingDetector<FieldDefinition>(
              InstructionCompliance.IsGetFieldInstruction,
              aspect => aspect.FieldSelector,
              new AroundInstructionWeaverFactory(new CallGetFieldInterceptorAroundInstructionBuilder(), aspectBuilder),
              instruction => (instruction.Operand as FieldReference).Resolve(),
              aspect => aspect.BeforeGetField,
              aspect => aspect.AfterGetField);
      }

      private static InstructionWeavingDetector<FieldDefinition> BuildCallUpdateFieldDetector(AspectBuilder aspectBuilder)
      {
          return new InstructionWeavingDetector<FieldDefinition>(
              InstructionCompliance.IsUpdateFieldInstruction,
              aspect => aspect.FieldSelector,
              new AroundInstructionWeaverFactory(new CallUpdateFieldInterceptorAroundInstructionBuilder(), aspectBuilder),
              instruction => (instruction.Operand as FieldReference).Resolve(),
              aspect => aspect.BeforeUpdateField,
              aspect => aspect.AfterUpdateField);
      }
      private static InstructionWeavingDetector<PropertyDefinition> BuildCallUpdatePropertyDetector(AspectBuilder aspectBuilder)
      {
          return new InstructionWeavingDetector<PropertyDefinition>(
              InstructionCompliance.IsSetPropertyCall,
              aspect => aspect.PropertySelector,
              new AroundInstructionWeaverFactory(new CallSetPropertyInterceptorAroundInstructionBuilder(), aspectBuilder),
              instruction => (instruction.Operand as MethodReference).Resolve().GetPropertyForGetter(),
              aspect => aspect.BeforeSetProperty,
              aspect => aspect.AfterSetProperty);
      }
      private static InstructionWeavingDetector<PropertyDefinition> BuildCallGetPropertyDetector(AspectBuilder aspectBuilder)
      {
          return new InstructionWeavingDetector<PropertyDefinition>(
              InstructionCompliance.IsGetPropertyCall,
              aspect => aspect.PropertySelector,
              new AroundInstructionWeaverFactory(new CallGetPropertyInterceptorAroundInstructionBuilder(), aspectBuilder),
              instruction => (instruction.Operand as MethodReference).Resolve().GetPropertyForGetter(),
              aspect => aspect.BeforeGetProperty,
              aspect => aspect.AfterGetProperty);
      }

      private static InstructionWeavingDetector<MethodDefinition> BuildCallMethodDetector(AspectBuilder aspectBuilder)
      {
          return new InstructionWeavingDetector<MethodDefinition>(
              InstructionCompliance.IsCallMethodInstruction,
              aspect => aspect.MethodSelector,
              new AroundInstructionWeaverFactory(new CallMethodInterceptorAroundInstructionBuilder(), aspectBuilder),
              instruction => (instruction.Operand as MethodReference).Resolve(),
              aspect => aspect.BeforeCallMethod,
              aspect => aspect.AfterCallMethod);
      }


   }
}
