using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Weavers.Helpers
{
    public static class ILProcessorExtensions
    {


        public static Instruction AppendLeave(this ILProcessor il, Instruction instruction_P)
        {
            Instruction instruction_L = il.Create(OpCodes.Leave, instruction_P);
            il.Append(instruction_L);
            return instruction_L;
        }

        public static void AppendCreateNewObject(this ILProcessor il,
                                     VariableDefinition interceptor,
                                     Type interceptorType,
                                     ModuleDefinition module)
        {
            il.Emit(OpCodes.Newobj, module.Import(interceptorType.GetConstructors()[0]));
            il.Emit(OpCodes.Stloc, interceptor);
        }



        public static VariableDefinition CreateAndInitializeVariable(this ILProcessor il, MethodDefinition method, Type interceptorType)
        {
            VariableDefinition interceptor = method.CreateVariable(interceptorType);
            il.AppendCreateNewObject(interceptor, interceptorType, method.Module);
            return interceptor;
        }
    }
}