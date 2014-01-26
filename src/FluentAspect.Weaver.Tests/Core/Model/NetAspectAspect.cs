using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using FieldAttributes = Mono.Cecil.FieldAttributes;
using MethodAttributes = Mono.Cecil.MethodAttributes;
using ParameterAttributes = Mono.Cecil.ParameterAttributes;

namespace FluentAspect.Weaver.Tests.Core.Model
{
    public class NetAspectAspect
    {
        private readonly TypeDefinition typeDefinition;

        public NetAspectAspect(TypeDefinition typeDefinition)
        {
            this.typeDefinition = typeDefinition;
        }

       public TypeDefinition TypeDefinition
       {
          get { return typeDefinition; }
       }

       public MethodDefinition AddDefaultConstructor()
        {
            var constructor = new MethodDefinition(".ctor", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, typeDefinition.Module.TypeSystem.Void);

            var instructions = constructor.Body.Instructions;

            var parentConstructor = (from m in typeDefinition.BaseType.Resolve().Methods where m.Name == ".ctor" && m.Parameters.Count == 0 select m).First();

            instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            instructions.Add(Instruction.Create(OpCodes.Call, parentConstructor));
            instructions.Add(Instruction.Create(OpCodes.Ret));

            return constructor;
        }

        public NetAspectInterceptor AddAfterInterceptor()
        {
            var interceptor = new MethodDefinition("After", MethodAttributes.Public, typeDefinition.Module.TypeSystem.Void);
           typeDefinition.Methods.Add(interceptor);
            return new NetAspectInterceptor(interceptor);
        }

        public NetAspectInterceptor AddAfterPropertyGetInterceptor()
        {
            var interceptor = new MethodDefinition("AfterPropertyGet", MethodAttributes.Public, typeDefinition.Module.TypeSystem.Void);
            typeDefinition.Methods.Add(interceptor);
            return new NetAspectInterceptor(interceptor);
        }

        public NetAspectInterceptor AddAfterPropertySetInterceptor()
        {
            var interceptor = new MethodDefinition("AfterPropertySet", MethodAttributes.Public, typeDefinition.Module.TypeSystem.Void);
            typeDefinition.Methods.Add(interceptor);
            return new NetAspectInterceptor(interceptor);
        }
    }

    public class NetAspectInterceptor
    {
        private MethodDefinition definition;

        public NetAspectInterceptor(MethodDefinition definition)
        {
            this.definition = definition;
        }



        public NetAspectInterceptor WithParameter<T>(string parameterName)
        {
           return WithParameter(parameterName, false, definition.Module.Import(typeof(T)), false);
        }
        public NetAspectInterceptor WithParameter(string parameterName, TypeReference type)
        {
           return WithParameter(parameterName, false, type, false);
        }

       private NetAspectInterceptor WithParameter(string parameterName, bool isByReference, TypeReference parameterType_L, bool isOut)
       {
          var fieldType = parameterType_L;
          if (isByReference)
             parameterType_L = new ByReferenceType(parameterType_L);

          var parameterDefinition = new ParameterDefinition(
             parameterName,
             ParameterAttributes.None,
             parameterType_L);

          parameterDefinition.IsOut = isOut;
          definition.Parameters.Add(parameterDefinition);
          var fieldDefinition = new FieldDefinition(definition.Name + parameterName + "Field", FieldAttributes.Public, fieldType)
             {
                IsStatic = true
             };
          definition.DeclaringType.Fields.Add(fieldDefinition);

          var instructions = definition.Body.Instructions;

          if (!fieldDefinition.IsStatic)
             instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
          instructions.Add(Instruction.Create(OpCodes.Ldarg, parameterDefinition));

          if (parameterDefinition.ParameterType.IsByReference)
             instructions.Add(Instruction.Create(OpCodes.Ldind_Ref));
          instructions.Add(Instruction.Create(fieldDefinition.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, fieldDefinition));
          return this;
       }

       public NetAspectInterceptor WithReturn()
        {
            definition.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            return this;
        }

       public NetAspectInterceptor WithUpdateParameter(string parameterName, string value)
        {
            var parameterDefinition = definition.Parameters.First(p => p.Name == parameterName);
            definition.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg, parameterDefinition));
            definition.Body.Instructions.Add(Instruction.Create(OpCodes.Ldstr, value));
            if (parameterDefinition.ParameterType.IsByReference)
                definition.Body.Instructions.Add(Instruction.Create(OpCodes.Stind_Ref));

           return this;
        }

        public NetAspectInterceptor WithReferencedParameter<T>(string parameterName)
        {
           return WithParameter(parameterName, true, definition.Module.Import(typeof(T)), false);
        }

        public NetAspectInterceptor WithOutParameter<T>(string parameterName)
        {
           return WithParameter(parameterName, true, definition.Module.Import(typeof(T)), true);
        }
    }
}