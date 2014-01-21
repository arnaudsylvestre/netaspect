using Mono.Cecil;

namespace FluentAspect.Weaver.Tests.Core
{
    public class TypeDefinitionDefiner
    {
        private TypeDefinition type;

        public TypeDefinitionDefiner(TypeDefinition type)
        {
            this.type = type;
        }

        public TypeDefinition Type
        {
            get { return type; }
        }

        public MethodDefinitionDefiner WithMethod(string methodName)
        {
            var methodDefinition = new MethodDefinition(methodName, MethodAttributes.Public, type.Module.TypeSystem.Void);
            //methodDefinition.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            type.Methods.Add(methodDefinition);
            return new MethodDefinitionDefiner(methodDefinition);
        }

        public FieldDefinitionDefiner WithField<T>(string fieldName)
        {
            var fieldDefinition = new FieldDefinition(fieldName, FieldAttributes.Public, type.Module.Import(typeof (T)));
            type.Fields.Add(fieldDefinition);
            return new FieldDefinitionDefiner(fieldDefinition);
        }
    }
}