using FluentAspect.Core.Methods;
using Mono.Cecil;

namespace FluentAspect.Weaver
{
   class TypeDefinitionAdapter : IType
   {
      private TypeReference type;

      public TypeDefinitionAdapter(TypeReference type_P)
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