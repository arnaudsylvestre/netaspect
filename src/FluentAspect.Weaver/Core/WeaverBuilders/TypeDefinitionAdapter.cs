using FluentAspect.Core.Methods;
using Mono.Cecil;

namespace FluentAspect.Weaver
{
   class TypeDefinitionAdapter : IType
   {
      private TypeDefinition type;

      public TypeDefinitionAdapter(TypeDefinition type_P)
      {
         type = type_P;
      }

      public string Name
      {
         get { return type.Name; }
      }

       public string FullName { get { return type.FullName; }}
   }
}