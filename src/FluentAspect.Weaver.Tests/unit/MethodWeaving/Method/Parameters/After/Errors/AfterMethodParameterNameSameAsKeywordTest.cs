using System;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Errors
{
   public class AfterMethodParameterNameSameAsKeywordTest : NetAspectTest<AfterMethodParameterNameSameAsKeywordTest.ClassToWeave>
   {
      protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
      {
         return errorHandler => errorHandler.Errors.Add(string.Format("The parameter instance is already declared"));
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved(int instance)
         {

         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public void After(int instance)
         {
         }
      }
   }

   
}