using Mono.Cecil;

namespace FluentAspect.Weaver.Tests.Core
{
    public class FieldDefinitionDefiner
    {
        private readonly FieldDefinition _fieldDefinition;

        public FieldDefinitionDefiner(FieldDefinition fieldDefinition)
        {
            _fieldDefinition = fieldDefinition;
        }

        public void AddAspect(CallFieldWeavingAspectDefiner aspect)
        {
            _fieldDefinition.CustomAttributes.Add(new CustomAttribute(aspect.Constructor));
        }
    }
}