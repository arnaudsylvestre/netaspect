using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weavers.Helpers;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.Constructors
{
    public class ConstructorAroundWeaver
    {
       public void CreateWeaver(MethodDefinition methodDefinition_P, IEnumerable<MethodWeavingConfiguration> interceptor_P,
                                 MethodDefinition wrappedMethod_P)
        {
            List<Instruction> callBaseInstructions = ExtractCallToBaseInstructions(wrappedMethod_P,
                                                                                   methodDefinition_P.DeclaringType);
            ClearCallToBase(wrappedMethod_P, methodDefinition_P.DeclaringType);

            var method_L = new Method(methodDefinition_P);
            method_L.MethodDefinition.Body.Instructions.Clear();
            method_L.Append(callBaseInstructions);

            var aroundWeaver_L = new WeavedMethodBuilder();
            aroundWeaver_L.Build(new MethodToWeave(interceptor_P.ToList(), method_L), wrappedMethod_P);
        }

        private List<Instruction> ExtractCallToBaseInstructions(MethodDefinition wrappedMethod_P,
                                                                TypeDefinition declaringType_P)
        {
            var instructions_L = new List<Instruction>();
            for (int i = 0; i < wrappedMethod_P.Body.Instructions.Count; i++)
            {
                instructions_L.Add(wrappedMethod_P.Body.Instructions[i]);
                if (IsCallToBase(wrappedMethod_P, declaringType_P, i))
                    break;
            }
            return instructions_L;
        }

        private void ClearCallToBase(MethodDefinition wrappedMethod_P, TypeDefinition declaringTypeP_P)
        {
            while (!IsCallToBase(wrappedMethod_P, declaringTypeP_P, 0))
            {
                wrappedMethod_P.Body.Instructions.RemoveAt(0);
            }
            wrappedMethod_P.Body.Instructions.RemoveAt(0);
        }

        private bool IsCallToBase(MethodDefinition wrappedMethod_P, TypeDefinition declaringTypePP_P, int iP_P)
        {
            Instruction instruction_L = wrappedMethod_P.Body.Instructions[iP_P];
            bool isCall = instruction_L.OpCode == OpCodes.Call;
            if (!isCall)
                return false;
            bool isBase = IsBase(instruction_L, declaringTypePP_P);
            if (!isBase)
                return false;
            bool isCtor = ((MethodReference) instruction_L.Operand).Name == ".ctor";
            return isCtor;
        }

        private static bool IsBase(Instruction instruction_L, TypeDefinition declaringTypePPP_P)
        {
            TypeDefinition typeDefinition_L = declaringTypePPP_P;
            return ((MethodReference) instruction_L.Operand).DeclaringType == typeDefinition_L.BaseType;
        }
    }
}