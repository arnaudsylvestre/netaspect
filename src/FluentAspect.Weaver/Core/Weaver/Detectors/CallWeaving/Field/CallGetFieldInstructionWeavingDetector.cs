using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Checkers.CallWeaving.Called;
using NetAspect.Weaver.Core.Weaver.Checkers.CallWeaving.Source;
using NetAspect.Weaver.Core.Weaver.Checkers.MethodWeaving.Instance;
using NetAspect.Weaver.Core.Weaver.Checkers.MethodWeaving.Member;
using NetAspect.Weaver.Core.Weaver.Checkers.MethodWeaving.Parameters;
using NetAspect.Weaver.Core.Weaver.Detectors.Helpers;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Engine.Instructions;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Call;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;
using NetAspect.Weaver.Helpers.IL;

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

        public InterceptorInfoExtensions.MyGenerator<T> Generator
        {
            get { return _myGenerator; }
        }

        public InterceptorInfo Interceptor
        {
            get { return _interceptor; }
        }

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

        public InterceptorParameterConfigurator<T> WhichPdbPresent()
        {
            _checker.Checkers.Add((parameter, errorListener) => Ensure.SequencePoint(_interceptor.Instruction, errorListener, parameter));
            return this;
        }

        public InterceptorParameterConfigurator<T> WhichCanNotBeOut()
        {
            _checker.Checkers.Add((parameter, errorListener) => Ensure.NotOut(parameter, errorListener));
            return this;
        }

        public InterceptorParameterConfigurator<T> WhereFieldCanNotBeStatic()
        {
            _checker.Checkers.Add((parameter, errorListener) => Ensure.NotStaticButDefaultValue(parameter, errorListener, _interceptor.GetOperandAsField()));
            return this;
        }

        public InterceptorParameterConfigurator<T> WhereCurrentMethodCanNotBeStatic()
        {
            _checker.Checkers.Add((parameter, errorListener) => Ensure.NotStatic(parameter, errorListener, _interceptor.Method));
            return this;
        }

        public InterceptorParameterConfigurator<T> WhichMustBeOfType<T1>()
        {
            allowedTypes.Add(typeof(T1).FullName);
            return this;
        }

        public InterceptorParameterConfigurator<T> OrOfType(TypeReference type)
        {
            return WhichMustBeOfTypeOf(type);
        }

        public InterceptorParameterConfigurator<T> WhichMustBeOfTypeOf(TypeReference type)
        {
            allowedTypes.Add(type.FullName);
            return this;
        }

        public InterceptorParameterConfigurator<T> OrOfFieldDeclaringType()
        {
            return OrOfType(_interceptor.GetOperandAsField().DeclaringType);
        }

        public InterceptorParameterConfigurator<T> OrOfCurrentMethodDeclaringType()
        {
            return OrOfType(_interceptor.Method.DeclaringType);
        }


        public InterceptorParameterConfigurator<T> AndInjectThePdbInfo(Func<SequencePoint, int> pdbInfoProvider)
        {
            _myGenerator.Generators.Add((parameter, instructions, info) =>
            {
                instructions.Add(InstructionFactory.Create(_interceptor.Instruction.GetLastSequencePoint(), pdbInfoProvider));
            });
            return this;
        }
        public InterceptorParameterConfigurator<T> AndInjectThePdbInfo(Func<SequencePoint, string> pdbInfoProvider)
        {
            _myGenerator.Generators.Add((parameter, instructions, info) =>
            {
                instructions.Add(InstructionFactory.Create(_interceptor.Instruction.GetLastSequencePoint(), pdbInfoProvider));
            });
            return this;
        }
        
    }


    public static class InterceptorParameterConfiguratorExtensionsForInstruction
    {

        public static InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> AndInjectTheCalledFieldInfo(this InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurator)
        {
            
            interceptorParameterConfigurator.Generator.Generators.Add((parameter, instructions, info) =>
            {
                var interceptor = interceptorParameterConfigurator.Interceptor;
                instructions.AppendCallToTargetGetType(interceptor.Method.Module, info.Called);
                instructions.AppendCallToGetField(interceptor.GetOperandAsField().Name, interceptor.Method.Module);
            });
            return interceptorParameterConfigurator;
        }

        public static InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> AndInjectTheCalledInstance(this InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurator)
        {
            interceptorParameterConfigurator.Generator.Generators.Add((parameter, instructions, info) =>
            {
                var called = info.Called;
                instructions.Add(called == null
                                 ? Instruction.Create(OpCodes.Ldnull)
                                 : Instruction.Create(OpCodes.Ldloc, called));
            });
            return interceptorParameterConfigurator;
        }
        public static InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> AndInjectTheCurrentInstance(this InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurator)
        {
            interceptorParameterConfigurator.Generator.Generators.Add((parameter, instructions, info) =>
            {
                instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            });
            return interceptorParameterConfigurator;
        }
        public static InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> AndInjectTheVariable(this InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurator, Func<IlInjectorAvailableVariablesForInstruction, VariableDefinition> variableProvider)
        {
            interceptorParameterConfigurator.Generator.Generators.Add((parameter, instructions, info) =>
            {
                instructions.Add(Instruction.Create(OpCodes.Ldloc, variableProvider(info)));
            });
            return interceptorParameterConfigurator;
        }
        public static InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> AndInjectTheParameter(this InterceptorParameterConfigurator<IlInjectorAvailableVariablesForInstruction> interceptorParameterConfigurator, ParameterDefinition parameter)
        {
            interceptorParameterConfigurator.Generator.Generators.Add((parameterInfo, instructions, info) =>
            {
                ModuleDefinition moduleDefinition = ((MethodDefinition)parameter.Method).Module;
                if (parameterInfo.ParameterType.IsByRef && !parameter.ParameterType.IsByReference)
                {
                    instructions.Add(Instruction.Create(OpCodes.Ldarga, parameter));
                }
                else if (!parameterInfo.ParameterType.IsByRef && parameter.ParameterType.IsByReference)
                {
                    instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter));
                    instructions.Add(Instruction.Create(OpCodes.Ldobj, moduleDefinition.Import(parameterInfo.ParameterType)));
                }
                else
                {
                    instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter));
                }
                if (parameter.ParameterType != moduleDefinition.TypeSystem.Object &&
                    parameterInfo.ParameterType == typeof(Object))
                {
                    TypeReference reference = parameter.ParameterType;
                    if (reference.IsByReference)
                    {
                        reference =
                            ((MethodDefinition)parameter.Method).GenericParameters.First(
                                t => t.Name == reference.Name.TrimEnd('&'));
                        instructions.Add(Instruction.Create(OpCodes.Ldobj, reference));
                    }
                    instructions.Add(Instruction.Create(OpCodes.Box, reference));
                }
            });
            return interceptorParameterConfigurator;
        }
    }

    public static class InterceptorInfoExtensionsParameters
    {
        public static InterceptorInfo AddCalled(this InterceptorInfo info)
        {
            info.AddPossibleParameter("called")
                .WhichCanNotBeReferenced()
                .WhereFieldCanNotBeStatic()
                .WhichMustBeOfType<object>().OrOfFieldDeclaringType()
                .AndInjectTheCalledInstance();
            return info;
        }
        public static InterceptorInfo AddCaller(this InterceptorInfo info)
        {
            info.AddPossibleParameter("caller")
                .WhichCanNotBeReferenced()
                .WhereCurrentMethodCanNotBeStatic()
                .WhichMustBeOfType<object>().OrOfCurrentMethodDeclaringType()
                .AndInjectTheCurrentInstance();
            return info;
        }
        public static InterceptorInfo AddCallerParameters(this InterceptorInfo info)
        {
            info.AddPossibleParameter("callerparameters")
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<object[]>()
                .AndInjectTheVariable(variables => variables.Parameters);
            return info;
        }
        public static InterceptorInfo AddCallerParameterNames(this InterceptorInfo info)
        {
            foreach (ParameterDefinition parameter in info.Method.Parameters)
            {

                info.AddPossibleParameter("caller" + parameter.Name.ToLower())
                    .WhichCanNotBeOut()
                    .WhichMustBeOfTypeOf(parameter.ParameterType)
                    .AndInjectTheParameter(parameter);
            }
            return info;
        }
        public static InterceptorInfo AddColumnNumber(this InterceptorInfo info)
        {
            info.AddPossibleParameter("columnnumber")
                .WhichCanNotBeReferenced()
                .WhichPdbPresent()
                .WhichMustBeOfType<int>()
                .AndInjectThePdbInfo(s => s.StartColumn);
            return info;
        }
        public static InterceptorInfo AddLineNumber(this InterceptorInfo info)
        {
            info.AddPossibleParameter("linenumber")
                .WhichCanNotBeReferenced()
                .WhichPdbPresent()
                .WhichMustBeOfType<int>()
                .AndInjectThePdbInfo(s => s.StartLine);
            return info;
        }
        public static InterceptorInfo AddFilePath(this InterceptorInfo info)
        {
            info.AddPossibleParameter("filepath")
                .WhichCanNotBeReferenced()
                .WhichPdbPresent()
                .WhichMustBeOfType<string>()
                .AndInjectThePdbInfo(s => s.Document.Url);
            return info;
        }
        public static InterceptorInfo AddFileName(this InterceptorInfo info)
        {
            info.AddPossibleParameter("filename")
                .WhichCanNotBeReferenced()
                .WhichPdbPresent()
                .WhichMustBeOfType<string>()
                .AndInjectThePdbInfo(s => Path.GetFileName(s.Document.Url));
            return info;
        }
        public static InterceptorInfo AddCalledFieldInfo(this InterceptorInfo info)
        {
            info.AddPossibleParameter("field")
                .WhichCanNotBeReferenced()
                .WhichMustBeOfType<FieldInfo>()
                .AndInjectTheCalledFieldInfo();
            return info;
        }
    }

    public class CallGetFieldInterceptorAroundInstructionFactory : IInterceptorAroundInstructionFactory
    {
        public void FillCommon(InterceptorInfo info)
        {
            info.AddCalled();
            info.AddCalledFieldInfo();

            info.AddCaller();
            info.AddCallerParameters();
            info.AddCallerParameterNames();

            info.AddColumnNumber();
            info.AddLineNumber();
            info.AddFilePath();
            info.AddFileName();
        }

        public void FillBeforeSpecific(InterceptorInfo info)
        {
        }

        public void FillAfterSpecific(InterceptorInfo info)
        {
        }
    }

    public interface IAroundInstructionWeaverFactory
    {
        IIlInjector<IlInjectorAvailableVariablesForInstruction> CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod, NetAspectDefinition aspect, Instruction instruction);

        IIlInjector<IlInjectorAvailableVariablesForInstruction> CreateForAfter(MethodDefinition method,
                                                                                               MethodInfo interceptorMethod,
                                                                                               NetAspectDefinition aspect, Instruction instruction);
    }

    public class AroundInstructionWeaverFactory : IAroundInstructionWeaverFactory
    {


        private IInterceptorAroundInstructionFactory _interceptorAroundInstructionFactory;
        public IIlInjector<IlInjectorAvailableVariablesForInstruction> CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod, NetAspectDefinition aspect, Instruction instruction)
        {


            var checker = new ParametersChecker();
            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>();
            var info = new InterceptorInfo()
                {
                    Generator = parametersIlGenerator,
                    Instruction                    = instruction,
                    Interceptor                    = interceptorMethod,
                    Method                    = method,
                };
            _interceptorAroundInstructionFactory.FillCommon(info);
            _interceptorAroundInstructionFactory.FillBeforeSpecific(info);

            return new MethodWeavingBeforeMethodInjector<IlInjectorAvailableVariablesForInstruction>(method, interceptorMethod, checker,
                                                                                       parametersIlGenerator, aspect);
        }
        

        public IIlInjector<IlInjectorAvailableVariablesForInstruction> CreateForAfter(MethodDefinition method,
                                                                               MethodInfo interceptorMethod,
                                                                               NetAspectDefinition aspect, Instruction instruction)
        {
            var checker = new ParametersChecker();
            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>();
            var info = new InterceptorInfo()
            {
                Generator = parametersIlGenerator,
                Instruction = instruction,
                Interceptor = interceptorMethod,
                Method = method,
            };
            _interceptorAroundInstructionFactory.FillCommon(info);
            _interceptorAroundInstructionFactory.FillAfterSpecific(info);

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