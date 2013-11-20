using System;
using FluentAspect.Core.Methods;
using Mono.Cecil;
using SheepAspect.Pointcuts;
using SheepAspect.Pointcuts.Impl;

namespace FluentAspect.Weaver
{
   public class FluentMethodPointcut : MemberPointcut<IMethodPointcut, MethodDefinition>, IMethodPointcut
   {
      public void SetMatcher(Func<IMethod, bool> matcher_P)
      {
         Where(m => matcher_P(new MethodDefinitionAdapter(m)));
      }
   }
   class TypeDefinitionAdapter : IType
   {
      private TypeDefinition type;

      public TypeDefinitionAdapter(TypeDefinition type_P)
      {
         type = type_P;
      }

      public string Name {
         get { return type.Name; }
      }
   }

   class MethodDefinitionAdapter : IMethod
   {
      private MethodDefinition method;

      public MethodDefinitionAdapter(MethodDefinition method_P)
      {
         method = method_P;
      }

      public string Name {
         get { return method.Name; }
      }
      public IType DeclaringType
      {
         get { return new TypeDefinitionAdapter(method.DeclaringType); }
      }
   }
}