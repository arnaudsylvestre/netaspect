using System;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Checkers.CallWeaving.Called;
using NetAspect.Weaver.Core.Weaver.Checkers.MethodWeaving.Instance;
using NetAspect.Weaver.Core.Weaver.Checkers.MethodWeaving.Parameters;
using NetAspect.Weaver.Core.Weaver.Detectors.Helpers;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Engine.Instructions;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Call;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Field
{
    public class InterceptorInfo
    {
        public ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction> Generator { get; set; }
        public Instruction Instruction { get; set; }
        public MethodDefinition Method { get; set; }
        public MethodInfo Interceptor { get; set; }
    }

    public interface IInterceptorAroundInstructionFactory
    {
        void FillCommon(InterceptorInfo info);
        void FillBeforeSpecific(InterceptorInfo info);
        void FillAfterSpecific(InterceptorInfo info);
    }

    public static class InterceptorInfoExtensions
    {
        public static FieldDefinition GetOperandAsField(this InterceptorInfo interceptor)
        {
            return (interceptor.Instruction.Operand as FieldReference).Resolve();
        }
    }

    public class CallGetFieldInterceptorAroundInstructionFactory : IInterceptorAroundInstructionFactory
    {


        public void FillCommon(InterceptorInfo info)
        {
            info.Generator.Add("called", 
                new CalledInterceptorParametersIlGenerator(),
                new CalledInterceptorParametersChercker(info.GetOperandAsField()));
            info.Generator.Add("caller", 
                new InstanceInterceptorParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>(), 
                new InstanceInterceptorParametersChercker(info.Method));
            info.Generator.Add("callerparameters", 
                new ParametersInterceptorParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>(),
                new ParametersInterceptorParametersChercker());
        }

        public void FillBeforeSpecific(InterceptorInfo info)
        {
            throw new NotImplementedException();
        }

        public void FillAfterSpecific(InterceptorInfo info)
        {
            throw new NotImplementedException();
        }
    }

    public class AroundInstructionWeaverFactory<TOperandType>
        where TOperandType : class
    {
        
        private Func<object, TOperandType> operandTypeConverter;

        public IIlInjector<IlInjectorAvailableVariablesForInstruction> CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod, NetAspectDefinition aspect, Instruction instruction)
        {
            var calledField = operandTypeConverter(instruction.Operand);
            var checker = new ParametersChecker();
            FillCommon(method, checker, calledField, instruction);


            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>();
            FillCommon(method, parametersIlGenerator, instruction);

            return new MethodWeavingBeforeMethodInjector<IlInjectorAvailableVariablesForInstruction>(method, interceptorMethod, checker,
                                                                                       parametersIlGenerator, aspect);
        }
        private void FillCommon(MethodDefinition method,
                                       ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction> parametersIlGenerator, Instruction instruction)
        {
            //parametersIlGenerator.CreateIlGeneratorForCalledParameter();
            //parametersIlGenerator.CreateIlGeneratorForCallerParameter();
            parametersIlGenerator.CreateIlGeneratorForCallerParameters();
            parametersIlGenerator.CreateIlGeneratorForCallerParametersName(method);
            parametersIlGenerator.CreateIlGeneratorForColumnNumber(instruction);
            parametersIlGenerator.CreateIlGeneratorForLineNumber(instruction);
            parametersIlGenerator.CreateIlGeneratorForFilename(instruction);
            parametersIlGenerator.CreateIlGeneratorForFilePath(instruction);
            parametersIlGenerator.CreateIlGeneratorForField(instruction, method.Module);
            //parametersIlGenerator.CreateIlGeneratorForMethodParameter();
            //parametersIlGenerator.CreateIlGeneratorForParametersParameter(method);
            //parametersIlGenerator.CreateIlGeneratorForParameterNameParameter(method);
        }

        private static void FillCommon(MethodDefinition method, ParametersChecker checker, TOperandType calledField, Instruction instruction)
        {
            //checker.CreateCheckerForCallerParameter(method);
            //checker.CreateCheckerForCalledParameter(calledField);
            //checker.CreateCheckerForCallerParameter(method);
            checker.CreateCheckerForCallerParameters(method);
            checker.CreateCheckerForCallerParametersName(method);
            checker.CreateCheckerForColumnNumberParameter(instruction);
            checker.CreateCheckerForLineNumberParameter(instruction);
            checker.CreateCheckerForFilenameParameter(instruction);
            checker.CreateCheckerForFilePathParameter(instruction);
            checker.CreateCheckerForField();
            //checker.CreateCheckerForParameterNameParameter(method);
            //checker.CreateCheckerForParametersParameter();
        }

        public static IIlInjector<IlInjectorAvailableVariablesForInstruction> CreateForAfter(MethodDefinition method,
                                                                               MethodInfo interceptorMethod,
                                                                               NetAspectDefinition aspect, Instruction instruction)
        {
            var calledField = (instruction.Operand as FieldReference).Resolve();
            var checker = new ParametersChecker();
            FillCommon(method, checker, calledField, instruction);
            //checker.CreateCheckerForResultParameter(method);


            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>();
            FillCommon(method, parametersIlGenerator, instruction);
            //parametersIlGenerator.CreateIlGeneratorForResultParameter();
            return new MethodWeavingBeforeMethodInjector<IlInjectorAvailableVariablesForInstruction>(method, interceptorMethod, checker,
                                                                                       parametersIlGenerator, aspect);
        }
    }

    public class CallGetFieldInstructionWeavingDetector : ICallWeavingDetector
    {
        private static bool IsFieldCall(Instruction instruction, NetAspectDefinition aspect, MethodDefinition method)
        {
            if (instruction.OpCode == OpCodes.Ldfld || instruction.OpCode == OpCodes.Ldflda ||
                instruction.OpCode == OpCodes.Ldsfld ||
                instruction.OpCode == OpCodes.Ldsflda)
            {
                var fieldReference = instruction.Operand as FieldReference;

                return AspectApplier.CanApply(fieldReference.Resolve(), aspect);
            }
            return false;
        }

        public IAroundInstructionWeaver DetectWeavingModel(MethodDefinition method, Instruction instruction, NetAspectDefinition aspect)
        {
            if (!IsFieldCall(instruction, aspect, method))
                return null;
            IIlInjector<IlInjectorAvailableVariablesForInstruction> before = new NoIIlInjector<IlInjectorAvailableVariablesForInstruction>();
            IIlInjector<IlInjectorAvailableVariablesForInstruction> after = new NoIIlInjector<IlInjectorAvailableVariablesForInstruction>();

            var beforCallInterceptorMethod = aspect.BeforeGetField.Method;
            if (beforCallInterceptorMethod != null)
            {
                before = CallWeavingFieldInjectorFactory.CreateForBefore(method, beforCallInterceptorMethod, aspect, instruction);
            }
            var afterCallInterceptorMethod = aspect.AfterGetField.Method;
            if (afterCallInterceptorMethod != null)
            {
                after = CallWeavingFieldInjectorFactory.CreateForAfter(method, afterCallInterceptorMethod, aspect, instruction);
            }

            return new AroundInstructionWeaver(before, after);
        }
    }
}