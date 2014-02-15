using System;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Method
{
   public class BeforeMethodMethodInfoParameterWithBadTypeTest : NetAspectTest<BeforeMethodMethodInfoParameterWithBadTypeTest.ClassToWeave>
   {
      protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
      {
         return errorHandler => errorHandler.Errors.Add(string.Format("the method parameter in the method Before of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Reflection.MethodInfo", typeof(MyAspect).FullName));
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved()
         {

         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public void Before(int method)
         {
         }
      }
   }

   
}