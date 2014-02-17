using System;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnFinally.Method
{
   public class OnFinallyMethodMethodInfoParameterWithBadTypeTest : NetAspectTest<OnFinallyMethodMethodInfoParameterWithBadTypeTest.ClassToWeave>
   {
      protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
      {
         return errorHandler => errorHandler.Errors.Add(string.Format("the method parameter in the method OnFinally of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Reflection.MethodInfo", typeof(MyAspect).FullName));
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

         public void OnFinally(int method)
         {
         }
      }
   }

   
}