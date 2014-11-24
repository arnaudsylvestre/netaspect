using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables.Method
{
    public class VariableCurrentMethodBuilder : Variable.IVariableBuilder
    {
        private List<VariableDefinition> methodVariables;

        public VariableCurrentMethodBuilder(List<VariableDefinition> methodVariables)
        {
            this.methodVariables = methodVariables;
        }

        public void Check(MethodDefinition method, ErrorHandler errorHandler)
        {

        }

        public VariableDefinition Build(InstructionsToInsert instructionsToInsert_P, MethodDefinition method, Mono.Cecil.Cil.Instruction instruction)
        {
            var variable = new VariableDefinition(method.Module.Import(method.IsConstructor ? typeof(ConstructorInfo) : typeof(MethodInfo)));
            var type = new VariableDefinition(method.Module.Import(typeof(Type)));
            methodVariables.Add(type);
            instructionsToInsert_P.BeforeInstructions.AppendCallToTypeOf(method.Module, method.DeclaringType);
            instructionsToInsert_P.BeforeInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, type));
            if (!method.IsConstructor)
                instructionsToInsert_P.BeforeInstructions.AppendCallToGetMethod(method, method.Module, methodVariables.Add, type);
            else
                instructionsToInsert_P.BeforeInstructions.AppendCallToGetConstructor(method, method.Module, methodVariables.Add, type);
            instructionsToInsert_P.BeforeInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, variable));
            return variable;
        }
    }


    /**
     * 
     * .method public hidebysig 
	instance !!T Weaved<T> (
		!!T toWeave,
		string param1
	) cil managed 
{
	// Method begins at RVA 0x11670
	// Code size 232 (0xe8)
	.maxstack 2
	.locals init (
		[0] class [mscorlib]System.Reflection.MethodInfo myMethod,
		[1] class [mscorlib]System.Reflection.MethodInfo[] methods,
		[2] class [mscorlib]System.Reflection.MethodInfo methodInfo,
		[3] class [mscorlib]System.Reflection.ParameterInfo[] parameters,
		[4] class [mscorlib]System.Type parameterType,
		[5] !!T CS$1$0000,
		[6] class [mscorlib]System.Reflection.MethodInfo[] CS$6$0001,
		[7] int32 CS$7$0002,
		[8] bool CS$4$0003
	)

	IL_0000: nop
	IL_0001: ldnull
	IL_0002: stloc.0
     * 
     * 
     * Call Gettype et GetMethods()
	IL_0003: ldarg.0
	IL_0004: call instance class [mscorlib]System.Type [mscorlib]System.Object::GetType()
	IL_0009: ldc.i4.s 60
	IL_000b: callvirt instance class [mscorlib]System.Reflection.MethodInfo[] [mscorlib]System.Type::GetMethods(valuetype [mscorlib]System.Reflection.BindingFlags)
	IL_0010: stloc.1
	IL_0011: nop
     * 
     * Copie temporaire dans une variable (à supprimer)
	IL_0012: ldloc.1
	IL_0013: stloc.s CS$6$0001
	IL_0015: ldc.i4.0
	IL_0016: stloc.s CS$7$0002
	IL_0018: br IL_00c3
	// loop start (head: IL_00c3)
		IL_001d: ldloc.s CS$6$0001
		IL_001f: ldloc.s CS$7$0002
		IL_0021: ldelem.ref
		IL_0022: stloc.2
		IL_0023: nop
		IL_0024: ldloc.2
		IL_0025: callvirt instance string [mscorlib]System.Reflection.MemberInfo::get_Name()
		IL_002a: ldstr "Weaved"
		IL_002f: call bool [mscorlib]System.String::op_Inequality(string, string)
		IL_0034: ldc.i4.0
		IL_0035: ceq
		IL_0037: stloc.s CS$4$0003
		IL_0039: ldloc.s CS$4$0003
		IL_003b: brtrue.s IL_003f

		IL_003d: br.s IL_00bd

		IL_003f: ldloc.2
		IL_0040: callvirt instance class [mscorlib]System.Type[] [mscorlib]System.Reflection.MethodBase::GetGenericArguments()
		IL_0045: ldlen
		IL_0046: conv.i4
		IL_0047: ldc.i4.1
		IL_0048: ceq
		IL_004a: stloc.s CS$4$0003
		IL_004c: ldloc.s CS$4$0003
		IL_004e: brtrue.s IL_0052

		IL_0050: br.s IL_00bd

		IL_0052: ldloc.2
		IL_0053: callvirt instance class [mscorlib]System.Reflection.ParameterInfo[] [mscorlib]System.Reflection.MethodBase::GetParameters()
		IL_0058: stloc.3
		IL_0059: ldloc.3
		IL_005a: ldlen
		IL_005b: conv.i4
		IL_005c: ldc.i4.2
		IL_005d: ceq
		IL_005f: stloc.s CS$4$0003
		IL_0061: ldloc.s CS$4$0003
		IL_0063: brtrue.s IL_0067

		IL_0065: br.s IL_00bd

		IL_0067: ldloc.3
		IL_0068: ldc.i4.0
		IL_0069: ldelem.ref
		IL_006a: callvirt instance class [mscorlib]System.Type [mscorlib]System.Reflection.ParameterInfo::get_ParameterType()
		IL_006f: stloc.s parameterType
		IL_0071: ldloc.s parameterType
		IL_0073: callvirt instance string [mscorlib]System.Reflection.MemberInfo::get_Name()
		IL_0078: ldstr "T"
		IL_007d: call bool [mscorlib]System.String::op_Inequality(string, string)
		IL_0082: ldc.i4.0
		IL_0083: ceq
		IL_0085: stloc.s CS$4$0003
		IL_0087: ldloc.s CS$4$0003
		IL_0089: brtrue.s IL_008d

		IL_008b: br.s IL_00bd

		IL_008d: ldloc.3
		IL_008e: ldc.i4.1
		IL_008f: ldelem.ref
		IL_0090: callvirt instance class [mscorlib]System.Type [mscorlib]System.Reflection.ParameterInfo::get_ParameterType()
		IL_0095: callvirt instance string [mscorlib]System.Type::get_FullName()
		IL_009a: ldtoken [mscorlib]System.String
		IL_009f: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_00a4: callvirt instance string [mscorlib]System.Type::get_FullName()
		IL_00a9: call bool [mscorlib]System.String::op_Inequality(string, string)
		IL_00ae: ldc.i4.0
		IL_00af: ceq
		IL_00b1: stloc.s CS$4$0003
		IL_00b3: ldloc.s CS$4$0003
		IL_00b5: brtrue.s IL_00b9

		IL_00b7: br.s IL_00bd

		IL_00b9: ldloc.2
		IL_00ba: stloc.0
		IL_00bb: br.s IL_00d4

		IL_00bd: ldloc.s CS$7$0002
		IL_00bf: ldc.i4.1
		IL_00c0: add
		IL_00c1: stloc.s CS$7$0002

		IL_00c3: ldloc.s CS$7$0002
		IL_00c5: ldloc.s CS$6$0001
		IL_00c7: ldlen
		IL_00c8: conv.i4
		IL_00c9: clt
		IL_00cb: stloc.s CS$4$0003
		IL_00cd: ldloc.s CS$4$0003
		IL_00cf: brtrue IL_001d
	// end loop

	IL_00d4: ldloc.0
	IL_00d5: ldstr "Elle est nulle !!!"
	IL_00da: call void [nunit.framework]NUnit.Framework.Assert::NotNull(object, string)
	IL_00df: nop
	IL_00e0: ldarg.1
	IL_00e1: stloc.s CS$1$0000
	IL_00e3: br.s IL_00e5

	IL_00e5: ldloc.s CS$1$0000
	IL_00e7: ret
} // end of method ClassToWeaveCheckMethod::Weaved

     * 
     * 
     */
}