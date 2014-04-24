using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Checkers.CallWeaving.Called;
using NetAspect.Weaver.Core.Weaver.Checkers.CallWeaving.Source;
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

        public static InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> AddPossibleParameter(this InterceptorInfo interceptor,
                                                                            string parameterName)
        {
            var myGenerator = new MyGenerator<IlInjectorAvailableVariablesForInstruction>();
            var checker = new MyInterceptorParameterChecker();
            interceptor.Generator.Add(parameterName, myGenerator, checker);
            return new InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction>(myGenerator, checker, interceptor);
        }

        public class MyInterceptorParameterChecker : IInterceptorParameterChecker
        {
            public List<Action<ParameterInfo, ErrorHandler>> Checkers = new List<Action<ParameterInfo, ErrorHandler>>();

            public void Check(ParameterInfo parameter, ErrorHandler errorListener)
            {
                foreach (var checker in Checkers)
                {
                    checker(parameter, errorListener);
                }
            }
        }


        public class MyGenerator<T> : IInterceptorParameterIlGenerator<T>
        {
            public List<Action<ParameterInfo, List<Instruction>, T>> Generators = new List<Action<ParameterInfo, List<Instruction>, T>>();

            public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, T info)
            {
                foreach (var generator in Generators)
                {
                    generator(parameterInfo, instructions, info);
                }
            }
        }
    }


    public class InterceptorParameterConfigurator<T>
    {
        private readonly InterceptorInfoExtensions.MyGenerator<T> _myGenerator;
        private readonly InterceptorInfoExtensions.MyInterceptorParameterChecker _checker;
        private readonly InterceptorInfo _interceptor;

        private List<string> allowedTypes; 

        public InterceptorParameterConfigurator(InterceptorInfoExtensions.MyGenerator<T> myGenerator, InterceptorInfoExtensions.MyInterceptorParameterChecker checker, InterceptorInfo interceptor)
        {
            _myGenerator = myGenerator;
            _checker = checker;
            _interceptor = interceptor;
        }

        public InterceptorParameterConfigurator<T> WhichCanNotBeReferenced()
        {
            _checker.Checkers.Add((parameter, errorListener) => Ensure.NotReferenced(parameter, errorListener));
            return this;
        }

        public InterceptorParameterConfigurator<T> WhereFieldCanNotBeStatic()
        {
            _checker.Checkers.Add((parameter, errorListener) => Ensure.NotStaticButDefaultValue(parameter, errorListener, _interceptor.GetOperandAsField()));
            return this;
        }

        public InterceptorParameterConfigurator<T> WhichMustBeOfType<T1>()
        {
            allowedTypes.Add(typeof(T1).FullName);
            return this;
        }

        public InterceptorParameterConfigurator<T> OrOfType(TypeReference type)
        {
            allowedTypes.Add(type.FullName);
            return this;
        }

        public InterceptorParameterConfigurator<T> OrOfFieldDeclaringType()
        {
            return OrOfType(_interceptor.GetOperandAsField().DeclaringType);
        }
    }

    public class CallGetFieldInterceptorAroundInstructionFactory : IInterceptorAroundInstructionFactory
    {


        public void FillCommon(InterceptorInfo info)
        {
            info.AddPossibleParameter("called")
                .WhichCanNotBeReferenced()
                .WhereFieldCanNotBeStatic()
                .WhichMustBeOfType<object>().OrOfFieldDeclaringType()
                .;

            info.Generator.Add("called", 
                new CalledInterceptorParametersIlGenerator(),
                new CalledInterceptorParametersChercker(info.GetOperandAsField()));
            info.Generator.Add("caller", 
                new InstanceInterceptorParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>(), 
                new InstanceInterceptorParametersChercker(info.Method));
            info.Generator.Add("callerparameters", 
                new ParametersInterceptorParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>(),
                new ParametersInterceptorParametersChercker());
            foreach (ParameterDefinition parameter in info.Method.Parameters)
            {
                info.Generator.Add("caller" + parameter.Name.ToLower(),
                                 new ParameterNameInterceptorParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>(parameter),
                                 new ParameterNameInterceptorParametersChercker(parameter));
            }
            info.Generator.Add("columnnumber",
                new SequencePointIntInterceptorParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>(info.Instruction, point => point.StartColumn),
                new ColumnNumberInterceptorParametersChercker(info.Instruction));

            info.Generator.Add("linenumber",
                new SequencePointIntInterceptorParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>(info.Instruction, point => point.StartLine),
                new ColumnNumberInterceptorParametersChercker(info.Instruction));

            info.Generator.Add("filename", new SequencePointStringInterceptorParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>(info.Instruction, i => Path.GetFileName(i.Document.Url)),
                new FilenameInterceptorParametersChercker(info.Instruction));

            info.Generator.Add("filepath", new SequencePointStringInterceptorParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>(info.Instruction, i => i.Document.Url),
                new FilenameInterceptorParametersChercker(info.Instruction));
        }

        public void FillBeforeSpecific(InterceptorInfo info)
        {
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
            //parametersIlGenerator.CreateIlGeneratorForCallerParameters();
            //parametersIlGenerator.CreateIlGeneratorForCallerParametersName(method);
            //parametersIlGenerator.CreateIlGeneratorForColumnNumber(instruction);
            //parametersIlGenerator.CreateIlGeneratorForLineNumber(instruction);
            //parametersIlGenerator.CreateIlGeneratorForFilename(instruction);
            //parametersIlGenerator.CreateIlGeneratorForFilePath(instruction);
            parametersIlGenerator.CreateIlGeneratorForField(instruction, method.Module);
        }

        private static void FillCommon(MethodDefinition method, ParametersChecker checker, TOperandType calledField, Instruction instruction)
        {
            //checker.CreateCheckerForCalledParameter(calledField);
            //checker.CreateCheckerForCallerParameter(method);
            //checker.CreateCheckerForCallerParameters(method);
            //checker.CreateCheckerForCallerParametersName(method);
            checker.CreateCheckerForColumnNumberParameter(instruction);
            checker.CreateCheckerForLineNumberParameter(instruction);
            checker.CreateCheckerForFilenameParameter(instruction);
            checker.CreateCheckerForFilePathParameter(instruction);
            checker.CreateCheckerForField();
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