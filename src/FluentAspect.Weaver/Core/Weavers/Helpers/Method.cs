﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Model;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.Helpers
{
    public class Method
    {
        private readonly MethodDefinition definition;
        private readonly ILProcessor il;

        public Method(MethodDefinition definition)
        {
            this.definition = definition;
            il = definition.Body.GetILProcessor();
        }

        public ILProcessor Il
        {
            get { return il; }
        }

        public MethodDefinition MethodDefinition
        {
            get { return definition; }
        }

        public List<VariableDefinition> CreateAndInitializeVariable(
            IEnumerable<MethodWeavingConfiguration> interceptorType)
        {
            return interceptorType.Select(type => il.CreateAndInitializeVariable(definition, type.Type)).ToList();
        }

        public VariableDefinition CreateArgsArrayFromParameters()
        {
            VariableDefinition args = definition.CreateVariable(typeof (object[]));

            il.Emit(OpCodes.Ldc_I4, definition.Parameters.Count);
            il.Emit(OpCodes.Newarr, definition.Module.Import(typeof (object)));
            il.Emit(OpCodes.Stloc, args);

            foreach (ParameterDefinition p in definition.Parameters.ToArray())
            {
                il.Emit(OpCodes.Ldloc, args);
                il.Emit(OpCodes.Ldc_I4, p.Index);
                il.Emit(OpCodes.Ldarg, p);
                if (p.ParameterType.IsValueType || p.ParameterType is GenericParameter)
                    il.Emit(OpCodes.Box, p.ParameterType);
                il.Emit(OpCodes.Stelem_Ref);
            }

            return args;
        }


        public VariableDefinition CreateMethodInfo()
        {
            VariableDefinition methodInfo = definition.CreateVariable(typeof (MethodInfo));
            il.AppendCallToThisGetType(definition.Module);
            il.AppendCallToGetMethod(definition.Name, definition.Module);
            il.AppendSaveResultTo(methodInfo);

            return methodInfo;
        }

        public void Append(List<Instruction> callBaseInstructions_P)
        {
            foreach (Instruction callBaseInstruction_L in callBaseInstructions_P)
            {
                il.Append(callBaseInstruction_L);
            }
        }

        public void Add(TryCatch tryCatch)
        {
            Instruction beforeCatch = CreateNopForCatch(Il);
            Instruction instruction_L = Il.Create(OpCodes.Nop);

            tryCatch.OnTryContent();

            Il.AppendLeave(instruction_L);

            Instruction onCatch = CreateNopForCatch(Il);
            tryCatch.OnCatch();
            Instruction endCatch = Il.AppendLeave(instruction_L);
            endCatch = Il.Create(OpCodes.Nop);
            Il.Append(endCatch);

            Il.Append(instruction_L);
            CreateExceptionHandler(MethodDefinition, onCatch, endCatch, beforeCatch);
        }

        private Instruction CreateNopForCatch(ILProcessor il)
        {
            Instruction nop = il.Create(OpCodes.Nop);
            il.Append(nop);
            return nop;
        }

        private void CreateExceptionHandler(MethodDefinition method,
                                            Instruction onCatch,
                                            Instruction endCatch,
                                            Instruction beforeCatchP_P)
        {
            var handler = new ExceptionHandler(ExceptionHandlerType.Catch)
                {
                    TryStart = beforeCatchP_P,
                    TryEnd = onCatch,
                    HandlerStart = onCatch,
                    HandlerEnd = endCatch,
                    CatchType = method.Module.Import(typeof (Exception)),
                };

            method.Body.ExceptionHandlers.Add(handler);
        }


        public VariableDefinition CreateWeavedResult()
        {
            if (MethodDefinition.ReturnType.MetadataType != MetadataType.Void)
            {
                return MethodDefinition.CreateVariable(MethodDefinition.ReturnType);
            }
            return null;
        }


        public void Return(VariableDefinition weavedResult)
        {
            if (MethodDefinition.ReturnType.MetadataType != MetadataType.Void)
                il.Emit(OpCodes.Ldloc, weavedResult);
            il.Emit(OpCodes.Ret);
        }
    }

    public class TryCatch
    {
        private readonly Action _onCatch;
        private readonly Action onTryContent;

        public TryCatch(Action onTryContent, Action onCatch)
        {
            this.onTryContent = onTryContent;
            _onCatch = onCatch;
        }

        public Action OnTryContent
        {
            get { return onTryContent; }
        }

        public Action OnCatch
        {
            get { return _onCatch; }
        }
    }
}