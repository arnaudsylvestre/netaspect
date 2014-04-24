using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Generators
{
    public delegate void IlGenerator(
        ParameterInfo parameterInfo, List<Instruction> instructions, IlInjectorAvailableVariablesForInstruction info);

    public class CalledInterceptorParametersIlGenerator : IInterceptorParameterIlGenerator<IlInjectorAvailableVariablesForInstruction>
    {
        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, IlInjectorAvailableVariablesForInstruction info)
        {
            if (info.Called == null)
            {
                instructions.Add(Instruction.Create(OpCodes.Ldnull));
            }
            else
                instructions.Add(Instruction.Create(OpCodes.Ldloc, info.Called));
        }
    }
}