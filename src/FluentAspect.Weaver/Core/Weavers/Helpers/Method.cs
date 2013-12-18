using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Weavers.Helpers
{
    public class Method
    {
        private MethodDefinition definition;
        private ILProcessor il;

        public Method(MethodDefinition definition)
        {
            this.definition = definition;
            il = definition.Body.GetILProcessor();
        }

       public TryCatch CreateTryCatch()
       {
          
       }

       public ILProcessor Il
       {
          get { return il; }
       }

       public MethodDefinition MethodDefinition
       {
          get { return definition; }
       }

       public VariableDefinition CreateAndInitializeVariable(Type interceptorType)
        {
            return il.CreateAndInitializeVariable(definition, interceptorType);
        }

        public VariableDefinition CreateArgsArrayFromParameters()
        {
            VariableDefinition args = definition.CreateVariable(typeof(object[]));

            il.Emit(OpCodes.Ldc_I4, definition.Parameters.Count);
            il.Emit(OpCodes.Newarr, definition.Module.Import(typeof(object)));
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
            VariableDefinition methodInfo = definition.CreateVariable(typeof(MethodInfo));
            il.AppendCallToThisGetType(definition.Module);
            il.AppendCallToGetMethod(definition.Name, definition.Module);
            il.AppendSaveResultTo(methodInfo);

            return methodInfo;
        }

       public void Append(List<Instruction> callBaseInstructions_P)
       {
          foreach (var callBaseInstruction_L in callBaseInstructions_P)
          {
             il.Append(callBaseInstruction_L);
          }
       }
    }
}