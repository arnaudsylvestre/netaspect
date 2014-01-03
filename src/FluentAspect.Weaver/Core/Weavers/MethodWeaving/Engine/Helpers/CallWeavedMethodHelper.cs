using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Helpers
{
    public static class CallWeavedMethodHelper
    {
        public static void CallWeavedMethod(this MethodToWeave method, MethodDefinition wrappedMethod, VariableDefinition weavedResult)
        {
            if (!wrappedMethod.IsStatic)
                method.Method.Il.Emit(OpCodes.Ldarg_0);
            foreach (ParameterDefinition p in method.Method.MethodDefinition.Parameters.ToArray())
            {
                method.Method.Il.Emit(OpCodes.Ldarg, p);
            }

            OpCode call = OpCodes.Callvirt;
            if ((method.Method.MethodDefinition.Attributes & MethodAttributes.Static) == MethodAttributes.Static)
                call = OpCodes.Call;
            method.Method.Il.Emit(call, wrappedMethod.MakeGeneric(method.Method.MethodDefinition.GenericParameters.ToArray()));

            if (method.Method.MethodDefinition.ReturnType.MetadataType != MetadataType.Void)
                method.Method.Il.Emit(OpCodes.Stloc, weavedResult);
        }
    }
}