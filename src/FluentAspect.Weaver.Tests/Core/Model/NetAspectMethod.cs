using System.Collections.Generic;
using Mono.Cecil;

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

    public class NetAspectMethod
    {
        public string Name { get; private set; }
        public ModuleDefinition ModuleDefinition { get; set; }

        public NetAspectMethod(string name, TypeReference type, ModuleDefinition moduleDefinition)
        {
            Name = name;
            Type = type;
            ModuleDefinition = moduleDefinition;
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