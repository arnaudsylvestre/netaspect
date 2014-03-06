using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Helpers
{
    public static class CallWeavedMethodHelper
    {
        public static void CallWeavedMethod(this MethodToWeave method, MethodDefinition wrappedMethod,
                                            VariableDefinition weavedResult)
        {
            if (!wrappedMethod.IsStatic)
                method.Method.MethodDefinition.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            foreach (ParameterDefinition p in method.Method.MethodDefinition.Parameters.ToArray())
                method.Method.MethodDefinition.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg, p));

            OpCode call = method.Method.MethodDefinition.IsStatic ? OpCodes.Call : OpCodes.Callvirt;
            method.Method.MethodDefinition.Body.Instructions.Add(Instruction.Create(call,
                                                                                    wrappedMethod.MakeGeneric(
                                                                                        method.Method.MethodDefinition
                                                                                              .GenericParameters.ToArray
                                                                                            ())));

            if (method.Method.MethodDefinition.ReturnType.MetadataType != MetadataType.Void)
                method.Method.MethodDefinition.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, weavedResult));
        }
    }
}