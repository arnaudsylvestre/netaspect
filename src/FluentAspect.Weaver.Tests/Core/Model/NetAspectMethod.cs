using System.Collections.Generic;
using Mono.Cecil;

namespace FluentAspect.Weaver.Tests.Core.Model
{
    public class NetAspectMethod
    {
        public string Name { get; private set; }

        public NetAspectMethod(string name, TypeReference type)
        {
            Name = name;
            Type = type;
        }

        public NetAspectVisibility Visibility { get; set; }

        public bool IsStatic { get; set; }

        public TypeReference Type { get; set; }

        private MethodDefinition Generate()
        {
            MethodAttributes attributes = Compute(Visibility);
            if (IsStatic)
                attributes |= MethodAttributes.Static;
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
    }
}