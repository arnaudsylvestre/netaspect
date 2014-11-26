using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Core.Helpers;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Core.Weaver.ToSort.ILInjector
{
    public class OverrideWeavingPreconditionInjector : IWeavingPreconditionInjector<VariablesForInstruction>
    {
        public void Inject(List<Mono.Cecil.Cil.Instruction> precondition, VariablesForInstruction availableInformations, MethodDefinition method)
        {
            Mono.Cecil.Cil.Instruction instruction_L = availableInformations.Instruction;
            if (!instruction_L.IsACallInstruction())
                return;
            MethodDefinition calledMethod = instruction_L.GetCalledMethod();
            if (calledMethod.IsVirtual)
            {
                precondition.AppendCallToTargetGetType(method.Module, availableInformations.Called.Definition);
                VariableDefinition type = new VariableDefinition(method.Module.Import(typeof(Type)));
                availableInformations.AddLocalVariable(type);
                precondition.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, type));
                precondition.AppendCallToGetMethod(calledMethod, method.Module, type, method);
                precondition.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Callvirt, method.Module.Import(typeof (MemberInfo).GetMethod("get_DeclaringType"))));
                precondition.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Callvirt, method.Module.Import(typeof(Type).GetMethod("get_FullName"))));
                precondition.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldstr, calledMethod.DeclaringType.FullName.Replace('/', '+')));
                precondition.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Call, method.Module.Import(typeof (string).GetMethod("op_Equality"))));
            }
        }
    }
}