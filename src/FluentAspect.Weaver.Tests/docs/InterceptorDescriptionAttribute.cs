using System;

namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.Before
{
   [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
   public class InterceptorDescriptionAttribute : Attribute
   {
      public InterceptorDescriptionAttribute(string before_P, string beforeTheMethodIsExecuted_P)
      {
      }
   }
}
