using System;

namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.Before
{
   public class PossibilityDocumentationAttribute : Attribute
   {

      public PossibilityDocumentationAttribute(string kind, string description, string description_P)
      {
      }
   }
}