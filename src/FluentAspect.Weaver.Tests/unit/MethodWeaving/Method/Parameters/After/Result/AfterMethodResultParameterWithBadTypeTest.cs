using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Result
{
   public class AfterMethodResultParameterWithBadTypeTest : NetAspectTest<AfterMethodResultParameterWithBadTypeTest.ClassToWeave>
   {
      protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
      {
         return errorHandler => errorHandler.Errors.Add(string.Format("the result parameter in the method After of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.String because the return type of the method Weaved in the type {1}", typeof(MyAspect).FullName, typeof(ClassToWeave).FullName));
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Weaved()
         {
             return "Hello";
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public void After(int result)
         {
         }
      }
   }

   
}