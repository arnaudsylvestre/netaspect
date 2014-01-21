using Mono.Cecil;

namespace FluentAspect.Weaver.Tests.Core
{
    public class CallWeavingAspectDefiner
    {
        private TypeDefinition typeDefinition;

        public CallWeavingAspectDefiner(TypeDefinition typeDefinition)
        {
            this.typeDefinition = typeDefinition;
        }

        public MethodDefinitionDefiner AddBeforeCall()
        {
            var definition = new MethodDefinition("BeforeCall", MethodAttributes.Public, typeDefinition.Module.Import(typeof (object)));
            typeDefinition.Methods.Add(definition);
            return new MethodDefinitionDefiner(definition);
        }
    }
}