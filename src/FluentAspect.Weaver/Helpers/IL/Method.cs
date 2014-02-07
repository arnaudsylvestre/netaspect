using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Helpers.IL
{
    public class Method
    {
        private readonly MethodDefinition definition;
        //private readonly ILProcessor il;

        public Method(MethodDefinition definition)
        {
            this.definition = definition;
            //il = definition.Body.GetILProcessor();
        }

        //public ILProcessor Il
        //{
        //    get { return il; }
        //}

        public MethodDefinition MethodDefinition
        {
            get { return definition; }
        }

        public List<VariableDefinition> CreateVariable(IEnumerable<Type> types)
        {
            return types.Select(type => definition.CreateVariable(type)).ToList();
        }

        public VariableDefinition CreateArgsArrayFromParameters(Collection<Instruction> instructions)
        {
            VariableDefinition args = definition.CreateVariable<object[]>();

            instructions.Add(Instruction.Create(OpCodes.Ldc_I4, definition.Parameters.Count));
            instructions.Add(Instruction.Create(OpCodes.Newarr, definition.Module.Import(typeof (object))));
            instructions.Add(Instruction.Create(OpCodes.Stloc, args));

            foreach (ParameterDefinition p in definition.Parameters.ToArray())
            {
                instructions.Add(Instruction.Create(OpCodes.Ldloc, args));
                instructions.Add(Instruction.Create(OpCodes.Ldc_I4, p.Index));
                instructions.Add(Instruction.Create(OpCodes.Ldarg, p));
                if (p.ParameterType.IsValueType || p.ParameterType is GenericParameter)
                    instructions.Add(Instruction.Create(OpCodes.Box, p.ParameterType));
                instructions.Add(Instruction.Create(OpCodes.Stelem_Ref));
            }

            return args;
        }


        public VariableDefinition CreateMethodInfo()
        {
            return definition.CreateVariable<MethodInfo>();
        }

        public void FIllMethod(Collection<Instruction> instructions, VariableDefinition methodInfo)
        {
            if (methodInfo == null)
                return;
            instructions.AppendCallToThisGetType(definition.Module);
            instructions.AppendCallToGetMethod(definition.Name, definition.Module);
            instructions.AppendSaveResultTo(methodInfo);
        }

        public void AddTryFinally(Instruction tryStart_L, Instruction tryEnd_L, Instruction handlerStart_L, Instruction handlerEnd_L)
        {
           MethodDefinition.Body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally)
           {
              TryStart = tryStart_L,
              TryEnd = tryEnd_L,
              HandlerStart = handlerStart_L,
              HandlerEnd = handlerEnd_L,
           });
        }

        public void AddTryCatch(Instruction tryStart_L, Instruction tryEnd_L, Instruction handlerStart_L, Instruction handlerEnd_L)
        {
           MethodDefinition.Body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Catch)
           {
              TryStart = tryStart_L,
              TryEnd = tryEnd_L,
              HandlerStart = handlerStart_L,
              HandlerEnd = handlerEnd_L,
           });
        }


        public void Return(VariableDefinition weavedResult, Collection<Instruction> instructions)
        {
            if (MethodDefinition.ReturnType.MetadataType != MetadataType.Void)
                instructions.Add(Instruction.Create(OpCodes.Ldloc, weavedResult));
            instructions.Add(Instruction.Create(OpCodes.Ret));
        }
    }

}