using FluentAspect.Weaver.Tests.Core;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Calls.Fields
{
    public class ClassAndAspectAndCall
    {
        public TypeDefinitionDefiner Class { get; set; }

        public CallFieldWeavingAspectDefiner Aspect { get; set; }

        public FieldDefinitionDefiner FieldToWeave { get; set; }

        public TypeDefinitionDefiner CallerType { get; set; }
    }
}