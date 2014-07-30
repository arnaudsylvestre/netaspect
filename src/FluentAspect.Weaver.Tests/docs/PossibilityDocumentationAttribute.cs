using System;

namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.Before
{

   [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
   public class PossibilityDocumentationAttribute : Attribute
   {

      public PossibilityDocumentationAttribute(string kind, string description, string description_P, string group)
      {
      }
   }
}