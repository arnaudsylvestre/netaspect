using FluentAspect.Core.Methods;
using Mono.Cecil;

namespace FluentAspect.Weaver
{
   class MethodDefinitionAdapter : IMethod
   {
      private MethodDefinition method;

      public MethodDefinitionAdapter(MethodDefinition method_P)
      {
         method = method_P;
      }

      public string Name
      {
         get { return method.Name; }
      }
      public IType DeclaringType
      {
         get { return new TypeDefinitionAdapter(method.DeclaringType); }
      }
   }
}