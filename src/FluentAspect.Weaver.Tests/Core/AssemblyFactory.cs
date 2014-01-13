﻿using FluentAspect.Weaver.Tests.Core;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters.Before
{
   public class AssemblyFactory
   {
   }

   public class SimpleClassAndWeaver
   {
      public TypeDefinitionDefiner ClassToWeave { get; set; }

      public MethodWeavingAspectDefiner Aspect { get; set; }

      public MethodDefinitionDefiner MethodToWeave { get; set; }

      public MethodDefinitionDefiner BeforeInterceptor
      {
         get { return Aspect.AddBefore(); }
      }

      public MethodDefinitionDefiner AfterInterceptor
      {
         get { return Aspect.AddAfter(); }
      }

      public MethodDefinitionDefiner OnExceptionInterceptor
      {
         get { return Aspect.AddOnException(); }
      }
   }
}