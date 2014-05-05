using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine
{
    public class ParameterConfigurationForMethod
    {
        private AroundMethodInfo aroundMethodInfo;
        private ParameterConfiguration<IlInjectorAvailableVariables> configuration;
        private List<string> allowedTypes = new List<string>();

        public ParameterConfigurationForMethod(AroundMethodInfo aroundMethodInfo, ParameterConfiguration<IlInjectorAvailableVariables> configuration)
        {
            this.aroundMethodInfo = aroundMethodInfo;
            this.configuration = configuration;
        }


        public ParameterConfigurationForMethod AndInjectTheCurrentInstance()
        {
            configuration.Generator.Generators.Add((parameter, instructions, info) =>
                {
                    instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
                });
            return this;
        }

        public ParameterConfigurationForMethod WhichCanNotBeReferenced()
        {
            configuration.Checker.Checkers.Add((parameter, errorListener) => Ensure.NotReferenced(parameter, errorListener));
            return this;
        }


        public ParameterConfigurationForMethod WhereCurrentMethodCanNotBeStatic()
        {
            configuration.Checker.Checkers.Add((parameter, errorListener) => Ensure.NotStatic(parameter, errorListener, aroundMethodInfo.Method));
            return this;
        }

        public ParameterConfigurationForMethod WhichMustBeOfType<T1>()
        {
            allowedTypes.Add(typeof(T1).FullName);
            return this;
        }



        public ParameterConfigurationForMethod OrOfType(TypeReference type)
        {
            return WhichMustBeOfTypeOf(type);
        }



        public ParameterConfigurationForMethod WhichMustBeOfTypeOf(TypeReference type)
        {
            allowedTypes.Add(type.FullName);
            return this;
        }

        public ParameterConfigurationForMethod OrOfCurrentMethodDeclaringType()
        {
            return OrOfType(aroundMethodInfo.Method.DeclaringType);
        }

        public ParameterConfigurationForInstruction AndInjectTheCurrentMethod()
        {
            configuration.Generator.Generators.Add((parameter, instructions, info) =>
                {
                    instructions.Add(Instruction.Create(OpCodes.Ldloc, info.CurrentMethodBase));
                });
            return this;
        }
    }
}