using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Model.Adapters
{
    internal class TypeDefinitionAdapter : IType
    {
        private readonly TypeReference type;

        public TypeDefinitionAdapter(TypeReference type_P)
        {
            type = type_P;
        }

        public string Name
        {
            get { return type.Name; }
        }

        public string FullName
        {
            get { return type.FullName.Replace("/", "+"); }
        }
    }
}