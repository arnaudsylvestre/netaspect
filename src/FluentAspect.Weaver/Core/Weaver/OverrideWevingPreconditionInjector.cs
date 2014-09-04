using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Core.Helpers;
using NetAspect.Weaver.Core.Weaver.Data;
using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver
{
    public class OverrideWevingPreconditionInjector : IWevingPreconditionInjector<VariablesForInstruction>
    {
        public void Inject(List<Instruction> precondition, VariablesForInstruction availableInformations, MethodInfo interceptorMethod_P, MethodDefinition method_P)
        {
            Instruction instruction_L = availableInformations.Instruction;
            if (!instruction_L.IsACallInstruction())
                return;
            MethodDefinition calledMethod = instruction_L.GetCalledMethod();
            if (calledMethod.IsVirtual)
            {
                precondition.Add(Instruction.Create(OpCodes.Ldstr, calledMethod.DeclaringType.FullName.Replace('/', '+')));
                precondition.AppendCallToTargetGetType(method_P.Module, availableInformations.Called.Definition);
                precondition.AppendCallToGetMethod(calledMethod.Name, method_P.Module);
                precondition.Add(Instruction.Create(OpCodes.Callvirt, method_P.Module.Import(typeof (MemberInfo).GetMethod("get_DeclaringType"))));
                precondition.Add(Instruction.Create(OpCodes.Callvirt, method_P.Module.Import(typeof (Type).GetMethod("get_FullName"))));
                precondition.Add(Instruction.Create(OpCodes.Call, method_P.Module.Import(typeof (string).GetMethod("op_Equality"))));
            }
        }
    }
}