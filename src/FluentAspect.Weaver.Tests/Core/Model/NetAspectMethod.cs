using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using MethodAttributes = Mono.Cecil.MethodAttributes;
using ParameterAttributes = Mono.Cecil.ParameterAttributes;

namespace FluentAspect.Weaver.Tests.Core.Model
{
    public class NetAspectParameter
    {
        private readonly bool _isOut;
        private ParameterDefinition parameterDefinition;
        public TypeReference Type { get; set; }
        public string Name { get; set; }

        public NetAspectParameter(string name, TypeReference type, bool isOut)
        {
            _isOut = isOut;
            Name = name;
            Type = type;
        }

        public ParameterDefinition ParameterDefinition
        {
            get { if (parameterDefinition == null)
            {
                ParameterAttributes attributes = ParameterAttributes.None;
                if (_isOut)
                    attributes |= ParameterAttributes.Out;
                parameterDefinition = new ParameterDefinition(Name, attributes, Type);
            }
            return parameterDefinition;
            }
        }
    }

   public interface INetAspectMethodInstruction
   {
      void Generate(Collection<Instruction> instructions);
   }

   public class ReturnInstruction : INetAspectMethodInstruction
   {
      public void Generate(Collection<Instruction> instructions)
      {
         instructions.Add(Instruction.Create(OpCodes.Ret));
      }
   }



    public class NetAspectMethod
    {
        public string Name { get; private set; }
        public ModuleDefinition ModuleDefinition { get; set; }
       List<INetAspectMethodInstruction> instructions = new List<INetAspectMethodInstruction>();
       private INetAspectType declaringType;

        public INetAspectType DeclaringType
        {
            get { return declaringType; }
        }

        public NetAspectMethod(string name, TypeReference type, ModuleDefinition moduleDefinition, INetAspectType declaringType_P)
        {
            Name = name;
            Type = type;
            ModuleDefinition = moduleDefinition;
           declaringType = declaringType_P;
        }

        public NetAspectVisibility Visibility { get; set; }

        public bool IsStatic { get; set; }

        public TypeReference Type { get; set; }

        List<NetAspectParameter> parameters = new List<NetAspectParameter>(); 

        private MethodDefinition methodDefinition;

        public MethodDefinition MethodDefinition
        {
            get
            {
                if (methodDefinition == null)
                {
                    methodDefinition = Generate();
                }
                return methodDefinition;
            }
        }

       public void AddInstruction(INetAspectMethodInstruction instruction)
       {
          instructions.Add(instruction);
       }


        private MethodDefinition Generate()
        {
            MethodAttributes attributes = Compute(Visibility);
            if (IsStatic)
                attributes |= MethodAttributes.Static;
            if (Name == ".ctor")
                attributes |= MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName |
                              MethodAttributes.RTSpecialName;

            MethodDefinition def = new MethodDefinition(Name, attributes, Type);
            foreach (var aspect in aspects)
            {
                def.CustomAttributes.Add(new CustomAttribute(aspect.DefaultConstructor));
                
            }
           var instructions_L = def.Body.Instructions;

           foreach (var parameter_L in parameters)
           {
              def.Parameters.Add(parameter_L.ParameterDefinition);
           }
           

           if (Name == ".ctor")
           {
              var baseType_L = declaringType.BaseType;
              if (baseType_L == null)
                 baseType_L = typeof (object);

              foreach (var fieldDefinition_L in declaringType.Fields)
           {
              if (fieldDefinition_L.DefaultValue != null)
              {
                 instructions_L.Add(Instruction.Create(OpCodes.Ldarg_0));
                 instructions_L.Add(Instruction.Create(OpCodes.Ldstr, fieldDefinition_L.DefaultValue));
                 instructions_L.Add(Instruction.Create(OpCodes.Stfld, fieldDefinition_L.Field));

              }
           }

              var constructorInfo_L = baseType_L.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)[0];
              instructions_L.Add(Instruction.Create(OpCodes.Ldarg_0));
              instructions_L.Add(Instruction.Create(OpCodes.Call, ModuleDefinition.Import(constructorInfo_L)));
           }

           foreach (var instruction_L in instructions)
           {
              instruction_L.Generate(instructions_L);
           }

           

            return def;
        }

        private MethodAttributes Compute(NetAspectVisibility visibility)
        {
            var attributes = new Dictionary<NetAspectVisibility, MethodAttributes>();
            attributes.Add(NetAspectVisibility.Internal, MethodAttributes.Assembly);
            attributes.Add(NetAspectVisibility.Public, MethodAttributes.Public);
            attributes.Add(NetAspectVisibility.Protected, MethodAttributes.Family);
            attributes.Add(NetAspectVisibility.Private, MethodAttributes.Private);

            return attributes[visibility];

        }

        List<NetAspectAspect> aspects = new List<NetAspectAspect>();

        public NetAspectMethod WithAspect(NetAspectAspect aspect)
        {
            aspects.Add(aspect);
            return this;
        }

        public NetAspectMethod Add(NetAspectParameter parameter)
        {
            parameters.Add(parameter);
            return this;
        }
    }
}