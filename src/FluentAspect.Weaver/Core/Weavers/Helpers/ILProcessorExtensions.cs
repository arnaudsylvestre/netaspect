using System;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.Helpers
{
    public static class ILProcessorExtensions
    {
        public static VariableDefinition This = new VariableDefinition(new TypeReference("", "", null, null));

        public static void AppendThrow(this ILProcessor il)
        {
            il.Emit(OpCodes.Rethrow);
        }

        public static void AppendCallToThisGetType(this ILProcessor il, ModuleDefinition module)
        {
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, module.Import(typeof(object).GetMethod("GetType", new Type[0])));
        }
        public static void AppendCallTo(this ILProcessor il, MethodInfo method, ModuleDefinition module, params VariableDefinition[] parameters)
        {
            foreach (var parameter in parameters)
            {
                if (parameter == This)
                    il.Emit(OpCodes.Ldarg_0);
                else
                    il.Emit(OpCodes.Ldloc, parameter);
            }
            il.Emit(OpCodes.Call, module.Import(method));
        }
        public static void AppendCallToGetMethod(this ILProcessor il, string methodName, ModuleDefinition module)
        {
            il.Emit(OpCodes.Ldstr, methodName);
            il.Emit(OpCodes.Callvirt, module.Import(typeof(Type).GetMethod("GetMethod", new[] { typeof(string) })));
        }
        public static void AppendSaveResultTo(this ILProcessor il, VariableDefinition variable)
        {
            il.Emit(OpCodes.Stloc, variable);
        }

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