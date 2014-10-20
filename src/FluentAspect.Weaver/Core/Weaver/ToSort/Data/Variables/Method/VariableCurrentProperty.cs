using System;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables.Method
{
    public class VariableCurrentProperty : Variable.IVariableBuilder
    {
        public void Check(MethodDefinition method, ErrorHandler errorHandler)
        {
            
        }

        public VariableDefinition Build(InstructionsToInsert instructionsToInsert, MethodDefinition method, Mono.Cecil.Cil.Instruction instruction)
        {
            var currentPropertyInfo = new VariableDefinition(method.Module.Import(typeof(PropertyInfo)));


            instructionsToInsert.BeforeInstructions.Add(
               Mono.Cecil.Cil.Instruction.Create(
                  OpCodes.Call,
                  method.Module.Import(
                     typeof(MethodBase).GetMethod(
                        "GetCurrentMethod",
                        new Type[] { }))));
            instructionsToInsert.BeforeInstructions.Add(
               Mono.Cecil.Cil.Instruction.Create(
                  OpCodes.Callvirt,
                  method.Module.Import(
                     typeof(MemberInfo).GetMethod(
                        "get_DeclaringType",
                        new Type[] { }))));
            instructionsToInsert.BeforeInstructions.AppendCallToGetProperty(
               method.Name.Replace("get_", "").Replace("set_", ""),
               method.Module);
            instructionsToInsert.BeforeInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, currentPropertyInfo));
            return currentPropertyInfo;
        }
    }
}