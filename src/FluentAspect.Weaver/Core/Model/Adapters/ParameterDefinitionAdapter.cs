using FluentAspect.Weaver.Core;
using FluentAspect.Weaver.Core.WeaverBuilders;
using Mono.Cecil;

namespace FluentAspect.Weaver.Helpers
{
    internal class ParameterDefinitionAdapter : IParameter
    {
        private readonly ParameterDefinition _parameterDefinition;

        public ParameterDefinitionAdapter(ParameterDefinition parameterDefinition_P)
        {
            _parameterDefinition = parameterDefinition_P;
        }

        public IType Type
        {
            get { return new TypeDefinitionAdapter(_parameterDefinition.ParameterType); }
        }

        public string Name
        {
            get { return _parameterDefinition.Name; }
        }
    }
}