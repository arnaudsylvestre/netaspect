using System.Collections.Generic;
using Mono.Cecil;

namespace FluentAspect.Weaver.Tests.Core.Model
{
    public class NetAspectField
    {
        public string Name { get; private set; }

        public NetAspectField(string name, TypeReference type)
        {
            Name = name;
            Type = type;
        }

        public NetAspectVisibility Visibility { get; set; }

        public bool IsStatic { get; set; }

        public TypeReference Type { get; set; }

        public FieldDefinition Generate()
        {
            FieldAttributes attributes = Compute(Visibility);
            if (IsStatic)
                attributes |= FieldAttributes.Static;
            return new FieldDefinition(Name, attributes, Type);
        }

        private FieldAttributes Compute(NetAspectVisibility visibility)
        {
            var attributes = new Dictionary<NetAspectVisibility, FieldAttributes>();
            attributes.Add(NetAspectVisibility.Internal, FieldAttributes.Assembly);
            attributes.Add(NetAspectVisibility.Public, FieldAttributes.Public);
            attributes.Add(NetAspectVisibility.Protected, FieldAttributes.Family);
            attributes.Add(NetAspectVisibility.Private, FieldAttributes.Private);

            return attributes[visibility];

        }
    }
}