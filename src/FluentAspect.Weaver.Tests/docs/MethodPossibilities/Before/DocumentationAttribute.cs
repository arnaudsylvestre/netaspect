using System;

namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.Before
{
   public class DocumentationAttribute : Attribute
   {
      public DocumentationAttribute(string description_P, string title_P, string kind)
      {
      }
   }
}